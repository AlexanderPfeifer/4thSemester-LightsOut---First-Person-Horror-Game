using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("MouseCamera")]
    [SerializeField] private float sensitivity = .85f;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private GameObject console;
    [SerializeField] private float consolePutAwaySpeed = 4;
    [SerializeField] private LayerMask selectableMask;

    private const float Acceleration = 10f;
    private float finalAcc;
    private Vector2 turn;
    private Vector3 playerHoldPos;
    private Transform selectedGameObject;
    [SerializeField] private Vector3 putAwayPos;
    [SerializeField] private MotherBehaviour motherBehaviour;
    public HoldObjectState holdObjectState;

    public enum HoldObjectState
    {
        ConsoleInHand,
        InHand,
        LayingDown,
        LiftingUp,
        OutOfHand,
    }
    
    private void Start()
    {
        holdObjectState = HoldObjectState.ConsoleInHand;
        putAwayPos = console.GetComponent<Console>().consolePutAwayPos;
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
        if (holdObjectState is HoldObjectState.OutOfHand or HoldObjectState.LayingDown)
        {
            turn.y += Input.GetAxis("Mouse X") * sensitivity;
            turn.x += Input.GetAxis("Mouse Y") * -sensitivity;
            
            var cameraRotation = vCam.transform.localRotation;
            cameraRotation = Quaternion.Lerp(cameraRotation, CameraRotation(turn.x, turn.y),Acceleration * Time.deltaTime);
            vCam.transform.localRotation = cameraRotation;   
        }
    }

    private Quaternion CameraRotation(float x, float y)
    {
        var cameraRotation = Quaternion.Euler(CameraViewClamp(x), CameraViewClamp(y), 0);
        return cameraRotation;
    }
    
    private float CameraViewClamp(float currentClampValue)
    {
        var clampValue = Mathf.Clamp(currentClampValue, -80, 80);
        return clampValue;
    }

    private void ConsoleBehaviour()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && holdObjectState is HoldObjectState.InHand or HoldObjectState.ConsoleInHand)
        {
            StartCoroutine(PutDownHoldingObjectCoroutine());
            holdObjectState = HoldObjectState.LayingDown;
        }

        if (holdObjectState == HoldObjectState.OutOfHand)
        {
            CheckSelectableObjectInCameraDir();
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedGameObject != null)
                {
                    StartCoroutine(LiftUpSelectedObjectCoroutine());
                }
            }
        }
    }

    private void CheckSelectableObjectInCameraDir()
    {
        if (Physics.Raycast(vCam.transform.position, vCam.transform.forward, out var raycastHit, float.MaxValue, selectableMask))
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

    private IEnumerator LiftUpSelectedObjectCoroutine()
    {
        selectedGameObject.GetChild(0).gameObject.SetActive(false);

        holdObjectState = HoldObjectState.LiftingUp;

        while (Vector3.Distance(selectedGameObject.position, playerHoldPos) > 0.01f)
        {
            vCam.transform.localRotation = Quaternion.Lerp(vCam.transform.localRotation, Quaternion.Euler(0, 0, 0),consolePutAwaySpeed * Time.deltaTime);
            
            selectedGameObject.localRotation = Quaternion.Lerp(selectedGameObject.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * consolePutAwaySpeed);
            selectedGameObject.position = Vector3.Lerp(selectedGameObject.position, playerHoldPos, Time.deltaTime * consolePutAwaySpeed);
            
            yield return null;
        }

        if (Vector3.Distance(selectedGameObject.position, playerHoldPos) <= 0.01f)
        {
            if (selectedGameObject.TryGetComponent(out MathBook mathBookBehaviour))
            {
                holdObjectState = HoldObjectState.InHand;
                putAwayPos = mathBookBehaviour.mathBookPutAwayPos;
                motherBehaviour.ResetAttackScore();
            }
            else if(selectedGameObject.TryGetComponent(out Console consoleBehaviour))
            {
                holdObjectState = HoldObjectState.ConsoleInHand;
                putAwayPos = consoleBehaviour.consolePutAwayPos;
            }
            yield return null;
        }
    }
    
    private IEnumerator PutDownHoldingObjectCoroutine()
    {
        if (holdObjectState is HoldObjectState.InHand or HoldObjectState.ConsoleInHand)
        {
            while (Vector3.Distance(selectedGameObject.position, putAwayPos) > 0.01f)
            {
                holdObjectState = HoldObjectState.LayingDown;
                selectedGameObject.position = Vector3.Lerp(selectedGameObject.position, putAwayPos, Time.deltaTime * consolePutAwaySpeed);
                selectedGameObject.localRotation = Quaternion.Lerp(selectedGameObject.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * consolePutAwaySpeed);
                yield return null;
            }

            if (Vector3.Distance(selectedGameObject.position, putAwayPos) <= 0.01f)
            {
                holdObjectState = HoldObjectState.OutOfHand;
                yield return null;
            }
        }
    }
}
