using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FallTrigger : MonoBehaviour
{
    private float prevCamHeight;
    private float prevCamZoom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("fall trigger");

            prevCamHeight = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().getCameraHeight();
            prevCamZoom = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().getCameraZoom();

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraHeight(0);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraZoom(-50);

            GetComponentInParent<GapTriggersController>().fall = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {        
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraHeight(prevCamHeight);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollowMod>().setCameraZoom(prevCamZoom);
        
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>().getResetPos();

        GetComponentInParent<GapTriggersController>().fall = false;

    }

}
