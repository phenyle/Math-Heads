using System;
using UnityEngine;

public class GameControllerPuzzle03 : GameControllerRoot
{
    public Transform plane;
    public float rotatedSpeed = 1f;
    public Transform tipsPoint2, tipsPoint3;
    public float tipsAnimationSpeed = 0.01f;

    [HideInInspector]
    public Puzzle03Window P03W;
    [HideInInspector]
    public DatabasePuzzle03 DBP03;

    [HideInInspector]
    public bool isSelecting = false;
    [HideInInspector]
    public Vector3 spanValue = new Vector3();
    [HideInInspector]
    public int choiceID = 0;

    [HideInInspector]
    public Vector3 finalRotation;
    private Vector3 currentRotation;
    private Vector3 diffRotation;
    private bool isRotate = false;
    private bool isFinded = false;

    private Vector3 tipsPoint2FinalPos;
    private Vector3 tipsPoint3FinalPos;
    private bool isTipsAnimate = false;

    private static bool showP03_00 = true;

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

        if(showP03_00)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle03_00"));
            showP03_00 = false;
        }
    }

    private void Update()
    {
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

        //*********************** Interaction with the trigger and submission *************************
        if(isSelecting && Input.GetKeyDown(KeyCode.E))
        {
            SetSpanValue();
        }

        if(P03W.choiceID1 != 0 && P03W.choiceID2 != 0 && Input.GetKeyDown(KeyCode.Return))
        {
            P03W.ClickSubmitBtn();
        }

        if(P03W.choiceID1 != 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            P03W.ClickClearChoice1Btn();
        }

        if(P03W.choiceID2 != 0 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            P03W.ClickClearChoice2Btn();
        }
        // ***********************************************************************************************

        if(isTipsAnimate)
        {
            tipsPoint2.localPosition = Vector3.Lerp(tipsPoint2.localPosition, tipsPoint2FinalPos, tipsAnimationSpeed);
            tipsPoint3.localPosition = Vector3.Lerp(tipsPoint3.localPosition, tipsPoint3FinalPos, tipsAnimationSpeed);

            Vector3 diffTP2 = tipsPoint2FinalPos - tipsPoint2.localPosition;
            Vector3 diffTP3 = tipsPoint3FinalPos - tipsPoint3.localPosition;

            if((Mathf.Abs(diffTP2.x) <= 0.05f && Mathf.Abs(diffTP2.y) <= 0.05f && Mathf.Abs(diffTP2.z) <= 0.05f) &&
               (Mathf.Abs(diffTP3.x) <= 0.05f && Mathf.Abs(diffTP3.y) <= 0.05f && Mathf.Abs(diffTP3.z) <= 0.05f))
            {
                tipsPoint2.localPosition = tipsPoint2FinalPos;
                tipsPoint3.localPosition = tipsPoint3FinalPos;

                isTipsAnimate = false;
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

    public void TriggerRotation(int ID1, int ID2, Vector3 tipsPoint2Pos, Vector3 tipsPoint3Pos)
    {
        foreach(SpanValue spanValue in DBP03.spanValues)
        {
            if((ID1 == spanValue.choiceID1 || ID1 == spanValue.choiceID2) && (ID2 == spanValue.choiceID1 || ID2 == spanValue.choiceID2))
            {
                finalRotation = new Vector3(spanValue.x, spanValue.y, spanValue.z);
                isFinded = true;
                break;
            }
        }

        if(isFinded)
        {
            SetTipsPointsValue(tipsPoint2Pos, tipsPoint3Pos);

            CalDiffRotation();

            isRotate = true;
        }
        else
        {
            Debug.Log("Please Try again");
        }

        isFinded = false;
    }

    public void SetSpanValue()
    {
        P03W.SetSpanValue(spanValue, choiceID);
    }

    public void SetTipsPointsValue(Vector3 tipsPoint2Pos, Vector3 tipsPoint3Pos)
    {
        tipsPoint2FinalPos = new Vector3(tipsPoint2Pos.x, tipsPoint2Pos.z, tipsPoint2Pos.y);
        tipsPoint3FinalPos = new Vector3(tipsPoint3Pos.x, tipsPoint3Pos.z, tipsPoint3Pos.y);
        isTipsAnimate = true;
    }
}
