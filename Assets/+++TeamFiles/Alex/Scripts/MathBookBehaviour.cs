using UnityEngine;

public class MathBookBehaviour : MonoBehaviour
{
    [HideInInspector] public Vector3 mathBookPutAwayPos;

    private void Start()
    {
        mathBookPutAwayPos = transform.position;
    }
}
