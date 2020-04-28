using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    private GameControllerPuzzle03 GCP03;

    private void Start()
    {
        GCP03 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle03>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GCP03.isInQuestion = true;
            GameRoot.ShowTips("Please press \"E\" to answer the question", true, false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            GCP03.isInQuestion = false;
            GameRoot.ShowTips("", false, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(GCP03.isTriggerQuestion)
            {
                GameRoot.ShowTips("", false, false);
            }
            else
            {
                GameRoot.ShowTips("Please press \"E\" to answer the question", true, false);
            }
        }
    }
}
