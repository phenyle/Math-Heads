using UnityEngine;

public class DatabasePuzzle03 : MonoBehaviour
{
    public Transform[] SwitchGroups;

    private GameControllerPuzzle03 GCP03;

    public void InitDatabase()
    {
        Debug.Log("Connect GameController of Puzzle03");
        GCP03 = GetComponent<GameControllerPuzzle03>();
    }
}
