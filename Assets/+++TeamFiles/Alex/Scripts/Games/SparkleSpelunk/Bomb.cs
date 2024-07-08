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

    private void Update() => BombDropped();

    //Drops the bomb on the place where it was when left click was released
    private void BombDropped()
    {
        if (!bombDropped)
        {
            transform.position = sparkleSpelunk.transform.GetChild(0).transform.position;
        }
    }

    #region Sounds

    //Plays first tick sound on animation
    public void FirstTickSound()
    {
        AudioManager.Instance.Play("SparkleSpelunkBombFirstTick");
    }    
    
    //Plays second tick sound on animation
    public void SecondTickSound()
    {
        AudioManager.Instance.Play("SparkleSpelunkBombSecondTick");
    }    
    
    //Plays third tick sound on animation
    public void ThirdTickSound()
    {
        AudioManager.Instance.Play("SparkleSpelunkBombThirdTick");
    }

    #endregion

    //Destroys the bomb and sets bool bomb is spawned to false
    private void DestroyBomb()
    {
        if (sparkleSpelunk.bombSpawned)
        {
            sparkleSpelunk.bombSpawned = false;
        }

        Destroy(gameObject);
    }
    
    //Checks whether a block or the player is in range and destroys/deals damage according the what is in range
    private void OnDestroy()
    {
        AudioManager.Instance.Play("SparkleSpelunkBombExplosion");
        
        if (Physics.Raycast(transform.position, -transform.up, out var blockHit, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.blockLayer))
        {
            sparkleSpelunk.allBlocks.Remove(blockHit.transform);
            Destroy(blockHit.transform.gameObject);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out var goldHit, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.goldLayer))
        {
            AudioManager.Instance.Play("SparkleSpelunkCollect");
            sparkleSpelunk.MinerEarned(sparkleSpelunk.minerEarnedSpriteHandGold);
            sparkleSpelunk.allBlocks.Remove(goldHit.transform);
            Destroy(goldHit.transform.gameObject);
            MotherTimerManager.Instance.TimeBonus(sparkleSpelunk.timeBonusGold);
        }
        else if (Physics.Raycast(transform.position, -transform.up, out var diamondHit, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.diamondLayer))
        {
            AudioManager.Instance.Play("SparkleSpelunkCollect");
            sparkleSpelunk.MinerEarned(sparkleSpelunk.minerEarnedSpriteHandDiamond);
            sparkleSpelunk.allBlocks.Remove(diamondHit.transform);
            Destroy(diamondHit.transform.gameObject);
            MotherTimerManager.Instance.TimeBonus(sparkleSpelunk.timeBonusDiamond);
        }
        
        if(Physics.Raycast(transform.position, -transform.up, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || Physics.Raycast(transform.position, -transform.right, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || Physics.Raycast(transform.position, transform.right, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer) || sparkleSpelunk.bombInHand ||  Physics.Raycast(transform.position, transform.position, sparkleSpelunk.block.transform.localScale.y, sparkleSpelunk.playerLayer))
        {
            if (!sparkleSpelunk.bombSpawned && sparkleSpelunk.bombInHand)
            {
                sparkleSpelunk.minerHead.sprite = sparkleSpelunk.minerIdleSpriteHead;
                sparkleSpelunk.minerHands.sprite = sparkleSpelunk.minerIdleSpriteHands;
                sparkleSpelunk.minerHands.transform.localPosition = new Vector3(sparkleSpelunk.minerHands.transform.localPosition.x, -0.709f, sparkleSpelunk.minerHands.transform.localPosition.z);
            }

            sparkleSpelunk.MinerHit();
            MotherTimerManager.Instance.TimePenalty(sparkleSpelunk.timePenalty);
        }
    }
}
