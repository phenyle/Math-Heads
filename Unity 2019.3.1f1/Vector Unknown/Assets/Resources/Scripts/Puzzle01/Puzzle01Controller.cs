using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public enum Direction
{
    X,
    Y,
    XY,
    XYZ
};

public class Puzzle01Controller : MonoBehaviour
{
    [Header("Object Assignment")]
    public GameObject player;    
    public GameObject start;
    public GameObject goal;
    public GameObject reset;
    public GameObject finish;

    [Header("View Controls")]
    [Range(-5, 25)]
    public float cameraHeight = 7;
    [Range(-50, 0)]
    public float cameraZoom;

    [Header("Puzzle Vectors")]
    public Vector3 GapVector;
    public bool wind;
    [Range(1,10)]
    public int MaxScalar;
    [Range(1, 10)]
    public int MaxMagnitudes;
    public Direction LimitAxis = new Direction();
    private Vector3 WindVector;


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

    private GameControllerPuzzle01 GCP01;


    // Start is called before the first frame update
    void Start()
    {
        GCP01 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>();

        cardVectors = new List<Vector3>();

        if (wind)
        {
            WindVector = WindGenerator();
        }
        else
            WindVector = new Vector3(0f, 0f, 0f);

        AnswerVector = GapVector + WindVector;

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
        if (start.GetComponent<PortalTrigger>().inPortal)
        {

            scalar1 = GCP01.P01W.getScalar1Value();
            scalar2 = GCP01.P01W.getScalar2Value();

            if (GCP01.P01W.getAnswer1() != null)
            {
                card1 = GCP01.P01W.getAnswer1().GetComponent<CardVectors>().getCardVector();
            }
            else
                card1 = new Vector3(0f, 0f, 0f);

            if (GCP01.P01W.getAnswer2() != null)
            {
                card2 = GCP01.P01W.getAnswer2().GetComponent<CardVectors>().getCardVector();
            }
            else
                card2 = new Vector3(0f, 0f, 0f);

            playerAnswer = scalar1 * card1 + scalar2 * card2;
            GCP01.P01W.setFinalAnswerDisplay(this);


            if(correct)
            {
                player.transform.position = finish.transform.position;
            }



            //    Debug.Log(AnswerVector);
            //    camera.GetComponent<Camera2DFollow>().setCameraHeight(cameraHeight);
            //    camera.GetComponent<Camera2DFollow>().setCameraZoom(cameraZoom);

        }

    }


    Vector3 WindGenerator()
    {
        int tempMagX = Random.Range(-1 * MaxMagnitudes, MaxMagnitudes);
        int tempMagY = Random.Range(-1 * MaxMagnitudes, MaxMagnitudes);
        int tempMagZ = Random.Range(-1 * MaxMagnitudes, MaxMagnitudes);
        int tempScal;

        //We don't want a 0 scalar, this do loop ensures we don't
        do
        {
            tempScal = Random.Range(-1 * MaxScalar, MaxScalar);
        } while (tempScal == 0);


        Debug.Log(LimitAxis.ToString());

        if (LimitAxis.ToString().CompareTo("X") == 0)
        {
            return new Vector3(tempMagX, 0f, 0f) * tempScal;
        }
        else if (LimitAxis.ToString().CompareTo("Y") == 0)
        {
            return new Vector3(0f, tempMagX, 0f) * tempScal;
        }
        else if (LimitAxis.ToString().CompareTo("XY") == 0)
        {
            return new Vector3(tempMagX, tempMagY, 0f) * tempScal;
        }
        else if (LimitAxis.ToString().CompareTo("XYZ") == 0)
        {
            return new Vector3(tempMagX, tempMagY, tempMagZ) * tempScal;
        }
        else
            return new Vector3(0f, 0f, 0f);
    }

    private void answerGenerators()
    {
        Vector3 answer1;
        Vector3 answer2;
        Vector3 random1;
        Vector3 random2;

        answer1 = randomGenerator();
        cardVectors.Add(answer1);
        answer2 = secAnsCard(answer1);
        cardVectors.Add(answer2);
        random1 = randomGenerator();
        cardVectors.Add(random1);
        random2 = randomGenerator();
        cardVectors.Add(random2);

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
            tempMagX = Random.Range(-1 * MaxMagnitudes, MaxMagnitudes);
            tempMagY = Random.Range(-1 * MaxMagnitudes, MaxMagnitudes);
            tempMagZ = Random.Range(-1 * MaxMagnitudes, MaxMagnitudes);
            
            //We don't want a 0 scalar, this do loop ensures we don't
            do
            {
                tempScal = Random.Range(-1 * MaxScalar, MaxScalar);
            } while (tempScal == 0);

            if (GCP01.Difficulty == 1)
            {
                if (LimitAxis.ToString().CompareTo("X") == 0)
                {
                    tempVect = new Vector3(tempMagX, 0f, 0f) * tempScal;
                }
                else if (LimitAxis.ToString().CompareTo("Y") == 0)
                {
                    tempVect = new Vector3(0f, tempMagX, 0f) * tempScal;
                }

            }
            else if (GCP01.Difficulty == 2)
            {
                tempVect = new Vector3(tempMagX, tempMagY, 0f) * tempScal;
            }
            else if (GCP01.Difficulty == 3)
            {
                tempVect = new Vector3(tempMagX, tempMagY, tempMagZ) * tempScal;
            }


            //Test that the new Random Vector is not a Duplicate
            for(int i = 0; i < cardVectors.Count; i++)
            {
                if (tempVect == cardVectors[i])
                {
                    unique = false;
                }
                else
                    unique = true;
            }

        } while (!unique);

        return tempVect;

    }

    private Vector3 secAnsCard(Vector3 first)
    {
        bool unique = true;
        int tempScal;
        Vector3 tempVect = new Vector3(0f, 0f, 0f);

        do
        {
            //We don't want a 0 scalar, this do loop ensures we don't
            do
            {
                tempScal = Random.Range(-1 * MaxScalar, MaxScalar);
            } while (tempScal == 0);

            tempVect = (AnswerVector - first) * tempScal;

            //Test that the new Vector is not a Duplicate
            for (int i = 0; i < cardVectors.Count; i++)
            {
                if (tempVect == cardVectors[i])
                {
                    unique = false;
                }
                else
                    unique = true;
            }

        } while (!unique);

        return tempVect;
    }

    public Vector3 getGapVector()
    {
        return GapVector;
    }

    public Vector3 getWindVector()
    {
        return WindVector;
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

    public bool checkFinalAnswer()
    {
        if (playerAnswer == AnswerVector)
        {
            correct = true;
            return true;


        }
        else
        {
            correct = false;
            return false;
        }

    }

}

