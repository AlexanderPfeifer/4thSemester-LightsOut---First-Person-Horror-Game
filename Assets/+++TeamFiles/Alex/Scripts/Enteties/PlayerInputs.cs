using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("MouseInput")]
    [SerializeField] private float mouseSensitivity = .85f;
    private Vector2 mousePosition;
    [HideInInspector] public bool isCaught;

    [Header("Camera")]
    public CinemachineVirtualCamera vCam;
    private const float CamSensitivity = 10f;
    private float currentVisibility;
    [SerializeField] private float clampVisibilityOutOfHand = 80;
    [SerializeField] private float clampVisibilityInHand = 20;

    [Header("SelectableObjects")]
    public HoldObjectState holdObjectState = HoldObjectState.InHand;
    public GameObject interactableObject;
    [SerializeField] private LayerMask interactableLayerMask;
    public GameObject[] games;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public enum HoldObjectState
    {
        InHand,
        LayingDown,
        LiftingUp,
        OutOfHand
    }

    private void Update()
    {
        LookAround();
        
        PutDownInteractable();
        
        SelectInteractable();
    }

    //Here I made a method which rotates the player according to the mouse movement. I also clamped it so the player cannot rotate around itself
    private void LookAround()
    {
        if(isCaught || holdObjectState == HoldObjectState.LayingDown || holdObjectState == HoldObjectState.LiftingUp)
            return;

        currentVisibility = holdObjectState == HoldObjectState.OutOfHand ? clampVisibilityOutOfHand : clampVisibilityInHand;
        
        mousePosition.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        mousePosition.x += Input.GetAxis("Mouse Y") * -mouseSensitivity;
            
        var cameraLocalRotation = vCam.transform.localRotation;
        var clampValueX = Mathf.Clamp(mousePosition.x, -currentVisibility, currentVisibility);
        var clampValueY = Mathf.Clamp(mousePosition.y, -currentVisibility, currentVisibility);
        var cameraRotation = Quaternion.Euler(clampValueX, clampValueY, 0);
        cameraLocalRotation = Quaternion.Lerp(cameraLocalRotation, cameraRotation,CamSensitivity * Time.deltaTime);
        vCam.transform.localRotation = cameraLocalRotation;
    }

    private void SelectInteractable()
    {
        if (holdObjectState == HoldObjectState.OutOfHand)
        {
            SelectInteractableInLookDir();
            
            if (!Input.GetMouseButtonDown(0)) 
                return;

            if (interactableObject != null)
            {
                if (!isCaught)
                {
                    InteractWithInteractable();
                }
                else
                {
                    SelectVisual(false);
                }
            }
        }
    }

    private void SelectInteractableInLookDir()
    {
        if (Physics.Raycast(vCam.transform.position, vCam.transform.forward, out var raycastHit, float.MaxValue, interactableLayerMask))
        {
            interactableObject = raycastHit.collider.gameObject;
            if (interactableObject.TryGetComponent(out Interaction interaction))
            {
                SelectVisual(true);
            }
        }
        else
        {
            if (interactableObject != null)
            {
                SelectVisual(false);
                interactableObject = null;   
            }
        }
    }
    
    private void SelectVisual(bool selected)
    {
        interactableObject.GetComponentInParent<Transform>().GetChild(0).gameObject.SetActive(selected);
    }

    private void InteractWithInteractable()
    {
        SelectVisual(false);

        holdObjectState = HoldObjectState.LiftingUp;

        StartCoroutine(TakeInteractable());
    }

    private IEnumerator TakeInteractable()
    {
        while (Vector3.Distance(interactableObject.transform.position, interactableObject.GetComponent<Interaction>().interactableObjectInHandPosition) > 0.01f)
        {
            vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.localRotation, Quaternion.Euler(0, 0, 0),interactableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            mousePosition = new Vector2(0, 0);
            
            interactableObject.GetComponent<Interaction>().TakeInteractableObject(interactableObject);

            if (interactableObject.TryGetComponent(out IChoosableGame iChoosableGame))
            {
                foreach (var currentGame in games)
                {
                    currentGame.SetActive(false);
                }
                
                iChoosableGame.OpenGame();
            }

            yield return null;
        }
        
        holdObjectState = HoldObjectState.InHand;
    }
    
    private void PutDownInteractable()
    {
        if (!Input.GetKeyDown(KeyCode.Tab) || holdObjectState is not HoldObjectState.InHand || isCaught) 
            return;
        
        StartCoroutine(PutDownInteractableCoroutine());
    }
    
    private IEnumerator PutDownInteractableCoroutine()
    {
        interactableObject.GetComponent<Interaction>().AssignPutDownPos();

        while (Vector3.Distance(interactableObject.transform.position, interactableObject.GetComponent<Interaction>().interactableObjectPutAwayPosition) > 0.01f)
        {
            var lookPos = interactableObject.transform.position - vCam.transform.position;
            lookPos.z = 0;
            var rotation = Quaternion.LookRotation(lookPos, vCam.transform.up);
            vCam.transform.rotation = Quaternion.Slerp(vCam.transform.rotation, rotation, Time.deltaTime);

            Transform camTransform;
            (camTransform = vCam.transform).localRotation = Quaternion.Lerp(vCam.transform.localRotation, Quaternion.Euler(vCam.transform.position - interactableObject.transform.position),interactableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            mousePosition = camTransform.eulerAngles;

            holdObjectState = HoldObjectState.LayingDown;
            
            interactableObject.GetComponent<Interaction>().PutDownInteractableObject(interactableObject);
            
            yield return null;
        }

        holdObjectState = HoldObjectState.OutOfHand;
        yield return null;
    }
}
