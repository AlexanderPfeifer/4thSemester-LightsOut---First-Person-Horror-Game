using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnDestroy()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var blockHit, .5f, FindObjectOfType<SparkleSpelunk>().wallLayer))
        {
            FindObjectOfType<SparkleSpelunk>().allBlocks.Remove(blockHit.transform);
            Destroy(blockHit.transform.gameObject);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out var pointHit, .5f, FindObjectOfType<SparkleSpelunk>().pointWallLayer))
        {
            FindObjectOfType<SparkleSpelunk>().allBlocks.Remove(pointHit.transform);
            Destroy(pointHit.transform.gameObject);
            UIScoreCounter.instance.TimeBonus();
        }
        else if(Physics.Raycast(transform.position, -transform.up, out var playerHit, .1f, FindObjectOfType<SparkleSpelunk>().playerLayer))
        {
            //Does not work rn & still need to implement a wall that comes down and a reset of all blocks
            FindObjectOfType<SparkleSpelunk>().ResetGame();
        }
    }
}
