using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{

    private Vector3 resetPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<GapTriggersController>().finish = true;
            resetPos = GetComponentInParent<GapTriggersController>().GetComponentInParent<Puzzle01Controller>().finish.transform.position;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01Redux>().setResetPos(resetPos);

            Debug.Log("finish trigger");
        }
    }
}
