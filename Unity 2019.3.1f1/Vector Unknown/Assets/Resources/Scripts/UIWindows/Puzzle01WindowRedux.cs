using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Puzzle01WindowRedux : WindowRoot
{
    //LOWER RIGHT PIN WINDOWS---------
    public Transform iptPanel;
    private Slider scalar1;
    private Slider scalar2;
    private Text scalarText1;
    private Text scalarText2;
//    private GameObject finalDisplay;
    private GameObject ansField1;
    private GameObject ansField2;
    private GameObject ansCard1;
    private GameObject ansCard2;

    //LEFT PIN WINDOWS------------------
    public Transform cardPanel;
    private List<GameObject> UIcards;

    //UPPER RIGHT PIN WINDOWS------------
    public Transform feedbackPanel;
    private GameObject gapDisplay;
    private GameObject windDisplay;
    private GameObject goalDisplay;
    private GameObject finalDisplay;

    //Legacy
    public bool isInit;
    
    
    public InputField iptScalar;
    public InputField iptX, iptY, iptZ;

    public Text txtInstruction;
    public Text txtFBFormula;
    public Text txtFBTF;
    public Text txtFBTips;
    public Image[] PlatformTips;
    public Image[] PlatformTipsChecked;

    private GameControllerPuzzle01Redux GCP01;
    private Puzzle01Controller PC01;
    [HideInInspector]
    public float defaultScalar;
    [HideInInspector]
    public float defaultX, defaultY, defaultZ;
    [HideInInspector]
    public int questionNum;

    private void Start()
    {
        GCP01 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01Redux>();

        if (isInit)
        {
            InitWindow();
        }
    }

    protected override void InitWindow()
    {
        //LOWER RIGHT INIT--------------------------------------
        scalar1 = GameObject.Find("Slider1").GetComponent<Slider>();
        scalar2 = GameObject.Find("Slider2").GetComponent<Slider>();
        scalar1.value = 1;
        scalar2.value = 1;
        //Assign the Text objects to the UI Gameobjects
        scalarText1 = GameObject.Find("ScalValue1").GetComponent<Text>();
        scalarText2 = GameObject.Find("ScalValue2").GetComponent<Text>();
        //Assign the Text field to the current Slider position
        scalarText1.text = scalar1.value.ToString();
        scalarText2.text = scalar2.value.ToString();

//        finalDisplay = GameObject.Find("FinalAnswer");

        ansField1 = GameObject.Find("AnsVector1");
        ansField2 = GameObject.Find("AnsVector2");


        //LEFT PIN INIT-----------------------------------------
        UIcards = new List<GameObject>();
        UIcards.Add(GameObject.Find("Card1"));
        UIcards.Add(GameObject.Find("Card2"));
        UIcards.Add(GameObject.Find("Card3"));
        UIcards.Add(GameObject.Find("Card4"));



        //UPPER RIGHT PIN INIT-----------------------------------
        gapDisplay = feedbackPanel.Find("Gap").gameObject;
        windDisplay = feedbackPanel.Find("Wind").gameObject;
        goalDisplay = feedbackPanel.Find("Goal").gameObject;
        finalDisplay = GameObject.Find("FinalAnswer").gameObject;

        if (GCP01.Difficulty == 1)
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
        else if (GCP01.Difficulty == 2)
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
        else if (GCP01.Difficulty == 3)
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
            if (GCP01.Difficulty == 1)
            {
                GameObject.Find("1DBracket").SetActive(true);
                GameObject.Find("2DBracket").SetActive(false);
                GameObject.Find("3DBracket").SetActive(false);

            }
            else if (GCP01.Difficulty == 2)
            {
                GameObject.Find("1DBracket").SetActive(false);
                GameObject.Find("2DBracket").SetActive(true);
                GameObject.Find("3DBracket").SetActive(false);
            }
            else if (GCP01.Difficulty == 3)
            {
                GameObject.Find("1DBracket").SetActive(false);
                GameObject.Find("2DBracket").SetActive(false);
                GameObject.Find("3DBracket").SetActive(true);
            }
        }

        setAnswer1(null);
        setAnswer2(null);



        //Legacy---------------------------------------------
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        Debug.Log("Call GameController of Puzzle01 to connect");
        GCP01.InitGameController(this);





        //Init Components
        txtInstruction.text = "";

        txtFBFormula.text = "";
        txtFBTF.text = "";
        txtFBTips.text = "";

        ShowInputPanel(false);
        ShowFeedbackPanel(false);
        ShowCardPanel(false);

        
        foreach (Image image in PlatformTips)
        {
            SetActive(image, false);
        }

        foreach (Image image in PlatformTipsChecked)
        {
            SetActive(image, false);
        }

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
    /// <param name="PC01"> Only used for 1D vectors in the X or Y direction</param>
    public void setCardDisplay(Puzzle01Controller PC01)
    {
       //1D Vectors----------------------------------
       if (PC01.getDirection().ToString().CompareTo("X") == 0)
       {
           for(int i = 0; i < UIcards.Count; i++)
           {
               UIcards[i].transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = UIcards[i].GetComponent<CardVectors>().getCardVector().x.ToString();
           }
        }
        if (PC01.getDirection().ToString().CompareTo("Y") == 0)
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

    public void setFeedbackDisplay(Puzzle01Controller PC01)
    {
        //1D Vectors------------------------------------------
        if (PC01.getDirection().ToString().CompareTo("X") == 0)
        {
            gapDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getGapVector().x.ToString();
            windDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getWindVector().x.ToString();
            goalDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getAnswerVector().x.ToString();
        }
        if (PC01.getDirection().ToString().CompareTo("Y") == 0)
        {

            gapDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getGapVector().y.ToString();
            windDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getWindVector().y.ToString();
            goalDisplay.transform.Find("1DBracket").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getAnswerVector().y.ToString();
        }

        //2D Vectors----------------------------------------
        gapDisplay.transform.Find("2DBracket").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC01.getGapVector().x.ToString();
        gapDisplay.transform.Find("2DBracket").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC01.getGapVector().y.ToString();
        windDisplay.transform.Find("2DBracket").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC01.getWindVector().x.ToString();
        windDisplay.transform.Find("2DBracket").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC01.getWindVector().y.ToString();
        goalDisplay.transform.Find("2DBracket").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC01.getAnswerVector().x.ToString();
        goalDisplay.transform.Find("2DBracket").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC01.getAnswerVector().y.ToString();


        //3D Vectors------------------------------------------
        gapDisplay.transform.Find("3DBracket").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC01.getGapVector().x.ToString();
        gapDisplay.transform.Find("3DBracket").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC01.getGapVector().y.ToString();
        gapDisplay.transform.Find("3DBracket").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC01.getGapVector().z.ToString();
        windDisplay.transform.Find("3DBracket").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC01.getWindVector().x.ToString();
        windDisplay.transform.Find("3DBracket").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC01.getWindVector().y.ToString();
        windDisplay.transform.Find("3DBracket").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC01.getWindVector().z.ToString();
        goalDisplay.transform.Find("3DBracket").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC01.getAnswerVector().x.ToString();
        goalDisplay.transform.Find("3DBracket").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC01.getAnswerVector().y.ToString();
        goalDisplay.transform.Find("3DBracket").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC01.getAnswerVector().z.ToString();
    }


    public void setFinalAnswerDisplay(Puzzle01Controller PC01)
    {
        //1D Vectors------------------------------------------
        if (PC01.getDirection().ToString().CompareTo("X") == 0)
        {
            finalDisplay.transform.Find("1DBracketFin").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getPlayerAnswer().x.ToString();
        }
        if (PC01.getDirection().ToString().CompareTo("Y") == 0)
        {

            finalDisplay.transform.Find("1DBracketFin").transform.Find("1Dval").GetComponentInChildren<Text>().text = PC01.getPlayerAnswer().y.ToString();
        }

        //2D Vectors----------------------------------------
        finalDisplay.transform.Find("2DBracketFin").transform.Find("2DvalX").GetComponentInChildren<Text>().text = PC01.getPlayerAnswer().x.ToString();
        finalDisplay.transform.Find("2DBracketFin").transform.Find("2DvalY").GetComponentInChildren<Text>().text = PC01.getPlayerAnswer().y.ToString();

        //3D Vectors------------------------------------------
        finalDisplay.transform.Find("3DBracketFin").transform.Find("3DvalX").GetComponentInChildren<Text>().text = PC01.getPlayerAnswer().x.ToString();
        finalDisplay.transform.Find("3DBracketFin").transform.Find("3DvalY").GetComponentInChildren<Text>().text = PC01.getPlayerAnswer().y.ToString();
        finalDisplay.transform.Find("3DBracketFin").transform.Find("3DvalZ").GetComponentInChildren<Text>().text = PC01.getPlayerAnswer().z.ToString();
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

    public void setPuzzleController(Puzzle01Controller PC01)
    {
        this.PC01 = PC01;
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
        float scalarValue;
        float xValue, yValue, zValue;


        if (PC01.checkFinalAnswer())
        {
            foreach(GameObject card in UIcards)
            {
                card.transform.position = card.GetComponent<CardVectors>().getStartPos();
            }
            setAnswer1(null);
            setAnswer2(null);

        }
/**
        if (iptScalar.text == "")
        {
            GameRoot.AddTips("Please input the scalar value");
        }
        else if (iptX.text == "" || iptY.text == "" || iptZ.text == "")
        {
            GameRoot.AddTips("Please input the vector value");
        }
        else if (!float.TryParse(iptScalar.text, out scalarValue) || !float.TryParse(iptX.text, out xValue) || !float.TryParse(iptY.text, out yValue) || !float.TryParse(iptZ.text, out zValue))
        {
            GameRoot.AddTips("Please input numbers");
        }
        else
        {

            if(GCP01.CheckAnswer(scalarValue, xValue, yValue, zValue))
            {
                SetActive(PlatformTips[questionNum - 1], false);
                SetActive(PlatformTipsChecked[questionNum - 1], true);
            }


            Debug.Log("Your answer is Scalar: " + iptScalar.text + ", X: " + iptX.text + ", Y: " + iptY.text + ", Z: " + iptZ.text);

            ClearInputField();
            SetActive(iptPanel, false);

            GCP01.isTriggerQuestion = false;

            if(!DialogueManager.isInDialogue)
            {
                GameRoot.instance.IsLock(false);
            }
        }
    **/

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


    public void ClearInputField()
    {
        iptScalar.text = "";
        iptX.text = "";
        iptY.text = "";
        iptZ.text = "";
        iptScalar.interactable = true;
        iptX.interactable = true;
        iptY.interactable = true;
        iptZ.interactable = true;
    }

    public void SetFeedback(string formula, string TF, Color color)
    {
        txtFBTips.text = "";

        txtFBFormula.text = formula;
        txtFBFormula.color = color;

        txtFBTF.text = TF;
        txtFBTF.color = color;
    }

    public void SetFeedbackQuestionTips(string tips)
    {
        txtFBFormula.text = "";
        txtFBTF.text = "";

        txtFBTips.text = tips;
    }

    public string GetCurrentInput()
    {
        return iptX.text + "|" + iptY.text + "|" + iptZ.text;
    }

    public void scalarUpdate()
    {
        float scalarValue;
        if (float.TryParse(iptScalar.text, out scalarValue))
        {
           // GCP01.updateLine(0, scalarValue);
        }
    }

    public void xUpdate()
    {
        float x;
        if (float.TryParse(iptX.text, out x))
        {
           // GCP01.updateLine(1, x);
        }
    }

    public void yUpdate()
    {
        float y;
        if (float.TryParse(iptY.text, out y))
        {
          //  GCP01.updateLine(2, y);
        }
    }

    public void zUpdate()
    {
        float z;
        if (float.TryParse(iptZ.text, out z))
        {
          //  GCP01.updateLine(3, z);
        }
    }
        /*
        public void SetGreenLineTips()
        {
            GCP01.DBP01.SetGreenLineTips();
        }*/
    }
