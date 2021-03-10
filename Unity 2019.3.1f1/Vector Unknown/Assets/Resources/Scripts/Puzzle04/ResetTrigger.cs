using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : MonoBehaviour
{
    private Vector3 resetPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            resetPos = GetComponentInParent<GapTriggersController>().GetComponentInParent<Puzzle04Controller>().reset.transform.position;
            GetComponentInParent<GapTriggersController>().reset = true;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle04>().setResetPos(resetPos);
            Debug.Log("reset trigger");
        }
    }
}
