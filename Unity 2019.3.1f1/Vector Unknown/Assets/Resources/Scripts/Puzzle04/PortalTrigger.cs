using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public bool inPortal = false;
    private GameControllerPuzzle04 GCP04;
    private Puzzle04Controller PC04;
    private GameObject mainCamera;
    private Vector3 prevCameraPos;
    private Quaternion prevCameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
        PC04 = GetComponentInParent<Puzzle04Controller>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inPortal = true;

            PC04.player.GetComponent<GrappleCode>().setGoalPoint(PC04.goalPoint);

            prevCameraPos = mainCamera.transform.position;
            prevCameraRotation = mainCamera.transform.rotation;

            mainCamera.GetComponent<Camera2DFollowMod>().setPortalStatus(true);
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
            GCP04.P04W.setAnswer2(null);

            mainCamera.GetComponent<Camera2DFollowMod>().enabled = false;
            mainCamera.transform.position = PC04.getCameraTransform().position;
            mainCamera.transform.LookAt(PC04.getCameraTarget().transform.position);


            turnOnVisualVectors();


            Debug.Log("portal enter");
            Debug.Log(PC04.getAnswerVector());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inPortal = false;

            GCP04.isInQues = false;

            mainCamera.GetComponent<Camera2DFollowMod>().setPortalStatus(false);
            
            
            GCP04.P04W.ShowInputPanel(false);
            GCP04.P04W.ShowFeedbackPanel(false);
            GCP04.P04W.ShowCardPanel(false);
            GCP04.P04W.ShowButtonPanel(false);

            GCP04.P04W.setInstructions("");

            GCP04.P04W.resetButtons();

            GCP04.P04W.setAnswer1(null);
            GCP04.P04W.setAnswer2(null);


            mainCamera.GetComponent<Camera2DFollowMod>().enabled = true;
            mainCamera.transform.position = prevCameraPos;
            mainCamera.transform.rotation = prevCameraRotation;

            GCP04.P04W.resetCardPos();

            GCP04.P04W.scalar1.value = 1;
            GCP04.P04W.scalar2.value = 1;

            turnOffVisualVectors();

            Debug.Log("portal exit");
        }
    }

    /// <summary>
    /// Sends the Vector values from this PuzzleController to 
    /// the UI for use by Puzzle01Window
    /// </summary>
    private void assignCards()
    {

        GCP04.P04W.setCardValues(PC04.getAnswerCards());
        /**
        GCP04.P04W.cardPanel.Find("Card1").GetComponent<CardVectors>().setCardVector(PC04.getAnswerCards()[0]);
        GCP04.P04W.cardPanel.Find("Card2").GetComponent<CardVectors>().setCardVector(PC04.getAnswerCards()[1]);
        GCP04.P04W.cardPanel.Find("Card3").GetComponent<CardVectors>().setCardVector(PC04.getAnswerCards()[2]);
        GCP04.P04W.cardPanel.Find("Card4").GetComponent<CardVectors>().setCardVector(PC04.getAnswerCards()[3]);
        **/
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

        PC04.getVisualVector().deactivateSphereGrid();
        PC04.getVisualVector().deactivateBarGrid();

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
                        PC04.getVisualVector().setGridSphereNodes(true, false, false);
                        PC04.getVisualVector().setGridBarNodes(true, false, false);
                        break;
                    case Direction.Y:
                        PC04.getVisualVector().setGridSphereNodes(false, true, false);
                        PC04.getVisualVector().setGridBarNodes(false, true, false);
                        break;
                }
                break;
            case 2:
                PC04.getVisualVector().setGridSphereNodes(true, true, false);
                PC04.getVisualVector().setGridBarNodes(true, true, false);
                break;
            case 3:
                PC04.getVisualVector().setGridSphereNodes(true, true, true);
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
