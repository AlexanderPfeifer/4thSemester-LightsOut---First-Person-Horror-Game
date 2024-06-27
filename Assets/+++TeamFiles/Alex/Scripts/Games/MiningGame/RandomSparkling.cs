using UnityEngine;

public class RandomSparkling : MonoBehaviour
{
    private Animator anim;
    private float time;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    private bool timerRunning = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        time = Random.Range(0, 4);
    }

    private void Update()
    {
        RandomSparklingAnimation();
    }

    private void RandomValue()
    {
        time = Random.Range(minTime, maxTime);
    }

    private void StartTimer()
    {
        timerRunning = true;
    }

    private void RandomSparklingAnimation()
    {
        if(!timerRunning)
            return;
        
        time -= Time.deltaTime;

        if (time <= 0)
        {
            anim.SetTrigger("Sparkle");
            timerRunning = false;
        }
    }
}
