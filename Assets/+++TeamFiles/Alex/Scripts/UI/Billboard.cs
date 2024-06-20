using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

    private void LateUpdate()
    {
        Vector3 camPos = mainCam.transform.position;

        camPos.y = transform.position.y;
        
        transform.LookAt(camPos);
        
        transform.Rotate(0,180,0);
    }
}
