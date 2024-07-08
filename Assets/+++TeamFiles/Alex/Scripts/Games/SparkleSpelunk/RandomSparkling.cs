using UnityEngine;

public class RandomSparkling : MonoBehaviour
{
    private Animator anim;
    private float time;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    private bool animationPlaying = true;

    private void Start()
    {
        time = Random.Range(0, 4);
        anim = GetComponent<Animator>();
    }

    private void Update() => RandomSparklingAnimation();
    
    //Sets random time value on end of sparkling animation
    private void RandomValue() => time = Random.Range(minTime, maxTime);

    //Checks if animation is playing
    private void StartTimer() => animationPlaying = true;

    //Play sparkling animation
    private void RandomSparklingAnimation()
    {
        if(!animationPlaying)
            return;
        
        time -= Time.deltaTime;

        if (time <= 0)
        {
            anim.SetTrigger("Sparkle");
            animationPlaying = false;
        }
    }
}
