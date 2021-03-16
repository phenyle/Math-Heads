using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleCode : MonoBehaviour
{
    [Header("Rope Parts")]
    public GameObject rope;
    public GameObject hook;
    public GameObject ropeEnd;
    public GameObject goalPoint;

    [Header("Player Parts")]
    public GameObject charHand;
    public GameObject charShoulder;

    private bool grappleAnimation;
    private bool[] stages = { false, false, false, false, false, false };

    public Transform prevShoulderRotation;


    // Start is called before the first frame update
    void Start()
    {
        // rope.SetActive(false);
        // ropeEnd.transform.position = charHand.transform.position;
        // hook.SetActive(false);
        // hook.transform.position = ropeEnd.transform.position;
        // grappleAnimation = false;
        // prevShoulderRotation.rotation = charShoulder.transform.rotation;
        
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
            //rotateShoulder
       //     charShoulder.transform.LookAt(goalPoint.transform);

      //      rope.SetActive(true);
      //      hook.SetActive(true);

      //      VectorBetweenPoints(rope, charHand.transform.position, goalPoint.transform.position, 0.2f);











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

    public void setGoalPoint(GameObject goal)
    {
        goalPoint = goal;
    }

    public void grappleToGoal()
    {
        grappleAnimation = true;
    }
}
