using UnityEngine;

public class Puzzle02Window : WindowRoot
{
    private GameControllerPuzzle02 GCP02;

    private void Start()
    {
        InitWindow();
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle02 window");
        base.InitWindow();

        GCP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle02>();

        Debug.Log("Call GameController of Puzzle02 to connect");
        GCP02.InitGameController(this);
    }


}
