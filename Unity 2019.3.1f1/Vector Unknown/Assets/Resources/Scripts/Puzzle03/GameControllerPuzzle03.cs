using System;
using UnityEngine;

public class GameControllerPuzzle03 : GameControllerRoot
{
    public Transform plane;

    public float rotatedSpeed = 1f;

    [HideInInspector]
    public Puzzle03Window P03W;
    [HideInInspector]
    public DatabasePuzzle03 DBP03;
    [HideInInspector]
    public bool isChoosing = false;
    [HideInInspector]
    public float rx, ry, rz;
    [HideInInspector]
    public Vector3 finalRotation;
    private Vector3 currentRotation;
    private Vector3 diffRotation;
    private bool isRotate = false;

    public override void InitGameController(Puzzle03Window P03W)
    {
        Debug.Log("Init GameController Puzzle03");
        base.InitGameController();

        Debug.Log("Connect Puzzle03 Window");
        this.P03W = P03W;

        Debug.Log("Connect Database of Puzzle03");
        DBP03 = GetComponent<DatabasePuzzle03>();

        Debug.Log("Call Database of Puzzle03 to connect");
        DBP03.InitDatabase();

        //Below is the custom functions
    }

    private void Update()
    {
        if(isChoosing && Input.GetKeyDown(KeyCode.E))
        {
            GameRoot.ShowTips("The island is rotating according to the span... Wow....", true, true);

            TriggerRotation();
        }

        if(isRotate)
        {
            if (Mathf.Abs(diffRotation.x) <= 0.5f && Mathf.Abs(diffRotation.y) <= 0.5f && Mathf.Abs(diffRotation.z) <= 0.5f)
            {
                plane.transform.eulerAngles = finalRotation;
                isRotate = false;
            }
            else
            {
                CalDiffRotation();

                currentRotation = Vector3.Slerp(currentRotation, finalRotation, 0.01f * rotatedSpeed);

                plane.transform.eulerAngles = currentRotation;
            }
        }
    }

    private void CalDiffRotation()
    {
        currentRotation = plane.transform.eulerAngles;

        currentRotation.x += currentRotation.x > 180f ? -360f : 0f;
        currentRotation.y += currentRotation.y > 180f ? -360f : 0f;
        currentRotation.z += currentRotation.z > 180f ? -360f : 0f;

        diffRotation = finalRotation - currentRotation;
    }

    private void TriggerRotation()
    {
        finalRotation = new Vector3(rx, ry, rz);
        
        CalDiffRotation();

        isRotate = true;
    }
}
