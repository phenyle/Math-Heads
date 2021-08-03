using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.Audio;
using UnityEngine;

public class GrappleCode : MonoBehaviour
{
    [Header("Rope Parts")]
    public GameObject player;
    public GameObject rope;
    public GameObject hook;
    public GameObject ropeStart;
    public GameObject ropeEnd;
    public GameObject goalPoint;
    public GameObject gunSmoke;
    private float ropeWidth;
    public float hookSpeed;
    private Vector3 hookDirection;
    private Vector3 startPos;


    private float speedIncri;

    [Header("Player Parts")]
    public GameObject charHand;
    public GameObject charShoulder;
    private float playerSpeed;
    private float distanceToGoal;


    private bool grappleAnimation;
    private bool[] stages = { true, false, false, false };
    private Puzzle04Controller PC04;
    private bool rightAnswer;


    public Transform prevShoulderRotation;


    // Start is called before the first frame update
    void Start()
    {
        rope.SetActive(false);
        ropeStart.transform.position = charHand.transform.position;
        ropeEnd.transform.position = charHand.transform.position;
        hook.SetActive(false);
        gunSmoke.SetActive(false);
        hook.transform.position = ropeEnd.transform.position;
        grappleAnimation = false;
        prevShoulderRotation = charShoulder.transform;
        prevShoulderRotation.rotation = charShoulder.transform.rotation;
        hookSpeed = 1.5f;
        playerSpeed = 1f;
        ropeWidth = 0.35f;
        this.enabled = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /**Theory of the Grapple Animation in steps
         * 1: player hits submit with the correct answer
         * 2: Set the pistol, rope, and hook active at hand (do later)
         * 3: Rotate shoulder towards the goal
         * 4: Move ropeEnd towards goal
         * 5: Update rope between hand and ropeEnd
         * 6: ropeEnd at the same position as goal
         * 7: Move player towards goal
         * 8: Update rope between goal and new player position
         * 9: When player is near goal, rotate player pos around goal
         * 10: rotate should be opposite of the terrain (raycasts?)
         * 11: when player is around and above goal disconnect rope
         * 12: continue moving player a set distance from goal
         */

        //player hits submit with the correct answer
        if (grappleAnimation)
        {
            //Player looks toward goal
            //rotateShoulder
            charShoulder.transform.LookAt(goalPoint.transform);
            ropeStart.transform.position = charHand.transform.position;
            if (rightAnswer) //Shoot hook to goal, move player over
            {
                ropeEnd.transform.LookAt(goalPoint.transform);
                ropeStart.transform.LookAt(goalPoint.transform);

                //Rope moving to goal stage
                if (stages[0])
                {
                    rope.SetActive(true);
                    hook.SetActive(true);
                    gunSmoke.SetActive(true);
                    player.transform.LookAt(new Vector3(goalPoint.transform.position.x, player.transform.position.y, goalPoint.transform.position.z));


                    //             hookSpeed = ropeEndMag / 100;

                    player.GetComponent<ThirdPersonUserControl>().enabled = false;

                    ropeEnd.transform.position = Vector3.MoveTowards(ropeEnd.transform.position, goalPoint.transform.position, hookSpeed);

                    VectorBetweenPoints(rope, ropeStart.transform.position, ropeEnd.transform.position, ropeWidth);

                    if ((ropeEnd.transform.position - goalPoint.transform.position).magnitude < 1)
                    {
                        PC04.getGameController().GetAudioService().PlayFXAudio("Puzzle04/grappleHit");
                        gunSmoke.SetActive(false);

                        player.GetComponent<Rigidbody>().useGravity = false;
                        Physics.gravity = Vector3.zero;

                        stages[0] = false;
                        stages[1] = true;
                    }

                }

                //Player moving to goal stage
                if (stages[1])
                {
                    ropeEnd.transform.position = goalPoint.transform.position;

                    VectorBetweenPoints(rope, ropeStart.transform.position, ropeEnd.transform.position, ropeWidth);

                    player.GetComponent<Animator>().Play("Airborne");                    

                    distanceToGoal = (player.transform.position - goalPoint.transform.position).magnitude;

                    if (distanceToGoal > 3)
                    {
                        //        player.GetComponent<Rigidbody>().AddRelativeForce(-Vector3.Normalize(playerTrajectory) * playerSpeed, ForceMode.VelocityChange);
                        player.transform.position = Vector3.MoveTowards(player.transform.position, goalPoint.transform.position + new Vector3(0, 2.5f, 0), playerSpeed);
                    }
                    else
                    {
                        //reset the player velocity if they had any
                        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        //this force pushes the player up and beind the goal anchor
                        player.GetComponent<Rigidbody>().AddForce(goalPoint.transform.TransformDirection(Vector3.right) * 25f + goalPoint.transform.TransformDirection(Vector3.up) * 10f, ForceMode.VelocityChange);
                        
                        player.GetComponent<Rigidbody>().useGravity = true;
                        Physics.gravity = new Vector3(0, -9.81f, 0);

                        stages[1] = false;
                        stages[2] = true;
                    }
                }

                //Player popping over Anchor
                if (stages[2])
                {
                    PC04.ToggleAllTriggers(true);
                    player.GetComponent<ThirdPersonUserControl>().enabled = true;

                    rope.SetActive(false);
                    hook.SetActive(false);
                    charShoulder.transform.rotation = prevShoulderRotation.rotation;

                    distanceToGoal = (player.transform.position - goalPoint.transform.position).magnitude;

                    ropeStart.transform.localPosition = Vector3.zero;
                    ropeEnd.transform.localPosition = Vector3.zero;
                    stages[2] = false;
                    grappleAnimation = false;
                    stages[0] = true;
                    this.enabled = false;

                }
            }
            else //Wrong answer, just shoot the hook at the final vector
            {
                //Rope moving to goal stage
                if (stages[0])
                {
                    ropeEnd.transform.LookAt(hookDirection);
                    ropeStart.transform.LookAt(hookDirection);

                    rope.SetActive(true);
                    hook.SetActive(true);
                    gunSmoke.SetActive(true);
                    player.transform.LookAt(new Vector3(hookDirection.x, player.transform.position.y, hookDirection.z));

                    ropeEnd.transform.position = Vector3.MoveTowards(ropeEnd.transform.position, hookDirection, hookSpeed);

                    VectorBetweenPoints(rope, ropeStart.transform.position, ropeEnd.transform.position, ropeWidth);

                    if ((ropeEnd.transform.position - hookDirection).magnitude < 1 || hook.GetComponent<HookTrigger>().isHitObject())
                    {
                        PC04.getGameController().GetAudioService().PlayFXAudio("Puzzle04/grappleMiss");

                        rope.SetActive(false);
                        hook.SetActive(false);
                        gunSmoke.SetActive(false);
                        ropeStart.transform.localPosition = Vector3.zero;
                        ropeEnd.transform.localPosition = Vector3.zero;
                        grappleAnimation = false;
                        hook.GetComponent<HookTrigger>().resetHitObject();
                        stages[0] = true;
                        this.enabled = false;
                    }

                }



            }

        }        
    }

    private void VectorBetweenPoints(GameObject visVector, Vector3 begin, Vector3 end, float width)
    {
        Vector3 offset = end - begin;
        Vector3 localScale = new Vector3(width, (offset.magnitude / 2.0f), width);
        Vector3 position = begin + (offset / 2.0f);

        visVector.transform.up = offset;
        visVector.transform.position = position;
        visVector.transform.localScale = localScale;
    }

    public void InitGrapple(bool correct, Vector3 direction, GameObject goal, Vector3 playerStartPos)
    {
        goalPoint = goal;
        rightAnswer = correct;
        hookDirection = direction;
        startPos = playerStartPos;
   //     ropeStart.transform.position = charHand.transform.localPosition;
    }


    public void grappleToGoal(Puzzle04Controller puzzleController)
    {
        PC04 = puzzleController;
        ropeStart.transform.position = charHand.transform.position;
        ropeEnd.transform.position = charHand.transform.position;

        PC04.getGameController().GetAudioService().PlayFXAudio("Puzzle04/grappleShot");

        grappleAnimation = true;
        this.enabled = true;
    }

    public void ReleaseGrapple()
    {
        PC04.ToggleAllTriggers(true);

        //reset the player velocity if they had any
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //this force pushes the player up and beind the goal anchor
        player.GetComponent<Rigidbody>().AddForce(goalPoint.transform.TransformDirection(Vector3.right) * 25f + goalPoint.transform.TransformDirection(Vector3.up) * 10f, ForceMode.VelocityChange);

        Physics.gravity = new Vector3(0, -9.81f, 0);
        player.GetComponent<Rigidbody>().useGravity = true;

        player.GetComponent<Collider>().enabled = true;
        player.GetComponent<ThirdPersonUserControl>().enabled = true;

        rope.SetActive(false);
        hook.SetActive(false);
        charShoulder.transform.rotation = prevShoulderRotation.rotation;

        distanceToGoal = (player.transform.position - goalPoint.transform.position).magnitude;

        ropeStart.transform.localPosition = Vector3.zero;
        ropeEnd.transform.localPosition = Vector3.zero;

        grappleAnimation = false;
        stages[2] = false;
        stages[1] = false;
        stages[0] = true;
        this.enabled = false;
    }

    public bool isGrappling()
    {
        return grappleAnimation;
    }
}
