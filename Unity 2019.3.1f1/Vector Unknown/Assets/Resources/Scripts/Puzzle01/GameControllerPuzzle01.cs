using UnityEngine;

public class GameControllerPuzzle01 : GameControllerRoot
{
    public GameObject topCamPlayer;

    [HideInInspector]
    public Puzzle01Window P01W;
    [HideInInspector]
    public DatabasePuzzle01 DBP01;
    [HideInInspector]
    public int questionNum;
    [HideInInspector]
    public bool isTriggerQuestion = false;
    private GameObject player;
    private bool isInQues = false;

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

        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_00"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchCamera();
        }

        if (isTriggerQuestion && Input.GetKeyDown(KeyCode.E))
        {
            if(!isInQues)
            {
                IsLock(true);
                P01W.ShowInputPanel(true);
                isInQues = true;
                SetText("- Press 'E' to exit the question.\n" +
                             "- Press 'Z' to switch into the top-down camera.\n");
            }
            else
            {
                IsLock(false);
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
                IsLock(true);
                break;

            case -1:
                Camera.main.depth = 1;
                topCamPlayer.GetComponent<TopCameraPlayer>().isTopCamera = false;
                if (!isInQues)
                {
                    IsLock(false);
                }
                break;
        }
    }

    public void SendConstrains(float defaultScalar, float defaultX, float defaultY, float defaultZ)
    {
        P01W.defaultScalar = defaultScalar;
        P01W.defaultX = defaultX;
        P01W.defaultY = defaultY;
        P01W.defaultZ = defaultZ;
    }

    public void CheckAnswer(float scalar, float x, float y, float z)
    {
        DBP01.Calculation(questionNum, scalar, x, y, z);
    }

    public void SetText(string content)
    {
        SetText(P01W.txtInstruction, content);
    }

    public void IsLock(bool value)
    {
        Camera.main.GetComponent<CameraController>().isLock = value;
        player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().isLock = value;
    }
}
