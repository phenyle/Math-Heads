using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualVector : MonoBehaviour
{
    public Puzzle04Controller PC04;

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
    public GameObject vector1ball;
    public GameObject vector2ball;
    public GameObject finalVector;
    public GameObject vector1end;
    public GameObject vector2end;

    //Grid Vector Components
    [Header("---Grid Vector Components---")]
    public GameObject Xvector;
    public GameObject Yvector;
    public GameObject Zvector;
    public GameObject Xnegative;
    public GameObject Xpositive;
    public GameObject Ynegative;
    public GameObject Ypositive; 
    public GameObject Znegative;
    public GameObject Zpositive;
    private List<GameObject> XSphereNodes;
    private List<GameObject> YSphereNodes;
    private List<GameObject> ZSphereNodes;
    private List<GameObject> XBarNodes;
    private List<GameObject> YBarNodes;
    private List<GameObject> ZYHoriBarNodes;
    private List<GameObject> ZYVertBarNodes;
    private List<GameObject> ZXHoriBarNodes;
    private List<GameObject> ZXVertBarNodes;
    private float axisThickness = 0.18f;
    private float gridThickness = 0.09f;
    private float gridNodeScale = 0.6f;
    private int gridSize = 50;
    private Color gridNodeColor = Color.black;
    private Color gridBarColor = Color.white;


    //Misc Controls
    private bool fudgeX;
    private bool fudgeY;
    private bool fudgeZ;


    // Start is called before the first frame update
    void Start()
    {  
        PC04 = this.GetComponent<Puzzle04Controller>();

        setFudge();

        gapVectorVV.SetActive(false);

        vector1.SetActive(false);
        vector2.SetActive(false);
        vector1ball.SetActive(false);
        vector2ball.SetActive(false);
        finalVector.SetActive(false);


        Xvector.SetActive(false);
        Yvector.SetActive(false);
        Zvector.SetActive(false);

        overallScale = setOverallScale();
        puzzleScale = setPuzzleScale();

        VectorBetweenPoints(gapVectorVV, goal.transform.position, start.transform.position, 0.25f);

        XSphereNodes = new List<GameObject>();
        YSphereNodes = new List<GameObject>();
        ZSphereNodes = new List<GameObject>();

        XBarNodes = new List<GameObject>();
        YBarNodes = new List<GameObject>();
        ZYHoriBarNodes = new List<GameObject>();
        ZYVertBarNodes = new List<GameObject>();
        ZXHoriBarNodes = new List<GameObject>();
        ZXVertBarNodes = new List<GameObject>();

        createSphereGrid();
        createBarGrid();


        VectorBetweenPoints(Xvector, Xnegative.transform.position, Xpositive.transform.position, axisThickness);
        VectorBetweenPoints(Yvector, Ynegative.transform.position, Ypositive.transform.position, axisThickness);
        VectorBetweenPoints(Zvector, Znegative.transform.position, Zpositive.transform.position, axisThickness);


    }

    // Update is called once per frame
    void Update()
    {
        if (this.PC04.getAnsCard1() != null || this.PC04.getAnsCard1() != new Vector3(0f, 0f, 0f))
        {
            vector1end.transform.position = start.transform.position + rescaledVector(PC04.getAnsCard1() * PC04.getScalar1());
            vector1ball.SetActive(true);
        }
        else
        {
            vector1end = start;
            vector1ball.SetActive(false);
        }

        if (this.PC04.getAnsCard2() != null || this.PC04.getAnsCard2() != new Vector3(0f, 0f, 0f))
        {
            vector2end.transform.position = vector1end.transform.position + rescaledVector(PC04.getAnsCard2() * PC04.getScalar2());
            vector2ball.SetActive(true);
        }
        else
        {
            vector2end.transform.position = vector1end.transform.position;
            vector2ball.SetActive(false);
        }

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
        switch(PC04.getDirection())
        {
            case Direction.X:
                fudgeX = false;
                fudgeY = true;
                fudgeZ = true;
                break;
            case Direction.Y:
                fudgeX = true;
                fudgeY = false;
                fudgeZ = true;
                break;
            case Direction.XY:
                fudgeX = false;
                fudgeY = false;
                fudgeZ = true;
                break;
            case Direction.XYZ:
                fudgeX = false;
                fudgeY = false;
                fudgeZ = false;
                break;
        }

        /**
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
        **/

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

    public GameObject getXvector()
    {
        return Xvector;
    }
    public GameObject getYvector()
    {
        return Yvector;
    }
    public GameObject getZvector()
    {
        return Zvector;
    }

    public void createSphereGrid()
    {
        for(int i = -gridSize; i < gridSize; i++)
        {
            GameObject xSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            xSphere.transform.localScale *= gridNodeScale;
            xSphere.layer = 10;
            xSphere.GetComponent<Collider>().enabled = false;
            xSphere.GetComponent<Renderer>().material.color = gridNodeColor;
            XSphereNodes.Add(xSphere);

            GameObject ySphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ySphere.transform.localScale *= gridNodeScale;
            ySphere.layer = 10;
            ySphere.GetComponent<Collider>().enabled = false;
            ySphere.GetComponent<Renderer>().material.color = gridNodeColor;
            YSphereNodes.Add(ySphere);

            GameObject zSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            zSphere.transform.localScale *= gridNodeScale;
            zSphere.layer = 10;
            zSphere.GetComponent<Collider>().enabled = false;
            zSphere.GetComponent<Renderer>().material.color = gridNodeColor;
            ZSphereNodes.Add(zSphere);
        }

        deactivateSphereGrid();

    }

    public void createBarGrid()
    {
        for (int i = -gridSize; i < gridSize; i++)
        {
            GameObject xCyln = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            xCyln.GetComponent<Collider>().enabled = false;
            xCyln.GetComponent<Renderer>().material.color = gridBarColor;
            XBarNodes.Add(xCyln);

            GameObject yCyln = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            yCyln.GetComponent<Collider>().enabled = false;
            yCyln.GetComponent<Renderer>().material.color = gridBarColor;
            YBarNodes.Add(yCyln);

            GameObject zyHoriCyln = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            zyHoriCyln.GetComponent<Collider>().enabled = false;
            zyHoriCyln.GetComponent<Renderer>().material.color = gridBarColor;
            ZYHoriBarNodes.Add(zyHoriCyln);

            GameObject zyVertCyln = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            zyVertCyln.GetComponent<Collider>().enabled = false;
            zyVertCyln.GetComponent<Renderer>().material.color = gridBarColor;
            ZYVertBarNodes.Add(zyVertCyln);

            GameObject zxCyln = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            zxCyln.GetComponent<Collider>().enabled = false;
            zxCyln.GetComponent<Renderer>().material.color = gridBarColor;
            ZXHoriBarNodes.Add(zxCyln);

            GameObject zxVertCyln = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            zxVertCyln.GetComponent<Collider>().enabled = false;
            zxVertCyln.GetComponent<Renderer>().material.color = gridBarColor;
            ZXVertBarNodes.Add(zxVertCyln);
        }

        deactivateBarGrid();

    }

    public void setGridSphereNodes(bool Xaxis, bool Yaxis, bool Zaxis)
    {

        for(int i = -gridSize; i < gridSize; i++)
        {
            if (Xaxis)
            {
                //Set X sphere nodes
                XSphereNodes[i + gridSize].transform.position = start.transform.position;
                XSphereNodes[i + gridSize].transform.position += new Vector3(i * puzzleScale.x, 0f, 0f);
            }

            if (Yaxis)
            {
                //Set Y sphere nodes
                YSphereNodes[i + gridSize].transform.position = start.transform.position;
                YSphereNodes[i + gridSize].transform.position += new Vector3(0f, i * puzzleScale.y, 0f);
            }

            if (Zaxis)
            {
                //Set Z sphere nodes
                ZSphereNodes[i + gridSize].transform.position = start.transform.position;
                ZSphereNodes[i + gridSize].transform.position += new Vector3(0f, 0f, i * puzzleScale.z);
            }
        }
    }

    public void setGridBarNodes(bool Xaxis, bool Yaxis, bool Zaxis)
    {

        for (int i = -gridSize; i < gridSize; i++)
        {
            //Set XY plane bar nodes
            if (Xaxis)
            {
                //X bars
                VectorBetweenPoints(XBarNodes[i + gridSize], Ypositive.transform.position + new Vector3(i * puzzleScale.x, 0f, 0f), Ynegative.transform.position + new Vector3(i * puzzleScale.x, 0f, 0f), gridThickness);
            }

            if (Yaxis)
            {
                //Ybars
                VectorBetweenPoints(YBarNodes[i + gridSize], Xpositive.transform.position + new Vector3(0f, i * puzzleScale.y, 0f), Xnegative.transform.position + new Vector3(0f, i * puzzleScale.y, 0f), gridThickness);
            }

            if (Zaxis)
            {
                //Set ZY plane nodes
                VectorBetweenPoints(ZYHoriBarNodes[i + gridSize], Zpositive.transform.position + new Vector3(0f, i * puzzleScale.y, 0f), Znegative.transform.position + new Vector3(0f, i * puzzleScale.y, 0f), gridThickness);
                VectorBetweenPoints(ZYVertBarNodes[i + gridSize], Ypositive.transform.position + new Vector3(0f, 0f, i * puzzleScale.z), Ynegative.transform.position + new Vector3(0f, 0f, i * puzzleScale.z), gridThickness);


                VectorBetweenPoints(ZXHoriBarNodes[i + gridSize], Zpositive.transform.position + new Vector3(i * puzzleScale.x, 0f, 0f), Znegative.transform.position + new Vector3(i * puzzleScale.x, 0f, 0f), gridThickness);
                VectorBetweenPoints(ZXVertBarNodes[i + gridSize], Xpositive.transform.position + new Vector3(0f, 0f, i * puzzleScale.z), Xnegative.transform.position + new Vector3(0f, 0f, i * puzzleScale.z), gridThickness);
            }

        }

    }


    public void deactivateSphereGrid()
    {
        Xvector.SetActive(false);
        Yvector.SetActive(false);
        Zvector.SetActive(false);

        foreach (GameObject sphere in XSphereNodes)
        {
            sphere.SetActive(false);
        }

        foreach (GameObject sphere in YSphereNodes)
        {
            sphere.SetActive(false);
        }

        foreach (GameObject sphere in ZSphereNodes)
        {
            sphere.SetActive(false);
        }
    }

    public void activateSphereGrid(bool Xaxis, bool Yaxis, bool Zaxis)
    {
        Xvector.SetActive(Xaxis);
        Yvector.SetActive(Yaxis);
        Zvector.SetActive(Zaxis);


        foreach (GameObject sphere in XSphereNodes)
        {
            sphere.SetActive(Xaxis);
        }

        foreach (GameObject sphere in YSphereNodes)
        {
            sphere.SetActive(Yaxis);
        }

        foreach (GameObject sphere in ZSphereNodes)
        {
            sphere.SetActive(Zaxis);
        }
        
    }

    public void deactivateBarGrid()
    {
        foreach (GameObject bar in XBarNodes)
        {
            bar.SetActive(false);
        }

        foreach (GameObject bar in YBarNodes)
        {
            bar.SetActive(false);
        }

        foreach (GameObject bar in ZYHoriBarNodes)
        {
            bar.SetActive(false);
        }
        foreach (GameObject bar in ZYVertBarNodes)
        {
            bar.SetActive(false);
        }

        foreach (GameObject bar in ZXHoriBarNodes)
        {
            bar.SetActive(false);
        }
        foreach (GameObject bar in ZXVertBarNodes)
        {
            bar.SetActive(false);
        }
    }

    public void activateBarGrid(bool Xaxis, bool Yaxis, bool Zaxis)
    {
        foreach (GameObject bar in XBarNodes)
        {
            bar.SetActive(Xaxis);
        }

        foreach (GameObject bar in YBarNodes)
        {
            bar.SetActive(Yaxis);
        }

        foreach (GameObject bar in ZYHoriBarNodes)
        {
            bar.SetActive(Zaxis);
        }
        foreach (GameObject bar in ZYVertBarNodes)
        {
            bar.SetActive(Zaxis);
        }

        foreach (GameObject bar in ZXHoriBarNodes)
        {
            bar.SetActive(Zaxis);
        }
        foreach (GameObject bar in ZXVertBarNodes)
        {
            bar.SetActive(Zaxis);
        }

    }

}
