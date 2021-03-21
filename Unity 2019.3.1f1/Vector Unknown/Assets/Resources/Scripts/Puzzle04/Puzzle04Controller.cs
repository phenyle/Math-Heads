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
    public GameObject goal;
    public GameObject goalPoint;
    public GameObject reset;
    public GameObject finish;
    private VisualVector VV01;

    [Header("---View Controls---")]
    public GameObject CameraPosition;
    [Range(-5, 25)]
    public float cameraHeight = 7;
    [Range(-50, 0)]
    public float cameraZoom;

    [Header("---Puzzle Vectors---")]
    public Vector3 MinVectorSize;
    public bool isDynamic;
    public bool isFractional;
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

    //Player Answers
    private int scalar1;
    private int scalar2;
    private Vector3 card1;
    private Vector3 card2;
    private Vector3 playerAnswer;
    private bool correct;

    private GameControllerPuzzle04 GCP04;


    // Awake is called when the Instance is Initialized
    void Awake()
    {
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
        VV01 = this.GetComponent<VisualVector>();

        cardVectors = new List<Vector3>();

        if (isDynamic)
        {
            DynamicVector = DynamicGenerator();
        }
        else
            DynamicVector = new Vector3(0f, 0f, 0f);

        AnswerVector = MinVectorSize + DynamicVector;

        scalar1 = 0;
        scalar2 = 0;

        card1 = new Vector3(0f, 0f, 0f);
        card2 = new Vector3(0f, 0f, 0f);

        correct = false;


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
            }
            else
                card1 = new Vector3(0f, 0f, 0f);

            if (GCP04.P04W.getAnswer2() != null)
            {
                card2 = GCP04.P04W.getAnswer2().GetComponent<CardVectors>().getCardVector();
            }
            else
                card2 = new Vector3(0f, 0f, 0f);

            playerAnswer = scalar1 * card1 + scalar2 * card2;
            GCP04.P04W.setFinalAnswerDisplay(this);


            if(correct)
            {
                player.transform.position = finish.transform.position;
            }



            //    Debug.Log(AnswerVector);
            //    camera.GetComponent<Camera2DFollow>().setCameraHeight(cameraHeight);
            //    camera.GetComponent<Camera2DFollow>().setCameraZoom(cameraZoom);

        }

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


        if (isFractional)
        {
            tempScal = Random.Range(2, 4);

            temp = MinVectorSize / tempScal;
        }
        else
            temp = MinVectorSize;


        tempScal = Random.Range(1, MaxDynamicScalar);
        temp = temp * tempScal;

        return temp;
    }

    /// <summary>
    /// ------------OUTDATED--------------
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
            tempScal = Random.Range(-1 * MaxDynamicScalar, MaxDynamicScalar);
        } while (tempScal == 0);

        Debug.Log(LimitAxis.ToString());

        do
        {
            tempMagX = Random.Range(-1 * MaxCardMagnitudes, MaxCardMagnitudes);
            tempMagY = Random.Range(-1 * MaxCardMagnitudes, MaxCardMagnitudes);
            tempMagZ = Random.Range(-1 * MaxCardMagnitudes, MaxCardMagnitudes);

            if (LimitAxis.ToString().CompareTo("X") == 0)
            {
                tempVect = new Vector3(tempMagX, 0f, 0f) * tempScal;
            }
            else if (LimitAxis.ToString().CompareTo("Y") == 0)
            {
                tempVect = new Vector3(0f, tempMagX, 0f) * tempScal;
            }
            else if (LimitAxis.ToString().CompareTo("XY") == 0)
            {
                tempVect = new Vector3(tempMagX, tempMagY, 0f) * tempScal;
            }
            else if (LimitAxis.ToString().CompareTo("XYZ") == 0)
            {
                tempVect = new Vector3(tempMagX, tempMagY, tempMagZ) * tempScal;
            }

        } while (tempVect == new Vector3(0f, 0f, 0f));

        return tempVect;
    }


    /// <summary>
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
        answer2b = randomGenerator();
        cardVectors.Add(answer2b);

        //Shuffle Cards
        int count = cardVectors.Count;
        int last = count - 1;
        for(int i = 0; i < last; i++)
        {
            int r = Random.Range(i, count);
            Vector3 temp = cardVectors[i];
            cardVectors[i] = cardVectors[r];
            cardVectors[r] = temp;
        }

    }

    /// <summary>
    /// Generates random and unique vectors for use by the AnswerGenerator function
    /// only returns vectors that are not <0,0,0> and are not already generated.
    /// </summary>
    /// <returns></returns>
    private Vector3 randomGenerator()
    {
        bool unique = true;
        int tempMagX;
        int tempMagY;
        int tempMagZ;
        int tempScal;
        Vector3 tempVect = new Vector3(0f, 0f, 0f);

        do
        {
            unique = true;

            //We don't want a 0 scalar, this do loop ensures we don't
            do
            {
                tempScal = Random.Range(-1 * MaxCardScalar, MaxCardScalar);           } while (tempScal == 0);


            //While scalar cannot be 0, some magnitudes MAY be zero,
            //this is acceptable as long as not ALL the magnitudes are zero
            //this do while loop ensure we don't end with a zero vector.
            do
            {
                tempMagX = Random.Range(-1 * MaxCardMagnitudes, MaxCardMagnitudes);
                tempMagY = Random.Range(-1 * MaxCardMagnitudes, MaxCardMagnitudes);
                tempMagZ = Random.Range(-1 * MaxCardMagnitudes, MaxCardMagnitudes);

                if (GCP04.Difficulty == 1)
                {
                    if (LimitAxis.ToString().CompareTo("X") == 0)
                    {
                        tempVect = new Vector3(tempMagX, 0f, 0f) * tempScal;
                    }
                    else if (LimitAxis.ToString().CompareTo("Y") == 0)
                    {
                        tempVect = new Vector3(0f, tempMagY, 0f) * tempScal;
                    }

                }
                else if (GCP04.Difficulty == 2)
                {
                    tempVect = new Vector3(tempMagX, tempMagY, 0f) * tempScal;
                }
                else if (GCP04.Difficulty == 3)
                {
                    tempVect = new Vector3(tempMagX, tempMagY, tempMagZ) * tempScal;
                }
            } while (tempVect == new Vector3(0f, 0f, 0f));


            //Test that the new Random Vector is not a Duplicate or scalar
            //cycle through cards:
            for (int i = 0; i < cardVectors.Count; i++)
            {
                //cycle through scalars:
                for (int j = -10; j < 11; j++)
                {                    
                    if (tempVect * j == cardVectors[i])
                    {
                        unique = false;
                        break;
                    }
                    else
                        unique = true;                

                }

                if (!unique)
                    break;
            }

        } while (!unique);

        return tempVect;

    }

    /// <summary>
    /// Retruns a Vector based on the displacement from the input Vector to the
    /// Answer Vector.  Used for generating a "pair" of vectors that lead to the
    /// correct answer.  Will not return a <0,0,0> vector or a duplicate vector.
    /// Vector MAY be fractional based on Scalar requirements.
    /// </summary>
    /// <param name="first"> input vector used to find displacement </param>
    /// <returns></returns>
    private Vector3 secAnsCard(Vector3 first)
    {
        bool unique = true;
        int tempScal;
        Vector3 tempVect = new Vector3(0f, 0f, 0f);

        do
        {
            unique = true;

            //We don't want a 0 scalar, this do loop ensures we don't
            do
            {
                tempScal = Random.Range(-1 * MaxCardScalar, MaxCardScalar);
            } while (tempScal == 0);

            tempVect = (AnswerVector - first);
            tempVect = tempVect / tempScal;

            //Test that the new Random Vector is not a Duplicate or scalar
            //cycle through cards:
            for (int i = 0; i < cardVectors.Count; i++)
            {
                //cycle through scalars:
                for (int j = -10; j < 11; j++)
                {
                    if (tempVect * j == cardVectors[i])
                    {
                        unique = false;
                        break;
                    }
                    else
                        unique = true;

                }

                if (!unique)
                    break;
            }

        } while (!unique);

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

    public Transform getCameraTransform()
    {
        return CameraPosition.transform;
    }

    public VisualVector getVisualVector()
    {
        return VV01;
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

