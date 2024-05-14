using UnityEngine;

public class RabbitGame : Interaction, IGame
{
    [SerializeField] private LayerMask starLayer;
    private Transform starGameObject;
    [SerializeField] private Rigidbody carrotRb;
    [SerializeField] private Rigidbody rabbitRb;
    [SerializeField] private float bounceForce = 15f;
    private PlayerInputs playerInputs;

    private Vector3 putAwayPos;

    private void Start()
    {
        putAwayPos = transform.position;

        playerInputs = FindObjectOfType<PlayerInputs>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && starGameObject != null)
        {
            CarrotBounceUp();
        }

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
        UIScoreCounter.instance.gameScore++;
    }

    private void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer) == starLayer.value)
        {
            starGameObject = col.gameObject.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if ((1 << col.gameObject.layer) == starLayer.value)
        {
            starGameObject = null;
        }
    }

    public override void TakeInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, interactableObjectInHandPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * interactableObjectPutAwaySpeed);
        interactableObjectPutAwayPosition = transform.position;
        //consoleHoldVolume.weight = 1;
    }
    
    public override void PutDownInteractableObject(GameObject interactable)
    {
        interactable.transform.position = Vector3.Lerp(interactable.transform.position, putAwayPos, Time.deltaTime * interactableObjectPutAwaySpeed);
        interactable.transform.localRotation = Quaternion.Lerp(interactable.transform.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * interactableObjectPutAwaySpeed);
    }

    public override GameObject GetGameObject()
    {
        return gameObject;
    }

    public void OpenGame()
    {
        //Open whatever
    }
}
