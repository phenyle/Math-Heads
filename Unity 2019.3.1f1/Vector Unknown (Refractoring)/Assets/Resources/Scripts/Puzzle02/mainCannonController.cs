using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCannonController : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;

    // Start is called before the first frame update
    void Start()
    {
        GCP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle02>();
    }

    //used to trigger UI
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GCP02.isMainCannonTrigger = true;
        }
    }

    //used to trigger UI
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GCP02.isMainCannonTrigger = false;
        }
    }
}
