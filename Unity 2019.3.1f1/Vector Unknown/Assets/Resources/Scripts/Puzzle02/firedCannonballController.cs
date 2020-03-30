using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firedCannonballController : MonoBehaviour
{
    public GameObject waterEffect;
    private GameObject tempEffect;
    private float timer = 0;
    private bool isEffectPlayed = false;

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.y < -7.0f)
        {
            if (isEffectPlayed == false)
            {
                tempEffect = Instantiate(waterEffect, this.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
                tempEffect.GetComponent<ParticleSystem>().Play(true);
                isEffectPlayed = true;
            }

            timer = timer + 1;
            Debug.Log(timer);

            if (timer > 450)
            {
                Destroy(tempEffect);
                Destroy(this.gameObject);
            }

        }
    }
}
