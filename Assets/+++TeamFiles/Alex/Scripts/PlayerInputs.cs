using System.Collections;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("MouseCamera")]
    [SerializeField] private float sensitivity = .85f;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject console;
    [SerializeField] private float consolePutAwaySpeed = 4;
    [SerializeField] private LayerMask selectableMask;

    private const float Acceleration = 10f;
    private float finalAcc;
    public bool consoleGameIsPaused;
    private bool noObjectInHand;
    private bool canMoveCam;
    private Vector2 turn;
    private Vector3 playerHoldPos;
    private Transform selectedGameObject;
    private Vector3 putAwayPos;

    private void Start()
    {
        selectedGameObject = console.transform;
        playerHoldPos = selectedGameObject.position;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MouseRotationForCamera();
        
        ConsoleBehaviour();
    }
    
    //Here I made a method which rotates the player according to the mouse movement. I also clamped it so the player cannot rotate around itself
    private void MouseRotationForCamera()
    {
        if (canMoveCam)
        {
            turn.y += Input.GetAxis("Mouse X") * sensitivity;
            turn.x += Input.GetAxis("Mouse Y") * -sensitivity;
            
            var cameraRotation = mainCam.transform.localRotation;
            cameraRotation = Quaternion.Lerp(cameraRotation, CameraRotation(turn.x, turn.y),Acceleration * Time.deltaTime);
            mainCam.transform.localRotation = cameraRotation;   
        }
    }

    private float CameraViewClamp(float currentClampValue)
    {
        var clampValue = Mathf.Clamp(currentClampValue, -80, 80);
        return clampValue;
    }

    private Quaternion CameraRotation(float x, float y)
    {
        var cameraRotation = Quaternion.Euler(CameraViewClamp(x), CameraViewClamp(y), 0);
        return cameraRotation;
    }

    private void ConsoleBehaviour()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(PutHoldingObjectDownCoroutine());
            consoleGameIsPaused = true;
        }

        if (noObjectInHand)
        {
            LookDirectionRaycastHit();
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedGameObject != null)
                {
                    StartCoroutine(LiftUpSelectedObjectUpCoroutine());
                }
            }
        }
    }

    private void LookDirectionRaycastHit()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out var raycastHit, float.MaxValue, selectableMask))
        {
            selectedGameObject = raycastHit.transform;
            selectedGameObject.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            if (selectedGameObject != null)
            {
                selectedGameObject.GetChild(0).gameObject.SetActive(false);
                selectedGameObject = null;
            }
        }
    }

    private IEnumerator PutHoldingObjectDownCoroutine()
    {
        if (selectedGameObject.TryGetComponent(out MathBookBehaviour mathBookBehaviour))
        {
            putAwayPos = mathBookBehaviour.mathBookPutAwayPos;
        }
        
        if (selectedGameObject.TryGetComponent(out ConsoleBehaviour consoleBehaviour))
        {
            putAwayPos = consoleBehaviour.consolePutAwayPos;
        }
        
        while (Vector3.Distance(selectedGameObject.position, putAwayPos) > 0.01f)
        {
            selectedGameObject.position = Vector3.Lerp(selectedGameObject.position, putAwayPos, Time.deltaTime * consolePutAwaySpeed);
            selectedGameObject.localRotation = Quaternion.Lerp(selectedGameObject.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * consolePutAwaySpeed);
            canMoveCam = true;
            yield return null;
        }

        if (Vector3.Distance(selectedGameObject.position, putAwayPos) <= 0.01f)
        {
            noObjectInHand = true;
            yield return null;
        }
    }
    
    private IEnumerator LiftUpSelectedObjectUpCoroutine()
    {
        noObjectInHand = false;
        selectedGameObject.GetChild(0).gameObject.SetActive(false);
        
        while (Vector3.Distance(selectedGameObject.position, playerHoldPos) > 0.01f)
        {
            mainCam.transform.localRotation = Quaternion.Lerp(mainCam.transform.localRotation, Quaternion.Euler(0, 0, 0),consolePutAwaySpeed * Time.deltaTime);
            
            selectedGameObject.localRotation = Quaternion.Lerp(selectedGameObject.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * consolePutAwaySpeed);
            selectedGameObject.position = Vector3.Lerp(selectedGameObject.position, playerHoldPos, Time.deltaTime * consolePutAwaySpeed);
            
            canMoveCam = false;
            yield return null;
        }

        if (Vector3.Distance(selectedGameObject.position, playerHoldPos) <= 0.01f)
        {
            if (selectedGameObject.TryGetComponent(out ConsoleBehaviour consoleBehaviour))
            {
                consoleGameIsPaused = false;
            }
            yield return null;
        }
    }
}
