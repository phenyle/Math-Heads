using UnityEngine;
using UnityEngine.UI;

public class Puzzle01Complete : MonoBehaviour
{
    public ParticleSystem congrats;
    public Transform endportal;
    public bool isPlayer = false;

    private void Start()
    {
        endportal.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(isPlayer && Input.GetKeyDown(KeyCode.E))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>().SetText("You found the mast!");
            congrats.Play();
            endportal.gameObject.SetActive(true);
            isPlayer = false;

            Destroy(this.gameObject, 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayer = true;
        }
    }
}