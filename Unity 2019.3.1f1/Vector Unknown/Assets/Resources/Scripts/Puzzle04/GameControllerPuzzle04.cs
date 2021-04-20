using UnityEngine;

public class GameControllerPuzzle04 : GameControllerRoot
{
    public GameObject topCamPlayer;

    [Header("STAGE LEVEL")]
    [Range(1, 3)]
    public int Difficulty;

    [Header("Environment Components")]
    public Transform sunLight;
    public Transform moonLight;
    public LPWAsset.LowPolyWaterScript ocean;
    public Material night;

    [Header("Question Trigger Components")]
    public int questionNum;
    public bool isInQues = false;
    public bool isTriggerQuestion = false;
    public bool isAnswerCorrect = true;

    [Header("Mast Trigger Components")]
    public bool isInMast = false;
    public bool isTriggerMast = false;
    public ParticleSystem congrats;
    //   public Transform endportal;

    public GameObject puzzleWindow;

    //MVC Components
    [HideInInspector]
    public Puzzle04Window P04W;
    [HideInInspector]
    //   public DatabasePuzzle01 DBP01;

    private AudioService audio04;

    private GameObject player;
    private Vector3 startPosition;
    private bool isFirstTimeTriggerQuestion = true;
    private Vector3 previousPosition;
    public int timer = 0;

    public void Start()
    {
        P04W = puzzleWindow.GetComponent<Puzzle04Window>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override void InitGameController(Puzzle04Window P04W)
    {
        Debug.Log("Init GameController for Puzzle04");
        base.InitGameController();

        Debug.Log("Connect Puzzle04 Window");
        this.P04W = P04W;

        audio04 = audioService;
  //      Debug.Log("Connect Database of Puzzle01");
  //      DBP01 = GetComponent<DatabasePuzzle01>();

  //     Debug.Log("Call Database of Puzzle01 to connect");
  //      DBP01.InitDatabase();

        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = player.transform.position;

        //Starter Conversation Managment
        conversationStart();

        if (GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] == true)
        {
            SetActive(sunLight, false);
            SetActive(moonLight, true);
            ocean.sun = moonLight.GetChild(0).GetComponent<Light>();
            RenderSettings.skybox = night;
        }

        //Init Components
       // SetActive(endportal, false);
     }

    private void Update()
    {

        if (player.transform.position.x - previousPosition.x < 0.01 && player.transform.position.y - previousPosition.y < 0.01 && player.transform.position.z - previousPosition.y < 0.01)
        {
            timer += (int)Time.deltaTime + 1;
            if (timer == 1200)
            {
                GameRoot.ShowTips("Press 'R' to reset position.", true, false);
            }
        }
        else
        {
            timer = 0;
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = startPosition;
            GameRoot.ShowTips("", true, false);
        }
/**
        //Update the instruction text based on the player status
        if (isInQues == true)
        {
            if (isTriggerQuestion)
            {
                SetText("- Press 'E': Exit the question.\n" +
                             "- Press 'Z': Top-down camera.");
            }
            else
            {
                SetText("- Press 'E': Trigger the question.");
            }
        }
        else
        {
            SetText("Please stand on the question platform.");
        }
        //************************************************************
**/

        if (!DialogueManager.isInDialogue)
        {
            // Z key to switch camera
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SwitchCamera();
            }

            if (isInQues && questionNum != 0)
            {
                // E key for the question tigger and exit
                if (!isTriggerQuestion && Input.GetKeyDown(KeyCode.E))
                {
                    //Show the dialogue when player trigger the question
                    if (questionNum == 1 && DialogueManager.showP01_01)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_01"));
                        DialogueManager.showP01_01 = false;
                    }
                    else if (questionNum == 2 && DialogueManager.showP01_03)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_03"));
                        DialogueManager.showP01_03 = false;
                    }
                    else if (questionNum == 3 && DialogueManager.showP01_05)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_05"));
                        DialogueManager.showP01_05 = false;
                    }
                    else if (questionNum == 4 && DialogueManager.showP01_07)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_07"));
                        DialogueManager.showP01_07 = false;
                    }
                    //********************************************************

                    isTriggerQuestion = true;

                    P04W.ShowInputPanel(true);
                    P04W.ShowFeedbackPanel(true);
                    P04W.ShowCardPanel(true);

                    GameRoot.instance.IsLock(true);

                    //Resume cannot unlock the lock
                    GameRoot.isPuzzleLock = true;
 
                    //Dialogue manager cannot unlock the lock;
                    DialogueManager.isPuzzleLock = true;

                    //Set current question tips in feedback panel
                    if (isFirstTimeTriggerQuestion)
                    {
                    //    P01W.SetFeedbackQuestionTips("Find the displacement\n" + DBP01.GetCurrentVector(questionNum) + " ->" + DBP01.GetCurrentVector(questionNum + 1));
                        isFirstTimeTriggerQuestion = false;
                    }
                }
                else if (isTriggerQuestion && Input.GetKeyDown(KeyCode.E))
                {
                    isTriggerQuestion = false;

                    P04W.ShowInputPanel(false);
                    P04W.ShowFeedbackPanel(false);
                    P04W.ShowCardPanel(false);


                    GameRoot.instance.IsLock(false);

                    //Resume can unlock the lock
                    GameRoot.isPuzzleLock = false;

                    //Dialogue manager can unlock the lock;
                    DialogueManager.isPuzzleLock = false;
                }
            }

            //Trigger Mast prop to end the puzzle01
            if(isInMast && !isTriggerMast)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    isTriggerMast = true;

                    congrats.Play();
                //    endportal.gameObject.SetActive(true);

                    //Congratulation FX
                    audioService.PlayFXAudio(Constants.audioP01Congratulation);

                    if (DialogueManager.showP01_09)
                    {
                        FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle01_09"));
                        DialogueManager.showP01_09 = false;
                    }
                }
            }
        }

        previousPosition = player.transform.position;
    }

    private void conversationStart()
    {
        switch(Difficulty)
        {
            case 1:
                if (DialogueManager.showP04_1[0])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle04-1_00"));
                    DialogueManager.showP04_1[0] = false;
                }

                break;
            case 2:
                if (DialogueManager.showP04_2[0])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle04-2_00"));
                    DialogueManager.showP04_2[0] = false;
                }

                break;
            case 3:
                if (DialogueManager.showP04_3[0])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle04-3_00"));
                    DialogueManager.showP04_3[0] = false;
                }

                break;
        }
    }

    /// <summary>
    /// Selects the conversation dialog to pop up
    /// Requires that the associated bool[] and conversation file be established
    /// with the DialogManager.cs
    /// </summary>
    /// <param name="val"></param>
    public void conversation(int val)
    {
        switch (Difficulty)
        {
            case 1: //Level 1 Dialogs
                if (DialogueManager.showP04_1[val])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle04-1_0" + val.ToString()));
                    DialogueManager.showP04_1[val] = false;
                }

                break;
            case 2: //Level 2 Dialogs
                if (DialogueManager.showP04_2[val])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle04-2_0" + val.ToString()));
                    DialogueManager.showP04_2[val] = false;
                }

                break;
            case 3: //Level 3 Dialogs
                if (DialogueManager.showP04_3[val])
                {
                    FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle04-3_0" + val.ToString()));
                    DialogueManager.showP04_3[val] = false;
                }

                break;
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
                if (!isTriggerQuestion)
                {
                    GameRoot.instance.IsLock(false);
                }
                break;
        }
    }

    public void setResetPos(Vector3 pos)
    {
        startPosition = pos;
    }

    public Vector3 getResetPos()
    {
        return startPosition;
    }

    public AudioService GetAudioService()
    {
        return audio04;
    }


}
