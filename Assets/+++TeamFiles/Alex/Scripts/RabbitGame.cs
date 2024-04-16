using UnityEngine;

public class RabbitGame : MonoBehaviour
{
    [SerializeField] private LayerMask starLayer;
    private Transform starGameObject;
    [SerializeField] private Rigidbody carrotRb;
    [SerializeField] private Rigidbody rabbitRb;
    [SerializeField] private float bounceForce = 15f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && starGameObject != null)
        {
            CarrotBounceUp();
        }

        if (FindObjectOfType<PlayerInputs>().holdObjectState == PlayerInputs.HoldObjectState.ConsoleInHand)
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
        UIScoreCounter.Instance.gameScore++;
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
}
