using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTutorial : MonoBehaviour
{
    public int value;
    public GameObject Text;
    public GameObject pressedE;
    public GameObject gameContoller;
    private TutorialContorller tutorialContorller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pressedE.SetActive(true);
            Text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pressedE.SetActive(false);
            Text.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (value == 3)
            {
                tutorialContorller.selectionComplete = true;
            }
        }
    }

    void Start()
    {
        pressedE.SetActive(false);
        Text.SetActive(false);
        tutorialContorller = gameContoller.GetComponent<TutorialContorller>();
    }
    
    //void Update()
    //{

    //}
}
