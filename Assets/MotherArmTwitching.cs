using UnityEngine;

public class MotherArmTwitching : MonoBehaviour
{
    private Animator anim;
    private float randomTime;

    private void Start()
    {
        randomTime = Random.Range(3, 7);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        randomTime -= Time.deltaTime;
        
        if (randomTime <= 0)
        {
            Twitching();
            randomTime = Random.Range(3, 7);
        }
    }

    private void Twitching()
    {
        anim.SetTrigger("Twitching");
    }
}
