using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotatedSpeed = 5.0f;
    public float rotatedSmoothTime = 0.12f;
    public float dstFromTarget = 3f;
    public Vector2 verticalMinMax = new Vector2(-30, 60);

    [HideInInspector]
    public bool isLock = false;
    private Vector3 rotationSmoothVelocity, currentRotation;
    private float horizontal, vertical;


    private void Start()
    {
        currentRotation = transform.rotation.eulerAngles;

        horizontal = transform.rotation.eulerAngles.y;
        vertical = transform.rotation.eulerAngles.x;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        //if(!Cursor.visible)
        //{
        //    CameraMovement();
        //}
        CameraMovement();

        if (Cursor.visible == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        toggleMouse();
    }

    private void CameraMovement()
    {
        horizontal += Input.GetAxis("Mouse X") * rotatedSpeed;
        vertical -= Input.GetAxis("Mouse Y") * rotatedSpeed;
        vertical = Mathf.Clamp(vertical, verticalMinMax.x, verticalMinMax.y);

        // only rotate when mouse is not visible
        if (!Cursor.visible)
        {
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(vertical, horizontal), ref rotationSmoothVelocity, rotatedSmoothTime);
            this.transform.eulerAngles = currentRotation;
        }

        this.transform.position = target.position - this.transform.forward * dstFromTarget;
    }

    private void toggleMouse()
    {
        // press left alt to toggle the mouse to interact with games
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Debug.Log("toggleMouse");
            isLock = !isLock;
            Cursor.visible = !Cursor.visible;
            //if (Cursor.lockState == CursorLockMode.Locked)
            //    Cursor.lockState = CursorLockMode.Confined;
            //else
            //    Cursor.lockState = CursorLockMode.Locked;
        }        
    }
}
