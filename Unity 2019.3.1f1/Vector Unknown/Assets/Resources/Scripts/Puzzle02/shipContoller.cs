using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipContoller : MonoBehaviour
{
    private GameControllerPuzzle02 GCP02;
    public bool isHit = false;
    public bool isplayed = false;
    public GameObject text1;
    public GameObject text2;
    public GameObject effect;
    private GameObject tempEffect;

    void Start()
    {
        GCP02 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerPuzzle02>();
         bool isHit = false;
    }
    
    void Update()
    {
        if (isHit)
        {
            if (isplayed == false) {
                tempEffect = Instantiate(effect, this.gameObject.transform.position, this.gameObject.transform.rotation);
                tempEffect.GetComponent<ParticleSystem>().Play(true);
                isplayed = !isplayed;
                GCP02.UpdateUI();
                GCP02.ActiveBoat += 1;
                print("Active boat: " + GCP02.ActiveBoat); 
            }
            transform.position += Vector3.down * 1.0f * Time.deltaTime;

            if (transform.position.y < -30)
            {
                Destroy(gameObject);
            }

            if (GCP02.topViewOn)
            {
                GCP02.TopViewText[GCP02.ActiveBoat].gameObject.SetActive(true);
                GCP02.normalText[GCP02.ActiveBoat].gameObject.SetActive(false);
            }
            else if (GCP02.ActiveBoat < 6)
            {
                GCP02.TopViewText[GCP02.ActiveBoat].gameObject.SetActive(false);
                GCP02.normalText[GCP02.ActiveBoat].gameObject.SetActive(true);
            }
        }
        
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision tiggered");
       
        isHit = true;
        Destroy(other.gameObject);
        GCP02.ballIsFlying = false;
        Debug.Log("Boat was hit");
        GCP02.playhit();
        GCP02.ballIsFlying = false;
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
    }
}

   




