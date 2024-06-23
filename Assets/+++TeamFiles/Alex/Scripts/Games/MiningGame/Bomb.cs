using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector] public bool bombDropped;
    
    private void Update()
    {
        if (!bombDropped)
        {
            transform.position = FindObjectOfType<SparkleSpelunk>().transform.GetChild(0).transform.position;
        }
    }

    public void DropBomb()
    {
        bombDropped = true;
    }
    
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
            MotherTimerManager.instance.TimeBonus();
        }
        else if(Physics.Raycast(transform.position, -transform.up, out var playerHit, .1f, FindObjectOfType<SparkleSpelunk>().playerLayer))
        {
            MotherTimerManager.instance.TimePenalty();
        }
    }
}
