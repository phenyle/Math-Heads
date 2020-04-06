using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MainSceneName,
    Puzzle01SceneName,
    Puzzle02SceneName,
    Puzzle03SceneName,
    CreditSceneName
}

public enum PuzzleComplete
{
    NullPuzzle,
    Puzzle01Complete,
    Puzzle02Complete,
    Puzzle03Complete
}

public class EnterExit : MonoBehaviour
{
    public SceneName sceneName;
    public PuzzleComplete puzzleComplete;

    private PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if(playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        if(other.tag == "Player")
        {
            Debug.Log("Test");
            playerController.isEnterExit = true;
            GameRoot.ShowTips("Press \"E\" to enter", true, false);

            switch(sceneName)
            {
                case SceneName.MainSceneName:
                    playerController.sceneName = Constants.mainSceneName;

                    if (SceneManager.GetActiveScene().name == Constants.puzzle01SceneName)
                        GameRoot.instance.exitPuzzle = 1;
                    else if (SceneManager.GetActiveScene().name == Constants.puzzle02SceneName)
                        GameRoot.instance.exitPuzzle = 2;
                    else if (SceneManager.GetActiveScene().name == Constants.puzzle03SceneName)
                        GameRoot.instance.exitPuzzle = 3;

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

            switch(puzzleComplete)
            {
                case PuzzleComplete.Puzzle01Complete:
                    GameRoot.instance.puzzleCompleted[0] = true;
                    break;

                case PuzzleComplete.Puzzle02Complete:
                    GameRoot.instance.puzzleCompleted[1] = true;
                    break;

                case PuzzleComplete.Puzzle03Complete:
                    GameRoot.instance.puzzleCompleted[2] = true;
                    break;


                default:
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
