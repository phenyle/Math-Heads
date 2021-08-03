﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Characters.ThirdPerson;

public enum Direction
{
    X,
    Y,
    Z,
    XY,
    XYZ
};

public class Puzzle04Controller : MonoBehaviour
{
    [Header("---Object Assignment---")]
    public GameObject player;
    //    public GameObject portal;
    public GameObject startPoint;
    public GameObject goal;
    public GameObject goalPoint;
    public GameObject reset;
    public GameObject finish;
    public int puzzleID;
    public bool isActive;

    static private UnityEvent events;

    [Header("---View Controls---")]
    public GameObject mainCamera;
    public GameObject puzzleCamera;
    public GameObject visVectCamera;
    public GameObject cameraTarget;
    public GameObject CameraStartPosition;
    private Vector3 prevCameraPosition;


    [Header("---Puzzle Vectors---")]
    public Vector3 InitFinalVector; //this is the vector input from the Unity Inspector, it's the basis for the FinalAnswer
    public bool isDynamic; //this bool tells the system to slightly randomize the InitFinalVector before setting it as the startingAnswer
    [Range(1, 10)]
    public int FinalDynamicRange;
    public Direction LimitAxis = new Direction();
    private Vector3 DynamicVector;

    [Header("---Answer Card Vectors---")]
    [Range(1, 10)]
    public int MaxCardScalar;
    //   [Range(1, 10)]
    //   public int MaxCardMagnitudes;

    //Object Tools
    [Header("Puzzle Tools")]
    public PortalTrigger portal;
    public GrappleCode grappleKit;
    public GapTriggersController gapTriggers;
    private bool inPortal = false;
    private VisualVector VV01;
    public ReleaseGrappleTrigger releaseTrigger;
    private ObjectGlide cameraGlide;
    private CameraRotate cameraRotate;



    [Header("---Conversation Select---")]
    [Range(0, 9)]
    [Tooltip("Sets the dialog number used.\n0 val means no dialog\n1-9 selects that specific dialog")]
    public int convoTriggerNumber;


    //Answer Cards
    private Vector3 startingAnswer; //this is the starting vector after being slightly randomized (when applicable) used for answer card calculation
    private Vector3 finalAnswerVector; //this is the Final answer vector that is tailored to match what the pair of answer cards can reproduce
    private List<Vector3> cardVectors;

    //Player Answers
    private int scalar1;
    private int scalar2;
    private Vector3 card1;
    private Vector3 card2;
    private Vector3 playerAnswer;
    private bool ans1Entered; //condition flag that a card is slotted in position1 in the UI
    private bool ans2Entered; //condition flag that a card is slotted in position2 in the UI
    private bool correct;

    private GameControllerPuzzle04 GCP04;

    //Database Records
    private float obstacleTime;

    [HideInInspector]
    [System.Serializable]
    public struct obsData
    {
        public int obsID;
        public float obsTime;
        public Vector3 answer;

        //Puzzle generated and Expected values (e)
        public int e_Scal1;
        public int e_Scal2;
        public Vector3 e_Vect1;
        public Vector3 e_Vect2;

        //Player generated and Input values (i)
        public int i_Scal1;
        public int i_Scal2;
        public Vector3 i_Vect1;
        public Vector3 i_Vect2;
    }

    [HideInInspector]
    public obsData puzzleData;



    // Awake is called when the Instance is Initialized
    void Awake()
    {
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
        VV01 = this.GetComponent<VisualVector>();
        cameraRotate = this.GetComponent<CameraRotate>();
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        puzzleCamera = GameObject.FindGameObjectWithTag("PuzzleCamera");
        cameraGlide = GameObject.FindGameObjectWithTag("cameraTools").GetComponent<ObjectGlide>();
        cameraRotate = GameObject.FindGameObjectWithTag("cameraTools").GetComponent<CameraRotate>();
        visVectCamera = GameObject.Find("VisVectorCamera");
        //have to flicker visvect clear flags on start, because of a weird bug.
        //it should be set to depth, but on Level 3 it only works if it gets reset to depth after
        //starting the level.  So, that's what this does.
        visVectCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        visVectCamera.GetComponent<Camera>().enabled = false;


        grappleKit = GameObject.Find("GrappleKit").GetComponent<GrappleCode>();
        
        gapTriggers.SetPuzzleController(this);
        portal.SetPuzzleController(this);
        inPortal = false;
        releaseTrigger.SetPuzzleController(this);
        VV01.SetPuzzleController(this);

        cardVectors = new List<Vector3>();

        scalar1 = 0;
        scalar2 = 0;

        card1 = new Vector3(0f, 0f, 0f);
        card2 = new Vector3(0f, 0f, 0f);

        //If vector is dynamic, generate an additional vector to add
        //to starting answer
        if (isDynamic)
        {
            DynamicVector = DynamicGenerator();
        }
        else
            DynamicVector = new Vector3(0f, 0f, 0f);


        //Final Answer is created after generating the cards       
        startingAnswer = InitFinalVector + DynamicVector;


        //Move camera target inbetween the starting position and the goal
        cameraTarget.transform.position += (goalPoint.transform.position - startPoint.transform.position) / 2;

        correct = false;

        obstacleTime = 0.0f;
        puzzleData = new obsData();
        puzzleData.obsID = puzzleID;

        //       intAnswerGenerator();
        //AnswerGenerators();
        AnswerCards();

        if(GCP04.Difficulty == 1)
            finalAnswerVector = SetFinalAnswer(0, Vector3.zero, 1, startingAnswer);

    }

    // Update is called once per frame
    void Update()
    {
        if (inPortal)
        {
            //if (cameraGlide.isAtDestination())
            //    cameraRotate.enabled = true;



            if (!DialogueManager.isInDialogue && !GameRoot.isPause)
                obstacleTime += Time.deltaTime;


            isActive = true;

            scalar1 = GCP04.P04W.getScalar1Value();
            scalar2 = GCP04.P04W.getScalar2Value();

            if (GCP04.P04W.getAnswer1() != null)
            {
                card1 = GCP04.P04W.getAnswer1().GetComponent<CardVectors>().getCardVector();
                ans1Entered = true;
            }
            else
            {
                card1 = Vector3.zero;
                ans1Entered = false;
            }

            if (GCP04.P04W.getAnswer2() != null)
            {
                card2 = GCP04.P04W.getAnswer2().GetComponent<CardVectors>().getCardVector();
                ans2Entered = true;
            }
            else
            {
                card2 = Vector3.zero;
                ans2Entered = false;
            }

            playerAnswer = scalar1 * card1 + scalar2 * card2;

            GCP04.P04W.setFinalAnswerDisplay(this);


            //Keyboard Scalar Controls
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (scalar1 > -10)
                    GCP04.P04W.scalar1.value--;
                if (GCP04.P04W.scalar1.value == 0)
                    GCP04.P04W.scalar1.value = -1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (scalar1 < 10)
                    GCP04.P04W.scalar1.value++;
                if (GCP04.P04W.scalar1.value == 0)
                    GCP04.P04W.scalar1.value = 1;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (scalar2 > -10)
                    GCP04.P04W.scalar2.value--;
                if (GCP04.P04W.scalar2.value == 0)
                    GCP04.P04W.scalar2.value = -1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (scalar2 < 10)
                    GCP04.P04W.scalar2.value++;
                if (GCP04.P04W.scalar2.value == 0)
                    GCP04.P04W.scalar2.value = 1;
            }




        }
        else
        {
            isActive = false;
        }

    }

    /// <summary>
    /// Generates a modified vector based from the MinVectorSize and MaxDynamicScalar.
    /// It first divides each vector dimension by a random float and rounds to an int
    /// then in multiplies the whole vector by a random float and rounds each dimension
    /// to an int.
    /// This returns a vector that is generally in the direction of the MinVectorSize
    /// while being varied enough that each playthrough has different results
    /// </summary>
    /// <returns></returns>
    private Vector3 DynamicGenerator()
    {
        Vector3 temp = Vector3.zero;

        switch (GCP04.Difficulty)
        {
            case 1:
                switch (LimitAxis)
                {
                    //Zero vectors are allowed here for "extra" vectors
                    case Direction.X:
                        temp.x += UnityEngine.Random.Range(0, FinalDynamicRange);
                        temp.y += 0.0f;
                        temp.z += 0.0f;
                        break;

                    case Direction.Y:
                        temp.x += 0.0f;
                        temp.y += UnityEngine.Random.Range(0, FinalDynamicRange);
                        temp.z += 0.0f;
                        break;
                }

                break;

            case 2:
                    temp.x += UnityEngine.Random.Range(0, FinalDynamicRange);
                    temp.y += UnityEngine.Random.Range(0, FinalDynamicRange);
                    temp.z += 0.0f;
                break;

            case 3:
                //No zero vectors allowed
                temp.x += UnityEngine.Random.Range(0, FinalDynamicRange);
                temp.y += UnityEngine.Random.Range(0, FinalDynamicRange);
                temp.z += UnityEngine.Random.Range(0, FinalDynamicRange);
                break;
        }

        return temp;
    }


    private void AnswerCards()
    {
        Vector3 answer1, answer2, extra1, extra2;
        int scal1;


        if(GCP04.Difficulty == 1)
        {
            if(this.getDirection() == Direction.X)
            {
                cardVectors.Add(new Vector3(1, 0, 0));
                cardVectors.Add(new Vector3(-1, 0, 0));

                if(UnityEngine.Random.value > 0.5f)                
                    cardVectors.Add(new Vector3(2, 0, 0));
                else
                    cardVectors.Add(new Vector3(-2, 0, 0));

                if (UnityEngine.Random.value > 0.5f)
                    cardVectors.Add(new Vector3(3, 0, 0));
                else
                    cardVectors.Add(new Vector3(-3, 0, 0));

            }
            if (this.getDirection() == Direction.Y)
            {
                cardVectors.Add(new Vector3(0, 1, 0));
                cardVectors.Add(new Vector3(0, -1, 0));

                if (UnityEngine.Random.value > 0.5f)
                    cardVectors.Add(new Vector3(0, 2, 0));
                else
                    cardVectors.Add(new Vector3(0, -2, 0));

                if (UnityEngine.Random.value > 0.5f)
                    cardVectors.Add(new Vector3(0, 3, 0));
                else
                    cardVectors.Add(new Vector3(0, -3, 0));
            }
        }
        else
        {
            scal1 = UnityEngine.Random.Range(Mathf.RoundToInt(MaxCardScalar / 2), MaxCardScalar + 1);
            do { answer1 = VectorGenerator(); } while (answer1 == Vector3.zero || (answer1 * scal1) == InitFinalVector);
            cardVectors.Add(answer1);

            answer2 = VectorPairFinder(scal1, answer1);
            cardVectors.Add(answer2);

            do { extra1 = VectorGenerator(); } while (extra1 == Vector3.zero);
            cardVectors.Add(extra1);

            do { extra2 = VectorGenerator(); } while (extra2 == Vector3.zero);
            cardVectors.Add(extra2);
        }

        answerShuffle();
    }

    /// <summary>
    /// This function generates a Vector in the set of V where V is defined as:
    ///  V = { [x,y,z] | x,y,z >= -3 and x,y,z <= 3 and x=y=z != 0 }
    /// </summary>
    /// <returns>returns a Vector3 in the set of V </returns>
    private Vector3 VectorGenerator()
    {
        Vector3 temp = Vector3.zero;

        switch (GCP04.Difficulty)
        {
            case 1:
                switch (LimitAxis)
                {
                    //Zero vectors are allowed here for "extra" vectors
                    case Direction.X:
                        temp.x = UnityEngine.Random.Range(-3, 4);
                        temp.y = 0.0f;
                        temp.z = 0.0f;
                        break;

                    case Direction.Y:
                        temp.x = 0.0f;
                        temp.y = UnityEngine.Random.Range(-3, 4);
                        temp.z = 0.0f;
                        break;
                }

                break;

            case 2:
                //No zero vectors allowed
                do
                {
                    temp.x = UnityEngine.Random.Range(-3, 4);
                    temp.y = UnityEngine.Random.Range(-3, 4);
                    temp.z = 0.0f;
                } while (temp == Vector3.zero && testUnique(temp));
                break;

            case 3:
                //No zero vectors allowed
                do
                {
                    temp.x = UnityEngine.Random.Range(-3, 4);
                    temp.y = UnityEngine.Random.Range(-3, 4);
                    temp.z = UnityEngine.Random.Range(-3, 4);
                } while (temp == Vector3.zero && testUnique(temp));

                break;
        }

        return temp;
    }

    private Vector3 VectorPairFinder(int scal1, Vector3 answer1)
    {
        Vector3 answer2 = Vector3.zero;
        float maxDimension = 0.0f;
        int scal2 = 0;

        //first find which version of scal is closest to goal
        if (V1reversedScal(scal1, answer1))
            scal1 *= -1;

        //Set answer2 to exact displacement
        answer2 = startingAnswer - (answer1 * scal1);

        //find the largest Dimension of displacement (answer2)        
        maxDimension = Mathf.Abs(answer2.x);
        if (maxDimension < Mathf.Abs(answer2.y))
            maxDimension = Mathf.Abs(answer2.y);
        if (maxDimension < Mathf.Abs(answer2.z))
            maxDimension = Mathf.Abs(answer2.z);

        //We'll use that maxDimension to determine what the scalar should be
        for (int i = 1; i <= MaxCardScalar + 1; i++)
        {
            //if there is no suitable scalar we'll shave 1/8 off and try again
            if (i == MaxCardScalar + 1)
            {
                answer2 *= 0.875f;

                maxDimension = Mathf.Abs(answer2.x);
                if (maxDimension < Mathf.Abs(answer2.y))
                    maxDimension = Mathf.Abs(answer2.y);
                if (maxDimension < Mathf.Abs(answer2.z))
                    maxDimension = Mathf.Abs(answer2.z);

                i = 1;
            }
            //if the largest dimension can be reduced to less than 3 by some scalar
            else if ((maxDimension / i ) <= 3)
            {
                scal2 = i;
                break;
            }
        }

        //With scal2 set, we need to chop the displacment vector (answer2) into that size and round the results
        answer2.x = Mathf.RoundToInt(answer2.x / scal2);
        answer2.y = Mathf.RoundToInt(answer2.y / scal2);
        answer2.z = Mathf.RoundToInt(answer2.z / scal2);

        //Now that we have both answer1 and answer2 created in the set of V, we need to adjust the finalGoal to match these rounded vectors
        finalAnswerVector = SetFinalAnswer(scal1, answer1, scal2, answer2);
        //set the expected card and scal values for the database object

        return answer2;
    }

    private Vector3 SetFinalAnswer(int scal1, Vector3 vect1, int scal2, Vector3 vect2)
    {
        puzzleData.e_Scal1 = scal1;
        puzzleData.e_Vect1 = vect1;
        puzzleData.e_Scal2 = scal2;
        puzzleData.e_Vect2 = vect2;
        puzzleData.answer = (vect1 * scal1) + (vect2 * scal2);
        return (vect1 * scal1) + (vect2 * scal2);
    }

    private bool V1reversedScal(int scal, Vector3 vector)
    {
        float origDisplacementMag = (startingAnswer - (vector * scal)).magnitude;
        float reversedDisplacementMag = (startingAnswer - (vector * (scal * (-1)))).magnitude;

        //basically, if the original displacement is smaller leave it as is, otherwise flip the scalar when calculating "answer2".
        return origDisplacementMag > reversedDisplacementMag;
    }

    /// <summary>
    /// Shuffles the order of the answer cards
    /// </summary>
    public void answerShuffle()
    {
        //Shuffle Cards
        int count = cardVectors.Count;
        int last = count - 1;
        for (int i = 0; i < last; i++)
        {
            int r = UnityEngine.Random.Range(i, count);
            Vector3 temp = cardVectors[i];
            cardVectors[i] = cardVectors[r];
            cardVectors[r] = temp;
        }
    }

    /// <summary>
    /// Tests if the input vector is duplicate or a scalar multiple
    /// of a card that already exists in the cardVectors list.
    /// If it's not a duplicate, it returns "True" that it's unique
    /// If it is a dupicate it returns "False".
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public bool testUnique(Vector3 test)
    {
        bool unique = true;

        //Test that the new Random Vector is not a Duplicate or scalar
        //cycle through scalars:
        for (int i = -MaxCardScalar; i <= MaxCardScalar; i++)
        {
            //cycle through cards:
            for (int j = 0; j < cardVectors.Count; j++)
            {
                if (test * i == cardVectors[j] || test == cardVectors[j] * i)
                {
                    unique = false;
                }
                else
                    unique = true;

                if (!unique)
                    break;
            }
            if (!unique)
                break;
        }

        return unique;
    }

    public Vector3 getMinVector()
    {
        return InitFinalVector;
    }

    public Vector3 getDynamicVector()
    {
        return DynamicVector;
    }

    public Vector3 getAnswerVector()
    {
        return finalAnswerVector;
    }

    public Vector3 getPlayerAnswer()
    {
        return playerAnswer;
    }

    public Direction getDirection()
    {
        return LimitAxis;
    }

    public List<Vector3> getAnswerCards()
    {
        return cardVectors;
    }

    public int getScalar1()
    {
        return scalar1;
    }
    public int getScalar2()
    {
        return scalar2;
    }

    public void setScalar1(int val)
    {
        scalar1 = val;
    }
    public void setScalar2(int val)
    {
        scalar2 = val;
    }
    public void setAnsCard1(Vector3 val)
    {
        card1 = val;
    }
    public void setAnsCard2(Vector3 val)
    {
        card2 = val;
    }

    public Vector3 getAnsCard1()
    {
        return card1;
    }
    public Vector3 getAnsCard2()
    {
        return card2;
    }

    public bool isAns1Entered()
    {
        return ans1Entered;
    }
    public bool isAns2Entered()
    {
        return ans2Entered;
    }

    public Transform getCameraTransform()
    {
        return CameraStartPosition.transform;
    }

    public GameObject getCameraTarget()
    {
        return cameraTarget;
    }

    public void setPrevCameraPos(Vector3 currPos)
    {
        prevCameraPosition = currPos;
    }

    public Vector3 getPrevCameraPos()
    {
        return prevCameraPosition;
    }

    public VisualVector getVisualVector()
    {
        return VV01;
    }

    public GameControllerPuzzle04 getGameController()
    {
        return GCP04;
    }

    public bool getCorrect()
    {
        return correct;
    }

    public bool checkFinalAnswer()
    {
        if (ans1Entered && ans2Entered)
        {
            if (playerAnswer == finalAnswerVector)
            {
                if (!correct)
                    GCP04.P04W.SetComplete();
                correct = true;

                grappleKit.InitGrapple(correct, finalAnswerVector, goalPoint, player.transform.position);
                grappleKit.grappleToGoal(this);

                puzzleData.obsTime = this.obstacleTime;
                puzzleData.i_Scal1 = this.scalar1;
                puzzleData.i_Vect1 = this.card1;
                puzzleData.i_Scal2 = this.scalar2;
                puzzleData.i_Vect2 = this.card2;

                GCP04.SaveLocalPuzzleData(this);

                return true;
            }
            else
            {
                correct = false;
                grappleKit.InitGrapple(correct, VV01.vector2end.transform.position, goalPoint, player.transform.position);
                grappleKit.grappleToGoal(this);
                return false;
            }
        }
        else
            return false;

    }

    //------------------------------------
    // TOOL ACTIONS
    //------------------------------------

    // PORTAL---------------

    public void OnPortalEnter()
    {
        inPortal = true;

        GCP04.SetPuzzlePause(true);

        visVectCamera.GetComponent<Camera>().enabled = true;
        visVectCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;

        cameraRotate.SetCameraTarget(cameraTarget.transform.position);

        puzzleCamera.transform.position = mainCamera.transform.position;
        puzzleCamera.transform.rotation = mainCamera.transform.rotation;

        if (mainCamera.transform.TryGetComponent<CameraController>(out CameraController cam))
            cam.GetComponent<CameraController>().SetPuzzle04Hack(true);
      
        
        puzzleCamera.GetComponent<Camera>().enabled = true;
        mainCamera.GetComponent<Camera>().enabled = false;

       

        cameraGlide.SetObjectMoveSpeed(0.25f);
        cameraGlide.SetObjectRotateSpeed(0.07f);
        cameraGlide.GlideToMovingPosition(puzzleCamera, CameraStartPosition, cameraTarget, prevCameraPosition);

        GameRoot.camEvents.AddListener(activateCamRotate);


        player.GetComponent<ThirdPersonCharacter>().enabled = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        GCP04.P04W.setPuzzleController(this);

        //Dialog Trigger
        if (convoTriggerNumber != 0)
        {
            GCP04.conversation(convoTriggerNumber);
        }

        GCP04.isInQues = true;

        GCP04.P04W.ShowInputPanel(true);
        GCP04.P04W.ShowFeedbackPanel(true);
        GCP04.P04W.ShowCardPanel(true);
        GCP04.P04W.ShowButtonPanel(true);


        GCP04.P04W.resetButtons();


        assignCards();
        GCP04.P04W.setAnswer1(null);
        setAnsCard1(Vector3.zero);
        GCP04.P04W.setAnswer2(null);
        setAnsCard2(Vector3.zero);

        turnOnVisualVectors();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OnPortalExit()
    {
        inPortal = false;
        GCP04.SetPuzzlePause(true);

        visVectCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        visVectCamera.GetComponent<Camera>().enabled = false;

        player.GetComponent<ThirdPersonUserControl>().enabled = true;


        if (getCorrect())
        {
            ToggleAllTriggers(false);
            cameraGlide.SetObjectMoveSpeed(6.0f);
            cameraGlide.SetObjectRotateSpeed(0.5f);
        }

        if (mainCamera.transform.TryGetComponent<CameraController>(out CameraController cam))
            cam.GetComponent<CameraController>().SetPuzzle04Hack(false);

        cameraRotate.enabled = false;
        cameraGlide.GlideToMovingPosition(puzzleCamera, mainCamera, player, puzzleCamera.transform.position);

        GameRoot.camEvents.AddListener(activateCamPlayer);


        GCP04.isInQues = false;

        GCP04.P04W.ShowInputPanel(false);
        GCP04.P04W.ShowFeedbackPanel(false);
        GCP04.P04W.ShowCardPanel(false);
        GCP04.P04W.ShowButtonPanel(false);


        GCP04.P04W.resetButtons();

        GCP04.P04W.resetCardPos();

        GCP04.P04W.setAnswer1(null);
        setAnsCard1(Vector3.zero);
        GCP04.P04W.setAnswer2(null);
        setAnsCard2(Vector3.zero);

        GCP04.P04W.scalar1.value = 1;
        GCP04.P04W.scalar2.value = 1;

        turnOffVisualVectors();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void assignCards()
    {
        GCP04.P04W.setCardValues(getAnswerCards());
        GCP04.P04W.setCardDisplay(this);
        GCP04.P04W.setFeedbackDisplay(this);
    }



    //GAP TRIGGERS------------------

    public void ToggleAllTriggers(bool value)
    {
        gapTriggers.ToggleFallTriggers(value);
        gapTriggers.ToggleFinishTriggers(value);
        gapTriggers.ToggleResetTriggers(value);
    }



    // VISUAL VECTOR----------------

    private void turnOnVisualVectors()
    {
        VV01.ToggleBasicVectors(true);

        switch (GCP04.Difficulty)
        {
            case 1:
                switch (getDirection())
                {
                    case Direction.X:
                        VV01.setAxisNodes(true, false, false);
                        VV01.setGridBarNodes(true, false, false);
                        break;
                    case Direction.Y:
                        VV01.setAxisNodes(false, true, false);
                        VV01.setGridBarNodes(false, true, false);
                        break;
                }
                break;
            case 2:
                VV01.setAxisNodes(true, true, false);
                VV01.setGridBarNodes(true, true, false);
                break;
            case 3:
                VV01.setAxisNodes(true, true, true);
                VV01.setGridBarNodes(true, true, true);
                break;
        }

    }

    private void turnOffVisualVectors()
    {
        //Basic is player vectors, goal vector, orbs
        VV01.ToggleBasicVectors(false);

        VV01.getXvector().SetActive(false);
        VV01.getYvector().SetActive(false);
        VV01.getZvector().SetActive(false);

        VV01.deactivateAxis();
        VV01.deactivateBarGrid();
        VV01.toggleProjections(false);
        VV01.toggleAnchors(false);
    }


    //CAMERA GLIDE/ROTATE------------------

    private void activateCamRotate()
    {
        GCP04.SetPuzzlePause(false);  
        GameRoot.camEvents.RemoveListener(activateCamRotate);
        player.GetComponent<ThirdPersonCharacter>().enabled = true;
        cameraRotate.enabled = true;
    }

    private void activateCamPlayer()
    {
        GCP04.SetPuzzlePause(false);
        GameRoot.camEvents.RemoveListener(activateCamPlayer);
        cameraRotate.enabled = false;    

        visVectCamera.GetComponent<Camera>().enabled = false;
        puzzleCamera.GetComponent<Camera>().enabled = false;
        mainCamera.GetComponent<Camera>().enabled = true;
    }
}

