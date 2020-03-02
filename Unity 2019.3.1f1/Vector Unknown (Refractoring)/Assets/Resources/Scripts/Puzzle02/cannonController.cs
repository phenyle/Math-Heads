using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonController : MonoBehaviour
{

    private GameControllerPuzzle02 GCP02;
    public int[] vector;
    public GameObject vectorText;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        vectorText.gameObject.SetActive(false);
        GCP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle02>();
    }

    //used to trigger UI
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("vector" + vector[0] + ", " + vector[1]);
            vectorText.gameObject.SetActive(true);
            GCP02.currentVector = vector;
            GCP02.isCannonTrigger = true;
        }
    }

    //used to trigger UI
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            vectorText.gameObject.SetActive(false);
            //GCP02.currentVector = null;
            GCP02.isCannonTrigger = false;
        }
    }
}
