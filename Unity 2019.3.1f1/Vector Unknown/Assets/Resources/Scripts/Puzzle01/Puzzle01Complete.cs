using UnityEngine;
using UnityEngine.UI;

public class Puzzle01Complete : MonoBehaviour
{
    private GameControllerPuzzle01 GCP01;

    private void Start()
    {
        GCP01 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GCP01.isInMast = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GCP01.isInMast = false;
        }
    }
}