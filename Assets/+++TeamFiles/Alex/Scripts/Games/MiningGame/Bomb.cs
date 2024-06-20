using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnDestroy()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var blockHit, .5f, FindObjectOfType<MiningGame>().wallLayer))
        {
            FindObjectOfType<MiningGame>().allBlocks.Remove(blockHit.transform);
            Destroy(blockHit.transform.gameObject);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out var pointHit, .5f, FindObjectOfType<MiningGame>().pointWallLayer))
        {
            FindObjectOfType<MiningGame>().allBlocks.Remove(pointHit.transform);
            Destroy(pointHit.transform.gameObject);
            UIScoreCounter.instance.AddPointsToScore();
        }
        else if(Physics.Raycast(transform.position, -transform.up, out var playerHit, .1f, FindObjectOfType<MiningGame>().playerLayer))
        {
            //Does not work rn & still need to implement a wall that comes down and a reset of all blocks
            UIScoreCounter.instance.ResetCombo();
            FindObjectOfType<MiningGame>().ResetGame();
        }
    }
}
