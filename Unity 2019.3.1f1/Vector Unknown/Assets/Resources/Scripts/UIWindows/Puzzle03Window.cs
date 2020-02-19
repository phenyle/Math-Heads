using UnityEngine;

public class Puzzle03Window : WindowRoot
{
    private GameControllerPuzzle03 GCP03;

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle01 window");
        base.InitWindow();

        GCP03 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle03>();
        GCP03.InitGameController();
    }
}
