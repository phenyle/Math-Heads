using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FallTrigger : MonoBehaviour
{
    private GameControllerPuzzle04 GCP04;
    private float prevCamHeight;
    private float prevCamZoom;

    public void Start()
    {
        GCP04 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("fall trigger");

            if (GCP04.Difficulty < 3)
            { 
                prevCamHeight = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().getCameraHeight();
                prevCamZoom = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().getCameraZoom();

                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraHeight(0);
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraZoom(-50);
            }
            GetComponentInParent<GapTriggersController>().fall = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GCP04.Difficulty < 3)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraHeight(prevCamHeight);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraZoom(prevCamZoom);
        }
        
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>().getResetPos();
        
        GetComponentInParent<GapTriggersController>().fall = false;

    }

}
