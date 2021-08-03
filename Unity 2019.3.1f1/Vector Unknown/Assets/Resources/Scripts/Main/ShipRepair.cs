﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


public class ShipRepair : MonoBehaviour
{
    [Header("Hull Parts")]
    public GameObject hullBroken;
    public GameObject hullFixed;

    [Header("Mast/Sail Parts")]
    public GameObject mast1;
    public GameObject mast2;
    public GameObject mast3;
    public GameObject sail1;
    public GameObject sail2;
    public GameObject sail3;
    public GameObject rigging;
    public GameObject sailMisc1;
    public GameObject sailMisc2;

    [Header("Cannon/CannonBall Parts")]
    public GameObject portCannons;
    public GameObject stbdCannons;
    public GameObject portCannonBalls;
    public GameObject stbdCannonBalls;

    [Header("Treasure Parts")]
    public GameObject treasureForecastle;
    public GameObject treasureStern;

    [Header("Camera Positions")]
    public Transform fromPuzzle1;
    public Transform fromPuzzle2;
    public Transform fromPuzzle3;
    public Transform fromPuzzle4;

    [Header("FX Settings")]
    public GameObject pulseFX;
    public GameObject explosionFX;
    [Range(1,1000)]
    public int fxFixedInterval = 25;
    [Range(1,1000)]
    public int fxMaxTime = 100;
    [Range(0.0f, 1.0f)]
    [Tooltip("Increasing this Can HUGELY effect performance if there a lot of objects\nSet to 0 to turn off all random effects.")]
    public float fxRand = 0.2f;
    private int fxIter = 0;
    private int waitIter = 0;
    private int waitTime = 200;

    [Header("Sounds")]
    public AudioClip fixedPulseSound;
    public AudioClip[] randomPulseSounds;
    public AudioClip fixedExploSound;
    public AudioClip[] randomExploSounds;



    private GameObject player;
    private GameObject mainCamera;
    private GameObject shipCamera;
    private Vector3 prevCameraPos;
    private Vector3 cameraPos;
    private Vector3 targetPos;



    public GameControllerMain GCM;
    private int animStage = 0;

    private bool animate = false;
    private bool fxComplete = false;

    // Start is called before the first frame update
    void Start()
    {
  //      GCM = GameObject.Find("GameController").GetComponent<GameControllerMain>();

        animStage = 0;
        fxIter = 0;
        waitIter = 0;

        shipCamera = GameObject.FindGameObjectWithTag("PuzzleCamera");

        pulseFX.SetActive(false);
        explosionFX.SetActive(false);

        //ShipUpdate();
    }


    public void LateUpdate()
    {
        if(animate)
        {
            switch(animStage)
            {
                case 0: //moving the camera to ship stage
                    if (GCM.GetComponent<ObjectGlide>().isAtDestination(cameraPos))
                        animStage = 1;
                    break;

                case 1: //doing sparkly FX stage
                    FXSelect(GameRoot.instance.prevStage, GameRoot.instance.prevLevel, fxIter);
                    fxIter++;

                    if (fxIter > fxMaxTime)
                    {
                        GameRoot.instance.puzzlesDone[GameRoot.instance.prevStage][GameRoot.instance.prevLevel] = true;
                        ShipUpdate();

                        animStage = 2;
                    }
                    break;

                case 2: //showing the new part for a few seconds
                    waitIter++;
                    
                    if(waitIter > waitTime)
                    {
                        waitIter = 0;
                        animStage = 3;
                        GameRoot.camEvents.AddListener(camAtPlayer);
                        GCM.GetComponent<ObjectGlide>().GlideToMovingPosition(shipCamera, mainCamera, player, prevCameraPos);
                    }
                    break;

                case 3: //moving the camera back to the player
                    if(GCM.GetComponent<ObjectGlide>().isAtDestination(mainCamera.transform.position))
                    {
                        player.GetComponentInChildren<ThirdPersonCharacter>().enabled = true;
                        mainCamera.GetComponent<CameraController>().enabled = true;

                        shipCamera.GetComponent<Camera>().enabled = false;
                        mainCamera.GetComponent<Camera>().enabled = true;

                        fxIter = 0;
                        animStage = 0;
                        animate = false;
                    }
                    break;
            }
        }

    }

    /// <summary>
    /// This function begins the Animated ship repair steps
    /// It first temporarily turns off player control and camera control
    /// Then, based on the previous stage/level completed from GameRoot it
    /// determines where to move the camera to and what it should look at
    /// When the parameters are set it tells the camera to glide over and enables
    /// the animate tag for the update to take over.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="camera"></param>
    public void AnimatePartUnlock(GameObject player, GameObject camera)
    {
        //since this only runs when a new part is unlocked
        //we must turn that part OFF first before running ShipUpdate()
        //then we'll turn that puzzleDones[][] back on after the sparkles and fanfair
        GameRoot.instance.puzzlesDone[GameRoot.instance.prevStage][GameRoot.instance.prevLevel] = false;
        ShipUpdate();
        this.player = player;
        this.mainCamera = camera;
        prevCameraPos = mainCamera.transform.position;

        this.player.GetComponentInChildren<ThirdPersonCharacter>().enabled = false;
        this.player.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        mainCamera.GetComponent<Camera>().enabled = false;
        shipCamera.transform.position = mainCamera.transform.position;
        shipCamera.transform.rotation = mainCamera.transform.rotation;
        shipCamera.GetComponent<Camera>().enabled = true;


        switch (GameRoot.instance.prevStage)
        {
            case 1:
                cameraPos = fromPuzzle1.position;
                targetPos = this.gameObject.transform.position;
                break;

            case 2:
                cameraPos = fromPuzzle2.position;
                switch(GameRoot.instance.prevLevel)
                {
                    case 1:
                        targetPos = stbdCannons.transform.position;
                        break;
                    case 2:
                        targetPos = portCannons.transform.position;
                        break;
                }    

                break;

            case 3:
                cameraPos = fromPuzzle3.position;
                switch(GameRoot.instance.prevLevel)
                {
                    case 1:
                        targetPos = treasureForecastle.transform.position;
                        break;
                }
                break;

            case 4:
                cameraPos = fromPuzzle4.position;
                switch(GameRoot.instance.prevLevel)
                {
                    case 1:
                        targetPos = sail1.transform.position;
                        break;
                    case 2:
                        targetPos = sail3.transform.position;
                        break;
                    case 3:
                        targetPos = sail2.transform.position;
                        break;
                }

                break;
        }


        GameRoot.camEvents.AddListener(camAtShip);
        GCM.GetComponent<ObjectGlide>().SetObjectMoveSpeed(((this.player.transform.position-cameraPos).magnitude / 1400.0f) * 6.0f);
        GCM.GetComponent<ObjectGlide>().GlideToPosition(shipCamera, cameraPos, targetPos, prevCameraPos);
        animate = true;
    }

    public void camAtShip()
    {
        GameRoot.camEvents.RemoveListener(camAtShip);
    }

    public void camAtPlayer()
    {
        player.GetComponentInChildren<ThirdPersonCharacter>().enabled = true;
        mainCamera.GetComponent<CameraController>().enabled = true;

        shipCamera.GetComponent<Camera>().enabled = false;
        mainCamera.GetComponent<Camera>().enabled = true;

        fxIter = 0;
        animStage = 0;
        animate = false;
        GameRoot.camEvents.RemoveListener(camAtPlayer);
    }

    /// <summary>
    /// This Method determines what parts of the ship to play effects at depending
    /// on which level was competeled
    /// </summary>
    /// <param name="stage">The stage that was just completed</param>
    /// <param name="level">The level at which the stage was completed</param>
    /// <param name="iter">the duration thusfar for the effects</param>
    public void FXSelect(int stage, int level, int iter)
    {
        switch(stage)
        {
            case 1:
                switch(level)
                {
                    case 1:
                        FXGlowRings(hullFixed.GetComponent<Renderer>(), iter, true);

                        //special case turn on/off for stage1
                        //stage1 has to turn an object on AND turn an object off since it's the main hull
                        //every other stage only has to turn an object on
                        if(iter == fxMaxTime)
                        {
                            hullFixed.SetActive(true);
                            hullBroken.SetActive(false);
                        }
                        //hullBroken.GetComponent<Renderer>().material.color = new Color(brokenMat.r, brokenMat.g, brokenMat.b, (fxSpeed - iter) / fxSpeed);
                        //hullFixed.GetComponent<Renderer>().material.color = new Color(fixedMat.r, fixedMat.g, fixedMat.b,  iter / fxSpeed);

                        break;                        
                }

                break;

            case 2:
                switch (level)
                {
                    case 1:
                        foreach (Renderer cannonRends in portCannons.GetComponentsInChildren<Renderer>())
                            FXGlowRings(cannonRends, iter, true);

                        foreach (Renderer cannonBallRends in portCannonBalls.GetComponentsInChildren<Renderer>())
                            FXGlowRings(cannonBallRends, iter, false);
                        break;

                    case 2:
                        foreach (Renderer cannonRends in stbdCannons.GetComponentsInChildren<Renderer>())
                            FXGlowRings(cannonRends, iter, true);

                        foreach (Renderer cannonBallRends in stbdCannonBalls.GetComponentsInChildren<Renderer>())
                            FXGlowRings(cannonBallRends, iter, false);

                        break;
                }

                break;

            case 3:
                switch (level)
                {
                    case 1:
                        //Might be too many objects here to really work
                        //might just make a custom fx method for this one

                        foreach (Renderer treasurRend in treasureForecastle.GetComponentsInChildren<Renderer>())
                            FXGlowRings(treasurRend, iter, false);

                        foreach (Renderer treasurRend in treasureStern.GetComponentsInChildren<Renderer>())
                            FXGlowRings(treasurRend, iter, false);

                        break;
                }

                break;

            case 4:
                switch (level)
                {
                    case 1:
                        FXGlowRings(mast1.GetComponent<Renderer>(), iter, true);
                        break;

                    case 2:
                        FXGlowRings(mast3.GetComponent<Renderer>(), iter, true);
                        break;

                    case 3:
                        FXGlowRings(mast2.GetComponent<Renderer>(), iter, true);
                        break;
                }

                break;
        }

        fxComplete = true;
    }

    private void FXGlowRings(Renderer objUnlocked, int iter, bool randomEffects)
    {
        GameObject explosions = GameObject.Instantiate(explosionFX);
        GameObject pulses = GameObject.Instantiate(pulseFX);

        pulses.transform.position = objUnlocked.bounds.center;
        pulses.transform.localScale = objUnlocked.bounds.extents;
        //fixed interval overall FX scaled to object size
        if (iter % fxFixedInterval == 0)
        {
            pulses.SetActive(true);
            pulses.GetComponent<ParticleSystem>().Play(true);
            GameRoot.instance.audioService.PlayFXAudio("ShipRepair/" + fixedPulseSound.name.ToString());            
        }

        if (randomEffects)
        {
            float rand = Random.Range(0.01f, 1.0f);
            if (rand < fxRand)
            {
                GameObject randPulse = GameObject.Instantiate(pulseFX);
                Vector3 randPos = new Vector3(Random.Range(-0.5f * objUnlocked.bounds.extents.x, 0.5f * objUnlocked.bounds.extents.x), Random.Range(-0.5f * objUnlocked.bounds.extents.y, 0.5f * objUnlocked.bounds.extents.y), Random.Range(-0.5f * objUnlocked.bounds.extents.z, 0.5f * objUnlocked.bounds.extents.z)) + objUnlocked.bounds.center;
                randPulse.transform.position = randPos;
                float randScal = Random.Range(0.05f, 0.5f);
                randPulse.transform.localScale = randScal * objUnlocked.bounds.extents;
                randPulse.SetActive(true);
                randPulse.GetComponent<ParticleSystem>().Play(true);
                int soundRand = Random.Range(0, randomPulseSounds.Length - 1);
                GameRoot.instance.audioService.PlayFXAudio("ShipRepair/" + randomPulseSounds[soundRand].name.ToString());
            }
        }
    }


    public void ShipUpdate()
    {
        //Stage1 Completed
        hullBroken.SetActive(!GameRoot.instance.puzzlesDone[1][1]);
        hullFixed.SetActive(GameRoot.instance.puzzlesDone[1][1]);


        //Stage2 Completed
        portCannons.SetActive(GameRoot.instance.puzzlesDone[2][1]);
        portCannonBalls.SetActive(GameRoot.instance.puzzlesDone[2][1]);

        stbdCannons.SetActive(GameRoot.instance.puzzlesDone[2][2]);
        stbdCannonBalls.SetActive(GameRoot.instance.puzzlesDone[2][2]);
        //this can be later broken up with more levels into something like:
        //level1 done: both port/stbd cannonballs
        //level2 done: portside Cannons
        //level3 done: stbd Cannons
        //all done: hull cannons (note: need to break hull model to rip hull cannons out)


        //Stage3 Completed
        treasureForecastle.SetActive(GameRoot.instance.puzzlesDone[3][1]);
        treasureStern.SetActive(GameRoot.instance.puzzlesDone[3][1]);

        //Stage4 Completed
        mast1.SetActive(GameRoot.instance.puzzlesDone[4][1]);
        mast2.SetActive(GameRoot.instance.puzzlesDone[4][3]); //mast2 is the main, so making it for level 3
        mast3.SetActive(GameRoot.instance.puzzlesDone[4][2]);

        sail1.SetActive(GameRoot.instance.puzzlesDone[4][1]);
        sail2.SetActive(GameRoot.instance.puzzlesDone[4][3]); //sail2 is the main, so making it for level 3
        sail3.SetActive(GameRoot.instance.puzzlesDone[4][2]);

        if(GameRoot.instance.puzzlesDone[4][1] &&
            GameRoot.instance.puzzlesDone[4][2] &&
            GameRoot.instance.puzzlesDone[4][3])
        {
            rigging.SetActive(true);
            sailMisc1.SetActive(true);
            sailMisc2.SetActive(true);
        }
        else
        {
            rigging.SetActive(false);
            sailMisc1.SetActive(false);
            sailMisc2.SetActive(false);

        }

    }


}
