using UnityEngine;

public class Carrot : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayer;
    private Vector3 respawnPoint;

    private void Start() => respawnPoint = transform.position;
    
    //Resets the combo streak when the carrot hits the floor
    private void OnTriggerExit(Collider col)
    {
        if ((1 << col.gameObject.layer) == floorLayer.value)
        {
            transform.position = respawnPoint;
        }
    }
}
