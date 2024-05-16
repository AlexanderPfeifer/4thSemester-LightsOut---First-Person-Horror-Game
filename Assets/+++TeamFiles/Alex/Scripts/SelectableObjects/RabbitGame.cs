using UnityEngine;

public class RabbitGame : Interaction
{
    [SerializeField] private LayerMask carrotLayer;
    [SerializeField] private LayerMask floorLayer;
    private Transform carrot;
    [SerializeField] private Rigidbody carrotRb;
    [SerializeField] private Rigidbody rabbitRb;
    [SerializeField] private float bounceForce = 15f;
    private PlayerInputs playerInputs;
    public int combo = 1;
    public int counterUntilMultiply;
    
    private void Start()
    {
        playerInputs = FindObjectOfType<PlayerInputs>();
    }

    private void Update()
    {
        CarrotJump();
        
        FreezeGame();
    }

    private void CarrotJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && carrot != null)
        {
            CarrotBounceUp();
        }
    }

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
    
    private void CarrotBounceUp()
    {
        carrotRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        UIScoreCounter.instance.gameScore += combo;

        counterUntilMultiply++;

        if (counterUntilMultiply >= 5)
        {
            combo++;
            counterUntilMultiply = 0;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer) == carrotLayer.value)
        {
            carrot = col.gameObject.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if ((1 << col.gameObject.layer) == carrotLayer.value)
        {
            carrot = null;
        }
    }

    public override void TakeInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactableObjectInHandPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * interactableObjectPutAwaySpeed);
        interactableObjectPutAwayPosition = transform.position;
    }
    
    public void OpenGame()
    {
        //Open whatever
    }
}
