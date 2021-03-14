using UnityEngine;
using UnityEngine.UI;

public class Puzzle02Window : WindowRoot
{
    public bool isInit = false;
    public Image[] shipImages;
    public Text Matrix, Vector;

    private GameControllerPuzzle02 GCP02_01;
    private GameControllerPuzzle02 GCP02_02;

    private GameObject stage01;
    private GameObject stage02;

    private void Start()
    {
        if(isInit)
        {
            InitWindow();
        }
    }

    protected override void InitWindow()
    {
        Debug.Log("Init Puzzle02 window");
        base.InitWindow();

        GCP02_01 = GameObject.Find("GameController_Puzzle02_01").GetComponent<GameControllerPuzzle02>();

        Debug.Log("Call GameController of Puzzle02 to connect");
        GCP02_01.InitGameController(this);
    }

    public void switchStage()
    {
        stage01 = GameObject.Find("Stage1");
        stage01.SetActive(false);
        // stage02 = GameObject.Find("Stage2");
        // stage02.SetActive(true); 
        GCP02_02 = GameObject.Find("GameController_Puzzle02_02").GetComponent<GameControllerPuzzle02>();
        GCP02_02.InitGameController(this);
    }
}
