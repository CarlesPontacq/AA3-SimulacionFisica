using QuaternionUtility;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Transform cameraPivot;

    public float mouseSensitivity = 1000f;
    public float minY = -35f;
    public float maxY = 60f;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = MathFUtils.ClampValue(xRotation, minY, maxY);
        
        QuaternionUtils eulerRotation = new QuaternionUtils();
         eulerRotation = eulerRotation.Euler(new VectorUtils3D(xRotation, 0f, 0f));
        
        cameraPivot.localRotation = eulerRotation.GetAsUnityQuaternion();

        player.Rotate(VectorUtils3D.up.GetAsUnityVector() * mouseX);
    }
}
