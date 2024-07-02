using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SparkleSpelunk : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] public LayerMask blockLayer;
    [SerializeField] public LayerMask goldLayer;
    [SerializeField] public LayerMask diamondLayer;
    [SerializeField] public LayerMask deleteZone;
    [SerializeField] public LayerMask playerLayer;
    [SerializeField] public LayerMask borderLayer;

    [Header("Prefabs")]
    [SerializeField] private Transform bomb;
    [SerializeField] public Transform block;
    [SerializeField] private Transform goldBlock;
    [SerializeField] private Transform diamondBlock;

    [Header("BlockSpawning")]
    [SerializeField] private Transform blockParent;
    [SerializeField] private List<Vector3> blockSpawnPoints;
    [HideInInspector] public List<Transform> allBlocks;
    [SerializeField] private Transform blocksElement;
    private bool wallCanGoUp;
    private bool canSpawnNewWall;
    private int diamondSpawnProbability = 2;
    private bool spawnDiamond;

    [Header("Miner")]
    [SerializeField] private Transform miner;
    [SerializeField] private float dropTime;
    [SerializeField] public SpriteRenderer minerHead;
    [SerializeField] public SpriteRenderer minerHands;
    [SerializeField] private Sprite minerHoldSpriteHead;
    [SerializeField] public Sprite minerIdleSpriteHead;
    [SerializeField] private Sprite minerHitSpriteHead;
    [SerializeField] private Sprite minerEarnedSpriteHead;
    [SerializeField] private Sprite minerHoldSpriteHands;
    [SerializeField] public Sprite minerEarnedSpriteHandDiamond;
    [SerializeField] public Sprite minerEarnedSpriteHandGold;
    [SerializeField] public Sprite minerIdleSpriteHands;
    private bool isFalling;
    private float currentDropTimer;
    private Transform spawnedBomb;
    private Transform firstBomb;
    private Transform secondBomb;
    public bool firstBombSpawned;
    public bool secondBombSpawned;
    [HideInInspector] public bool bombInHand;

    [Header("Score")] 
    [SerializeField] private int winScore;
    public int currentScore;
    private float bombSpeedMultiplierAdd = 3;
    private float fallSpeedSubtraction = .2f;
    [HideInInspector] public float bombSpeedMultiplier = 1;

    [Header("Time")]
    [SerializeField] public int timeBonusGold;
    [SerializeField] public int timeBonusDiamond;
    [SerializeField] public int timePenalty;

    private void Start()
    {
        bombSpeedMultiplierAdd /= winScore;

        fallSpeedSubtraction /= winScore;
        
        allBlocks = new List<Transform>();
        
        for (int i = 0; i < blocksElement.childCount; i++)
        {
            allBlocks.Add(blocksElement.GetChild(i));
        }
    }

    void Update()
    {
        SpawnBomb();
        
        MoveByBlock();
        
        SpawnNewBlocks();
    }

    public void MinerHit()
    {
        StartCoroutine(MinerHitCoroutine());
        PlayerInputs.instance.PlayChildAggressiveAnimation();
    }
    
    public void MinerEarned(Sprite hands)
    {
        StartCoroutine(MinerEarnedCoroutine(hands));
    }
    
    private IEnumerator MinerEarnedCoroutine(Sprite hands)
    {
        var currentSpriteHead = minerHead.sprite;
        var currentSpriteHands = minerHands.sprite;
        minerHead.sprite = minerEarnedSpriteHead;
        minerHands.sprite = hands;

        yield return new WaitForSeconds(1f);
        
        minerHead.sprite = currentSpriteHead;
        minerHands.sprite = currentSpriteHands;
    }
    
    private IEnumerator MinerHitCoroutine()
    {
        var currentSprite = minerHead.sprite;
        minerHead.sprite = minerHitSpriteHead;
        minerHead.color = Color.red;
        minerHands.color = Color.red;

        yield return new WaitForSeconds(0.25f);
        
        minerHead.sprite = currentSprite;
        minerHead.color = Color.white;
        minerHands.color = Color.white;
    }

    //Spawns a bomb for destroying walls
    private void SpawnBomb()
    {
        if (PlayerInputs.instance.holdObjectState != PlayerInputs.HoldObjectState.InHand || secondBombSpawned)
            return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.Play("SparkleSpelunkTakeOutBomb");
            bombInHand = true;

            minerHead.sprite = minerHoldSpriteHead;
            minerHands.sprite = minerHoldSpriteHands;
            minerHands.transform.localPosition = new Vector3(minerHands.transform.localPosition.x, 0.064f, minerHands.transform.localPosition.z);
            spawnedBomb = Instantiate(bomb, new Vector3(0.089f, 0.323f, 0f), Quaternion.identity, miner);
        }
        else if(Input.GetKeyUp(KeyCode.Space) && !isFalling)
        {
            bombInHand = false;
            
            minerHead.sprite = minerIdleSpriteHead;
            minerHands.sprite = minerIdleSpriteHands;
            minerHands.transform.localPosition = new Vector3(minerHands.transform.localPosition.x, -0.709f, minerHands.transform.localPosition.z);
            spawnedBomb.transform.localPosition = new Vector3(spawnedBomb.transform.localPosition.x, -0.636f, spawnedBomb.transform.localPosition.z);
            spawnedBomb.GetComponent<Bomb>().bombDropped = true;
            spawnedBomb.SetParent(miner.parent);
            
            if (!firstBombSpawned)
            {
                firstBomb = spawnedBomb;
                firstBombSpawned = true;
            }
            else if(!secondBombSpawned)
            {
                secondBomb = spawnedBomb;
                secondBombSpawned = true;
            }
        }
    }

    //Moves miner and blocks in block metric
    private void MoveByBlock()
    {
        if (PlayerInputs.instance.holdObjectState != PlayerInputs.HoldObjectState.InHand)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!RaycastInDirection(miner.right, borderLayer) && !isFalling)
            {
                MoveObject(miner, 0, block.transform.localScale.x);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            if(!RaycastInDirection(-miner.right, borderLayer) && !isFalling)
            {
                MoveObject(miner, 0, -block.transform.localScale.x);
            }
        }
        
        if(!RaycastInDirection(-miner.up, blockLayer) && !RaycastInDirection(-miner.up, goldLayer) && !RaycastInDirection(-miner.up, diamondLayer))
        {
            isFalling = true;
            
            currentDropTimer += Time.deltaTime;

            if(currentDropTimer >= dropTime && !wallCanGoUp)
            {
                MoveObject(miner, block.transform.localScale.y, 0);
                currentDropTimer = 0;
            }
            else if(currentDropTimer >= dropTime && wallCanGoUp)
            {
                currentScore++;
                dropTime -= fallSpeedSubtraction;
                bombSpeedMultiplier += bombSpeedMultiplierAdd;
                if (currentScore >= winScore)
                {
                    MotherBehaviour.instance.PlayerWon();
                    AudioManager.Instance.Play("SparkleSpelunkWon");
                }
                
                foreach (var blockTransform in allBlocks)
                {
                    MoveObject(blockTransform, -block.transform.localScale.y, 0);
                }
                
                currentDropTimer = 0;

                if (firstBombSpawned)
                {
                    MoveObject(firstBomb, -block.transform.localScale.y, 0);
                }
                
                if (secondBombSpawned)
                {
                    MoveObject(secondBomb, -block.transform.localScale.y, 0);
                }
            }
        }
        else
        {
            isFalling = false;
            canSpawnNewWall = true;
            wallCanGoUp = true;
        }
    }

    //Checks when the miner has dropped by a wall and spawns new walls 
    private void SpawnNewBlocks()
    {
        if(RaycastInDirection(miner.right, blockLayer) || RaycastInDirection(miner.right, goldLayer) || RaycastInDirection(-miner.right, goldLayer) || RaycastInDirection(-miner.right, blockLayer) || RaycastInDirection(miner.right, diamondLayer) || RaycastInDirection(-miner.right, diamondLayer))
        {
            if (canSpawnNewWall)
            {
                var randomPointBlock = Random.Range(0, 4);

                var randomRow = Random.Range(0, diamondSpawnProbability);
                
                for (var i = 0; i < blockSpawnPoints.Count; i++)
                {
                    if (randomPointBlock == i)
                    {
                        if (randomRow == 0 && spawnDiamond)
                        {
                            var spawnedPointBlock = Instantiate(diamondBlock, blockSpawnPoints[i], Quaternion.identity, blockParent);
                            spawnedPointBlock.localPosition = blockSpawnPoints[i];
                            allBlocks.Add(spawnedPointBlock);
                            spawnDiamond = false;
                        }
                        else
                        {
                            var spawnedPointBlock = Instantiate(goldBlock, blockSpawnPoints[i], Quaternion.identity, blockParent);
                            spawnedPointBlock.localPosition = blockSpawnPoints[i];
                            allBlocks.Add(spawnedPointBlock);   
                        }
                    }
                    else
                    {
                        var spawnedBlock = Instantiate(block, blockSpawnPoints[i], Quaternion.identity, blockParent);
                        spawnedBlock.localPosition = blockSpawnPoints[i];
                        allBlocks.Add(spawnedBlock);
                    }
                }

                diamondSpawnProbability--;

                if (diamondSpawnProbability == 0)
                {
                    spawnDiamond = true;
                    diamondSpawnProbability = 2;
                }
                
                canSpawnNewWall = false;
            }
        }
    }

    //Shortcut for making a raycast in a direction
    private bool RaycastInDirection(Vector3 direction, LayerMask layer)
    {
        return Physics.Raycast(miner.position, direction, block.transform.localScale.y, layer);
    }

    //Shortcut for moving an object in a specific direction
    private void MoveObject(Transform objectTransform, float positionOffsetY, float positionOffsetX)
    {
        var position = objectTransform.position;
        position = new Vector3(position.x - positionOffsetX, position.y - positionOffsetY, position.z);
        objectTransform.position = position;
    }
}
