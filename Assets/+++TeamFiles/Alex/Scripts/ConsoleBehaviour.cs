using UnityEngine;

public class ConsoleBehaviour : MonoBehaviour
{
    [HideInInspector] public Vector3 consolePutAwayPos;
    
    private void Start()
    {
        var position = transform.position;
        consolePutAwayPos =  new Vector3(position.x, position.y - 3, position.z + 3);
    }
}
