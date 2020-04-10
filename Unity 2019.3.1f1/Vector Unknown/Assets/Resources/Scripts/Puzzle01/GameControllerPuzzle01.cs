using UnityEngine;

public class GameControllerPuzzle01 : GameControllerRoot
{
    public GameObject topCamPlayer;
    public Transform sunLight;
    public Transform moonLight;
    public LPWAsset.LowPolyWaterScript ocean;
    public Material night;

    [HideInInspector]
    public Puzzle01Window P01W;
    [HideInInspector]
    public DatabasePuzzle01 DBP01;
    [HideInInspector]
    public int questionNum;
    [HideInInspector]
    public bool isTriggerQuestion = false;
    private GameObject player;
    [HideInInspector]
    public bool isInQues = false;

    public override void InitGameController(Puzzle01Window P01W)
    {
        Debug.Log("Init GameController for Puzzle01");
        base.InitGameController();

        Debug.Log("Connect Puzzle01 Window");
        this.P01W = P01W;

        Debug.Log("Connect Database of Puzzle01");
        DBP01 = GetComponent<DatabasePuzzle01>();

        Debug.Log("Call Database of Puzzle01 to connect");
        DBP01.InitDatabase();

        player = GameObject.FindGameObjectWithTag("Player");

        if(DialogueManager.showP01_00)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_00"));
            DialogueManager.showP01_00 = false;
        }

        if (GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] == true)
        {
            SetActive(sunLight, false);
            SetActive(moonLight, true);
            ocean.sun = moonLight.GetChild(0).GetComponent<Light>();
            RenderSettings.skybox = night;
        }
     }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchCamera();
        }

        if (isTriggerQuestion && questionNum !=0 && Input.GetKeyDown(KeyCode.E))
        {
            if(questionNum == 1 && DialogueManager.showP01_01)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_01"));
                DialogueManager.showP01_01 = false;
            }

            if(!isInQues)
            {
                GameRoot.instance.IsLock(true);
                P01W.ShowInputPanel(true);
                isInQues = true;
                SetText("- Press 'E' to exit the question.\n" +
                             "- Press 'Z' to switch into the top-down camera.\n");
            }
            else
            {
                GameRoot.instance.IsLock(false);
                P01W.ShowInputPanel(false);
                isInQues = false;
                SetText("- Press 'E' to input your answer.\n" +
                             "- Press 'Z' to switch into the top-down camera.\n");
            }
        }
    }

    private void SwitchCamera()
    {
        switch (Camera.main.depth)
        {
            case 1:
                Camera.main.depth = -1;
                topCamPlayer.GetComponent<TopCameraPlayer>().isTopCamera = true;
                GameRoot.instance.IsLock(true);
                break;

            case -1:
                Camera.main.depth = 1;
                topCamPlayer.GetComponent<TopCameraPlayer>().isTopCamera = false;
                if (!isInQues)
                {
                    GameRoot.instance.IsLock(false);
                }
                break;
        }
    }

    public void SendConstrains(float defaultScalar, float defaultX, float defaultY, float defaultZ, int questionNum)
    {
        P01W.defaultScalar = defaultScalar;
        P01W.defaultX = defaultX;
        P01W.defaultY = defaultY;
        P01W.defaultZ = defaultZ;
        P01W.questionNum = questionNum;

        this.questionNum = questionNum;
    }

    public bool CheckAnswer(float scalar, float x, float y, float z)
    {
        bool check = DBP01.Calculation(questionNum, scalar, x, y, z);

        if (questionNum == 1 && check && DialogueManager.showP01_02)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_02"));
            DialogueManager.showP01_02 = false;

        }

        if (check)
        {
            questionNum = 0;
        }

        return check;
    }

    public void SetText(string content)
    {
        SetText(P01W.txtInstruction, content);
    }
}
