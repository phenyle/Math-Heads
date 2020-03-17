using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWindow : WindowRoot
{
    private GameControllerMain GCM;

    private void Start()
    {
        InitWindow();
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Main Window");
        base.InitWindow();

        GCM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerMain>();
        GCM.InitGameController(this);
    }
}
