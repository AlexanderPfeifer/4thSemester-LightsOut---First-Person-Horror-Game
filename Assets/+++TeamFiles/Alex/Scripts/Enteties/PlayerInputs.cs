using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInputs : MonoBehaviour
{
    [Header("MouseInput")]
    [SerializeField] private float mouseSensitivity = .85f;
    private Vector2 mousePosition;
    [HideInInspector] public bool gotCaught;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera vCam;
    private const float CamSensitivity = 10f;

    [Header("SelectableObjects")]
    private protected Volume consoleHoldVolume;
    public HoldObjectState holdObjectState = HoldObjectState.InHand;
    public GameObject interactableObject;

    public enum HoldObjectState
    {
        InHand,
        LayingDown,
        LiftingUp,
        OutOfHand
    }
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        LookAround();
        
        PutDownHoldingObject();
        
        SelectInteractableObject();
        
        Debug.Log(holdObjectState);
    }

    //Here I made a method which rotates the player according to the mouse movement. I also clamped it so the player cannot rotate around itself
    private void LookAround()
    {
        if (holdObjectState is not (HoldObjectState.OutOfHand or HoldObjectState.LayingDown) || gotCaught)
            return;
        
        mousePosition.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        mousePosition.x += Input.GetAxis("Mouse Y") * -mouseSensitivity;
            
        var cameraLocalRotation = vCam.transform.localRotation;
        var clampValueX = Mathf.Clamp(mousePosition.x, -80, 80);
        var clampValueY = Mathf.Clamp(mousePosition.y, -80, 80);
        var cameraRotation = Quaternion.Euler(clampValueX, clampValueY, 0);
        cameraLocalRotation = Quaternion.Lerp(cameraLocalRotation, cameraRotation,CamSensitivity * Time.deltaTime);
        vCam.transform.localRotation = cameraLocalRotation;
    }
    
    private void PutDownHoldingObject()
    {
        if (!Input.GetKeyDown(KeyCode.Tab) || holdObjectState is not HoldObjectState.InHand) 
            return;
        
        StartCoroutine(PutDownHoldingObjectCoroutine());
        //consoleHoldVolume.weight = 0;
    }

    private void SelectInteractableObject()
    {
        if (holdObjectState == HoldObjectState.OutOfHand)
        {
            CheckSelectableObjectInCameraDirection();
            
            if (!Input.GetMouseButtonDown(0)) 
                return;
        
            if (interactableObject != null)
            {
                InteractWithSelectedObject();
            }
        }
    }

    private void CheckSelectableObjectInCameraDirection()
    {
        if (Physics.Raycast(vCam.transform.position, vCam.transform.forward, out var raycastHit, float.MaxValue))
        {
            interactableObject = raycastHit.collider.gameObject;
            if (!interactableObject.TryGetComponent(out Interaction interaction)) 
                return;
            
            interactableObject = interaction.GetGameObject();
            SetIsSelectable(true);
        }
        else
        {
            if (interactableObject == null) 
                return;
            
            SetIsSelectable(false);
            interactableObject = null;
        }
    }

    private void InteractWithSelectedObject()
    {
        SetIsSelectable(false);

        holdObjectState = HoldObjectState.LiftingUp;

        StartCoroutine(CheckSelectedObjectDistance());
    }

    private void SetIsSelectable(bool selected)
    {
        interactableObject.transform.GetChild(0).gameObject.SetActive(selected);
    }

    private IEnumerator CheckSelectedObjectDistance()
    {
        while (Vector3.Distance(interactableObject.transform.position, interactableObject.GetComponent<Interaction>().interactableObjectInHandPosition) > 0.01f)
        {
            vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.localRotation, Quaternion.Euler(0, 0, 0),interactableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            
            //Take object

            yield return null;
        }
        
        holdObjectState = HoldObjectState.InHand;
    }
    
    private IEnumerator PutDownHoldingObjectCoroutine()
    {
        while (Vector3.Distance(interactableObject.transform.position, interactableObject.GetComponent<Interaction>().interactableObjectPutAwayPosition) > 0.01f)
        {
            holdObjectState = HoldObjectState.LayingDown;
            
            interactableObject.GetComponent<Interaction>().PutDownInteractableObject(interactableObject);
            
            yield return null;
        }

        holdObjectState = HoldObjectState.OutOfHand;
        yield return null;
    }
}
