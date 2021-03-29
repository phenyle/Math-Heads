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
    private VisualVector VV01;

    [Header("---View Controls---")]
    public GameObject mainCamera;
    public GameObject cameraTarget;
    public GameObject CameraStartPosition;
    private Vector3 prevCameraPosition;
    private float rotateSpeed = 1;
    private float zoomSpeed = 2;

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


    //Answer Cards
    private Vector3 AnswerVector;
    private List<Vector3> cardVectors;
    private List<int> possibleScal1;
    private List<int> possibleScal2;
    private List<Vector3> possibleVec1;
    private List<Vector3> possibleVec2;
    [Tooltip("Limits the number of possible vector combinations\nA larger number means more calculations which for more difficult puzzles (larger scalars/magnitudes, higher dimensions) can take VERY long time")]
    public int MaxPossibleAnswers = 50;
    
    private bool filledAnswers;

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


    // Awake is called when the Instance is Initialized
    void Awake()
    {
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
        VV01 = this.GetComponent<VisualVector>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");


        cardVectors = new List<Vector3>();

        if (isDynamic)
        {
            DynamicVector = DynamicGenerator();
        }
        else
            DynamicVector = new Vector3(0f, 0f, 0f);

        AnswerVector = MinVectorSize + DynamicVector;

        cameraTarget.transform.position += (goalPoint.transform.position - startPoint.transform.position) / 2; 

        scalar1 = 0;
        scalar2 = 0;

        card1 = new Vector3(0f, 0f, 0f);
        card2 = new Vector3(0f, 0f, 0f);

        filledAnswers = false;
        correct = false;

 //       intAnswerGenerator();
        answerGenerators();        

    }
    
    // Update is called once per frame
    void Update()
    { 
        if (portal.GetComponent<PortalTrigger>().inPortal)
        {

            scalar1 = GCP04.P04W.getScalar1Value();
            scalar2 = GCP04.P04W.getScalar2Value();

            if (GCP04.P04W.getAnswer1() != null)
            {
                card1 = GCP04.P04W.getAnswer1().GetComponent<CardVectors>().getCardVector();
                ans1Entered = true;
            }
            else
            {
                card1 = new Vector3(0f, 0f, 0f);
                ans1Entered = false;
            }

            if (GCP04.P04W.getAnswer2() != null)
            {
                card2 = GCP04.P04W.getAnswer2().GetComponent<CardVectors>().getCardVector();
                ans2Entered = true;
            }
            else
            {
                card2 = new Vector3(0f, 0f, 0f);
                ans2Entered = false;
            }

            playerAnswer = scalar1 * card1 + scalar2 * card2;

            GCP04.P04W.setFinalAnswerDisplay(this);


            if(correct)
            {
                player.transform.position = finish.transform.position;
            }


            //Camera Movementa and Zoom around Puzzle
            if(GCP04.P04W.getCameraDragController().isCameraDragging())
            {
                RotateCamera();
            }

            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                mainCamera.transform.position += Vector3.Normalize(mainCamera.transform.forward) * zoomSpeed;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                mainCamera.transform.position -= Vector3.Normalize(mainCamera.transform.forward) * zoomSpeed;
            }

        }

    }

    private void RotateCamera()
    {
        Debug.Log("rotating camera");
        mainCamera.transform.LookAt(cameraTarget.transform.position);
        mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed);
        mainCamera.transform.RotateAround(cameraTarget.transform.position, Vector3.left, Input.GetAxis("Mouse Y") * rotateSpeed);

    }

    /// <summary>
    /// Generates a modified vector based from the MinVectorSize.  If isFractional
    /// is selected, the generated vector will be first divided by 2,3,or 4.  Then
    /// that vector will be mutiplied by the MaxDynamicScalar input.
    /// </summary>
    /// <returns></returns>
    private Vector3 DynamicGenerator()
    {
        Vector3 temp = new Vector3(0f, 0f, 0f);
        int tempScal;

        tempScal = Random.Range(1, MaxDynamicScalar + Random.Range(0,3));
        temp.x = Mathf.RoundToInt(MinVectorSize.x / tempScal);
        tempScal = Random.Range(1, MaxDynamicScalar + Random.Range(0, 3));
        temp.y = Mathf.RoundToInt(MinVectorSize.y / tempScal);
        tempScal = Random.Range(1, MaxDynamicScalar + Random.Range(0, 3));
        temp.z = Mathf.RoundToInt(MinVectorSize.z / tempScal);


        tempScal = Random.Range(1, MaxDynamicScalar);
        temp = temp * tempScal;

        return temp;
    }

    /// <summary>
    /// ------------OUTDATED--------------
    /// Superceded by DynamicGenerator()
    /// 
    /// Used to generate a secondary vector that offsets AnswerVector
    /// Returns a Vector based on MaxScalar, MaxMagnitudes and LimitAxis.
    /// </summary>
    /// <returns></returns>
    private Vector3 WindGenerator()
    {
        int tempMagX;
        int tempMagY;
        int tempMagZ;
        int tempScal;
        Vector3 tempVect = new Vector3(0f, 0f, 0f);

        //We don't want a 0 scalar, this do loop ensures we don't
        do
        {
            tempScal = Random.Range(-MaxDynamicScalar, MaxDynamicScalar + 1);
        } while (tempScal == 0);

        Debug.Log(LimitAxis.ToString());

        do
        {
            tempMagX = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);
            tempMagY = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);
            tempMagZ = Random.Range(-MaxCardMagnitudes, MaxCardMagnitudes + 1);

            switch(LimitAxis)
            {
                case Direction.X:
                    tempVect = new Vector3(tempMagX, 0f, 0f) * tempScal;
                    break;
                case Direction.Y:
                    tempVect = new Vector3(0f, tempMagX, 0f) * tempScal;
                    break;
                case Direction.XY:
                    tempVect = new Vector3(tempMagX, tempMagY, 0f) * tempScal;
                    break;
                case Direction.XYZ:
                    tempVect = new Vector3(tempMagX, tempMagY, tempMagZ) * tempScal;
                    break;
            }

        } while (tempVect == new Vector3(0f, 0f, 0f));

        return tempVect;
    }


    public void intAnswerGenerator()
    {
        int chosenVec;
        int scal1, scal2;
        float coinFlip1, coinFlip2;

        possibleScal1 = new List<int>();
        possibleScal2 = new List<int>();
        possibleVec1 = new List<Vector3>();
        possibleVec2 = new List<Vector3>();


        //Walk Through all Possible Scalars
        coinFlip1 = Random.Range(0.0f, 1.0f);
        coinFlip2 = Random.Range(0.0f, 1.0f);


        if (coinFlip1 < 0.5f)
            for(scal1 = -MaxCardScalar; scal1 <= MaxCardScalar; scal1++)
            {
                if (scal1 == 0)
                    scal1++;
                
                if(coinFlip2 < 0.5f)
                    for(scal2 = -MaxCardScalar; scal2 <= MaxCardScalar; scal2++)
                    {
                        if (scal2 == 0)
                            scal2++;

                        vectorAnswerWalks(scal1, scal2);

                        if (this.filledAnswers)
                            break;
                    }
                else
                    for (scal2 = MaxCardScalar; scal2 >= -MaxCardScalar; scal2--)
                    {
                        if (scal2 == 0)
                            scal2++;

                        vectorAnswerWalks(scal1, scal2);

                        if (this.filledAnswers)
                            break;
                    }

                if (this.filledAnswers)
                    break;

            }
        else
            for (scal1 = MaxCardScalar; scal1 >= -MaxCardScalar; scal1--)
            {
                if (scal1 == 0)
                    scal1++;

                if (coinFlip2 < 0.5f)
                    for (scal2 = -MaxCardScalar; scal2 <= MaxCardScalar; scal2++)
                    {
                        if (scal2 == 0)
                            scal2++;

                        vectorAnswerWalks(scal1, scal2);

                        if (this.filledAnswers)
                            break;
                    }
                else
                    for (scal2 = MaxCardScalar; scal2 >= -MaxCardScalar; scal2--)
                    {
                        if (scal2 == 0)
                            scal2++;

                        vectorAnswerWalks(scal1, scal2);

                        if (this.filledAnswers)
                            break;
                    }

                if (this.filledAnswers)
                    break;

            }



        if (possibleVec1.Count != 0)
        {
            //Higher scalar means lower Magnitudes (in theory)
            //So this part decides if we take a pair from the highest working negative scalar
            //or the highest working positive scalar
            chosenVec = (int)Random.Range(0, possibleVec1.Count - 1);                
            cardVectors.Add(possibleVec1[chosenVec]);
            cardVectors.Add(possibleVec2[chosenVec]);
            cardVectors.Add(randomGenerator());
            cardVectors.Add(randomGenerator());
            answerShuffle();
        }
        else
        {
            backUpGenerator();
        }
    }

    /// <summary>
    /// Walks through all possible vector combinations to find possible combinations
    /// of vectors that may equal the answer vector given the entered scalars.
    /// </summary>
    /// <param name="scal1">taken from the generator, determines Vector1 scalar</param>
    /// <param name="scal2">taken from the generator, determines Vector2 scalar</param>
    public void vectorAnswerWalks(int scal1, int scal2)
    {
        int x1, x2, y1, y2, z1, z2;
        Vector3 emptyVector = new Vector3(0f, 0f, 0f);

        if (AnswerVector.x != 0) //Walk Through all Possible X values
        {
            for (x1 = -MaxCardMagnitudes; x1 <= MaxCardMagnitudes; x1++)
            {
                for (x2 = -MaxCardMagnitudes; x2 <= MaxCardMagnitudes; x2++)
                {
                    if (AnswerVector.y != 0) //Walk Through all Possible Y Values
                    {
                        for (y1 = -MaxCardMagnitudes; y1 <= MaxCardMagnitudes; y1++)
                        {
                            for (y2 = -MaxCardMagnitudes; y2 <= MaxCardMagnitudes; y2++)
                            {
                                if (AnswerVector.z != 0) //Walk Through all Possible Z Values
                                {
                                    for (z1 = -MaxCardMagnitudes; z1 <= MaxCardMagnitudes; z1++)
                                    {
                                        for (z2 = -MaxCardMagnitudes; z2 <= MaxCardMagnitudes; z2++)
                                        {
                                            if (new Vector3(x1, y1, z1) * scal1 + new Vector3(x2, y2, z2) * scal2 == AnswerVector)
                                            {
                                                if (new Vector3(x1, y1, z1) != emptyVector && new Vector3(x2, y2, z2) != emptyVector)
                                                {
                                                    //We have a sovlable pair, add it to the lists
                                                    possibleScal1.Add(scal1);
                                                    possibleScal2.Add(scal2);
                                                    possibleVec1.Add(new Vector3(x1, y1, z1));
                                                    possibleVec2.Add(new Vector3(x2, y2, z2));

                                                    if (possibleVec1.Count > MaxPossibleAnswers)
                                                        this.filledAnswers = true;
                                                }
                                            }
                                            if (this.filledAnswers) //break out of z2 loop
                                                break;
                                        }
                                        if (this.filledAnswers) //break out of z1 loop
                                            break;
                                    }
                                }
                                else //Skip walking through Z | Test X and Y
                                {
                                    z1 = 0;
                                    z2 = 0;
                                    if (new Vector3(x1, y1, z1) * scal1 + new Vector3(x2, y2, z2) * scal2 == AnswerVector)
                                    {
                                        if (new Vector3(x1, y1, z1) != emptyVector && new Vector3(x2, y2, z2) != emptyVector)
                                        {
                                            //We have a sovlable pair, add it to the lists
                                            possibleScal1.Add(scal1);
                                            possibleScal2.Add(scal2);
                                            possibleVec1.Add(new Vector3(x1, y1, z1));
                                            possibleVec2.Add(new Vector3(x2, y2, z2));

                                            if (possibleVec1.Count > MaxPossibleAnswers)
                                                this.filledAnswers = true;
                                        }
                                    }
                                }
                                if (this.filledAnswers)
                                    break;
                            }
                            if (this.filledAnswers)
                                break;
                        }
                    }
                    else //Skip walking through Y | Test X and Z
                    {
                        y1 = 0;
                        y2 = 0;

                        if (AnswerVector.z != 0) //Walk through all Possible Z values
                        {
                            for (z1 = -MaxCardMagnitudes; z1 <= MaxCardMagnitudes; z1++)
                            {
                                for (z2 = -MaxCardMagnitudes; z2 <= MaxCardMagnitudes; z2++)
                                {
                                    if (new Vector3(x1, y1, z1) * scal1 + new Vector3(x2, y2, z2) * scal2 == AnswerVector)
                                    {
                                        if (new Vector3(x1, y1, z1) != emptyVector && new Vector3(x2, y2, z2) != emptyVector)
                                        {
                                            //We have a sovlable pair, add it to the lists
                                            possibleScal1.Add(scal1);
                                            possibleScal2.Add(scal2);
                                            possibleVec1.Add(new Vector3(x1, y1, z1));
                                            possibleVec2.Add(new Vector3(x2, y2, z2));

                                            if (possibleVec1.Count > MaxPossibleAnswers)
                                                this.filledAnswers = true;
                                        }
                                    }
                                    if (this.filledAnswers) // break out of z2 loop
                                        break;
                                }
                                if (this.filledAnswers) //break out of z1 loop
                                    break;
                            }
                        }
                        else //Skip walking through Z and Y | Test X
                        {
                            z1 = 0;
                            z2 = 0;
                            if (new Vector3(x1, y1, z1) * scal1 + new Vector3(x2, y2, z2) * scal2 == AnswerVector)
                            {
                                if (new Vector3(x1, y1, z1) != emptyVector && new Vector3(x2, y2, z2) != emptyVector)
                                {
                                    //We have a sovlable pair, add it to the lists
                                    possibleScal1.Add(scal1);
                                    possibleScal2.Add(scal2);
                                    possibleVec1.Add(new Vector3(x1, y1, z1));
                                    possibleVec2.Add(new Vector3(x2, y2, z2));

                                    if (possibleVec1.Count > MaxPossibleAnswers)
                                        this.filledAnswers = true;
                                }
                            }
                        }
                    }
                    if (this.filledAnswers) //break out of x2 loop
                        break;
                }
                if (this.filledAnswers) //break out of x1 loop
                    break;
            }
        }
        else //Skip walking through X | Test Y and Z
        {
            x1 = 0;
            x2 = 0;

            if (AnswerVector.y != 0) //Walk through all possible Y values
            {
                for (y1 = -MaxCardMagnitudes; y1 <= MaxCardMagnitudes; y1++)
                {
                    for (y2 = -MaxCardMagnitudes; y2 <= MaxCardMagnitudes; y2++)
                    {
                        if (AnswerVector.z != 0) //Walk through all Possible Z values
                        {
                            for (z1 = -MaxCardMagnitudes; z1 <= MaxCardMagnitudes; z1++)
                            {
                                for (z2 = -MaxCardMagnitudes; z2 <= MaxCardMagnitudes; z2++)
                                {
                                    if (new Vector3(x1, y1, z1) * scal1 + new Vector3(x2, y2, z2) * scal2 == AnswerVector)
                                    {
                                        if (new Vector3(x1, y1, z1) != emptyVector && new Vector3(x2, y2, z2) != emptyVector)
                                        {
                                            //We have a sovlable pair, add it to the lists
                                            possibleScal1.Add(scal1);
                                            possibleScal2.Add(scal2);
                                            possibleVec1.Add(new Vector3(x1, y1, z1));
                                            possibleVec2.Add(new Vector3(x2, y2, z2));

                                            if (possibleVec1.Count > MaxPossibleAnswers)
                                                this.filledAnswers = true;
                                        }
                                        if (this.filledAnswers) //break out of z2 loop
                                            break;
                                    }
                                }
                                if (this.filledAnswers) //break out of z1 loop
                                    break;
                            }
                        }
                        else //Skip walking through Z and X | Test Y
                        {
                            z1 = 0;
                            z2 = 0;
                            if (new Vector3(x1, y1, z1) * scal1 + new Vector3(x2, y2, z2) * scal2 == AnswerVector)
                            {
                                if (new Vector3(x1, y1, z1) != emptyVector && new Vector3(x2, y2, z2) != emptyVector)
                                {
                                    //We have a sovlable pair, add it to the lists
                                    possibleScal1.Add(scal1);
                                    possibleScal2.Add(scal2);
                                    possibleVec1.Add(new Vector3(x1, y1, z1));
                                    possibleVec2.Add(new Vector3(x2, y2, z2));

                                    if (possibleVec1.Count > MaxPossibleAnswers)
                                        this.filledAnswers = true;
                                }
                            }
                        }
                        if (this.filledAnswers) //break out of y2 loop
                            break;
                    }
                    if (this.filledAnswers) //break out of y1 loop
                        break;
                }
            }
            else //Skip walking through Y and X | Test Z
            {
                y1 = 0;
                y2 = 0;

                if (AnswerVector.z != 0) // Walk through all Possible Z values
                {
                    for (z1 = -MaxCardMagnitudes; z1 <= MaxCardMagnitudes; z1++)
                    {
                        for (z2 = -MaxCardMagnitudes; z2 <= MaxCardMagnitudes; z2++)
                        {
                            if (new Vector3(x1, y1, z1) * scal1 + new Vector3(x2, y2, z2) * scal2 == AnswerVector)
                            {
                                if (new Vector3(x1, y1, z1) != emptyVector && new Vector3(x2, y2, z2) != emptyVector)
                                {
                                    //We have a sovlable pair, add it to the lists
                                    possibleScal1.Add(scal1);
                                    possibleScal2.Add(scal2);
                                    possibleVec1.Add(new Vector3(x1, y1, z1));
                                    possibleVec2.Add(new Vector3(x2, y2, z2));

                                    if (possibleVec1.Count > MaxPossibleAnswers)
                                        this.filledAnswers = true;
                                }
                            }
                            if (this.filledAnswers) //break out of z2 loop
                                break;
                        }
                        if (this.filledAnswers) //break out of z1 loop
                            break;
                    }
                }
                else //Skip walking through Z, Y and X
                {
                    //If we got here, somehow the Answer Vector is <0,0,0>                          
                }
            }
        }
    }

    /// <summary>
    ///Sometimes, for reasons that elude me, even after testing ALL possible
    ///Combinations to make a pair, we get an empty list
    ///SO, this little code is just to randomly cut the AnswerVector into
    ///two parts and use those.  It's not pretty, and the scalars are always 1
    ///but it doesn't cause it to crash.
    /// </summary>
    public void backUpGenerator()
    {
        int split;
        Vector3 ansVecPart1, ansVecPart2;

        split = (int)Random.Range(2, 5);
        ansVecPart1.x = Mathf.RoundToInt(AnswerVector.x / split);
        split = (int)Random.Range(2, 5);
        ansVecPart1.y = Mathf.RoundToInt(AnswerVector.y / split);
        split = (int)Random.Range(2, 5);
        ansVecPart1.z = Mathf.RoundToInt(AnswerVector.z / split);

        ansVecPart2 = AnswerVector - ansVecPart1;

        cardVectors.Add(ansVecPart1);
        cardVectors.Add(ansVecPart2);
        cardVectors.Add(randomGenerator());
        cardVectors.Add(randomGenerator());
        answerShuffle();
    }


    /// <summary>
    /// ------OUTDATED------------
    /// Superceded by intAnswerGenerator()
    /// 
    /// Used for generating Answer Cards based on the Vector created from
    /// the AnswerVector.
    /// </summary>
    private void answerGenerators()
    {
        Vector3 answer1a;
        Vector3 answer1b;
        Vector3 answer2a;
        Vector3 answer2b;

        answer1a = randomGenerator();
        cardVectors.Add(answer1a);
        answer1b = secAnsCard(answer1a);
        cardVectors.Add(answer1b);
        answer2a = randomGenerator();
        cardVectors.Add(answer2a);
        answer2b = secAnsCard(answer2a);
        cardVectors.Add(answer2b);

        answerShuffle();
    }

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
        //cycle through cards:
        for (int i = -MaxCardScalar; i <= MaxCardScalar; i++)
        {
            for (int j = 0; j < cardVectors.Count; j++)
            {
                if (test * i == cardVectors[j] || test == cardVectors[j] * i)
                {
                    unique = false;
                    break;
                }
                else
                    unique = true;
            }
        }

        return unique;
    }

    /// <summary>
    /// Generates random and unique vectors for use by the AnswerGenerator function
    /// only returns vectors that are not <0,0,0> and are not already generated
    /// in the cardVectors list.
    /// </summary>
    /// <returns></returns>
    private Vector3 randomGenerator()
    {
        int tempMagX;
        int tempMagY;
        int tempMagZ;
        int tempScal;
        Vector3 tempVect = new Vector3(0f, 0f, 0f);

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
                        switch(LimitAxis)
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


        } while (!testUnique(tempVect));

        return tempVect;

    }

    /// <summary>
    /// ----------OUTDATED----------------
    /// Retruns a Vector based on the displacement from the input Vector to the
    /// Answer Vector.  Used for generating a "pair" of vectors that lead to the
    /// correct answer.  Will not return a <0,0,0> vector or a duplicate vector.
    /// Vector MAY be fractional based on Scalar requirements.
    /// </summary>
    /// <param name="first"> input vector used to find displacement </param>
    /// <returns></returns>
    private Vector3 secAnsCard(Vector3 first)
    {
        int tempScal;
        Vector3 tempVect = new Vector3(0f, 0f, 0f);

        do
        {
            //We don't want a 0 scalar, this do loop ensures we don't
            do
            {
                tempScal = Random.Range(-MaxCardScalar, MaxCardScalar + 1);
            } while (tempScal == 0);

            tempVect = (AnswerVector - first);
            tempVect = tempVect / tempScal;                       

        } while (!testUnique(tempVect));

        return tempVect;
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
        return AnswerVector;
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

    public bool checkFinalAnswer()
    {
        //Need to add a check that both Answer Slots are being used

        if (playerAnswer == AnswerVector)
        {
            correct = true;
            player.GetComponent<GrappleCode>().grappleToGoal();
            return true;


        }
        else
        {
            correct = false;
            return false;
        }

    }

}

