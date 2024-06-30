using UnityEngine;

public class Bomb : MonoBehaviour
{
    [HideInInspector] public bool bombDropped;
    private SparkleSpelunk sparkleSpelunk;
    private Animator anim;
    private Animator bombAnim;

    private void Start()
    {
        sparkleSpelunk = FindObjectOfType<SparkleSpelunk>();
        
        GetComponent<Animator>().SetFloat("speedMultiplier", sparkleSpelunk.bombSpeedMultiplier);
    }
    
    private void Update()
    {
        if (!bombDropped)
        {
            transform.position = sparkleSpelunk.transform.GetChild(0).transform.position;
        }
        
        Debug.Log( sparkleSpelunk.secondBombSpawned);
    }

    private void DestroyBomb()
    {
        if (sparkleSpelunk.firstBombSpawned)
        {
            sparkleSpelunk.firstBombSpawned = false;
        }
        else if(sparkleSpelunk.secondBombSpawned)
        {
            sparkleSpelunk.secondBombSpawned = false;
        }
        
        Destroy(gameObject);
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
            sparkleSpelunk.MinerEarned(sparkleSpelunk.minerEarnedSpriteHandGold);
            sparkleSpelunk.allBlocks.Remove(goldHit.transform);
            Destroy(goldHit.transform.gameObject);
            MotherTimerManager.instance.TimeBonus(sparkleSpelunk.timeBonusGold);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out var diamondHit, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.diamondLayer))
        {
            sparkleSpelunk.MinerEarned(sparkleSpelunk.minerEarnedSpriteHandDiamond);
            sparkleSpelunk.allBlocks.Remove(diamondHit.transform);
            Destroy(diamondHit.transform.gameObject);
            MotherTimerManager.instance.TimeBonus(sparkleSpelunk.timeBonusDiamond);
        }
        
        if(Physics.Raycast(transform.position, -transform.up, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || Physics.Raycast(transform.position, -transform.right, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || Physics.Raycast(transform.position, transform.right, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || sparkleSpelunk.bombInHand)
        {
            if (!sparkleSpelunk.firstBombSpawned && sparkleSpelunk.bombInHand)
            {
                sparkleSpelunk.minerHead.sprite = sparkleSpelunk.minerIdleSpriteHead;
                sparkleSpelunk.minerHands.sprite = sparkleSpelunk.minerIdleSpriteHands;
                sparkleSpelunk.minerHands.transform.localPosition = new Vector3(sparkleSpelunk.minerHands.transform.localPosition.x, -0.709f, sparkleSpelunk.minerHands.transform.localPosition.z);
            }

            sparkleSpelunk.MinerHit();
            MotherTimerManager.instance.TimePenalty(sparkleSpelunk.timePenalty);
        }
    }
}
