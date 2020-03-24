using UnityEngine;
using UnityEngine.UI;

public class Puzzle01Window : WindowRoot
{
    public bool isInit;
    public Transform panelStart;
    public Transform iptPanel;
    public InputField iptScalar;
    public InputField iptX, iptY, iptZ;
    public Text txtInstruction;

    private GameControllerPuzzle01 GCP01;
    [HideInInspector]
    public float defaultScalar;
    [HideInInspector]
    public float defaultX, defaultY, defaultZ;

    private void Start()
    {
        if(isInit)
        {
            InitWindow();
        }
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        SetActive(panelStart, true);
        SetActive(iptPanel, false);

        GCP01 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>();

        Debug.Log("Call GameController of Puzzle01 to connect");
        GCP01.InitGameController(this);
    }

    public void ClickAnswerSubmitBtn()
    {
        float scalarValue;
        float xValue, yValue, zValue;

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
            GCP01.CheckAnswer(scalarValue, xValue, yValue, zValue);
            Debug.Log("Your answer is Scalar: " + iptScalar.text + ", X: " + iptX.text + ", Y: " + iptY.text + ", Z: " + iptZ.text);

            ClearInputField();
            SetActive(iptPanel, false);
            GameRoot.instance.IsLock(false);
            //GCP01.IsLock(false);
        }

        audioService.PlayUIAudio(Constants.audioUIClickBtn);
    }

    public void ClickInstructionSubmitBtn()
    {
        audioService.PlayUIAudio(Constants.audioUIClickBtn);
        SetActive(panelStart, false);
    }

    public void ShowInputPanel(bool value)
    {
        InitDefaultValue();
        SetActive(iptPanel, value);
    }

    private void InitDefaultValue()
    {
        if (defaultScalar != 0)
        {
            iptScalar.text = defaultScalar.ToString();
            iptScalar.interactable = false;
        }

        if(defaultX != 0)
        {
            iptX.text = defaultX.ToString();
            iptX.interactable = false;
        }

        if (defaultY != 0)
        {
            iptY.text = defaultY.ToString();
            iptY.interactable = false;
        }

        if (defaultX != 0)
        {
            iptZ.text = defaultZ.ToString();
            iptZ.interactable = false;
        }
    }

    private void ClearInputField()
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
}
