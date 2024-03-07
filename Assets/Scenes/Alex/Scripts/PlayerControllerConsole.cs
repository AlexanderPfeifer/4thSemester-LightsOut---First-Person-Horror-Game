using System;
using UnityEngine;

public class PlayerControllerConsole : MonoBehaviour
{
    [SerializeField] private LayerMask starLayer;
    public static PlayerControllerConsole Instance;
    private Transform starGameObject;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && starGameObject != null)
        {
            starGameObject.gameObject.GetComponent<StarBehaviourConsole>().StarBounceUp();
        }
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
