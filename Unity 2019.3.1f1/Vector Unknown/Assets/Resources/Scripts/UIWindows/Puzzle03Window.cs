using UnityEngine;
using UnityEngine.UI;

public class Puzzle03Window : WindowRoot
{
    public Text txtChoice1;
    public Text txtChoice2;
    public int choiceID1 = 0;
    public int choiceID2 = 0;
    public Vector3 choice1Pos;
    public Vector3 choice2Pos;

    private GameControllerPuzzle03 GCP03;

    private void Start()
    {
        InitWindow();
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        GCP03 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle03>();
        GCP03.InitGameController(this);

        ClearSpanValues();
    }

    public void SetSpanValue(Vector3 spanValue, int choiceID)
    {
        if(choiceID1 == choiceID || choiceID2 == choiceID)
        {
            Debug.Log("You Already Selected");
        }

        if (choiceID1 == 0)
        {
            txtChoice1.text = spanValue[0] + "\n" + spanValue[1] + "\n" + spanValue[2];
            choiceID1 = choiceID;
            choice1Pos = spanValue;
        }
        else if (choiceID2 == 0)
        {
            txtChoice2.text = spanValue[0] + "\n" + spanValue[1] + "\n" + spanValue[2];
            choiceID2 = choiceID;
            choice2Pos = spanValue;
        }
        else
        {
            Debug.Log("Full");
        }
    }

    public void ClickSubmitBtn()
    {
        GCP03.TriggerRotation(choiceID1, choiceID2, choice1Pos, choice2Pos);
        ClearSpanValues();
    }

    public void ClickClearChoice1Btn()
    {
        txtChoice1.text = "";
        choiceID1 = 0;
        choice1Pos = Vector3.zero;
    }

    public void ClickClearChoice2Btn()
    {
        txtChoice2.text = "";
        choiceID2 = 0;
        choice2Pos = Vector3.zero;
    }

    public void ClearSpanValues()
    {
        txtChoice1.text = "";
        txtChoice2.text = "";
        choiceID1 = 0;
        choiceID2 = 0;
        choice1Pos = Vector3.zero;
        choice2Pos = Vector3.zero;
    }
}
