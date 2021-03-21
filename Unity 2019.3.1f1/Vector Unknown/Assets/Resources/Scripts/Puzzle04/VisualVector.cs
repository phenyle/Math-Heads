using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualVector : MonoBehaviour
{
    private Puzzle04Controller PC04;

    private Vector3 puzzleScale;
    private float overallScale;

    //Puzzle Vector Components
    [Header("---Puzzle Vector Components---")]
    public GameObject gapVectorVV;
    public GameObject start;
    public GameObject goal;
    public GameObject windEnd;

    //Player Vector Components
    [Header("---Player Vector Components---")]
    public GameObject vector1;
    public GameObject vector2;
    public GameObject finalVector;
    public GameObject vector1end;
    public GameObject vector2end;

    //Misc Controls
    private bool fudgeX;
    private bool fudgeY;
    private bool fudgeZ;


    // Start is called before the first frame update
    void Start()
    {  
        PC04 = GetComponent<Puzzle04Controller>();

        setFudge();

        gapVectorVV.SetActive(false);

        vector1.SetActive(false);
        vector2.SetActive(false);
        finalVector.SetActive(false);

        overallScale = setOverallScale();
        puzzleScale = setPuzzleScale();

        VectorBetweenPoints(gapVectorVV, goal.transform.position, start.transform.position, 0.25f);

    }

    // Update is called once per frame
    void Update()
    {
        if (PC04.getAnsCard1() != null || PC04.getAnsCard1() != new Vector3(0f, 0f, 0f))
            vector1end.transform.position = start.transform.position + rescaledVector(PC04.getAnsCard1() * PC04.getScalar1());
        else
            vector1end = start;

        if (PC04.getAnsCard2() != null || PC04.getAnsCard2() != new Vector3(0f, 0f, 0f))
            vector2end.transform.position = vector1end.transform.position + rescaledVector(PC04.getAnsCard2() * PC04.getScalar2());
        else
            vector2end.transform.position = vector1end.transform.position;

        VectorBetweenPoints(vector1, start.transform.position, vector1end.transform.position, 0.25f);
        VectorBetweenPoints(vector2, vector1end.transform.position, vector2end.transform.position, 0.25f);

        if ((PC04.getAnsCard1() != null || PC04.getAnsCard1() != new Vector3(0f,0f,0f)) && (PC04.getAnsCard2() != null || PC04.getAnsCard2() != new Vector3(0f, 0f, 0f)))
        {
            finalVector.SetActive(true);
            VectorBetweenPoints(finalVector, start.transform.position, vector2end.transform.position, 0.25f);
        }
        else
            finalVector.SetActive(false);

    }

    private float setOverallScale()
    {
        Vector3 worldVector, gapVector;
        float worldMag, gapMag;

        worldVector = start.transform.position - goal.transform.position;
        gapVector = PC04.getAnswerVector();

        worldMag = worldVector.magnitude;
        gapMag = gapVector.magnitude;

        return worldMag / gapMag;
    }


    private Vector3 setPuzzleScale()
    {
        Vector3 worldVector, gapVector, returnVector;  

        worldVector = start.transform.position - goal.transform.position;
        gapVector = PC04.getAnswerVector();

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

    private Vector3 rescaledVector(Vector3 original)
    {
        Vector3 temp, worldVector, gapVector;

        worldVector = start.transform.position - goal.transform.position;
        gapVector = PC04.getAnswerVector();

        //rescale X
        if (gapVector.x != 0)
            temp.x = original.x * puzzleScale.x;
        else if (fudgeX)
            temp.x = original.x - worldVector.x / 2;
        else
            temp.x = original.x - worldVector.x;

        //rescale Y
        if (gapVector.y != 0)
            temp.y = original.y * puzzleScale.y;
        else if (fudgeY)
            temp.y = original.y - worldVector.y / 2;
        else
            temp.y = original.y - worldVector.y;

        //rescale Z
        if (gapVector.z != 0)
            temp.z = original.z * puzzleScale.z;
        else if (fudgeZ)
            temp.z = original.z - worldVector.z / 2;
        else
            temp.z = original.z - worldVector.z;


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
        if (PC04.getDirection().ToString().CompareTo("X") == 0)
        {
            fudgeX = false;
            fudgeY = true;
            fudgeZ = true;
        }
        if (PC04.getDirection().ToString().CompareTo("Y") == 0)
        {
            fudgeX = true;
            fudgeY = false;
            fudgeZ = true;
        }
        if (PC04.getDirection().ToString().CompareTo("XY") == 0)
        {
            fudgeX = false;
            fudgeY = false;
            fudgeZ = true;
        }
        if (PC04.getDirection().ToString().CompareTo("XYZ") == 0)
        {
            fudgeX = false;
            fudgeY = false;
            fudgeZ = false;
        }

    }


    public GameObject getGapVector()
    {
        return gapVectorVV;
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
