using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("MouseCamera")]
    private Vector2 turn;
    private const float Acceleration = 10f;
    private float finalAcc;
    [SerializeField] private float sensitivity = .85f;
    private Camera mainCam;
    [SerializeField] private GameObject console;
    private Vector3 consoleStartPos;
    private Vector3 consolePutAwayPos;
    [HideInInspector] public bool putAwayConsole;
    public bool gameIsPaused;
    [SerializeField] private float consolePutAwaySpeed = 4;
    private bool canMoveCam;

    private void Start()
    {
        var position = console.transform.position;
        consoleStartPos = position;
        consolePutAwayPos =  new Vector3(position.x, position.y - 3, position.z + 3);
        
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        MouseRotationForCamera();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            putAwayConsole = !putAwayConsole;
        }

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
        switch (putAwayConsole)
        {
            case true:
                console.transform.position = Vector3.Lerp(console.transform.position, consolePutAwayPos, Time.deltaTime * consolePutAwaySpeed);
                console.transform.localRotation = Quaternion.Lerp(console.transform.localRotation, Quaternion.Euler(0, 90, 90), Time.deltaTime * consolePutAwaySpeed);
                canMoveCam = true;
                break;
            case false:
                canMoveCam = false;
                mainCam.transform.localRotation = Quaternion.Lerp(mainCam.transform.localRotation, Quaternion.Euler(0, 0, 0),consolePutAwaySpeed * Time.deltaTime);
                console.transform.position = Vector3.Lerp(console.transform.position, consoleStartPos, Time.deltaTime * consolePutAwaySpeed);
                console.transform.localRotation = Quaternion.Lerp(console.transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * consolePutAwaySpeed);
                break;
        }

        gameIsPaused = !(Vector3.Distance(console.transform.position, consoleStartPos) < 0.1f);
    }
}
