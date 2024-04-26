using UnityEngine;

public class Console : MonoBehaviour
{
    [HideInInspector] public Vector3 consolePutAwayPos;

    private void Awake()
    {
        var position = transform.position;
        consolePutAwayPos =  new Vector3(position.x + 3, position.y - 3, position.z + 3);
    }
}
