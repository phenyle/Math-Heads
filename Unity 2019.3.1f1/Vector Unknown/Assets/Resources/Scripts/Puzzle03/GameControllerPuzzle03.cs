﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerPuzzle03 : GameControllerRoot
{
    // Found at 68, 1, -12 in hub

    public int Difficulty = 1;
    public GameObject finishArea;
    public GameObject divisionWall;
    public GameObject trigger;
    public GameObject puzzCamStart;
    public Transform startingFloor;
    public GameObject tipsGlow1;
    public GameObject tipsGlow2;

    private Vector3 startRandRotate;

    public GameObject mainCamera;
    public GameObject puzzleCamera;
    public GameObject CameraTools;

    public Camera ConsoleCamera;
    public float rotatedSpeed = 1f;
    public Transform plane;
    public Transform tipsPoint2, tipsPoint3;
    public float tipsAnimationSpeed = 0.01f;
    public float planeShiftSpeed = 0.01f;
//    public List<ChoiceClickButton> BtnChoices;
    // public int choicesAmount = 0;
//    public bool[] subPuzzleComplete = { false, false, false };
    public Transform playerPosition;
//    public Transform[] playerPositionList;
    public Transform endWall;       //When Player Finish all sub-level, the endWall will inactive

    [HideInInspector]
    public Puzzle03Window P03W;
    [HideInInspector]
    public DatabasePuzzle03 DBP03;

    public bool isInQuestion = false;
    public bool isTriggerQuestion = false;
//    public bool beatLvl1 = false;

    public Vector3 finalRotation;
    public Vector3 oldRotation;
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

    public GameObject player;
    private Vector3 startPosition;

    public GameObject puzzleOverall;
    private int puzzleRotNum;
    public GameObject endZone;
    public GameObject currPuzzle;
    public float cameraHorizontal, cameraVertical;

    //Database Records

    [HideInInspector]
    [System.Serializable]
    public struct obsData
    {
        public int obsID;
        public float obsTime;
        public Vector3 startingRotation;
        public int numOfMoves;
    }

    public string movesList;

    [HideInInspector]
    public obsData puzzleData;

    public void Start()
    {
        divisionWall.SetActive(true);
        finishArea.SetActive(false);
        puzzleOverall.transform.Rotate(SetPuzzleRandomRotate());
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = player.transform.localPosition;
        movesList = "";
    }

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

        conversation(0);

        //if(DialogueManager.showP03_00)
        //{
        //    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle03_00"));
        //    DialogueManager.showP03_00 = false;
        //}



        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = player.transform.localPosition;

        //Get all the buttons of first sub-buttons from window
        //       BtnChoices = P03W.BtnChoices1;

        //Initilize Components
        plane = currPuzzle.transform;
        topCamera = ConsoleCamera;
        P03W.SetInstructionalText("Choose a number to assign 'b' a new value. The environment will then imitate the newly generated span.");

        cameraHorizontal = GameObject.Find("MainCamera").GetComponent<CameraController>().horizontal;
        cameraVertical = GameObject.Find("MainCamera").GetComponent<CameraController>().vertical;



        //DATABASE INIT STUFF
        puzzleData.obsID = Difficulty;
        puzzleData.obsTime = 0.0f;
        puzzleData.startingRotation = startRandRotate;
        puzzleData.numOfMoves = 0;


    }

    private void Update()
    {
        if (!DialogueManager.isInDialogue && !GameRoot.isPause && isInQuestion)
        {
            puzzleData.obsTime += Time.deltaTime;

            int minutes = (int)puzzleData.obsTime / 60;
            int seconds = (int)puzzleData.obsTime - 60 * minutes;
            if (seconds < 10)
                P03W.SetTime(minutes.ToString() + " : 0" + seconds.ToString());
            else
                P03W.SetTime(minutes.ToString() + " : " + seconds.ToString());
        }

        if (isInQuestion)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = false;
            }
        }

        //if(subPuzzleID < 3)
        //    GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = true;
        
        //if (P03W.bVal)
        //{
        //    P03W.SetFirstSpanValue(Vector3.right, 5);
        //}
        //else if (!P03W.bVal)
        //{
        //    P03W.SetFirstSpanValue(Vector3.up, 4);
        //}

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
   //             diffRotation = Vector3.zero;
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

    public void TriggerRotation(int ID1, int ID2, Vector3 tipsPoint2Pos, Vector3 tipsPoint3Pos)
    {
        Vector3 temp = Vector3.zero;
        if (ID1 == 2)
        {
            switch (ID2)
            {
                case 4:
                    switch (puzzleRotNum)
                    {
                        case 1:
                            temp.x = -35;
                            break;
                        case 2:
                            temp.z = -35;
                            break;
                        case 3:
                            temp.x = 35;
                            break;
                        case 4:
                            temp.z = 35;
                            break;
                    }

                    break;
                case 5:
                    break;
                case 6:
                    switch (puzzleRotNum)
                    {
                        case 1:
                            temp.x = 35;
                            break;
                        case 2:
                            temp.z = 35;
                            break;
                        case 3:
                            temp.x = -35;
                            break;
                        case 4:
                            temp.z = -35;
                            break;
                    }
                    break;
            }
        }
        else if (ID1 == 5)
        {
            switch (ID2)
            {
                case 1:
                    switch (puzzleRotNum)
                    {
                        case 1:
                            temp.z = 35;
                            break;
                        case 2:
                            temp.x = -35;
                            break;
                        case 3:
                            temp.z = -35;
                            break;
                        case 4:
                            temp.x = 35;
                            break;
                    }
                    break;
                case 2:
                    break;
                case 3:
                    switch (puzzleRotNum)
                    {
                        case 1:
                            temp.z = -35;
                            break;
                        case 2:
                            temp.x = 35;
                            break;
                        case 3:
                            temp.z = 35;
                            break;
                        case 4:
                            temp.x = -35;
                            break;
                    }
                    break;
            }
        }
        else if (ID1 == 0 || ID2 == 0)
            temp = Vector3.zero;

        finalRotation = temp;
        isFinded = true;
        movesList += CreateAttemptItem(tipsPoint2Pos, tipsPoint3Pos);
        P03W.SetMoves(puzzleData.numOfMoves); 


        //foreach (SpanValue spanValue in DBP03.spanValues)
        //{
        //    if((ID1 == spanValue.choiceID1 || ID1 == spanValue.choiceID2) && (ID2 == spanValue.choiceID1 || ID2 == spanValue.choiceID2))
        //    {
        //        finalRotation = new Vector3(spanValue.x, spanValue.y, spanValue.z);
        //        isFinded = true;
        //        break;
        //    }
        //}

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

    private string CreateAttemptItem(Vector3 span1, Vector3 span2) 
    {
        string temp = "";
        puzzleData.numOfMoves++;

        temp += puzzleData.numOfMoves.ToString() + ", ";
        temp += puzzleData.obsTime.ToString("0.00") + ", ";
        temp += span1.x.ToString() + ", " + span1.y.ToString() + ", " + span1.z.ToString() + ", ";
        temp += span2.x.ToString() + ", " + span2.y.ToString() + ", " + span2.z.ToString() + "\n";
        
        return temp;
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
        if(P03W.bVal)
        {
            tipsPoint2FinalPos = new Vector3(tipsPoint2Pos.x, tipsPoint2Pos.z, tipsPoint2Pos.y);
            tipsPoint3FinalPos = new Vector3(tipsPoint3Pos.x, tipsPoint3Pos.z, tipsPoint3Pos.y);
        }
        else
        {
            tipsPoint3FinalPos = new Vector3(tipsPoint2Pos.x, tipsPoint2Pos.z, tipsPoint2Pos.y);
            tipsPoint2FinalPos = new Vector3(tipsPoint3Pos.x, tipsPoint3Pos.z, tipsPoint3Pos.y);
        }
        isTipsAnimate = true;
    }

    public void ReactivateChoiceBtn(int choiceID)
    {
        //foreach(ChoiceClickButton T in BtnChoices)
        //{
        //    if(choiceID == T.choiceID)
        //    {
        //        T.GetComponent<Button>().interactable = true;
        //    }
        //}
    }

    public void activateOtherChoiceBtn(int choiceID)
    {
        //foreach(ChoiceClickButton T in BtnChoices)
        //{
        //    if(choiceID != T.choiceID)
        //    {
        //        T.GetComponent<Button>().interactable = true;
        //    }
        //}
    }

    public void FinishSubLevel()
    {
        GameRoot.instance.IsLock(false);
        GameRoot.isPuzzleLock = false;
        P03W.ShowChoicePanel(false);
        isTriggerQuestion = false;

        startingFloor.localScale = new Vector3(10, 1, 300);
        startingFloor.localPosition = new Vector3(0, 0, 5);
 //       this.subPuzzleID = subPuzzleID;

        topCamera.depth = 0;

        //Stop audio FX rotated puzzle environment
        plane.GetComponent<PuzzleEnvironmentController>().PlayRotatedSoundFX(false);

        isRotate = false;

    //    puzzles[subPuzzleID - 1].SetActive(false);

        if (subPuzzleID < DBP03.subLevelPlanes.Length)
        {
            topCamera = ConsoleCamera;
  //          BtnChoices = P03W.BtnChoices1;
            P03W.SetPanelChoice(0);

  //          DBP03.SetPuzzleActive(subPuzzleID);
   //         plane = DBP03.GetSubLevelPlanes(subPuzzleID);
        }

        //if(subPuzzleID == 3)
        //{
        //    endZone.SetActive(true);
        //}

        divisionWall.SetActive(false);
        finishArea.SetActive(true);
        trigger.SetActive(false);

        CamGlideToPlayer();

        finalRotation = Vector3.zero;
        oldRotation = Vector3.zero;

    //    beatLvl1 = true;

        GameRoot.instance.IsLock(false);
        P03W.ClearSpanValues("", "");
        P03W.SwapFeedbackPanel();
        isTriggerQuestion = false;

        SetTipsPointsValue(new Vector3(1,0,0),new Vector3(0,1,0));
        GameObject.Find("MainCamera").GetComponent<CameraController>().vertical = cameraVertical;
        GameObject.Find("MainCamera").GetComponent<CameraController>().horizontal = cameraHorizontal;
        isShiftPlane = true;
    }

    private Vector3 SetPuzzleRandomRotate()
    {
        int rand = UnityEngine.Random.Range(1, 5);
        Vector3 temp = Vector3.zero;

        switch (rand)
        {
            case 1:
                temp = new Vector3(0, 0, 0);
                break;
            case 2:
                temp = new Vector3(0, 90, 0);
                break;
            case 3:
                temp = new Vector3(0, 180, 0);
                break;
            case 4:
                temp = new Vector3(0, 270, 0);
                break;
        }
        puzzleRotNum = rand;
        startRandRotate = temp;
        return temp;
    }

    /// <summary>
    /// Selects the conversation dialog to pop up
    /// Requires that the associated bool[] and conversation file be established
    /// with the DialogManager.cs
    /// </summary>
    /// <param name="val"></param>
    public void conversation(int val)
    {
        switch (Difficulty)
        {
            case 1: //Level 1 Dialogs
                if (DialogueManager.showP03_1[val])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle03-1_0" + val.ToString()));
                    DialogueManager.showP03_1[val] = false;
                }

                break;
            case 2: //Level 2 Dialogs
                if (DialogueManager.showP03_2[val])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle03-2_0" + val.ToString()));
                    DialogueManager.showP03_2[val] = false;
                }

                break;
            case 3: //Level 3 Dialogs
                if (DialogueManager.showP03_3[val])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle03-3_0" + val.ToString()));
                    DialogueManager.showP03_3[val] = false;
                }

                break;
        }
    }

    public int getSubPuzzleID()
    {
        return subPuzzleID;
    }

    public void CamGlideToPuzzle()
    {
        mainCamera.GetComponent<Camera>().enabled = false;
        puzzleCamera.GetComponent<Camera>().enabled = true;

        GameRoot.camEvents.AddListener(CamAtPuzzle);
        CameraTools.GetComponent<ObjectGlide>().glideObject = puzzleCamera;
        CameraTools.GetComponent<ObjectGlide>().SetObjectMoveSpeed(0.3f);
        CameraTools.GetComponent<ObjectGlide>().GlideToMovingPosition(puzzleCamera, puzzCamStart, currPuzzle, puzzleCamera.transform.position);
    }

    public void CamGlideToPlayer()
    {
        CameraTools.GetComponent<CameraRotate>().enabled = false;

        GameRoot.camEvents.AddListener(CamAtPlayer);
        CameraTools.GetComponent<ObjectGlide>().glideObject = puzzleCamera;
        CameraTools.GetComponent<ObjectGlide>().SetObjectMoveSpeed(0.4f * ((puzzleCamera.transform.position - puzzCamStart.transform.position).magnitude) / (puzzCamStart.transform.position - trigger.transform.position).magnitude);
        CameraTools.GetComponent<ObjectGlide>().glideObject = puzzleCamera;
        CameraTools.GetComponent<ObjectGlide>().GlideToMovingPosition(puzzleCamera, mainCamera, player, puzzleCamera.transform.position);
    }

    public void CamAtPuzzle()
    {
        CameraTools.GetComponent<CameraRotate>().rotCamera = puzzleCamera.transform;
        CameraTools.GetComponent<ObjectGlide>().enabled = false;
        CameraTools.GetComponent<CameraRotate>().enabled = true;
        GameRoot.camEvents.RemoveListener(CamAtPuzzle);
        GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = false;
    }

    public void CamAtPlayer()
    {
        mainCamera.GetComponent<Camera>().enabled = true;
        puzzleCamera.GetComponent<Camera>().enabled = false;

        CameraTools.GetComponent<ObjectGlide>().enabled = false;

        GameRoot.camEvents.RemoveListener(CamAtPlayer);
    }


    public void RecordData()
    {
        switch (Difficulty) {

            case 1:
                if (GameRoot.player.users.p3_1clear_time == 0.0f)
                    GameRoot.player.users.p3_1clear_time = puzzleData.obsTime;
                else if (puzzleData.obsTime < GameRoot.player.users.p3_1clear_time)
                    GameRoot.player.users.p3_1clear_time = puzzleData.obsTime;

                GameRoot.player.p3_1.obs = puzzleData;
                GameRoot.player.p3_1.obs_movesList = movesList;

                GameRoot.player.users.p3_1attempts++;
                GameRoot.player.p3_1.attemptNum = GameRoot.player.users.p3_1attempts;
                GameRoot.player.p3_1.usernameAttempt = GameRoot.player.users.username + "." + GameRoot.player.users.p3_1attempts.ToString();
                break;

            case 2:
                if (GameRoot.player.users.p3_2clear_time == 0.0f)
                    GameRoot.player.users.p3_2clear_time = puzzleData.obsTime;
                else if (puzzleData.obsTime < GameRoot.player.users.p3_2clear_time)
                    GameRoot.player.users.p3_2clear_time = puzzleData.obsTime;

                GameRoot.player.p3_2.obs = puzzleData;
                GameRoot.player.p3_2.obs_movesList = movesList;

                GameRoot.player.users.p3_2attempts++;
                GameRoot.player.p3_2.attemptNum = GameRoot.player.users.p3_2attempts;
                GameRoot.player.p3_2.usernameAttempt = GameRoot.player.users.username + "." + GameRoot.player.users.p3_2attempts.ToString();
                break;

            case 3:
                if (GameRoot.player.users.p3_3clear_time == 0.0f)
                    GameRoot.player.users.p3_3clear_time = puzzleData.obsTime;
                else if (puzzleData.obsTime < GameRoot.player.users.p3_3clear_time)
                    GameRoot.player.users.p3_3clear_time = puzzleData.obsTime;

                GameRoot.player.p3_3.obs = puzzleData;
                GameRoot.player.p3_3.obs_movesList = movesList;

                GameRoot.player.users.p3_3attempts++;
                GameRoot.player.p3_3.attemptNum = GameRoot.player.users.p3_3attempts;
                GameRoot.player.p3_3.usernameAttempt = GameRoot.player.users.username + "." + GameRoot.player.users.p3_3attempts.ToString();
                break;

        }
    }
}
