using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonballController : MonoBehaviour
{

    private GameControllerPuzzle02 GCP02;
    private DatabasePuzzle02 DBP02;
    public GameObject text;
    public transformMatrix transform;
    public int index; 

    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);

        GCP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle02>();

        DBP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<DatabasePuzzle02>();
        DBP02.InitDatabase();

        transform.values = new int[] { 0, 0, 0, 0 };
        transform.values = DBP02.transforms[index].values;

        Debug.Log("Matrix" + index + ": " + transform.values[0] + ", " + transform.values[1]
                + ", " + transform.values[2] + ", " + transform.values[3]);

        text.GetComponent<TextMesh>().text = "" + transform.values[0] + ", " + transform.values[1] + "\n" 
            + transform.values[2] + ", " + transform.values[3];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.values = DBP02.transforms[index].values;
            Debug.Log("Matrix" + index + ": " + transform.values[0] + ", " + transform.values[1]
                + ", " + transform.values[2] + ", " + transform.values[3]);
            text.gameObject.SetActive(true);
            GCP02.currentTransformMatrix = transform.values;
            GCP02.isCannonballTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(false);
            GCP02.isCannonballTrigger = false;
        }
    }
}
