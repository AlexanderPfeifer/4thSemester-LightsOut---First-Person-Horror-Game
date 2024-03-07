using UnityEngine;

public class StarBehaviourConsole : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float bounceForce;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StarBounceUp()
    {
        rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }
}
