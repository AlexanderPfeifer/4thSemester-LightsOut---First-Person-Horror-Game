using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MouseCamera")]
    private Vector2 turn;
    private const float Acceleration = 10f;
    private float finalAcc;
    [SerializeField] private float sensitivity = .85f;
    private Camera mainCam;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = FindObjectOfType<Camera>();
    }

    void Update()
    {
        MouseRotation();
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
}
