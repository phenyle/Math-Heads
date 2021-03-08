using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;


public class FallTrigger : MonoBehaviour
{
    private float prevCamHeight;
    private float prevCamZoom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("fall trigger");

            prevCamHeight = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollow>().getCameraHeight();
            prevCamZoom = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollow>().getCameraZoom();

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollow>().setCameraHeight(0);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollow>().setCameraZoom(-50);

            GetComponentInParent<GapTriggersController>().fall = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {        
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollow>().setCameraHeight(prevCamHeight);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera2DFollow>().setCameraZoom(prevCamZoom);
        
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01Redux>().getResetPos();

        GetComponentInParent<GapTriggersController>().fall = false;

    }

}
