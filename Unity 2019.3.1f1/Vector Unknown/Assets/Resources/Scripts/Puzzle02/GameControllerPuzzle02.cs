﻿using UnityEngine;
using UnityEngine.UI;

public class GameControllerPuzzle02 : GameControllerRoot
{
    //variables 
    public int[] currentTransformMatrix = new int[] { 0, 0, 0, 0 };
    public int[] currentVector = new int[] { 0, 0 };

    private int[] selectedTransformMatrix = new int[] { 0, 0, 0, 0 };
    private int[] selectedVector = new int[] { 0, 0 };

    public GameObject firingCannon;

    [HideInInspector]
    public Puzzle02Window P02W;
    [HideInInspector]
    public DatabasePuzzle02 DBP02;
    [HideInInspector]
    public GameRoot GR;
    private GameObject player;

    //camera stuff
    public GameObject topCamera;
    public GameObject MainCamera;
    public bool topCameraActive = false;

    //trigger bools
    public bool isCannonTrigger = false;
    public bool isCannonballTrigger = false;
    public bool isMainCannonTrigger = false;
    public bool ballIsFlying = false;
    public bool isCannonSelected = false;
    public bool isBallSelected = false;

    //cannonball 
    public Transform cannonball;
    private Transform tempCannonball;
    public Vector3 targetPosition;

    //main cannonbarrel
    public GameObject cannonBarrel;
    public GameObject maincannonText;
    public GameObject maincannonBrackets;

    //art stuff
    public Material trailMaterial;
    public Material trailMaterialCorrect;
    public Material trailMaterialWrong;

    //UI
    public GameObject[] TopViewText;
    public GameObject[] normalText;
    public bool topViewOn = false;
    public int ActiveBoat = 0;
    public Text Matrix;
    public Text Vector;
    public Image[] shipImages;
    //public Image activeBoat;
    public Sprite inactiveBoat;

    //particle systems
    public GameObject cannonBlast;

    //Dialogue systems
    private static bool showP02_00 = true;

    //Boat selection feature
    

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

        //GR = GameObject.FindGameObjectWithTag("").GetComponent<GameRoot>();

        player = GameObject.FindGameObjectWithTag("Player");

        //Show Dialogue once
        if (showP02_00)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(resourceService.LoadConversation("Puzzle02_00"));
            showP02_00 = false;
        }

        cannonBarrel.gameObject.SetActive(false);

        trailMaterial = trailMaterialWrong;

        foreach (GameObject text in TopViewText) { text.gameObject.SetActive(false); }
        foreach (GameObject text in normalText) { text.gameObject.SetActive(false); }
        normalText[0].gameObject.SetActive(true);

        //stop particle effects from playing at start
        cannonBlast.GetComponent<ParticleSystem>().playOnAwake = false;

        maincannonText.gameObject.GetComponent<TextMesh>().text = "0\n0";

        Matrix = P02W.Matrix;
        Vector = P02W.Vector;
        shipImages = P02W.shipImages;
    }
   
    void Update()
    {
        if (ActiveBoat == 6)
        {
            GameRoot.ShowTips("You Completed the level!", true, false);
            GameRoot.instance.puzzleCompleted[1] = true;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchCamera();
            topViewOn = !topViewOn;

            if (topViewOn)
            {
                TopViewText[ActiveBoat].gameObject.SetActive(true);
                TopViewText[6].gameObject.SetActive(true);
                normalText[ActiveBoat].gameObject.SetActive(false); 
            }
            else
            {
                TopViewText[ActiveBoat].gameObject.SetActive(false);
                TopViewText[6].gameObject.SetActive(false);
                normalText[ActiveBoat].gameObject.SetActive(true);
            }

            Debug.Log("Z was pressed");
        }

        if (isCannonTrigger && Input.GetKeyDown(KeyCode.E))
        {
            cannonBarrel.SetActive(true);

            selectedVector = currentVector;
            isCannonSelected = true;
            Debug.Log("Selected Vector " + selectedVector[0] + ", " + selectedVector[1]);
            maincannonText.gameObject.GetComponent<TextMesh>().text = selectedVector[0] + "\n" + selectedVector[1];
            Vector.text = selectedVector[0] + "\n" + selectedVector[1];
            //---------------------------------New Tips Function--------------------------------------
            if (isCannonSelected && isBallSelected)
                GameRoot.ShowTips("The main gun is ready to fire", true, true);
            else
                GameRoot.ShowTips("Go pick the Cannon Ball or pick another Cannon", true, true);
            //--------------------------------------------------------------------------------------------
        }

        if (isCannonballTrigger && Input.GetKeyDown(KeyCode.E))
        {
            selectedTransformMatrix = currentTransformMatrix;
            isBallSelected = true;
            Debug.Log("Selected Matrix " + selectedTransformMatrix[0] + ", " + selectedTransformMatrix[1] + ", " +
                selectedTransformMatrix[2] + ", " + selectedTransformMatrix[3]);
            Matrix.text = selectedTransformMatrix[0] + " " + selectedTransformMatrix[1] + "\n" +
                selectedTransformMatrix[2] + " " + selectedTransformMatrix[3];
            //---------------------------------New Tips Function--------------------------------------
            if (isCannonSelected && isBallSelected)
                GameRoot.ShowTips("The main gun is ready to fire", true, true);
            else
                GameRoot.ShowTips("Go pick the Cannon or pick another Cannon Ball", true, true);
            //--------------------------------------------------------------------------------------------
        }

        if (isMainCannonTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (selectedTransformMatrix != new int[] { 0, 0, 0, 0 } && selectedVector != new int[] { 0, 0 })
            {
                FireCannon();
                Debug.Log("cannon fired");
                isBallSelected = false; 
                isCannonSelected = false;
            }
        }

        if (ballIsFlying)
        {
            tempCannonball.transform.position += targetPosition * Time.deltaTime;

            if (tempCannonball.transform.position.y <= -10)
            {
                ballIsFlying = false;
            }
        }
    }

    private void SwitchCamera()
    {
        if (topCameraActive)
        {
            MainCamera.gameObject.SetActive(true);

            topCamera.gameObject.SetActive(false);

            topCameraActive = false;
        }
        else if (!topCameraActive)
        {
            MainCamera.gameObject.SetActive(false);

            topCamera.gameObject.SetActive(true);

            topCameraActive = true;
        }
    }

    //public void updateCannonBalls()
    //{
    //    cannonballs[0].GetComponent<cannonballController>().UpdateValues(ActiveBoat);
    //    cannonballs[1].GetComponent<cannonballController>().UpdateValues(ActiveBoat);
    //    cannonballs[2].GetComponent<cannonballController>().UpdateValues(ActiveBoat);
    //    cannonballs[3].GetComponent<cannonballController>().UpdateValues(ActiveBoat);
    //}

    public void UpdateUI()
    {
        shipImages[ActiveBoat].sprite = inactiveBoat;
    }

    private void FireCannon()
    {
        targetPosition = new Vector3 (0, -5, 0);
        int[] targetVector = DBP02.calculation(selectedVector, selectedTransformMatrix);
        targetPosition.x = targetVector[0];
        targetPosition.z = targetVector[1];
        maincannonText.gameObject.GetComponent<TextMesh>().text = targetPosition.x + "\n" + targetPosition.z;
        Debug.Log("Cannon is aiming at :(" + targetPosition.x + ", " + targetPosition.z + ")");
        cannonBarrel.SetActive(false);
        tempCannonball = Instantiate(cannonball, firingCannon.transform.position + new Vector3(0, 2.1f, 1.6f), firingCannon.transform.rotation);

        tempCannonball.GetComponent<TrailRenderer>().material = trailMaterialWrong;
        maincannonBrackets.GetComponent<TextMesh>().color = Color.red;
        maincannonText.GetComponent<TextMesh>().color = Color.red;

        for (int i = 0; i < 6; i++)
        {
           // Debug.Log("target is compared to :(" + DBP02.tragetMatracies[i, 0] + ", " + DBP02.tragetMatracies[i, 1] + ")");
            if (DBP02.tragetMatracies[i, 0] == targetPosition.x && DBP02.tragetMatracies[i, 1] == targetPosition.z)
            {
                tempCannonball.GetComponent<TrailRenderer>().material = trailMaterialCorrect;
                maincannonBrackets.GetComponent<TextMesh>().color = Color.green;
                maincannonText.GetComponent<TextMesh>().color = Color.green;
                Debug.Log("Will Hit!");
            }
        }

        ballIsFlying = true;
        cannonBlast.SetActive(true);
        cannonBlast.GetComponent<ParticleSystem>().Play(true);  
    }
}
