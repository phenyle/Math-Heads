using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipContoller : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;
    public bool isHit = false;

    void Start()
    {
        GCP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle02>();
         bool isHit = false;
    }
    
    void Update()
    {
        if (isHit)
        {
            transform.position += Vector3.down * 1.0f * Time.deltaTime;

            if (transform.position.y < -30)
            {
                Destroy(gameObject);
            }
        }
        
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision tiggered");
       
        isHit = true;
        Destroy(other.gameObject);
        Debug.Log("Boat was hit");
        GCP02.ballIsFlying = false;
    }
}

   




