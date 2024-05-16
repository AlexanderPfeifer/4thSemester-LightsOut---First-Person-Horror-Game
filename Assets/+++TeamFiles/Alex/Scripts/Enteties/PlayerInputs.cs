using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInputs : MonoBehaviour
{
    [Header("MouseInput")]
    [SerializeField] private float mouseSensitivity = .85f;
    private Vector2 mousePosition;
    [FormerlySerializedAs("canMove")] [FormerlySerializedAs("gotCaught")] [HideInInspector] public bool isCaught;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera vCam;
    private const float CamSensitivity = 10f;

    [Header("SelectableObjects")]
    public HoldObjectState holdObjectState = HoldObjectState.InHand;
    public GameObject interactableObject;
    [SerializeField] private LayerMask interactableLayerMask;

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
        
        PutDownInteractable();
        
        SelectInteractable();
    }

    //Here I made a method which rotates the player according to the mouse movement. I also clamped it so the player cannot rotate around itself
    private void LookAround()
    {
        if (holdObjectState is not (HoldObjectState.OutOfHand or HoldObjectState.LayingDown) || isCaught)
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

    private void SelectInteractable()
    {
        if (holdObjectState == HoldObjectState.OutOfHand)
        {
            SelectInteractableInLookDir();
            
            if (!Input.GetMouseButtonDown(0)) 
                return;
        
            if (interactableObject != null)
            {
                InteractWithInteractable();
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
            
            interactableObject.GetComponent<Interaction>().TakeInteractableObject(interactableObject);

            if (interactableObject.TryGetComponent(out IChoosableGame iChoosableGame))
            {
                iChoosableGame.OpenGame();
            }

            yield return null;
        }
        
        holdObjectState = HoldObjectState.InHand;
    }
    
    private void PutDownInteractable()
    {
        if (!Input.GetKeyDown(KeyCode.Tab) || holdObjectState is not HoldObjectState.InHand) 
            return;
        
        StartCoroutine(PutDownInteractableCoroutine());
    }
    
    private IEnumerator PutDownInteractableCoroutine()
    {
        interactableObject.GetComponent<Interaction>().AssignPutDownPos();

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
