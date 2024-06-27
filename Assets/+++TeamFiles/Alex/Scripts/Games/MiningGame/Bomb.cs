using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector] public bool bombDropped;
    private SparkleSpelunk sparkleSpelunk;

    private void Start() => sparkleSpelunk = FindObjectOfType<SparkleSpelunk>();
    
    private void Update()
    {
        if (!bombDropped)
        {
            transform.position = sparkleSpelunk.transform.GetChild(0).transform.position;
        }
    }

    public void DropBomb()
    {
        bombDropped = true;
    }
    
    private void OnDestroy()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var blockHit, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.blockLayer))
        {
            sparkleSpelunk.allBlocks.Remove(blockHit.transform);
            Destroy(blockHit.transform.gameObject);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out var goldHit, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.goldLayer))
        {
            sparkleSpelunk.allBlocks.Remove(goldHit.transform);
            Destroy(goldHit.transform.gameObject);
            MotherTimerManager.instance.TimeBonus(sparkleSpelunk.timeBonusGold);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out var diamondHit, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.diamondLayer))
        {
            sparkleSpelunk.allBlocks.Remove(diamondHit.transform);
            Destroy(diamondHit.transform.gameObject);
            MotherTimerManager.instance.TimeBonus(sparkleSpelunk.timeBonusDiamond);
        }
        
        if(Physics.Raycast(transform.position, -transform.up, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || Physics.Raycast(transform.position, -transform.right, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || Physics.Raycast(transform.position, transform.right, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer))
        {
            MotherTimerManager.instance.TimePenalty(sparkleSpelunk.timePenalty);
        }
    }
}
