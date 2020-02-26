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

    private void LateUpdate()
    {
        if(!isLock)
        {
            CameraMovement();
        }
    }

    private void CameraMovement()
    {
        horizontal += Input.GetAxis("Mouse X") * rotatedSpeed;
        vertical -= Input.GetAxis("Mouse Y") * rotatedSpeed;
        vertical = Mathf.Clamp(vertical, verticalMinMax.x, verticalMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(vertical, horizontal), ref rotationSmoothVelocity, rotatedSmoothTime);
        this.transform.eulerAngles = currentRotation;

        this.transform.position = target.position - this.transform.forward * dstFromTarget;
    }
}
