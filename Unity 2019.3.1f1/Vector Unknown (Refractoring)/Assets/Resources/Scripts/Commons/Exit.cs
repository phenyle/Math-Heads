using UnityEngine;

public class Exit : MonoBehaviour
{   
    private void OnTriggerEnter(Collider other)
    {
        GameRoot.instance.puzzleSystem.EnterPuzzle(Constants.mainSceneName);
    }
}
