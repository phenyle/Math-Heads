using UnityEngine;
using UnityEngine.UI;

public class ChoiceClickButton : MonoBehaviour
{
    public Vector3 spanValue = new Vector3();
    public int choiceID;

    private Button btnChoice;
    private GameControllerPuzzle03 GCP03;

    private void Start()
    {
        GCP03 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle03>();

        btnChoice = GetComponent<Button>();

        btnChoice.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        if (GCP03.choicesAmount < 2)
        {
            btnChoice.interactable = false;
            GCP03.SetSpanValue(spanValue, choiceID);
        }
    }
}
