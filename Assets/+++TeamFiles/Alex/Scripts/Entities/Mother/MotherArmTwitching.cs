using UnityEngine;

public class MotherArmTwitching : MonoBehaviour
{
    private Animator anim;
    private float randomTimeUntilArmTwitching;

    private void Start()
    {
        anim = GetComponent<Animator>();
        randomTimeUntilArmTwitching = Random.Range(9, 12);
    }

    private void Update()
    {
        ArmTwitchingCountDownUpdate();
    }

    //counting down the time until the arm is twitching and resets the time
    private void ArmTwitchingCountDownUpdate()
    {
        randomTimeUntilArmTwitching -= Time.deltaTime;
        
        if (randomTimeUntilArmTwitching <= 0)
        {
            Twitching();
            randomTimeUntilArmTwitching = Random.Range(9, 12);
        }
    }

    //Triggers a random twitching animation
    private void Twitching()
    {
        var randomTrigger = Random.Range(0, 2);
        
        switch (randomTrigger)
        {
            case 0 :
                anim.SetTrigger("Twitching");
                break;
            case 1 :
                anim.SetTrigger("TwitchingOne");
                break;
            case 2 :
                anim.SetTrigger("TwitchingTwo");
                break;
        }
    }
}
