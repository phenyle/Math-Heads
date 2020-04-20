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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            GCP03.isInQuestion = false;
        }
    }
}
