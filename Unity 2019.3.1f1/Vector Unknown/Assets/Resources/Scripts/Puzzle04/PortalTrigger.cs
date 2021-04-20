using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PortalTrigger : MonoBehaviour
{
    public bool inPortal = false;
    private GameControllerPuzzle04 GCP04;
    public Puzzle04Controller PC04;
    private GameObject mainCamera;
    private Vector3 prevCameraPos;
    private Vector3 prevPlayerPos;
    private Quaternion prevCameraRotation;
    private bool cameraToPuzzle;
    private bool cameraToPlayer;
    private float triggerDelay;
    private float cameraMoveSpeed = 0.25f;
    private float cameraRotateSpeed = 0.07f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
        PC04 = this.GetComponentInParent<Puzzle04Controller>();
        cameraToPuzzle = false;
        cameraToPlayer = false;
    }

    public void Update()
    {
        if(cameraToPuzzle)
        {
            PC04.player.GetComponent<ThirdPersonCharacter>().enabled = false;
            PC04.player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            PC04.setCameraControls(false);
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, PC04.CameraStartPosition.transform.position, cameraMoveSpeed);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, Quaternion.LookRotation((PC04.cameraTarget.transform.position - mainCamera.transform.position).normalized), cameraRotateSpeed);

            if ((mainCamera.transform.position - PC04.CameraStartPosition.transform.position).magnitude < 2)
            {
                PC04.setCameraControls(true);
                cameraToPuzzle = false;

                PC04.player.GetComponent<ThirdPersonCharacter>().enabled = true;

            }

        }

        if(cameraToPlayer)
        {
            PC04.setCameraControls(false);
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, prevCameraPos + (PC04.player.transform.position - prevPlayerPos), cameraMoveSpeed);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, prevCameraRotation, cameraRotateSpeed);


            if ((mainCamera.transform.position - (prevCameraPos + (PC04.player.transform.position - prevPlayerPos))).magnitude < 0.1)
            {
                mainCamera.transform.rotation = prevCameraRotation;
                PC04.setCameraControls(true);
                triggerDelay = 10f;
                cameraToPlayer = false;

                if (GCP04.Difficulty < 3)
                {
                    mainCamera.GetComponent<Camera2DFollowMod>().setPortalStatus(false);
                    mainCamera.GetComponent<Camera2DFollowMod>().enabled = true;
                    mainCamera.GetComponent<Camera2DFollowMod>().resetCamera();
                }
                else
                {
                    mainCamera.GetComponent<CameraController>().enabled = true;
                    mainCamera.GetComponent<CameraController>().isLock = true;
                }
            }
        }

        if (triggerDelay > 0)
        {
            this.GetComponent<Collider>().enabled = false;
            triggerDelay--;
        }
        else
            this.GetComponent<Collider>().enabled = true;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inPortal = true;

            cameraMoveSpeed = 0.25f;
            cameraRotateSpeed = 0.07f;

            //Dialog Trigger
            if (PC04.convoTriggerNumber != 0)
            {
                GCP04.conversation(PC04.convoTriggerNumber);
            }

            prevCameraPos = mainCamera.transform.position;
            prevCameraRotation = mainCamera.transform.rotation;

            GCP04.isInQues = true;

            GCP04.P04W.setPuzzleController(PC04);

            GCP04.P04W.ShowInputPanel(true);
            GCP04.P04W.ShowFeedbackPanel(true);
            GCP04.P04W.ShowCardPanel(true);
            GCP04.P04W.ShowButtonPanel(true);

            GCP04.P04W.setInstructions("-L-Click, hold and Drag Answers\n-Hold R-Click and Drag to Rotate\n-Use MouseWheel to Zoom In/Out");

            GCP04.P04W.resetButtons();

            assignCards();
            GCP04.P04W.setAnswer1(null);
            PC04.setAnsCard1(Vector3.zero);
            GCP04.P04W.setAnswer2(null);
            PC04.setAnsCard2(Vector3.zero);
            turnOnVisualVectors();

            cameraToPuzzle = true;

            if (GCP04.Difficulty < 3)
            {
                mainCamera.GetComponent<Camera2DFollowMod>().setPortalStatus(true);
                mainCamera.GetComponent<Camera2DFollowMod>().enabled = false;
            }
            else
            {
                mainCamera.GetComponent<CameraController>().enabled = false;
                mainCamera.GetComponent<CameraController>().isLock = false;
            }

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

 //           mainCamera.transform.position = PC04.getCameraTransform().position;
 //           mainCamera.transform.LookAt(PC04.getCameraTarget().transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PC04.player.GetComponent<ThirdPersonUserControl>().enabled = true;
            prevPlayerPos = PC04.player.transform.position;

            if(PC04.getCorrect())
            {
                cameraMoveSpeed = 6f;
                cameraRotateSpeed = 0.5f;
            }

            inPortal = false;

            GCP04.isInQues = false;      
            
            GCP04.P04W.ShowInputPanel(false);
            GCP04.P04W.ShowFeedbackPanel(false);
            GCP04.P04W.ShowCardPanel(false);
            GCP04.P04W.ShowButtonPanel(false);

            GCP04.P04W.setInstructions(GCP04.P04W.defaultInstructions);

            GCP04.P04W.resetButtons();

            GCP04.P04W.resetCardPos();

            GCP04.P04W.setAnswer1(null);
            PC04.setAnsCard1(Vector3.zero);
            GCP04.P04W.setAnswer2(null);
            PC04.setAnsCard2(Vector3.zero);

            GCP04.P04W.scalar1.value = 1;
            GCP04.P04W.scalar2.value = 1;

            turnOffVisualVectors();

            cameraToPlayer = true;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

//            mainCamera.transform.position = prevCameraPos;
 //           mainCamera.transform.rotation = prevCameraRotation;

        }
    }

    /// <summary>
    /// Sends the Vector values from this PuzzleController to 
    /// the UI for use by Puzzle01Window
    /// </summary>
    private void assignCards()
    {
        GCP04.P04W.setCardValues(PC04.getAnswerCards());
        GCP04.P04W.setCardDisplay(PC04);
        GCP04.P04W.setFeedbackDisplay(PC04);
    }


    private void turnOffVisualVectors()
    {
        PC04.getVisualVector().getGapVector().SetActive(false);
        PC04.getVisualVector().getVector1().SetActive(false);
        PC04.getVisualVector().getVector2().SetActive(false);
        PC04.getVisualVector().getFinalVector().SetActive(false);

        PC04.getVisualVector().getXvector().SetActive(false);
        PC04.getVisualVector().getYvector().SetActive(false);
        PC04.getVisualVector().getZvector().SetActive(false);

        PC04.getVisualVector().deactivateAxis();
        PC04.getVisualVector().deactivateBarGrid();
        PC04.getVisualVector().toggleProjections(false);
        PC04.getVisualVector().toggleAnchors(false);
    }

    private void turnOnVisualVectors()
    {
        PC04.getVisualVector().getGapVector().SetActive(true);
        PC04.getVisualVector().getVector1().SetActive(true);
        PC04.getVisualVector().getVector2().SetActive(true);
        PC04.getVisualVector().getFinalVector().SetActive(true);

        switch(GCP04.Difficulty)
        {
            case 1:
                switch(PC04.getDirection())
                {
                    case Direction.X:
                        PC04.getVisualVector().setAxisNodes(true, false, false);
                        PC04.getVisualVector().setGridBarNodes(true, false, false);
                        break;
                    case Direction.Y:
                        PC04.getVisualVector().setAxisNodes(false, true, false);
                        PC04.getVisualVector().setGridBarNodes(false, true, false);
                        break;
                }
                break;
            case 2:
                PC04.getVisualVector().setAxisNodes(true, true, false);
                PC04.getVisualVector().setGridBarNodes(true, true, false);
                break;
            case 3:
                PC04.getVisualVector().setAxisNodes(true, true, true);
                PC04.getVisualVector().setGridBarNodes(true, true, true);
                break;
        }

        /**
        //if the puzzle difficulty is 2D or less:
        //do not draw the Z axis grids
        if (GCP04.Difficulty < 3)
        {
            PC04.getVisualVector().setGridSphereNodes(true, true, false);
            PC04.getVisualVector().setGridBarNodes(true, true, false);
        }
        else 
        {
            PC04.getVisualVector().setGridSphereNodes(true, true, true);
            PC04.getVisualVector().setGridBarNodes(true, true, true);
        }
        **/

    }    
}
