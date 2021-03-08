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
            resetPos = GetComponentInParent<GapTriggersController>().GetComponentInParent<Puzzle01Controller>().reset.transform.position;
            GetComponentInParent<GapTriggersController>().reset = true;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01Redux>().setResetPos(resetPos);
            Debug.Log("reset trigger");
        }
    }
}
