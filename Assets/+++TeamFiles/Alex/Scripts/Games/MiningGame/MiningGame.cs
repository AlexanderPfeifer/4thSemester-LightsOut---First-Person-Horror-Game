using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiningGame : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public LayerMask pointWallLayer;
    [SerializeField] public LayerMask deleteZone;
    [SerializeField] private LayerMask borderLayer;
    [SerializeField] public LayerMask playerLayer;

    [Header("Prefabs")]
    [SerializeField] private Transform bomb;
    [SerializeField] private Transform block;
    [SerializeField] private Transform pointBlock;

    [Header("BlockSpawning")]
    [SerializeField] private Transform blockParent;
    [SerializeField] private List<Vector3> blockSpawnPoints;
    [SerializeField] public List<Transform> allBlocks;
    private bool wallCanGoUp;
    private bool canSpawnNewWall;

    [Header("Miner")]
    [SerializeField] private Transform miner;
    [SerializeField] private float dropTime;
    public Vector3 spawnPoint;
    private bool isFalling;
    private float currentDropTimer;

    private void Start() => spawnPoint = miner.transform.position;

    void Update()
    {
        SpawnBomb();
        
        MoveByBlock();
        
        SpawnNewBlocks();
    }

    public void ResetGame()
    {
        miner.localPosition = spawnPoint;
    }

    //Spawns a bomb for destroying walls
    private void SpawnBomb()
    {
        if (PlayerInputs.instance.holdObjectState != PlayerInputs.HoldObjectState.InHand)
            return;

        if (Input.GetMouseButtonDown(0) && !isFalling)
        {
            var spawnedBomb = Instantiate(bomb, miner.transform.position, Quaternion.identity, miner.parent);
            Destroy(spawnedBomb.gameObject, 1.5f);
        }
    }

    //Moves miner and blocks in block metric
    private void MoveByBlock()
    {
        if (PlayerInputs.instance.holdObjectState != PlayerInputs.HoldObjectState.InHand)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!RaycastInDirection(-miner.right, borderLayer) && !isFalling)
            {
                MoveObject(miner, 0, block.transform.localScale.x);
            }
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            if(!RaycastInDirection(miner.right, borderLayer) && !isFalling)
            {
                MoveObject(miner, 0, -block.transform.localScale.x);
            }
        }
        
        if(!RaycastInDirection(-miner.up, wallLayer) && !RaycastInDirection(-miner.up, pointWallLayer))
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
                foreach (var b in allBlocks)
                {
                    MoveObject(b, -block.transform.localScale.y, 0);
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
        if(RaycastInDirection(miner.right, wallLayer) || RaycastInDirection(miner.right, pointWallLayer) || RaycastInDirection(-miner.right, pointWallLayer) || RaycastInDirection(-miner.right, wallLayer))
        {
            if (canSpawnNewWall)
            {
                int randomPointBlock = Random.Range(0, 4);
                
                for (int i = 0; i < blockSpawnPoints.Count; i++)
                {
                    if (randomPointBlock == i)
                    {
                        var spawnedPointBlock = Instantiate(pointBlock, blockSpawnPoints[i], Quaternion.identity, blockParent);
                        spawnedPointBlock.localPosition = blockSpawnPoints[i];
                        allBlocks.Add(spawnedPointBlock);
                    }
                    else
                    {
                        var spawnedBlock = Instantiate(block, blockSpawnPoints[i], Quaternion.identity, blockParent);
                        spawnedBlock.localPosition = blockSpawnPoints[i];
                        allBlocks.Add(spawnedBlock);
                    }
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
