using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerInputs : AbstractGameManager
{
    [Header("MouseInput")]
    [SerializeField] private float mouseSensitivity = .85f;
    private Vector2 mousePosition;
    [HideInInspector] public bool gotCaught;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera vCam;
    private const float CamSensitivity = 10f;

    [Header("SelectableObjects")]
    [HideInInspector] public bool canInteract = true;
    [SerializeField] private float interactableObjectPutAwaySpeed = 4;
    [SerializeField] private LayerMask[] selectableMasks;
    private Vector3 interactableObjectHoldPosition;
    public Transform interactableObject;
    protected Volume consoleHoldVolume;
    public HoldObjectState holdObjectState = HoldObjectState.InHand;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ObjectGotSelected();
    }

    private void Update()
    {
        LookAround();
        
        PutDownHoldingObject();
        
        SelectInteractableObject();
        
        CheckHoldObjectState();
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
        consoleHoldVolume.weight = 0;
        holdObjectState = HoldObjectState.LayingDown;
    }

    private void SelectInteractableObject()
    {
        if (holdObjectState != HoldObjectState.OutOfHand || !canInteract) 
            return;
        
        CheckSelectableObjectInCameraDirection();

        if (!Input.GetMouseButtonDown(0)) 
            return;
        
        if (interactableObject != null)
        {
            InteractWithSelectedObject();
        }
    }

    private void CheckSelectableObjectInCameraDirection()
    {
        foreach (var selectableMask in selectableMasks)
        {
            if (Physics.Raycast(vCam.transform.position, vCam.transform.forward, out var raycastHit, float.MaxValue, selectableMask))
            {
                interactableObject = raycastHit.transform;
                SetIsSelectable(true);
            }
            else
            {
                if (interactableObject != null)
                {
                    SetIsSelectable(false);
                    interactableObject = null;
                }
            }
        }
    }
    
    public enum HoldObjectState
    {
        InHand,
        LayingDown,
        LiftingUp,
        OutOfHand
    }
    
    private void CheckHoldObjectState()
    {
        switch (holdObjectState)
        {
            case HoldObjectState.InHand :
                
                break;
            case HoldObjectState.LayingDown :
                //consoleHoldingVolume.weight = 0;
                break;
            case HoldObjectState.LiftingUp :
                break;
            case HoldObjectState.OutOfHand :
                break;
        }
    }
    
    protected virtual void ObjectGotSelected()
    {
        holdObjectState = HoldObjectState.InHand;
        
        //selectableObject order is: Console, MathBook, PizzaGame, MiningGame, RabbitGame, MemorizeGame, 
        switch (selectableMasks.Rank)
        {
            case 0 :
                if(interactableObject.TryGetComponent(out Console console))
                    console.Selected();
                break;
            case 1 :
                if(interactableObject.TryGetComponent(out MathBook mathBook))
                    mathBook.Selected();
                break;
            case 2 :
                if(interactableObject.TryGetComponent(out PizzaGame pizzaGame))
                    pizzaGame.Selected();
                break;
            case 3 :
                if(interactableObject.TryGetComponent(out MiningGame miningGame))
                    miningGame.Selected();
                break;
            case 4 :
                if(interactableObject.TryGetComponent(out RabbitGame rabbitGame))
                    rabbitGame.Selected();
                break;
            case 5 :
                if(interactableObject.TryGetComponent(out MemorizeGame memorizeGame))
                    memorizeGame.Selected();
                break;
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
        interactableObject.GetChild(0).gameObject.SetActive(selected);
    }

    private IEnumerator CheckSelectedObjectDistance()
    {
        while (Vector3.Distance(interactableObject.position, interactableObjectHoldPosition) > 0.01f)
        {
            vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.localRotation, Quaternion.Euler(0, 0, 0),interactableObjectPutAwaySpeed * Time.deltaTime);
            
            interactableObject.localRotation = Quaternion.Lerp(interactableObject.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * interactableObjectPutAwaySpeed);
            interactableObject.position = Vector3.Lerp(interactableObject.position, interactableObjectHoldPosition, Time.deltaTime * interactableObjectPutAwaySpeed);

            yield return null;
        }
        
        ObjectGotSelected();
    }
    
    private IEnumerator PutDownHoldingObjectCoroutine()
    {
        while (Vector3.Distance(interactableObject.position, putAwayPosition) > 0.01f)
        {
            holdObjectState = HoldObjectState.LayingDown;
            interactableObject.position = Vector3.Lerp(interactableObject.position, putAwayPosition, Time.deltaTime * interactableObjectPutAwaySpeed);
            interactableObject.localRotation = Quaternion.Lerp(interactableObject.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * interactableObjectPutAwaySpeed);
            yield return null;
        }

        holdObjectState = HoldObjectState.OutOfHand;
        yield return null;
    }
}
