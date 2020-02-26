using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class VectorPoint : MonoBehaviour
{
    public int questionNum;
    public float defaultScalar = 0;
    public float defaultX = 0, defaultY = 0, defaultZ = 0;
    public Text info;
    public Text infoTop;

    private GameControllerPuzzle01 GCP01;

    private void Start()
    {
        GCP01 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle01>();
    }

    private void LateUpdate()
    {
        info.text = "(" + this.transform.position.x + "," + this.transform.position.z + "," + this.transform.position.y + ")";
        infoTop.text = info.text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(questionNum != 0)
            {
                GCP01.SetText("- Press 'E' to input your answer.\n" +
                                         "- Press 'Z' to switch into the top-down camera.\n");
                GCP01.isTriggerQuestion = true;
                GCP01.questionNum = questionNum;
                GCP01.SendConstrains(defaultScalar, defaultX, defaultY, defaultZ);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GCP01.SetText("- Press Z for top view.\n" +
                                     "- Round to whole numbers" +
                                     "- Stand on a platform to enter values.");
            GCP01.isTriggerQuestion = false;
            GCP01.questionNum = 0;
        }
    }
}
