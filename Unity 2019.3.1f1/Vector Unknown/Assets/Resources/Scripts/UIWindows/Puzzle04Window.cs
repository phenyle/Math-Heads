using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Puzzle04Window : WindowRoot
{
    //LOWER RIGHT PIN WINDOWS---------
    [Header("Input Displays")]
    public Transform iptPanel;
    public Slider scalar1;
    public Slider scalar2;
    public Text scalarText1;
    public Text scalarText2;
//    private GameObject finalDisplay;
    private GameObject ansField1;
    private GameObject ansField2;
    private GameObject ansCard1;
    private GameObject ansCard2;

    //LEFT PIN WINDOWS------------------
    public Transform cardPanel;
    private List<GameObject> UIcards;

    //UPPER RIGHT PIN WINDOWS------------
    [Header("Answer Displays")]
    public Transform feedbackPanel;
    public GameObject gapDisplay;
    public GameObject windDisplay;
    public GameObject goalDisplay;
    public GameObject finalDisplay;

    //Legacy
    public bool isInit;
    private bool doOnce = true;


    public Text txtInstruction;

    private GameControllerPuzzle04 GCP04;
    private Puzzle04Controller PC04;


    private void Start()
    {
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
   
        if (isInit)
        {
            InitWindow();
        }

    }

    protected override void InitWindow()
    {
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();


        //LOWER RIGHT INIT--------------------------------------
        //scalar1 = GameObject.Find("Slider1").GetComponent<Slider>();
        //scalar2 = GameObject.Find("Slider2").GetComponent<Slider>();
        scalar1.value = 1;
        scalar2.value = 1;
        //Assign the Text objects to the UI Gameobjects
        //scalarText1 = GameObject.Find("ScalValue1").GetComponent<Text>();
        //scalarText2 = GameObject.Find("ScalValue2").GetComponent<Text>();
        //Assign the Text field to the current Slider position
        scalarText1.text = scalar1.value.ToString();
        scalarText2.text = scalar2.value.ToString();

//        finalDisplay = GameObject.Find("FinalAnswer");

        ansField1 = GameObject.Find("AnsVector1");
        ansField2 = GameObject.Find("AnsVector2");


        //LEFT PIN INIT-----------------------------------------
        if (doOnce)
        {
            UIcards = new List<GameObject>();
            UIcards.Add(GameObject.Find("Card1"));
            UIcards.Add(GameObject.Find("Card2"));
            UIcards.Add(GameObject.Find("Card3"));
            UIcards.Add(GameObject.Find("Card4"));
            doOnce = false;
        }



        //UPPER RIGHT PIN INIT-----------------------------------
        /**
        gapDisplay = feedbackPanel.gameObject.transform.Find("Gap").gameObject;
        windDisplay = feedbackPanel.Find("Wind").gameObject;
        goalDisplay = feedbackPanel.Find("Goal").gameObject;
        finalDisplay = iptPanel.Find("FinalAnswer").gameObject;
        **/

        if (GCP04.Difficulty == 1)
        {
            gapDisplay.transform.Find("1DBracket").gameObject.SetActive(true);
            gapDisplay.transform.Find("2DBracket").gameObject.SetActive(false);
            gapDisplay.transform.Find("3DBracket").gameObject.SetActive(false);
            windDisplay.transform.Find("1DBracket").gameObject.SetActive(true);
            windDisplay.transform.Find("2DBracket").gameObject.SetActive(false);
            windDisplay.transform.Find("3DBracket").gameObject.SetActive(false);
            goalDisplay.transform.Find("1DBracket").gameObject.SetActive(true);
            goalDisplay.transform.Find("2DBracket").gameObject.SetActive(false);
            goalDisplay.transform.Find("3DBracket").gameObject.SetActive(false);
            finalDisplay.transform.Find("1DBracketFin").gameObject.SetActive(true);
            finalDisplay.transform.Find("2DBracketFin").gameObject.SetActive(false);
            finalDisplay.transform.Find("3DBracketFin").gameObject.SetActive(false);
        }
        else if (GCP04.Difficulty == 2)
        {
            gapDisplay.transform.Find("1DBracket").gameObject.SetActive(false);
            gapDisplay.transform.Find("2DBracket").gameObject.SetActive(true);
            gapDisplay.transform.Find("3DBracket").gameObject.SetActive(false);
            windDisplay.transform.Find("1DBracket").gameObject.SetActive(false);
            windDisplay.transform.Find("2DBracket").gameObject.SetActive(true);
            windDisplay.transform.Find("3DBracket").gameObject.SetActive(false);
            goalDisplay.transform.Find("1DBracket").gameObject.SetActive(false);
            goalDisplay.transform.Find("2DBracket").gameObject.SetActive(true);
            goalDisplay.transform.Find("3DBracket").gameObject.SetActive(false);
            finalDisplay.transform.Find("1DBracketFin").gameObject.SetActive(false);
            finalDisplay.transform.Find("2DBracketFin").gameObject.SetActive(true);
            finalDisplay.transform.Find("3DBracketFin").gameObject.SetActive(false);
        }
        else if (GCP04.Difficulty == 3)
        {
            gapDisplay.transform.Find("1DBracket").gameObject.SetActive(false);
            gapDisplay.transform.Find("2DBracket").gameObject.SetActive(false);
            gapDisplay.transform.Find("3DBracket").gameObject.SetActive(true);
            windDisplay.transform.Find("1DBracket").gameObject.SetActive(false);
            windDisplay.transform.Find("2DBracket").gameObject.SetActive(false);
            windDisplay.transform.Find("3DBracket").gameObject.SetActive(true);
            goalDisplay.transform.Find("1DBracket").gameObject.SetActive(false);
            goalDisplay.transform.Find("2DBracket").gameObject.SetActive(false);
            goalDisplay.transform.Find("3DBracket").gameObject.SetActive(true);
            finalDisplay.transform.Find("1DBracketFin").gameObject.SetActive(false);
            finalDisplay.transform.Find("2DBracketFin").gameObject.SetActive(false);
            finalDisplay.transform.Find("3DBracketFin").gameObject.SetActive(true);
        }

        foreach (GameObject card in UIcards)
        {
            if (GCP04.Difficulty == 1)
            {
                card.transform.Find("1DBracket").gameObject.SetActive(true);
                card.transform.Find("2DBracket").gameObject.SetActive(false);
                card.transform.Find("3DBracket").gameObject.SetActive(false);

            }
            else if (GCP04.Difficulty == 2)
            {
                card.transform.Find("1DBracket").gameObject.SetActive(false);
                card.transform.Find("2DBracket").gameObject.SetActive(true);
                card.transform.Find("3DBracket").gameObject.SetActive(false);
            }
            else if (GCP04.Difficulty == 3)
            {
                card.transform.Find("1DBracket").gameObject.SetActive(false);
                card.transform.Find("2DBracket").gameObject.SetActive(false);
                card.transform.Find("3DBracket").gameObject.SetActive(true);
            }
        }

        setAnswer1(null);
        setAnswer2(null);



        //Legacy---------------------------------------------
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        Debug.Log("Call GameController of Puzzle01 to connect");
        GCP04.InitGameController(this);





        //Init Components
        txtInstruction.text = "";


        ShowInputPanel(false);
        ShowFeedbackPanel(false);
        ShowCardPanel(false);              


    }

    /// <summary>
    /// Gets the cards display info from "CardVectors" and sets the vector values info
    /// to the UIcards.
    /// 
    /// NOTE: Which vectors display is dependent on the InitWindow() method where
    /// certain vector types are displayed/not displayed dependent on the 
    /// GameControllerPuzzle01.Difficutly
    /// NOTE2: If GameControllerPuzzle01.Diffculty is set to 1 then the resulting
    /// displayed values will be dependent on if the PARTICULAR Puzzle01Controller
    /// is set for an X or Y direction constraint.  This is done for flexibliy reasons.
    /// </summary>
    /// <param name="PC04"> Only used for 1D vectors in the X or Y direction</param>
    public void setCardDisplay(Puzzle04Controller PC04)
    {
       //1D Vectors----------------------------------
       if (PC04.getDirection().ToString().CompareTo("X") == 0)
       {
           for(int i = 0; i < UIcards.Count; i++)
           {
               UIcards[i].transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().x.ToString();
           }
        }
        if (PC04.getDirection().ToString().CompareTo("Y") == 0)
       {
           for (int i = 0; i < UIcards.Count; i++)
           {
               UIcards[i].transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().y.ToString();
           }
        }

        //2D Vectors----------------------------------------
        for (int i = 0; i < UIcards.Count; i++)
        {
            UIcards[i].transform.Find("2DBracket").transform.Find("2DvalX").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().x.ToString();
            UIcards[i].transform.Find("2DBracket").transform.Find("2DvalY").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().y.ToString();
        }

        //3D Vectors------------------------------------------
        for (int i = 0; i < UIcards.Count; i++)
        {
            UIcards[i].transform.Find("3DBracket").transform.Find("3DvalX").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().x.ToString();
            UIcards[i].transform.Find("3DBracket").transform.Find("3DvalY").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().y.ToString();
            UIcards[i].transform.Find("3DBracket").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().z.ToString();
        }

    }

    public void setFeedbackDisplay(Puzzle04Controller PC04)
    {
        //1D Vectors------------------------------------------
        if (PC04.getDirection().ToString().CompareTo("X") == 0)
        {
            gapDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getMinVector().x.ToString();
            windDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getDynamicVector().x.ToString();
            goalDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getAnswerVector().x.ToString();
        }
        if (PC04.getDirection().ToString().CompareTo("Y") == 0)
        {

            gapDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getMinVector().y.ToString();
            windDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getDynamicVector().y.ToString();
            goalDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getAnswerVector().y.ToString();
        }

        //2D Vectors----------------------------------------
        gapDisplay.transform.Find("2DBracket").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC04.getMinVector().x.ToString();
        gapDisplay.transform.Find("2DBracket").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC04.getMinVector().y.ToString();
        windDisplay.transform.Find("2DBracket").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC04.getDynamicVector().x.ToString();
        windDisplay.transform.Find("2DBracket").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC04.getDynamicVector().y.ToString();
        goalDisplay.transform.Find("2DBracket").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC04.getAnswerVector().x.ToString();
        goalDisplay.transform.Find("2DBracket").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC04.getAnswerVector().y.ToString();


        //3D Vectors------------------------------------------
        gapDisplay.transform.Find("3DBracket").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC04.getMinVector().x.ToString();
        gapDisplay.transform.Find("3DBracket").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC04.getMinVector().y.ToString();
        gapDisplay.transform.Find("3DBracket").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC04.getMinVector().z.ToString();
        windDisplay.transform.Find("3DBracket").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC04.getDynamicVector().x.ToString();
        windDisplay.transform.Find("3DBracket").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC04.getDynamicVector().y.ToString();
        windDisplay.transform.Find("3DBracket").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC04.getDynamicVector().z.ToString();
        goalDisplay.transform.Find("3DBracket").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC04.getAnswerVector().x.ToString();
        goalDisplay.transform.Find("3DBracket").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC04.getAnswerVector().y.ToString();
        goalDisplay.transform.Find("3DBracket").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC04.getAnswerVector().z.ToString();
    }


    public void setFinalAnswerDisplay(Puzzle04Controller PC04)
    {
        //1D Vectors------------------------------------------
        if (PC04.getDirection().ToString().CompareTo("X") == 0)
        {
            finalDisplay.transform.Find("1DBracketFin").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getPlayerAnswer().x.ToString();
        }
        if (PC04.getDirection().ToString().CompareTo("Y") == 0)
        {

            finalDisplay.transform.Find("1DBracketFin").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC04.getPlayerAnswer().y.ToString();
        }

        //2D Vectors----------------------------------------
        finalDisplay.transform.Find("2DBracketFin").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC04.getPlayerAnswer().x.ToString();
        finalDisplay.transform.Find("2DBracketFin").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC04.getPlayerAnswer().y.ToString();

        //3D Vectors------------------------------------------
        finalDisplay.transform.Find("3DBracketFin").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC04.getPlayerAnswer().x.ToString();
        finalDisplay.transform.Find("3DBracketFin").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC04.getPlayerAnswer().y.ToString();
        finalDisplay.transform.Find("3DBracketFin").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC04.getPlayerAnswer().z.ToString();
    }


    public void Update()
    {
        if (scalar1.value == 0)
            scalar1.value = 1;
        if (scalar2.value == 0)
            scalar2.value = 1;

        scalarText1.text = scalar1.value.ToString();
        scalarText2.text = scalar2.value.ToString();

    }

    public List<GameObject> getCards()
    {
        return UIcards;
    }

    public void setCardValues(List<Vector3> cardVals)
    {
        for(int i = 0; i < UIcards.Count; i++)
        {
            UIcards[i].GetComponent<CardVectors>().setCardVector(cardVals[i]);
        }
    }

    public void resetCardPos()
    {
        for (int i = 0; i < UIcards.Count; i++)
        {
            UIcards[i].GetComponent<CardVectors>().resetCard();
        }

        ansCard1 = null;
        ansCard2 = null;
    }

    public void setPuzzleController(Puzzle04Controller PC04)
    {
        this.PC04 = PC04;
    }

    public void setAnswer1(GameObject card)
    {
        ansCard1 = card;
    }
    public void setAnswer2(GameObject card)
    {
        ansCard2 = card;
    }

    public GameObject getAnswer1()
    {
        return ansCard1;
    }

    public GameObject getAnswer2()
    {
        return ansCard2;
    }

    public int getScalar1Value()
    {
        return (int)scalar1.value;
    }
    public int getScalar2Value()
    {
        return (int)scalar2.value;
    }

    public Transform getAnsField1trans()
    {
        return ansField1.transform;
    }
    public Transform getAnsField2trans()
    {
        return ansField2.transform;
    }


    public void ClickAnswerSubmitBtn()
    {
        if (PC04.checkFinalAnswer())
        {
            foreach(GameObject card in UIcards)
            {
                card.transform.position = card.GetComponent<CardVectors>().getStartPos();
            }
            setAnswer1(null);
            setAnswer2(null);
        }

        audioService.PlayUIAudio(Constants.audioUIClickBtn);
    }


    public void ShowInputPanel(bool status)
    {
        SetActive(iptPanel, status);
    }

    public void ShowFeedbackPanel(bool status)
    {
        SetActive(feedbackPanel, status);
    }

    public void ShowCardPanel(bool status)
    {
        SetActive(cardPanel, status);
    }

}
