﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotatedSpeed = 5.0f;
    public float rotatedSmoothTime = 0.12f;
    public float dstFromTarget = 3f;
    public Vector2 verticalMinMax = new Vector2(-30, 60);
    public float keyboardRotatedSpeed = 50.0f;


    public bool isLock = true;
    [HideInInspector]
    //public bool isLock = false;
    private Vector3 rotationSmoothVelocity, currentRotation;
    private float horizontal, vertical;

    public bool postPuzzleLock = false;


    private void Start()
    {
        currentRotation = transform.rotation.eulerAngles;

        horizontal = transform.rotation.eulerAngles.y;
        vertical = transform.rotation.eulerAngles.x;

        isLock = true;
        postPuzzleLock = false;
        rotatedSpeed = 0.4f;
    }

    private void LateUpdate()
    {
        if (isLock)
        {
            MouseMovement();
        }
        CameraMovement();

        toggleMouse();
    }

    public void MouseMovement()
    {
        horizontal += Input.GetAxis("Mouse X") * rotatedSpeed;
        vertical -= Input.GetAxis("Mouse Y") * rotatedSpeed;
        vertical = Mathf.Clamp(vertical, verticalMinMax.x, verticalMinMax.y);
    }

    private void CameraMovement()
    {
        if(Input.GetKey(KeyCode.J))
        {
            horizontal -= Time.deltaTime * keyboardRotatedSpeed;
        }
        if(Input.GetKey(KeyCode.L))
        {
            horizontal += Time.deltaTime * keyboardRotatedSpeed;
        }
        if(Input.GetKey(KeyCode.I))
        {
            vertical -= Time.deltaTime * keyboardRotatedSpeed;
        }
        if(Input.GetKey(KeyCode.K))
        {
            vertical += Time.deltaTime * keyboardRotatedSpeed;
        }


        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(vertical, horizontal), ref rotationSmoothVelocity, rotatedSmoothTime);
        this.transform.eulerAngles = currentRotation;


        this.transform.position = target.position - this.transform.forward * dstFromTarget;
    }

    private void toggleMouse()
    {
        // press tab to toggle the mouse to interact with games
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("mouse hidden:" + isLock);
            isLock = !isLock;
        }

        //if (DialogueManager.isPuzzleLock && newScene == true)
        //{
        //    isLock = false;
        //    newScene = false;
        //}

        if ((DialogueManager.isInDialogue || DialogueManager.isPuzzleLock) && postPuzzleLock == false)
        {
            //Debug.Log("free mouse");
            GameRoot.instance.IsLock(true);
            isLock = false;          
        }

        if (isLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
