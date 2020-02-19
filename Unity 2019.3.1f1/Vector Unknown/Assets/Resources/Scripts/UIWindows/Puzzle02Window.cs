using UnityEngine;

public class Puzzle02Window : WindowRoot
{
    private GameControllerPuzzle02 GCP02;

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        GCP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle02>();
        GCP02.InitGameController();
    }
}
