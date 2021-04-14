using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.ThirdPerson;
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
    private float ropeWidth;

    private float playerSpeed;
    private float speedIncri;

    [Header("Player Parts")]
    public GameObject charHand;
    public GameObject charShoulder;
    private Vector3 playerBearing; //from player pos to goal point

    private float distanceToGoal;
    public float hookSpeed;

    private bool grappleAnimation;
    private bool[] stages = { true, false, false, false };
    private Puzzle04Controller PC04;

    public Transform prevShoulderRotation;


    // Start is called before the first frame update
    void Start()
    {
        rope.SetActive(false);
//        ropeStart.transform.position = charHand.transform.localPosition;
//        ropeEnd.transform.position = charHand.transform.localPosition;
        hook.SetActive(false);
//        hook.transform.position = ropeEnd.transform.position;
        grappleAnimation = false;
        prevShoulderRotation = charShoulder.transform;
        prevShoulderRotation.rotation = charShoulder.transform.rotation;
        hookSpeed = 0.75f;
        playerSpeed = 0.5f;
        ropeWidth = 0.35f;
    }

    // Update is called once per frame
    void Update()
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

            ropeEnd.transform.LookAt(goalPoint.transform);

            //Rope moving to goal stage
            if (stages[0])
            {
                rope.SetActive(true);
                hook.SetActive(true);
                player.transform.LookAt(new Vector3(goalPoint.transform.position.x, player.transform.position.y, goalPoint.transform.position.z));


                //             hookSpeed = ropeEndMag / 100;

                player.GetComponent<ThirdPersonUserControl>().enabled = false;

                ropeEnd.transform.position = Vector3.MoveTowards(ropeEnd.transform.position, goalPoint.transform.position, hookSpeed);

                VectorBetweenPoints(rope, ropeStart.transform.position, ropeEnd.transform.position, ropeWidth);

                /**
                if (hookSpeed > 1)
                    ropeEndMag++;
                else
                {
                    playerTrajectory = player.transform.position - goalPoint.transform.position;

                    stages[0] = false;
                    stages[1] = true;
                }
                **/

                if ((ropeEnd.transform.position - goalPoint.transform.position).magnitude < 1)
                {
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
                player.GetComponent<Rigidbody>().useGravity = false;

                distanceToGoal = (player.transform.position - goalPoint.transform.position).magnitude;

                if(!PC04.portal.GetComponent<PortalTrigger>().inPortal)
                    player.GetComponent<Collider>().enabled = false;

                if (distanceToGoal > 3)
                {
            //        player.GetComponent<Rigidbody>().AddRelativeForce(-Vector3.Normalize(playerTrajectory) * playerSpeed, ForceMode.VelocityChange);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, goalPoint.transform.position + new Vector3(0,2.5f,0), playerSpeed);
                }
                else
                {
                    player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    player.GetComponent<Rigidbody>().AddForce(goalPoint.transform.TransformDirection(Vector3.right) * 20f + goalPoint.transform.TransformDirection(Vector3.up) * 4f, ForceMode.VelocityChange);

                    stages[1] = false;
                    stages[2] = true;
                }
            }

            //Player popping over Anchor
            if(stages[2])
            {
                player.GetComponent<Collider>().enabled = true;
                player.GetComponent<ThirdPersonUserControl>().enabled = true;

                player.GetComponent<Rigidbody>().useGravity = true;

                rope.SetActive(false);
                hook.SetActive(false);
                charShoulder.transform.rotation = prevShoulderRotation.rotation;

                distanceToGoal = (player.transform.position - goalPoint.transform.position).magnitude;


                ropeStart.transform.localPosition = Vector3.zero;
                ropeEnd.transform.localPosition = Vector3.zero;
                stages[2] = false;
                grappleAnimation = false;
                stages[0] = true;

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

    public void InitGrapple(Vector3 start, GameObject goal)
    {
        goalPoint = goal;
   //     ropeStart.transform.position = charHand.transform.localPosition;
    }


    public void grappleToGoal(Puzzle04Controller puzzleController)
    {
        PC04 = puzzleController;
        ropeStart.transform.position = charHand.transform.position;
        ropeEnd.transform.position = charHand.transform.position;

        grappleAnimation = true;
    }
}
