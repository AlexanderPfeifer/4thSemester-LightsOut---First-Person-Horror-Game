using System;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    [SerializeField] private LayerMask floorLayer;
    private Vector3 respawnPoint;

    private void Start() => respawnPoint = transform.position;
    
    private void OnTriggerEnter(Collider col)
    {
        if ((1 << col.gameObject.layer) == floorLayer.value)
        {
            transform.position = respawnPoint;
            FindObjectOfType<RabbitGame>().combo = 1;
            FindObjectOfType<RabbitGame>().counterUntilMultiply = 0;
        }
    }
}
