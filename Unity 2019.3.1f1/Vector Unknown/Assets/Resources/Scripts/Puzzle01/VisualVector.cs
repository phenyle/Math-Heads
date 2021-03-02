using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualVector : MonoBehaviour
{
    private Puzzle01Controller PC01;

    private Vector3 puzzleScale;
    private float overallScale;

    //Puzzle Vector Components
    private GameObject gapVector;
    private GameObject windVector;
    private GameObject answerVector;
    private Vector3 start;
    private Vector3 goal;
    private Vector3 windEnd;

    //Player Vector Components
    private GameObject vector1;
    private GameObject vector2;
    private GameObject finalVector;
    private Vector3 vector1end;
    private Vector3 vector2end;

    //Misc Controls
    private bool fudgeX;
    private bool fudgeY;
    private bool fudgeZ;



    // Start is called before the first frame update
    void Start()
    {  
        PC01 = GetComponent<Puzzle01Controller>();

        setFudge();

        gapVector = GameObject.Find("gapVectorGO");
        windVector = GameObject.Find("windVectorGO");
        answerVector = GameObject.Find("answerVectorGO");

        vector1 = GameObject.Find("Vector1GO");
        vector2 = GameObject.Find("Vector2GO");
        finalVector = GameObject.Find("finalVectorGO");

        gapVector.SetActive(false);
        windVector.SetActive(true);
        answerVector.SetActive(false);

        vector1.SetActive(false);
        vector2.SetActive(false);
        finalVector.SetActive(false);



        start = GameObject.Find("StartProjectionPoint").transform.position;
        goal = GameObject.Find("GoalProjectionPoint").transform.position;
        vector1end = GameObject.Find("Vector1end").transform.position;
        vector2end = GameObject.Find("Vector2end").transform.position;
        windEnd = GameObject.Find("windend").transform.position;



        overallScale = setOverallScale();
        puzzleScale = setPuzzleScale();






        VectorBetweenPoints(gapVector, goal, start, 0.25f);

        windEnd = goal + PC01.getWindVector() * overallScale;
        VectorBetweenPoints(windVector, goal, windEnd, 0.25f);



    }

    // Update is called once per frame
    void Update()
    {
        if (PC01.getAnsCard1() != null)
            vector1end = start + rescaledVector(PC01.getAnsCard1() * PC01.getScalar1(), true);
        else
            vector1end = start;

        if (PC01.getAnsCard2() != null)
            vector2end = vector1end + rescaledVector(PC01.getAnsCard2() * PC01.getScalar2(), true);
        else
            vector2end = vector1end;

        VectorBetweenPoints(vector1, start, vector1end, 0.25f);
        VectorBetweenPoints(vector2, vector1end, vector2end, 0.25f);

        if (PC01.getAnsCard1() != null && PC01.getAnsCard2() != null)
        {
            finalVector.SetActive(true);
            VectorBetweenPoints(finalVector, start, vector2end, 0.25f);
        }
        else
            finalVector.SetActive(false);


        windEnd = goal + PC01.getWindVector() * overallScale;
        VectorBetweenPoints(windVector, goal, windEnd, 0.25f);


    }

    private float setOverallScale()
    {
        Vector3 worldVector, gapVector;
        float worldMag, gapMag;

        worldVector = start - goal;
        gapVector = PC01.getGapVector();

        worldMag = worldVector.magnitude;
        gapMag = gapVector.magnitude;

        return worldMag / gapMag;
    }


    private Vector3 setPuzzleScale()
    {
        Vector3 worldVector, gapVector, returnVector;  

        worldVector = start - goal;
        gapVector = PC01.getGapVector();

        //size you want, divided by the size you have
        //X-scale
        if (gapVector.x != 0)
            returnVector.x = Mathf.Abs(worldVector.x / gapVector.x);
        else
            returnVector.x = 0;

        //Y-scale
        if (gapVector.y != 0)
            returnVector.y = Mathf.Abs(worldVector.y / gapVector.y);
        else
            returnVector.y = 0;

        //Z-scale
        if (gapVector.z != 0)
            returnVector.z = Mathf.Abs(worldVector.z / gapVector.z);
        else
            returnVector.z = 0;

        
        return returnVector;
    }

    private Vector3 rescaledVector(Vector3 original, bool fudge)
    {
        Vector3 temp, worldVector, gapVector;

        worldVector = start - goal;
        gapVector = PC01.getGapVector();

        if (fudge)
        {
            if (gapVector.x != 0)
                temp.x = original.x * puzzleScale.x;
            else if (fudgeX)
                temp.x = original.x - worldVector.x / 2;
            else
                temp.x = original.x - worldVector.x;

            if (gapVector.y != 0)
                temp.y = original.y * puzzleScale.y;
            else if (fudgeY)
                temp.y = original.y - worldVector.y / 2;
            else
                temp.y = original.y - worldVector.y;

            if (gapVector.z != 0)
                temp.z = original.z * puzzleScale.z;
            else if (fudgeZ)
                temp.z = original.z - worldVector.z / 2;
            else
                temp.z = original.z - worldVector.z;
        }
        else
        {
            if (gapVector.x != 0)
                temp.x = original.x * puzzleScale.x;
            else
                temp.x = original.x - worldVector.x;

            if (gapVector.y != 0)
                temp.y = original.y * puzzleScale.y;
            else
                temp.y = original.y - worldVector.y;

            if (gapVector.z != 0)
                temp.z = original.z * puzzleScale.z;
            else
                temp.z = original.z - worldVector.z;
        }


        return temp;
    }


    private void VectorBetweenPoints(GameObject visVector, Vector3 begin, Vector3 end, float width)
    {
        Vector3 offset = end - begin;
        Vector3 localScale = new Vector3(width, (offset.magnitude / 2.0f), width);
        Vector3 position = begin + (offset / 2.0f) ;

        visVector.transform.up = offset;
        visVector.transform.position = position;
        visVector.transform.localScale = localScale;
    }

    /// <summary>
    /// Turns on/off flags to "fudge" the final landing spot of the last vector.
    /// This is becuase sometimes the game Physical start/end vectors vs the PC01 gap entered
    /// is no exactly the same.  This is especially true if the entered gapVector
    /// contains a zero, in which case making a scale factor is impossible (divide by zero).
    /// 
    /// NOTE: This fudge offset only fixes VERY MINOR discrepancies, if the difference
    /// is too large it will look extremely wrong visually.
    /// </summary>
    private void setFudge()
    {
        if (PC01.getDirection().ToString().CompareTo("X") == 0)
        {
            fudgeX = false;
            fudgeY = true;
            fudgeZ = true;
        }
        if (PC01.getDirection().ToString().CompareTo("Y") == 0)
        {
            fudgeX = true;
            fudgeY = false;
            fudgeZ = true;
        }
        if (PC01.getDirection().ToString().CompareTo("XY") == 0)
        {
            fudgeX = true;
            fudgeY = true;
            fudgeZ = true;
        }
        if (PC01.getDirection().ToString().CompareTo("XYZ") == 0)
        {
            fudgeX = false;
            fudgeY = false;
            fudgeZ = false;
        }

    }


    public GameObject getGapVector()
    {
        return gapVector;
    }

    public GameObject getWindVector()
    {
        return windVector;
    }

    public GameObject getAnswerVector()
    {
        return answerVector;
    }

    public GameObject getVector1()
    {
        return vector1;
    }

    public GameObject getVector2()
    {
        return vector2;
    }

    public GameObject getFinalVector()
    {
        return finalVector;
    }
}
