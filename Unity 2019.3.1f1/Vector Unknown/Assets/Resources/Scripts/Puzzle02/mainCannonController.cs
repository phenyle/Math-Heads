using UnityEngine;

public class mainCannonController : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;
    public GameObject text;
    public GameObject text2;

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
            text.GetComponent<TextMesh>().color = Color.yellow;
            text2.GetComponent<TextMesh>().color = Color.yellow;
        }
    }

    //used to trigger UI
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GCP02.isMainCannonTrigger = false;
            text.GetComponent<TextMesh>().color = Color.white;
            text2.GetComponent<TextMesh>().color = Color.white;
        }
    }
}
