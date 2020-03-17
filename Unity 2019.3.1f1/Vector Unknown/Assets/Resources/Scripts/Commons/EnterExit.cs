using UnityEngine;

public enum SceneName
{
    MainSceneName,
    Puzzle01SceneName,
    Puzzle02SceneName,
    Puzzle03SceneName,
    CreditSceneName
}

public class EnterExit : MonoBehaviour
{
    public SceneName sceneName;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerController.isEnterExit = true;
            GameRoot.ShowTips("Press \"E\" to enter", true, false);

            switch(sceneName)
            {
                case SceneName.MainSceneName:
                    playerController.sceneName = Constants.mainSceneName;
                    break;

                case SceneName.Puzzle01SceneName:
                    playerController.sceneName = Constants.puzzle01SceneName;
                    break;

                case SceneName.Puzzle02SceneName:
                    playerController.sceneName = Constants.puzzle02SceneName;
                    break;

                case SceneName.Puzzle03SceneName:
                    playerController.sceneName = Constants.puzzle03SceneName;
                    break;

                case SceneName.CreditSceneName:
                    playerController.sceneName = Constants.creditSceneName;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerController.isEnterExit = false;

            GameRoot.ShowTips("", false, false);
        }
    }
}
