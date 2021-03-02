using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PortalTrigger : MonoBehaviour
{
    public bool inPortal = false;
    private GameControllerPuzzle01 GCP01;
    private Puzzle01Controller PC01;
    private GameObject camera;
    private float prevCamHeight;
    private float prevCamZoom;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        GCP01 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>();
        PC01 = GetComponentInParent<Puzzle01Controller>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inPortal = true;

            prevCamHeight = camera.GetComponent<Camera2DFollow>().getCameraHeight();
            prevCamZoom = camera.GetComponent<Camera2DFollow>().getCameraZoom();
            camera.GetComponent<Camera2DFollow>().setCameraHeight(GetComponentInParent<Puzzle01Controller>().cameraHeight);
            camera.GetComponent<Camera2DFollow>().setCameraZoom(GetComponentInParent<Puzzle01Controller>().cameraZoom);
            GCP01.isInQues = true;

            GCP01.P01W.setPuzzleController(PC01);

            GCP01.P01W.ShowInputPanel(true);
            GCP01.P01W.ShowFeedbackPanel(true);
            GCP01.P01W.ShowCardPanel(true);

            assignCards();
            GCP01.P01W.setAnswer1(null);
            GCP01.P01W.setAnswer2(null);

            turnOnVisualVectors();


            Debug.Log("portal enter");
            Debug.Log(PC01.getAnswerVector());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inPortal = false;

            camera.GetComponent<Camera2DFollow>().setCameraHeight(prevCamHeight);
            camera.GetComponent<Camera2DFollow>().setCameraZoom(prevCamZoom);
            GCP01.isInQues = false;


            GCP01.P01W.ShowInputPanel(false);
            GCP01.P01W.ShowFeedbackPanel(false);
            GCP01.P01W.ShowCardPanel(false);

            GCP01.P01W.setAnswer1(null);
            GCP01.P01W.setAnswer2(null);

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
        GameObject.Find("Card1").GetComponent<CardVectors>().setCardVector(PC01.getAnswerCards()[0]);
        GameObject.Find("Card2").GetComponent<CardVectors>().setCardVector(PC01.getAnswerCards()[1]);
        GameObject.Find("Card3").GetComponent<CardVectors>().setCardVector(PC01.getAnswerCards()[2]);
        GameObject.Find("Card4").GetComponent<CardVectors>().setCardVector(PC01.getAnswerCards()[3]);
        GCP01.P01W.setCardDisplay(PC01);
        GCP01.P01W.setFeedbackDisplay(PC01);
    }


    private void turnOffVisualVectors()
    {
        PC01.getVisualVector().getGapVector().SetActive(false);
        PC01.getVisualVector().getWindVector().SetActive(false);
        PC01.getVisualVector().getAnswerVector().SetActive(false);
        PC01.getVisualVector().getVector1().SetActive(false);
        PC01.getVisualVector().getVector2().SetActive(false);
        PC01.getVisualVector().getFinalVector().SetActive(false);
    }

    private void turnOnVisualVectors()
    {
        PC01.getVisualVector().getGapVector().SetActive(true);

        PC01.getVisualVector().getWindVector().SetActive(true);

   //     PC01.getVisualVector().getAnswerVector().SetActive(true);
        PC01.getVisualVector().getVector1().SetActive(true);
        PC01.getVisualVector().getVector2().SetActive(true);
        PC01.getVisualVector().getFinalVector().SetActive(true);
    }    
}
