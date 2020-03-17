using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool isEnterExit = false;
    [HideInInspector]
    public string sceneName;

    private void Update()
    {
        if(isEnterExit)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameRoot.ShowTips("", false, false);

                GameRoot.instance.puzzleSystem.EnterPuzzle(sceneName);
            }
        }
    }
}
