using UnityEngine;

public class PlayerController : MonoBehaviour
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
    private bool putAwayConsole;

    private void Start()
    {
        var position = console.transform.position;
        consoleStartPos = position;
        consolePutAwayPos =  new Vector3(position.x, position.y - 4, position.z);
        
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        MouseRotation();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            putAwayConsole = !putAwayConsole;
        }

        CloseConsole();
    }
    
    //Here I made a method which rotates the player according to the mouse movement. I also clamped it so the player cannot rotate around itself
    private void MouseRotation()
    {
        turn.y += Input.GetAxis("Mouse X") * sensitivity;
        turn.x += Input.GetAxis("Mouse Y") * -sensitivity;

        var cameraRotation = mainCam.transform.localRotation;
        cameraRotation = Quaternion.Lerp(cameraRotation, CameraRotation(turn.x, turn.y),Acceleration * Time.deltaTime);
        mainCam.transform.localRotation = cameraRotation;
    }

    private float CameraClamp(float currentClampValue)
    {
        var clampValue = Mathf.Clamp(currentClampValue, -80, 80);
        return clampValue;
    }

    private Quaternion CameraRotation(float x, float y)
    {
        var cameraRotation = Quaternion.Euler(CameraClamp(x), CameraClamp(y), 0);
        return cameraRotation;
    }

    private void CloseConsole()
    {
        if (putAwayConsole)
        {
            console.transform.position = Vector3.Lerp(console.transform.position, consolePutAwayPos, Time.deltaTime * 4);
        }
        
        if(!putAwayConsole)
        {
            console.transform.position = Vector3.Lerp(console.transform.position, consoleStartPos, Time.deltaTime * 4);
        }
    }
}
