﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.ThirdPerson;

public class ObjectGlide : MonoBehaviour
{
    [Header("Glide Object")]
    public GameObject glideObject;

    private GameObject targetPos;
    private GameObject destinationPos;
    private Vector3 startPos;
    private Vector3 halfwayPoint;
    private bool reachedDestination = false;

    [Header("Glide Controls")]
    [Range(0.001f, 1.0f)]
    public float objectMoveSpeed = 0.1f;
    [Range(0.001f, 0.5f)]
    public float objectRotateSpeed = 0.07f;
    [Range(0.0f, 50.0f)]
    public float arcHeight = 0.0f;



    void Awake()
    {
        this.enabled = false;
        targetPos = new GameObject();
        destinationPos = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        glideObject.transform.position = Vector3.MoveTowards(glideObject.transform.position, destinationPos.transform.position, objectMoveSpeed);
  //      glideObject.transform.position += new Vector3(0, CalcArcHeight(), 0);
        glideObject.transform.rotation = Quaternion.Slerp(glideObject.transform.rotation, Quaternion.LookRotation((targetPos.transform.position - glideObject.transform.position).normalized), objectRotateSpeed);

        if ((glideObject.transform.position - destinationPos.transform.position).magnitude < 0.2f)
        {
            reachedDestination = true;
            GameRoot.camEvents.Invoke();
            this.enabled = false;
        }      
    }


    public void GlideToPosition(Vector3 newDestination, Vector3 target, Vector3 newStartPos)
    {
        SetDestinationPos(newDestination);
        SetTargetPos(target);
        SetStartPos(newStartPos);
        halfwayPoint = (new Vector3(destinationPos.transform.position.x, 0, destinationPos.transform.position.z) - new Vector3(startPos.x, 0, startPos.z)) / 2;

        reachedDestination = false;
        this.enabled = true;
    }

    public void GlideToPosition(Vector3 newDestionation, Vector3 target, Vector3 newStartPos, float arc)
    {
        SetDestinationPos(newDestionation);
        SetTargetPos(target);
        SetStartPos(newStartPos);
        arcHeight = arc;
        halfwayPoint = (new Vector3(destinationPos.transform.position.x, 0, destinationPos.transform.position.z) - new Vector3(startPos.x, 0, startPos.z)) / 2;

        reachedDestination = false;
        this.enabled = true;
    }

    public void GlideToPosition(GameObject gobject, Vector3 newDestination, Vector3 target, Vector3 newStartPos)
    {

        glideObject = gobject;
        SetDestinationPos(newDestination);
        SetTargetPos(target);
        SetStartPos(newStartPos);
        
        halfwayPoint = (new Vector3(destinationPos.transform.position.x, 0, destinationPos.transform.position.z) - new Vector3(startPos.x, 0, startPos.z)) / 2;

        reachedDestination = false;
        this.enabled = true;
        
    }

    public void GlideToPosition(GameObject gobject, Vector3 newDestionation, Vector3 target, Vector3 newStartPos, float arc)
    {
        glideObject = gobject;
        SetDestinationPos(newDestionation);
        SetTargetPos(target);
        SetStartPos(newStartPos);
        arcHeight = arc;
        halfwayPoint = (new Vector3(destinationPos.transform.position.x, 0, destinationPos.transform.position.z) - new Vector3(startPos.x, 0, startPos.z)) / 2;

        reachedDestination = false;
        this.enabled = true;
    }

    public void GlideToMovingPosition(GameObject gobject, GameObject newDestination, GameObject target, Vector3 newStartPos)
    {
        glideObject = gobject;
        SetDestinationPos(newDestination);
        SetTargetPos(target);
        SetStartPos(newStartPos);
        halfwayPoint = (new Vector3(destinationPos.transform.position.x, 0, destinationPos.transform.position.z) - new Vector3(startPos.x, 0, startPos.z)) / 2;

        reachedDestination = false;
        this.enabled = true;
    }

    public void GlideToMovingPosition(GameObject gobject, GameObject newDestionation, GameObject target, Vector3 newStartPos, float arc)
    {
        glideObject = gobject;
        SetDestinationPos(newDestionation);
        SetTargetPos(target);
        SetStartPos(newStartPos);
        arcHeight = arc;
        halfwayPoint = (new Vector3(destinationPos.transform.position.x, 0, destinationPos.transform.position.z) - new Vector3(startPos.x, 0, startPos.z)) / 2;

        reachedDestination = false;
        this.enabled = true;
    }


    public void SetStartPos(Vector3 newStartPos)
    {
        startPos = newStartPos;
    }

    public Vector3 GetStartPos()
    {
        return startPos;
    }

    public void SetDestinationPos(Vector3 newPos)
    {
        destinationPos.transform.position = newPos;
    }

    public void SetTargetPos(Vector3 newPos)
    {
        targetPos.transform.position = newPos;
    }

    public void SetDestinationPos(GameObject newPos)
    {
        destinationPos = newPos;
    }

    public void SetTargetPos(GameObject newPos)
    {
        targetPos = newPos;
    }

    public void SetObjectMoveSpeed(float val)
    {
        objectMoveSpeed = val;
    }

    public void SetObjectRotateSpeed(float val)
    {
        objectRotateSpeed = val;
    }

    public bool isAtDestination(Vector3 destination)
    {
        if ((glideObject.transform.position - destination).magnitude < 1)
        {
            reachedDestination = true;
            this.enabled = false;
            return reachedDestination;
        }
        else
        {
            reachedDestination = false;
            return reachedDestination;
        }
    }

    public bool isAtDestination()
    {       
        return reachedDestination;
    }

    private float CalcArcHeight()
    {
        float prctHeight;
        float disTravelMag = (glideObject.transform.position - startPos).magnitude;
        float halfwayMag = halfwayPoint.magnitude;
         
        if(disTravelMag < halfwayMag)
        {
            prctHeight = disTravelMag / halfwayMag;
        }
        else
        {
            prctHeight = 1 - ((disTravelMag - halfwayMag) / halfwayMag);
        }

        if (prctHeight > 0.001)
            return prctHeight * arcHeight;
        else
            return 0.0f;
    }

}
