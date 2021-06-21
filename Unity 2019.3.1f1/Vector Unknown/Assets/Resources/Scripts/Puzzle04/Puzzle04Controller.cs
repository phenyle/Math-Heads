using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    X,
    Y,
    XY,
    XYZ
};

public class Puzzle04Controller : MonoBehaviour
{
    [Header("---Object Assignment---")]
    public GameObject player;    
    public GameObject portal;
    public GameObject startPoint;
    public GameObject goal;
    public GameObject goalPoint;
    public GameObject reset;
    public GameObject finish;
    public int puzzleID;
    public bool isActive;
    private VisualVector VV01;

    [Header("---View Controls---")]
    public GameObject mainCamera;
    public GameObject cameraTarget;
    public GameObject CameraStartPosition;
    private Vector3 prevCameraPosition;
    private float rotateSpeed = 1;
    private float zoomSpeed = 2;
    private bool cameraControls;
    public ObjectGlide cameraGlide;
    public CameraRotate cameraRotate;

    [Header("---Puzzle Vectors---")]
    public Vector3 MinVectorSize;
    public bool isDynamic;
    [Range(1,10)]
    public int MaxDynamicScalar;
    public Direction LimitAxis = new Direction();
    private Vector3 DynamicVector;

    [Header("---Answer Card Vectors---")]
    [Range(1, 10)]
    public int MaxCardScalar;
    [Range(1, 10)]
    public int MaxCardMagnitudes;

    //Grapple Kit
    [Header("Grapple Kit")]
    public GameObject grappleKit;
    //public GrappleCode grappleKit;

    [Header("---Conversation Select---")]
    [Range(0, 9)]
    [Tooltip("Sets the dialog number used.\n0 val means no dialog\n1-9 selects that specific dialog")]
    public int convoTriggerNumber;


    //Answer Cards
    private Vector3 startingAnswer;
    private Vector3 finalAnswerVector;
    private List<Vector3> cardVectors;

    //Player Answers
    private int scalar1;
    private int scalar2;
    private Vector3 card1;
    private Vector3 card2;
    private Vector3 playerAnswer;
    private bool ans1Entered;
    private bool ans2Entered;
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
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraControls = false;

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


        cameraGlide = this.GetComponent<ObjectGlide>();
        cameraGlide.glideObject = mainCamera;

        cameraRotate = this.GetComponent<CameraRotate>();
        cameraRotate.rotCamera = mainCamera.transform;

        //Final Answer is created after generating the cards
        startingAnswer = MinVectorSize + DynamicVector;

        //Move camera target inbetween the starting position and the goal
        cameraTarget.transform.position += (goalPoint.transform.position - startPoint.transform.position) / 2; 

        correct = false;

        obstacleTime = 0.0f;
        puzzleData = new obsData();
        puzzleData.obsID = puzzleID;


 //       intAnswerGenerator();
        AnswerGenerators();        

    }
    
    // Update is called once per frame
    void Update()
    {
        if (portal.GetComponent<PortalTrigger>().inPortal)
        {
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


            if (cameraControls)
            {
                //Mouse Camera Movement and Zoom Controls
                if (GCP04.P04W.getCameraDragController().isCameraDragging())
                {
                    RotateCamera();
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0f && (mainCamera.transform.position - cameraTarget.transform.position).magnitude > 5)
                {
                    mainCamera.transform.position += Vector3.Normalize(mainCamera.transform.forward) * zoomSpeed;
                }

                if (Input.GetAxis("Mouse ScrollWheel") < 0f && (mainCamera.transform.position - cameraTarget.transform.position).magnitude < 150)
                {
                    mainCamera.transform.position -= Vector3.Normalize(mainCamera.transform.forward) * zoomSpeed;
                }

                //Keyboard Camera Movement and Zoom Controls
                if (Input.GetKey(KeyCode.J))
                {
                    mainCamera.transform.LookAt(cameraTarget.transform.position);
                    mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.up, rotateSpeed * 0.5f);
                }

                if (Input.GetKey(KeyCode.L))
                {
                    mainCamera.transform.LookAt(cameraTarget.transform.position);
                    mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.up, -rotateSpeed * 0.5f);
                }

                if (Input.GetKey(KeyCode.I))
                {
                    mainCamera.transform.LookAt(cameraTarget.transform.position);
                    mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.left, -rotateSpeed * 0.5f);
                }

                if (Input.GetKey(KeyCode.K))
                {
                    mainCamera.transform.LookAt(cameraTarget.transform.position);
                    mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.left, rotateSpeed * 0.5f);
                }

                if (Input.GetKey(KeyCode.U) && (mainCamera.transform.position - cameraTarget.transform.position).magnitude > 5)
                {
                    mainCamera.transform.position += Vector3.Normalize(mainCamera.transform.forward) * zoomSpeed * 0.25f;
                }

                if (Input.GetKey(KeyCode.O) && (mainCamera.transform.position - cameraTarget.transform.position).magnitude < 150)
                {
                    mainCamera.transform.position -= Vector3.Normalize(mainCamera.transform.forward) * zoomSpeed * 0.25f;
                }
            }




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
            isActive = false;

    }

    private void RotateCamera()
    {
        Debug.Log("rotating camera");
        mainCamera.transform.LookAt(cameraTarget.transform.position);
        mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed);
        mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.left, Input.GetAxis("Mouse Y") * rotateSpeed);

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
        float tempScal;

        tempScal = Random.Range(1f, MaxDynamicScalar);
        temp.x = Mathf.RoundToInt(MinVectorSize.x / tempScal);
        tempScal = Random.Range(1f, MaxDynamicScalar);
        temp.y = Mathf.RoundToInt(MinVectorSize.y / tempScal);
        tempScal = Random.Range(1f, MaxDynamicScalar);
        temp.z = Mathf.RoundToInt(MinVectorSize.z / tempScal);


        tempScal = Random.Range(0.5f, MaxDynamicScalar);
        temp = temp * tempScal;
        temp.x = Mathf.RoundToInt(temp.x);
        temp.y = Mathf.RoundToInt(temp.y);
        temp.z = Mathf.RoundToInt(temp.z);

        return temp;
    }

    /// <summary>
    /// Used for generating Answer Cards based on the Vector created from
    /// the AnswerVector.
    /// Creates the first Scalar and sends it both the first and second cards
    /// for calculating the finalAnswerVector (set in secondAnsCard()
    /// </summary>
    private void AnswerGenerators()
    {
        int scal1;
        Vector3 answer1a;
        Vector3 answer1b;
        Vector3 answer2a;
        Vector3 answer2b;

        do
        {
            scal1 = Random.Range(-MaxCardScalar, MaxCardScalar + 1);
        } while (scal1 == 0);

        answer1a = firstAnsCard(scal1);
        cardVectors.Add(answer1a);
        answer1b = secondAnsCard(scal1, answer1a);
        cardVectors.Add(answer1b);
        answer2a = randomGenerator();
        cardVectors.Add(answer2a);
        answer2b = randomGenerator();
        cardVectors.Add(answer2b);

        puzzleData.e_Vect1 = answer1a;
        puzzleData.e_Vect2 = answer1b;

        answerShuffle();
    }

    /// <summary>
    /// Creates the first Answer card for use in the puzzle.
    /// This card is generated SIMILAR to the randomGenerator().
    /// Uses the input scal value to divide then round the vector
    /// The round value is the final version of the card which is 
    /// returned
    /// </summary>
    /// <param name="scal">Used to divide the random vector</param>
    /// <returns>A Vector3 rounded in all dimensions (or 0)</returns>
    private Vector3 firstAnsCard(int scal)
    {
        int uniqueEscape; //used to prevent getting stuck in an endless do/while
        int tempMagX;
        int tempMagY;
        int tempMagZ;
        Vector3 tempVect = Vector3.zero;

        uniqueEscape = 0;
        do
        {
            //While scalar cannot be 0, some magnitudes MAY be zero,
            //this is acceptable as long as not ALL the magnitudes are zero
            //this do while loop ensure we don't end with a zero vector.
            do
            {
                tempMagX = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);
                tempMagY = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);
                tempMagZ = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);


                switch (GCP04.Difficulty)
                {
                    case 1:
                        switch (LimitAxis)
                        {
                            case Direction.X:
                                tempVect.x = Mathf.RoundToInt(tempMagX / scal);
                                break;
                            case Direction.Y:
                                tempVect.y = Mathf.RoundToInt(tempMagY / scal);
                                break;
                        }
                        break;
                    case 2:
                        tempVect.x = Mathf.RoundToInt(tempMagX / scal);
                        tempVect.y = Mathf.RoundToInt(tempMagY / scal);
                        break;
                    case 3:
                        tempVect.x = Mathf.RoundToInt(tempMagX / scal);
                        tempVect.y = Mathf.RoundToInt(tempMagY / scal);
                        tempVect.z = Mathf.RoundToInt(tempMagZ / scal);
                        break;
                }

            } while (tempVect == new Vector3(0f, 0f, 0f));
            //while the vector is all Zeros (random chance), try again
            //also if the magnitude is 1, try again.  This is a tweak for Level1
            //since magnitude 1 means every other card will be a scalar of it
            //causing the system to lock when testing unique
            uniqueEscape++;
        } while (!testUnique(tempVect) && uniqueEscape < 10);
        //while the vector is a duplicate/scalar of previous card, try again



        return tempVect;
    }

    /// <summary>
    /// Used for creating the Second answer card.  This second card is the displacement
    /// from the First card (times its scalar) and the desired startingAnswer.
    /// After this card is created it adjust the finalAnswerVector to a value
    /// that both card1 and card2 can create.
    /// </summary>
    /// <param name="scal1">the scalar by which to multiply ans1 vector by</param>
    /// <param name="ans1">the previously determined vector for which to base the
    /// displacement needed to meet the startingAnswer</param>
    /// <returns></returns>
    private Vector3 secondAnsCard(int scal1, Vector3 ans1)
    {
        int uniqueEscape; //used to prevent getting stuck in an endless do/while
        int scal2;
        Vector3 tempVect = Vector3.zero;

        uniqueEscape = 0;
        do
        {
            tempVect = (startingAnswer - (ans1 * scal1));

            //We don't want a 0 scalar, this do loop ensures we don't
            do
            {
                scal2 = Random.Range(-MaxCardScalar, MaxCardScalar + 1);
            } while (scal2 == 0);


            tempVect.x = Mathf.RoundToInt(tempVect.x / scal2);
            tempVect.y = Mathf.RoundToInt(tempVect.y / scal2);
            tempVect.z = Mathf.RoundToInt(tempVect.z / scal2);

            uniqueEscape++;
        } while (!testUnique(tempVect) && uniqueEscape < 10);
        //while the vector is a duplicate/scalar of previous card, try again

        puzzleData.e_Scal1 = scal1;
        puzzleData.e_Scal2 = scal2;

        //Final Answer Adjustment
        finalAnswerVector = (ans1 * scal1) + (tempVect * scal2);

        puzzleData.answer = finalAnswerVector;

        return tempVect;
    }

    /// <summary>
    /// Generates random and unique vectors for use by the AnswerGenerator function
    /// only returns vectors that are not <0,0,0> and are not already generated
    /// in the cardVectors list.
    /// </summary>
    /// <returns></returns>
    private Vector3 randomGenerator()
    {
        int uniqueEscape; //used to prevent getting stuck in an endless do/while
        int tempMagX;
        int tempMagY;
        int tempMagZ;
        int tempScal;
        Vector3 tempVect = Vector3.zero;

        uniqueEscape = 0;
        do
        {
            //We don't want a 0 scalar, this do loop ensures we don't
            do
            {
                tempScal = Random.Range(-MaxCardScalar, MaxCardScalar + 1);
            } while (tempScal == 0);


            //While scalar cannot be 0, some magnitudes MAY be zero,
            //this is acceptable as long as not ALL the magnitudes are zero
            //this do while loop ensure we don't end with a zero vector.
            do
            {
                tempMagX = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);
                tempMagY = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);
                tempMagZ = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);


                switch (GCP04.Difficulty)
                {
                    case 1:
                        switch (LimitAxis)
                        {
                            case Direction.X:
                                tempVect = new Vector3(tempMagX, 0f, 0f) * tempScal;
                                break;
                            case Direction.Y:
                                tempVect = new Vector3(0f, tempMagY, 0f) * tempScal;
                                break;
                        }
                        break;
                    case 2:
                        tempVect = new Vector3(tempMagX, tempMagY, 0f) * tempScal;
                        break;
                    case 3:
                        tempVect = new Vector3(tempMagX, tempMagY, tempMagZ) * tempScal;
                        break;
                }

            } while (tempVect == new Vector3(0f, 0f, 0f));
            //while the vector is all Zeros (random chance), try again

            uniqueEscape++;
        } while (!testUnique(tempVect) && uniqueEscape < 10);
        //while the vector is a duplicate/scalar of previous card, try again

        return tempVect;
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
            int r = Random.Range(i, count);
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
        return MinVectorSize;
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

    public void setCameraControls(bool val)
    {
        cameraControls = val;
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
                correct = true;
                grappleKit.GetComponent<GrappleCode>().InitGrapple(correct, finalAnswerVector, goalPoint);
                grappleKit.GetComponent<GrappleCode>().grappleToGoal(this);


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
                grappleKit.GetComponent<GrappleCode>().InitGrapple(correct, VV01.vector2end.transform.position, goalPoint);
                grappleKit.GetComponent<GrappleCode>().grappleToGoal(this);
                return false;
            }
        }
        else
            return false;

    }

}

