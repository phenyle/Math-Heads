﻿using UnityEngine;

public class GameControllerMain : GameControllerRoot
{
    [Header("Player & Camera")]
  
    public Transform PCFromPuzzle01;
    public Transform PCFromPuzzle02;
    public Transform PCFromPuzzle03;
    public Transform Chest_1;
    public Transform Chest_2;
    public Transform Chest_3;
    public Transform Chest_4;
    // public Transform Loadin;
    public Transform PCFromPuzzle04; // puzzle 4 area to pop out of

    public Transform sunLight;
    public Transform moonLight;
    public LPWAsset.LowPolyWaterScript ocean;
    public Material night;
    public Transform portalPuzzle03; 
    public GameObject camera;
    public Sprite[] spriteChecks;

    [HideInInspector]
    public MainWindow MW;

    private Transform player;
    private Vector3 startPosition;
    private Vector3 previousPosition;
    private int timer = 0;

    public override void InitGameController(MainWindow MW)
    {
        Debug.Log("Init GameController Main");
        base.InitGameController();

        Debug.Log("Connect Main Window");
        this.MW = MW;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = player.transform.position;

        switch (GameRoot.instance.exitPuzzle)
        { 
            case 1:
                SetActive(PCFromPuzzle01, true);
                SetActive(PCFromPuzzle02, false);
                SetActive(PCFromPuzzle03, false);
               // SetActive(Loadin, true);
                SetActive(PCFromPuzzle04, false);

                break;

            case 2:
                SetActive(PCFromPuzzle01, false);
                SetActive(PCFromPuzzle02, true);
                SetActive(PCFromPuzzle03, false);
               // SetActive(Loadin, false);
                SetActive(PCFromPuzzle04, false);
                break;

            case 3:
                SetActive(PCFromPuzzle01, false);
                SetActive(PCFromPuzzle02, false);
                SetActive(PCFromPuzzle03, true);
               // SetActive(Loadin, false);
                SetActive(PCFromPuzzle04, false);
                break;


            case 4: // new player object is activated and others are turned off
                SetActive(PCFromPuzzle01, false);
                SetActive(PCFromPuzzle02, false);
                SetActive(PCFromPuzzle03, false);
                //SetActive(Loadin, false);
                 SetActive(PCFromPuzzle04, true);
                break;
        }
        //*****************Start of the game 
        if (GameRoot.instance.puzzleCompleted[0] == false)
        {
            if (DialogueManager.showIntro)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Intro"));
                DialogueManager.showIntro = false;
                MW.SetInstructionText("Explore the Land.");
            }
            else
            {
                camera.GetComponent<CameraController>().isLock = true;
            }

          
        }

        //*****************If player only finish puzzle 01 and not 2
        if (GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] != true)
        {
            if(DialogueManager.showM_00)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Main_00"));
                DialogueManager.showM_00 = false;
            }
            else
            {
                camera.GetComponent<CameraController>().isLock = true;
            }

            //Set the instructional text and check image
            MW.SetInstructionText("Find & enter the cave to beat the pirates.");
            MW.SetCheckImage(0, spriteChecks[0]);
            SetActive(Chest_1, true);
        }
        //If player finish puzzle 01 & puzzle 02
        else if(GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] == true && GameRoot.instance.puzzleCompleted[2] == false)
        {
            SetActive(sunLight, false);
            SetActive(moonLight, true);
            ocean.sun = moonLight.GetChild(0).GetComponent<Light>();

       
            
            SetActive(Chest_2, true);

            RenderSettings.skybox = night;
            SetActive(portalPuzzle03, true);

            if(DialogueManager.showM_01)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Main_01"));
                DialogueManager.showM_01 = false;
            }
            else
            {
                camera.GetComponent<CameraController>().isLock = true;
            }

            MW.SetInstructionText("Find & enter the pirates' hideout to save villagers.");
            MW.SetCheckImage(0, spriteChecks[0]);
            MW.SetCheckImage(1, spriteChecks[1]);
        }
        //***************** if PLayer finishes puzzle 1, 2 ,and 3
        else if (GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] == true && GameRoot.instance.puzzleCompleted[2])
        {

            if (DialogueManager.showM_00)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Main_OnePuzzleLeft"));
                DialogueManager.showM_00 = false;
            }
            else
            {
                camera.GetComponent<CameraController>().isLock = true;
            }

            SetActive(Chest_3, true);
            MW.SetCheckImage(0, spriteChecks[0]);
            MW.SetCheckImage(1, spriteChecks[1]);
            MW.SetCheckImage(2, spriteChecks[2]);

            camera.GetComponent<CameraController>().isLock = true;
        }

        //***************** if PLayer finishes puzzle 1, 2, 3 and 4
        else if (GameRoot.instance.puzzleCompleted[0] == true && GameRoot.instance.puzzleCompleted[1] == true && GameRoot.instance.puzzleCompleted[2] && GameRoot.instance.puzzleCompleted[3])
        {

           
            SetActive(Chest_4, true);
            MW.SetCheckImage(0, spriteChecks[0]);
            MW.SetCheckImage(1, spriteChecks[1]);
            MW.SetCheckImage(2, spriteChecks[2]);
            MW.SetCheckImage(3, spriteChecks[3]);

            camera.GetComponent<CameraController>().isLock = true;
        }
        //***************** if Player finishes puzzle 2
        if (GameRoot.instance.puzzleCompleted[1] == true)
        {


            SetActive(Chest_2, true);
            MW.SetCheckImage(1, spriteChecks[1]);

            camera.GetComponent<CameraController>().isLock = true;
        }
        //***************** if PLayer finishes puzzle 3
        if (GameRoot.instance.puzzleCompleted[2] == true)
        {


            SetActive(Chest_3, true);
            MW.SetCheckImage(2, spriteChecks[2]);

            camera.GetComponent<CameraController>().isLock = true;
        }
        //***************** if PLayer finishes puzzle 4
         if (GameRoot.instance.puzzleCompleted[3] == true)
        {
            if (DialogueManager.showM_01)
            {
                FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Main_P4_completed"));
                DialogueManager.showM_01 = false;
            }
            else
            {
                camera.GetComponent<CameraController>().isLock = true;
            }

            SetActive(Chest_4, true);
           
            MW.SetCheckImage(3, spriteChecks[3]);

            camera.GetComponent<CameraController>().isLock = true;
        }
        // DialogueManager.isInDialogue = true;
        // DialogueManager.isPuzzleLock = false;

    }

	void Update()
    {
        // reset position tip
        // if (player.transform.position.x - previousPosition.x < 0.01 && player.transform.position.y - previousPosition.y < 0.01 && player.transform.position.y - previousPosition.y < 0.01)
        // {
        //     timer += (int)Time.deltaTime + 1;
        //     if (timer == 1200)
        //     {
        //         GameRoot.ShowTips("Press 'R' to reset position.", true, false);
        //     }
        // }
        // else
        // {
        //     timer = 0;
        // }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = startPosition;
            GameRoot.ShowTips("", true, false);
        }

		// if (!DialogueManager.isInDialogue)
		// {
		// 	DialogueManager.isInDialogue = false;
		// }

        previousPosition = player.transform.position;
    }
}
