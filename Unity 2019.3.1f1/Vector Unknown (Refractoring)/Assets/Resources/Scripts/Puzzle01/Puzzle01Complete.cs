using UnityEngine;
using UnityEngine.UI;

public class Puzzle01Complete : MonoBehaviour
{
    public ParticleSystem congrats;
    public Transform endportal;

    private void Start()
    {
        endportal.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>().SetText("You found the mast!");
            congrats.Play();
            endportal.gameObject.SetActive(true);
        }
    }
}