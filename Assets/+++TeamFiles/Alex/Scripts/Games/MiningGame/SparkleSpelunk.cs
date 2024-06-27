using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] public List<Transform> allBlocks;
    private bool wallCanGoUp;
    private bool canSpawnNewWall;
    private int diamondSpawnProbability = 2;
    private bool spawnDiamond;

    [Header("Miner")]
    [SerializeField] private Transform miner;
    [SerializeField] private float dropTime;
    [SerializeField] private SpriteRenderer minerHead;
    [SerializeField] private Sprite minerHoldSpriteHead;
    [SerializeField] private Sprite minerIdleSpriteHead;
    [SerializeField] private GameObject minerHands;
    [SerializeField] private Sprite minerHoldSpriteHands;
    [SerializeField] private Sprite minerIdleSpriteHands;
    private bool isFalling;
    private float currentDropTimer;
    private Transform spawnedBomb;
    
    [Header("Score")] 
    [SerializeField] private int winScore;
    public int currentScore;
    
    [Header("Time")]
    [SerializeField] public int timeBonusGold;
    [SerializeField] public int timeBonusDiamond;
    [SerializeField] public int timePenalty;

    void Update()
    {
        SpawnBomb();
        
        MoveByBlock();
        
        SpawnNewBlocks();
    }

    //Spawns a bomb for destroying walls
    private void SpawnBomb()
    {
        if (PlayerInputs.instance.holdObjectState != PlayerInputs.HoldObjectState.InHand)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            minerHead.sprite = minerHoldSpriteHead;
            minerHands.GetComponent<SpriteRenderer>().sprite = minerHoldSpriteHands;
            minerHands.transform.localPosition = new Vector3(minerHands.transform.localPosition.x, 0.064f, minerHands.transform.localPosition.z);
            spawnedBomb = Instantiate(bomb, new Vector3(0.089f, 0.323f, 0f), Quaternion.identity, miner);
            Destroy(spawnedBomb.gameObject, 3f);
        }
        else if(Input.GetKeyUp(KeyCode.Space) && !isFalling)
        {
            minerHead.sprite = minerIdleSpriteHead;
            minerHands.GetComponent<SpriteRenderer>().sprite = minerIdleSpriteHands;
            minerHands.transform.localPosition = new Vector3(minerHands.transform.localPosition.x, -0.709f, minerHands.transform.localPosition.z);
            spawnedBomb.transform.localPosition = new Vector3(spawnedBomb.transform.localPosition.x, -0.636f,
                spawnedBomb.transform.localPosition.z);
            spawnedBomb.parent = miner.parent;
            spawnedBomb.GetComponent<Bomb>().DropBomb();
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
                currentScore++;
                if (currentScore >= winScore)
                {
                    MotherBehaviour.instance.PlayerWon();
                }
                
                MoveObject(miner, block.transform.localScale.y, 0);
                currentDropTimer = 0;
            }
            else if(currentDropTimer >= dropTime && wallCanGoUp)
            {
                foreach (var blockTransform in allBlocks)
                {
                    MoveObject(blockTransform, -blockTransform.transform.localScale.y, 0);
                    currentDropTimer = 0;
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
