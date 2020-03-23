using UnityEngine;

public class MainWindow : WindowRoot
{
    public bool isInit;

    private GameControllerMain GCM;

    private void Start()
    {
        if(isInit)
        {
            InitWindow();
        }
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Main Window");
        base.InitWindow();

        GCM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerMain>();
        GCM.InitGameController(this);
    }
}
