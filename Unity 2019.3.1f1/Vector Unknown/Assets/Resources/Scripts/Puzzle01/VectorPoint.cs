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
                GCP01.isInQues = true;
                GCP01.SendConstrains(defaultScalar, defaultX, defaultY, defaultZ, questionNum);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GCP01.isInQues = false;
            GCP01.questionNum = 0;
        }
    }
}
