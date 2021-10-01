using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    private GameControllerPuzzle03 GCP03;
    public int convoNumber;

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
            GCP03.conversation(convoNumber);

            GCP03.player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = true;

            GCP03.CamGlideToPuzzle();
            GCP03.isTriggerQuestion = true;
            GameRoot.instance.IsLock(true);
            GameRoot.isPuzzleLock = true;
            GCP03.P03W.ShowChoicePanel(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = true;
            GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03TriggerQuestion);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            GCP03.isInQuestion = false;
            GameRoot.ShowTips("", false, false);

            GCP03.CamGlideToPlayer();
            GameRoot.instance.IsLock(false);
            GameRoot.isPuzzleLock = false;
            GCP03.P03W.ShowChoicePanel(false);
            GCP03.isTriggerQuestion = false;
            GameRoot.instance.audioService.PlayUIAudio(Constants.audioP03ExitQuestion);
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
