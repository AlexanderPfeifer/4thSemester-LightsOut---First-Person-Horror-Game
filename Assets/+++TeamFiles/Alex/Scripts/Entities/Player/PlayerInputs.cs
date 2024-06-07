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
    private float currentVisibilityAngle;
    [SerializeField] private float clampVisibilityOutOfHand = 80;
    [SerializeField] private float clampVisibilityInHand = 25;

    [Header("SelectableObjects")]
    public HoldObjectState holdObjectState = HoldObjectState.InHand;
    public GameObject interactableObject;
    [SerializeField] private LayerMask interactableLayerMask;
    public GameObject[] games;

    public static PlayerInputs instance;

    private void Awake() => instance = this;

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
        if (isCaught || holdObjectState == HoldObjectState.LayingDown || holdObjectState == HoldObjectState.LiftingUp)
            return;

        currentVisibilityAngle = holdObjectState == HoldObjectState.OutOfHand ? clampVisibilityOutOfHand : clampVisibilityInHand;
        
        mousePosition.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        mousePosition.x += Input.GetAxis("Mouse Y") * -mouseSensitivity;
            
        var clampValueX = Mathf.Clamp(mousePosition.x, -currentVisibilityAngle, currentVisibilityAngle);
        var clampValueY = Mathf.Clamp(mousePosition.y, -currentVisibilityAngle, currentVisibilityAngle);
        var cameraRotation = Quaternion.Euler(clampValueX, clampValueY, 0);
        vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.localRotation, cameraRotation, CamSensitivity * Time.deltaTime);;
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
        if (Physics.Raycast(vCam.transform.position, vCam.transform.forward, out var raycastHit, float.MaxValue, interactableLayerMask) && !isCaught)
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
                PutDownInteractable();
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

            interactableObject.GetComponent<Collider>().enabled = false;

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
        
        holdObjectState = HoldObjectState.LayingDown;

        StartCoroutine(PutDownInteractableCoroutine());
    }
    
    public IEnumerator PutDownInteractableCoroutine()
    {
        interactableObject.GetComponent<Interaction>().AssignPutDownPos();

        while (Vector3.Distance(interactableObject.transform.position, interactableObject.GetComponent<Interaction>().interactableObjectPutAwayPosition) > 0.01f)
        {
            Vector3 direction = interactableObject.GetComponent<Interaction>().interactableObjectPutAwayPosition - vCam.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.rotation, toRotation, interactableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            
            interactableObject.GetComponent<Interaction>().PutDownInteractableObject(interactableObject);
            
            yield return null;
        }
        mousePosition.x = GetCamInspectorRotationX();
        mousePosition.y = GetCamInspectorRotationY();
        
        interactableObject.GetComponent<Collider>().enabled = true;

        holdObjectState = HoldObjectState.OutOfHand;
        yield return null;
    }
    
    private float GetCamInspectorRotationX()
    {
        if(vCam.transform.eulerAngles.x > 180)
        {
            return vCam.transform.eulerAngles.x - 360;
        }
        
        return vCam.transform.eulerAngles.x;
    }
    
    private float GetCamInspectorRotationY()
    {
        if(vCam.transform.eulerAngles.y > 180)
        {
            return vCam.transform.eulerAngles.y - 360;
        }
        
        return vCam.transform.eulerAngles.y;
    }
}
