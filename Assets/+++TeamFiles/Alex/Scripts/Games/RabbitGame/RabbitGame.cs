using UnityEngine;

public class RabbitGame : Interaction
{
    [SerializeField] private LayerMask carrotLayer;
    private Transform carrot;
    [SerializeField] private Rigidbody carrotRb;
    [SerializeField] private Rigidbody rabbitRb;
    [SerializeField] private float bounceForce = 15f;
    private PlayerInputs playerInputs;

    private void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();
    }

    private void Update()
    {
        CarrotBounceUp();
        
        FreezeGame();
    }

    //When W is pressed and the carrot is in range of the rabbit, it bounces up and adds points to the score
    private void CarrotBounceUp()
    {
        if (Input.GetKeyDown(KeyCode.W) && carrot != null)
        {
            carrotRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        
            UIScoreCounter.instance.AddPointsToScore();   
        }
    }
    
    //Because the game works with gravity, when it is put away, the objects need to freeze, so they do not fall through the console
    private void FreezeGame()
    {
        if (playerInputs.holdObjectState == PlayerInputs.HoldObjectState.InHand && playerInputs.interactableObject.TryGetComponent(out Console console))
        {
            carrotRb.constraints = ~RigidbodyConstraints.FreezePositionY;
            rabbitRb.constraints = ~RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            carrotRb.constraints = RigidbodyConstraints.FreezeAll;
            rabbitRb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    //Assigns the carrot so it can jump up
    private void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer) == carrotLayer.value)
        {
            carrot = col.gameObject.GetComponent<Transform>();
        }
    }

    //Sets carrot to null, so it cannot jump the whole time
    private void OnTriggerExit(Collider col)
    {
        if ((1 << col.gameObject.layer) == carrotLayer.value)
        {
            carrot = null;
        }
    }
}
