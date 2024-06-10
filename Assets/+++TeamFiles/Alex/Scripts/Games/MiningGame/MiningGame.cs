using System.Collections.Generic;
using UnityEngine;

public class MiningGame : MonoBehaviour
{
    [Header("Layers")]
    [SerializeField] public LayerMask wallLayer;
    [SerializeField] public LayerMask pointWallLayer;
    [SerializeField] public LayerMask deleteZone;
    
    [Header("Prefabs")]
    [SerializeField] private Transform bomb;
    [SerializeField] private Transform block;
    
    [SerializeField] private Transform miner;
    [SerializeField] private List<Vector3> blockSpawnPoints;
    [SerializeField] public List<Transform> allBlocks;
    [SerializeField] private float dropTime;
    private bool wallCanGoUp;
    private bool isFalling;
    private float timer;
    
    void Update()
    {
        SpawnBomb();
        
        MoveByBlock();
    }

    private void SpawnBomb()
    {
        if (Input.GetMouseButtonDown(0) && !isFalling)
        {
            var spawnedBomb = Instantiate(bomb, miner.transform.position, Quaternion.identity, miner.parent);
            Destroy(spawnedBomb.gameObject, 1.5f);
        }
    }

    private void MoveByBlock()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(!RaycastInDirection(-miner.right, wallLayer) && !isFalling)
            {
                MoveObject(miner, 0, block.transform.localScale.x);
            }
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            if(!RaycastInDirection(miner.right, wallLayer) && !isFalling)
            {
                MoveObject(miner, 0, -block.transform.localScale.x);
            }
        }
        
        if(!RaycastInDirection(-miner.up, wallLayer) && !RaycastInDirection(-miner.up, pointWallLayer))
        {
            isFalling = true;
            
            timer += Time.deltaTime;
            
            if(timer >= dropTime && !wallCanGoUp)
            {
                MoveObject(miner, block.transform.localScale.y, 0);
                timer = 0;
            }
            else if(timer >= dropTime && wallCanGoUp)
            {
                foreach (var b in allBlocks)
                {
                    MoveObject(b, -block.transform.localScale.y, 0);
                    timer = 0;
                }
            }
        }
        else
        {
            isFalling = false;
            wallCanGoUp = true;
        }
        
        if(RaycastInDirection(miner.right, wallLayer) || RaycastInDirection(miner.right, pointWallLayer))
        {
            for (int i = 0; i < blockSpawnPoints.Count; i++)
            {
                var spawnedBlock = Instantiate(block, blockSpawnPoints[i], Quaternion.identity);
                allBlocks.Add(spawnedBlock);
            }
            //Walls are getting spawned too often, need to spawn once
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
