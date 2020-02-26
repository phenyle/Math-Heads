using UnityEngine;

public class DatabasePuzzle02 : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;

    public void InitDatabase()
    {
        Debug.Log("Connect GameController of Puzzle02");
        GCP02 = GetComponent<GameControllerPuzzle02>();
    }


}
