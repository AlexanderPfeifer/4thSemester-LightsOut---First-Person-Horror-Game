using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("MouseInput")]
    [SerializeField] private float mouseSensitivity = .85f;
    private Vector2 mousePosition;
    [HideInInspector] public bool canInteract;

    [Header("Camera")]
    public CinemachineVirtualCamera vCam;
    private const float CamSensitivity = 10f;
    private float currentVisibilityAngle;
    private const float ClampVisibilityOutOfHand = 50;
    private const float ClampVisibilityInHand = 30;
    [SerializeField] private GameObject hudCanvas;

    [Header("SelectableObjects")]
    public HoldObjectState holdObjectState = HoldObjectState.InHand;
    public GameObject currentInteractableObject;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private Animator consoleAnim;

    public static PlayerInputs instance;

    private void Awake() => instance = this;

    //Locks the cursor and makes it invisible
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(ResetHudUi());
        canInteract = true;
    }
    
    private IEnumerator ResetHudUi()
    {
        while (true)
        {
            yield return new WaitForSeconds(30);
            hudCanvas.SetActive(false);
            hudCanvas.SetActive(true);
            yield return null;
        }
    }

    //An enum of all the states holding there are
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
        
        SelectInteractableInLookDir();
    }

    public void PlayChildAggressiveAnimation()
    {
        consoleAnim.SetTrigger("aggression");
    }
    
    //Here I made a method which rotates the player according to the mouse movement. I also clamped it so the player cannot rotate around itself
    private void LookAround()
    {
        if (!canInteract || holdObjectState == HoldObjectState.LayingDown || holdObjectState == HoldObjectState.LiftingUp)
            return;
        
        currentVisibilityAngle = holdObjectState == HoldObjectState.OutOfHand ? ClampVisibilityOutOfHand : ClampVisibilityInHand;
        
        mousePosition.y += Input.GetAxis("Mouse X") * mouseSensitivity;
        mousePosition.x += Input.GetAxis("Mouse Y") * -mouseSensitivity;

        mousePosition.y = Mathf.Clamp(mousePosition.y, -currentVisibilityAngle, currentVisibilityAngle);
        mousePosition.x = Mathf.Clamp(mousePosition.x, -currentVisibilityAngle, currentVisibilityAngle);

        var cameraRotation = Quaternion.Euler(mousePosition.x, mousePosition.y, 0);
        vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.localRotation, cameraRotation, CamSensitivity * Time.deltaTime);;
    }

    //When hovering over an object, it activates another object that indicates, that it is selected
    private void SelectInteractableInLookDir()
    {
        if (!canInteract || holdObjectState == HoldObjectState.LayingDown || holdObjectState == HoldObjectState.LiftingUp)
        {
            SelectVisual(false);
            return;   
        }

        if (holdObjectState == HoldObjectState.OutOfHand)
        {
            if (Physics.Raycast(vCam.transform.position, vCam.transform.forward, out var raycastHit, float.MaxValue, interactableLayerMask))
            {
                currentInteractableObject = raycastHit.collider.gameObject;

                if (FindObjectOfType<StartMemScape>() != null && !FindObjectOfType<StartMemScape>().canInteractWithConsole)
                {
                    if (!currentInteractableObject.TryGetComponent(out Console console))
                    {
                        if (currentInteractableObject.TryGetComponent(out Interaction interaction)) 
                        {
                            SelectVisual(true);
                        }
                
                        InteractWithInteractable();
                    }   
                }
                else
                {
                    if (currentInteractableObject.TryGetComponent(out Interaction interaction)) 
                    {
                        SelectVisual(true);
                    }
                
                    InteractWithInteractable();
                }
            }
            else
            {
                SelectVisual(false);
            }
        }
    }
    
    //Shortcut to set the selected visual to the bool parameter
    private void SelectVisual(bool selected)
    {
        if(currentInteractableObject == null)
            return;
        
        if (currentInteractableObject.activeSelf)
        {
            currentInteractableObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(selected);
        }
    }

    //Interacts with object when clicked on it(takes it to hold position)
    private void InteractWithInteractable()
    {
        if ((!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.E)) || currentInteractableObject == null) 
            return;

        if (canInteract)
        {
            SelectVisual(false);

            holdObjectState = HoldObjectState.LiftingUp;

            StartCoroutine(TakeInteractable());
        }
    }

    //Picks up the interactable object
    private IEnumerator TakeInteractable()
    {
        if (FindObjectOfType<StartMemScape>() != null && !FindObjectOfType<StartMemScape>().canInteractWithConsole)
        {
            TextManager.Instance.ClearText();
        }
        
        while (Vector3.Distance(currentInteractableObject.transform.position, Vector3.zero) > 0.01f)
        {
            vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.localRotation, Quaternion.Euler(0, 0, 0),currentInteractableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);

            currentInteractableObject.GetComponent<Interaction>().TakeInteractableObject(currentInteractableObject);

            currentInteractableObject.GetComponent<Collider>().enabled = false;
            
            yield return null;
        }

        currentInteractableObject.transform.position = Vector3.zero;

        currentInteractableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed = 4;

        mousePosition = new Vector2(0, 0);

        holdObjectState = HoldObjectState.InHand;
    }
    
    //Puts Down the interactable, is as void so the Put Down coroutine works without stopping
    private void PutDownInteractable()
    {
        if (!Input.GetKeyDown(KeyCode.E) || holdObjectState is not HoldObjectState.InHand || !canInteract || currentInteractableObject == null || MotherTimerManager.instance.gameStarted) 
            return;
        
        StartCoroutine(PutDownInteractableCoroutine());
    }
    
    public IEnumerator PutDownInteractableCoroutine()
    {
        holdObjectState = HoldObjectState.LayingDown;

        currentInteractableObject.GetComponent<Interaction>().AssignPutDownPos();
        currentInteractableObject.GetComponent<Interaction>().AssignPutDownRot();
        
        while (Vector3.Distance(currentInteractableObject.transform.position, currentInteractableObject.GetComponent<Interaction>().interactablePutAwayPosition) > 0.01f)
        {
            Vector3 direction = currentInteractableObject.GetComponent<Interaction>().interactablePutAwayPosition - vCam.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
            vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.rotation, toRotation, currentInteractableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed * Time.deltaTime);
            
            currentInteractableObject.GetComponent<Interaction>().PutDownInteractableObject(currentInteractableObject);
        
            yield return null;
        }

        if (currentInteractableObject.TryGetComponent(out Book bookEnd) && FindObjectOfType<EndManager>())
        {
            FindObjectOfType<MotherTextManager>().EndMotherText();
        }
        
        if (FindObjectOfType<StartMemScape>() != null && !FindObjectOfType<StartMemScape>().canInteractWithConsole)
        {
            if (!currentInteractableObject.TryGetComponent(out Console console))
            {
                FindObjectOfType<StartMemScape>().OpenMemScape();
            }   
        }
        
        if (FindObjectOfType<StartPapanicePizza>() != null)
        {
            if (!currentInteractableObject.TryGetComponent(out Console console))
            {
                FindObjectOfType<StartPapanicePizza>().OpenPapanicePizza();
            }   
        }
        
        if (FindObjectOfType<StartSparkleSpelunk>() != null)
        {
            if (!currentInteractableObject.TryGetComponent(out Console console))
            {
                FindObjectOfType<StartSparkleSpelunk>().OpenSparkleSpelunk();
            }   
        }
        
        mousePosition.x = GetCamInspectorRotationX();
        mousePosition.y = GetCamInspectorRotationY();
        
        currentInteractableObject.GetComponent<Collider>().enabled = true;
        
        currentInteractableObject.GetComponent<Interaction>().interactableObjectPutAwaySpeed = 4;

        holdObjectState = HoldObjectState.OutOfHand;

        yield return null;
    }
    
    //The methods below are for a calculation of the camera where angle resets in script to 360 degrees instead of going to negative
    //I need that for calculate when the camera is facing when putting down an object
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
