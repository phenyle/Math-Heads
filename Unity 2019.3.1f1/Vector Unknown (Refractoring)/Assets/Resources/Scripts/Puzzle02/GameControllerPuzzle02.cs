using UnityEngine;

public class GameControllerPuzzle02 : GameControllerRoot
{

    [HideInInspector]
    public Puzzle02Window P02W;
    [HideInInspector]
    public DatabasePuzzle02 DBP02;

    public override void InitGameController(Puzzle02Window P02W)
    {
        Debug.Log("Init GameController Puzzle02");
        base.InitGameController();

        Debug.Log("Connect Puzzle02 Window");
        this.P02W = P02W;

        Debug.Log("Connect Database of Puzzle02");
        DBP02 = GetComponent<DatabasePuzzle02>();

        Debug.Log("Call Database of Puzzle02 to connect");
        DBP02.InitDatabase();
    }


}
