using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerPuzzle03 : GameControllerRoot
{
    // Found at 68, 1, -12 in hub

    public Camera[] topCameraList;
    public float rotatedSpeed = 1f;
    public Transform plane;
    public Transform tipsPoint2, tipsPoint3;
    public float tipsAnimationSpeed = 0.01f;
    public float planeShiftSpeed = 0.01f;
    public List<ChoiceClickButton> BtnChoices;
    // public int choicesAmount = 0;
    public bool[] subPuzzleComplete = { false, false, false };
    public Transform playerPosition;
    public Transform[] playerPositionList;
    public Transform endWall;       //When Player Finish all sub-level, the endWall will inactive

    [HideInInspector]
    public Puzzle03Window P03W;
    [HideInInspector]
    public DatabasePuzzle03 DBP03;

    public bool isInQuestion = false;
    public bool isTriggerQuestion = false;
    public bool beatLvl1 = false;

    public Vector3 finalRotation;
    public Vector3 oldRotation = Vector3.zero;
    public float timeCount = 0.0f;

    private Camera topCamera;
    public Vector3 currentRotation;
    public Vector3 diffRotation;
    private bool isRotate = false;
    private bool isFinded = false;

    private Vector3 tipsPoint2FinalPos;
    private Vector3 tipsPoint3FinalPos;
    private bool isTipsAnimate = false;

    public int subPuzzleID = 1;
    private bool isShiftPlane = false;

    private GameObject player;
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private int timer = 0;

    public GameObject endPortal;
    public GameObject[] puzzles;

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

        if(DialogueManager.showP03_00)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle03_00"));
            DialogueManager.showP03_00 = false;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = player.transform.localPosition;

        //Get all the buttons of first sub-buttons from window
        BtnChoices = P03W.BtnChoices[0];

        //Initilize Components
        plane = DBP03.subLevelPlanes[0];
        topCamera = topCameraList[0];
        P03W.SetInstructionalText("Choose a number to assign 'b' a new value. The environment will then imitate the newly generated span.");
    }

    private void Update()
    {
        if(subPuzzleID < 3)
            GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = true;
        
        if (P03W.bVal)
        {
            P03W.SetFirstSpanValue(Vector3.right, 5);
        }
        else if (!P03W.bVal)
        {
            P03W.SetFirstSpanValue(Vector3.up, 4);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.localPosition = startPosition;

            GameRoot.instance.IsLock(false);
            P03W.ShowChoicePanel(false);
            isTriggerQuestion = false;

            GameRoot.ShowTips("", true, false);
        }

        if(isRotate)
        {
            if(currentRotation == finalRotation)
            {
                plane.transform.eulerAngles = finalRotation;
                isRotate = false;

                //Stop audio FX rotated puzzle environment
                plane.GetComponent<PuzzleEnvironmentController>().PlayRotatedSoundFX(false);
                plane.transform.localEulerAngles = finalRotation;
                currentRotation = finalRotation;
                diffRotation = Vector3.zero;
                oldRotation = finalRotation;
                timeCount = 0f;
            }
            else
            {              
                currentRotation = Vector3.Slerp(oldRotation, finalRotation, timeCount);
                timeCount = timeCount + Time.deltaTime;

                float xvalue = currentRotation.x;
                if(oldRotation.x == finalRotation.x)
                    xvalue = 0f;
                float yvalue = currentRotation.y;
                if(oldRotation.y == finalRotation.y)
                    yvalue = 0f;
                float zvalue = currentRotation.z;
                if(oldRotation.z == finalRotation.z)
                    zvalue = 0f;
                currentRotation = (new Vector3(xvalue, yvalue, zvalue));  

                plane.transform.localEulerAngles = currentRotation;
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            topCamera.depth = topCamera.depth == 0 ? 1 : 0;
        }

        //*********************** Interaction with the trigger and submission *************************

        if(isInQuestion && Input.GetKeyDown(KeyCode.E))
        {
            if(isTriggerQuestion)
            {
                GameRoot.instance.IsLock(false);
                GameRoot.isPuzzleLock = false;
                P03W.ShowChoicePanel(false);
                isTriggerQuestion = false;

                GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03ExitQuestion);
            }
            else
            {
                isTriggerQuestion = true;
                GameRoot.instance.IsLock(true);
                GameRoot.isPuzzleLock = true;
                P03W.ShowChoicePanel(true);

                GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03TriggerQuestion);
            }
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

        //When player finish sub-level, the puzzle environment shift into next sub-level
        if(isShiftPlane)
        {
            playerPosition.position = Vector3.Lerp(playerPosition.position, playerPositionList[subPuzzleID].position, planeShiftSpeed);

            Vector3 diffPP = playerPositionList[subPuzzleID].position - playerPosition.position;

            if(Mathf.Abs(diffPP.z) <= 0.05f)
            {
                if(subPuzzleID >= DBP03.subLevelPlanes.Length)
                {
                    playerPosition.position = playerPositionList[subPuzzleID].position;
                    SetActive(endWall, false);
                    P03W.ShowChoicePanel(false);

                    GameRoot.instance.IsLock(false);
                    GameRoot.ShowTips("", false, false);
                    isInQuestion = false;
                    P03W.SetInstructionalText("Go to the exit.");
                }

                isShiftPlane = false;
            }
        }

        previousPosition = player.transform.position;
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

            isRotate = true;

            //Activate audio FX rotated puzzle environment
            plane.GetComponent<PuzzleEnvironmentController>().PlayRotatedSoundFX(true);
        }
        else
        {
            Debug.Log("Please Try again");
        }

        isFinded = false;
    }
    
    
    public void SetSpanValue(Vector3 spanValue, int choiceID)
    {
        if(P03W.bVal)
            spanValue = new Vector3(spanValue[0], spanValue[1], spanValue[2]);
        else
            spanValue = new Vector3(spanValue[1], spanValue[0], spanValue[2]);

        P03W.SetSecondSpanValue(spanValue, choiceID);
    }

    public void SetTipsPointsValue(Vector3 tipsPoint2Pos, Vector3 tipsPoint3Pos)
    {
        tipsPoint2FinalPos = new Vector3(tipsPoint2Pos.x, tipsPoint2Pos.z, tipsPoint2Pos.y);
        tipsPoint3FinalPos = new Vector3(tipsPoint3Pos.x, tipsPoint3Pos.z, tipsPoint3Pos.y);
        isTipsAnimate = true;
    }

    public void ReactivateChoiceBtn(int choiceID)
    {
        foreach(ChoiceClickButton T in BtnChoices)
        {
            if(choiceID == T.choiceID)
            {
                T.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void activateOtherChoiceBtn(int choiceID)
    {
        foreach(ChoiceClickButton T in BtnChoices)
        {
            if(choiceID != T.choiceID)
            {
                T.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void FinishSubLevel(int subPuzzleID)
    {
        GameRoot.instance.IsLock(false);
        GameRoot.isPuzzleLock = false;
        P03W.ShowChoicePanel(false);
        isTriggerQuestion = false;

        this.subPuzzleID = subPuzzleID;

        topCamera.depth = 0;

        //Stop audio FX rotated puzzle environment
        plane.GetComponent<PuzzleEnvironmentController>().PlayRotatedSoundFX(false);

        isRotate = false;

        puzzles[subPuzzleID - 1].SetActive(false);

        if (subPuzzleID < DBP03.subLevelPlanes.Length)
        {
            topCamera = topCameraList[subPuzzleID];
            BtnChoices = P03W.BtnChoices[subPuzzleID];
            P03W.SetPanelChoice(subPuzzleID);

            DBP03.SetPuzzleActive(subPuzzleID);
            plane = DBP03.GetSubLevelPlanes(subPuzzleID);
        }

        if(subPuzzleID == 3)
        {
            endPortal.SetActive(true);
        }

        finalRotation = Vector3.zero;
        oldRotation = Vector3.zero;

        beatLvl1 = true;

        GameRoot.instance.IsLock(false);
        P03W.ShowChoicePanel(false);
        P03W.ClearSpanValues("", "");
        P03W.SwapFeedbackPanel();
        isTriggerQuestion = false;

        isShiftPlane = true;
    }

    public int getSubPuzzleID()
    {
        return subPuzzleID;
    }
}
