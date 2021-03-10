using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PortalTrigger : MonoBehaviour
{
    public bool inPortal = false;
    private GameControllerPuzzle04 GCP04;
    private Puzzle04Controller PC04;
    private GameObject camera;
    private Quaternion prevCameraRotate;
    private float prevCamHeight;
    private float prevCamZoom;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
        PC04 = GetComponentInParent<Puzzle04Controller>();
        prevCameraRotate = camera.transform.rotation;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inPortal = true;

         //   prevCamHeight = camera.GetComponent<Camera2DFollow>().getCameraHeight();
        //    prevCamZoom = camera.GetComponent<Camera2DFollow>().getCameraZoom();
            prevCameraRotate = camera.transform.rotation;
            camera.GetComponent<Camera2DFollow>().setPortalStatus(true);
            camera.GetComponent<Camera2DFollow>().setTarget(PC04.getCameraTransform());
            camera.GetComponent<Camera2DFollow>().setRotation(PC04.getCameraTransform().rotation);
            //           camera.GetComponent<Camera2DFollow>().setCameraHeight(GetComponentInParent<Puzzle01Controller>().cameraHeight);
            //           camera.GetComponent<Camera2DFollow>().setCameraZoom(GetComponentInParent<Puzzle01Controller>().cameraZoom);
            GCP04.isInQues = true;

            GCP04.P04W.setPuzzleController(PC04);

            GCP04.P04W.ShowInputPanel(true);
            GCP04.P04W.ShowFeedbackPanel(true);
            GCP04.P04W.ShowCardPanel(true);

            assignCards();
            GCP04.P04W.setAnswer1(null);
            GCP04.P04W.setAnswer2(null);

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

        //    camera.GetComponent<Camera2DFollow>().setCameraHeight(prevCamHeight);
         //   camera.GetComponent<Camera2DFollow>().setCameraZoom(prevCamZoom);
            GCP04.isInQues = false;

            camera.GetComponent<Camera2DFollow>().setTarget(GameObject.FindGameObjectWithTag("Player").transform);
            camera.GetComponent<Camera2DFollow>().setRotation(prevCameraRotate);
            camera.GetComponent<Camera2DFollow>().setPortalStatus(false);
            
            
            GCP04.P04W.ShowInputPanel(false);
            GCP04.P04W.ShowFeedbackPanel(false);
            GCP04.P04W.ShowCardPanel(false);

            GCP04.P04W.setAnswer1(null);
            GCP04.P04W.setAnswer2(null);

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
        PC04.getVisualVector().getWindVector().SetActive(false);
        PC04.getVisualVector().getAnswerVector().SetActive(false);
        PC04.getVisualVector().getVector1().SetActive(false);
        PC04.getVisualVector().getVector2().SetActive(false);
        PC04.getVisualVector().getFinalVector().SetActive(false);
    }

    private void turnOnVisualVectors()
    {
        PC04.getVisualVector().getGapVector().SetActive(true);

        PC04.getVisualVector().getWindVector().SetActive(true);

   //     PC01.getVisualVector().getAnswerVector().SetActive(true);
        PC04.getVisualVector().getVector1().SetActive(true);
        PC04.getVisualVector().getVector2().SetActive(true);
        PC04.getVisualVector().getFinalVector().SetActive(true);
    }    
}
