using UnityEngine;

public class MathBook : MonoBehaviour
{
    [HideInInspector] public Vector3 mathBookPutAwayPos;


    private void Start() => mathBookPutAwayPos = transform.position;
}
