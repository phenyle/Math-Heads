﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardVectors : EventTrigger
{
    private Vector3 cardValues;
    private Vector3 startPos;
    private bool dragging = false;

    private Puzzle01Window P01W;

    public void Start()
    {
        P01W = GameObject.Find("Puzzle01Window").GetComponent<Puzzle01Window>(); ;
        startPos = this.transform.position;
        dragging = false;
    }


    public void Update()
    {
        if (dragging)
        {
            this.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Dragging this: " + this.gameObject.name);
        dragging = true;
        this.transform.SetAsLastSibling();

        if (P01W.getAnswer1() != null && this.cardValues == P01W.getAnswer1().GetComponent<CardVectors>().cardValues)
            P01W.setAnswer1(null);

        if (P01W.getAnswer2() != null && this.cardValues == P01W.getAnswer2().GetComponent<CardVectors>().cardValues)
            P01W.setAnswer2(null);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Dropped this: " + this.gameObject.name);
        dragging = false;


        if (inAnsSlot1(this.gameObject))
        {
            this.transform.position = P01W.getAnsField1trans().position;

            //Look to see if another card is already there and returning the old card
            for (int i = 0; i < P01W.getCards().Count; i++)
            {
                //if this card is not the card we just dragged
                if (P01W.getCards()[i].gameObject.name.CompareTo(this.gameObject.name) != 0)
                {
                    //if this other card is already in AnsSlot1 return it to its start position
                    if (inAnsSlot1(P01W.getCards()[i]))
                    {
                        GameObject.Find(P01W.getCards()[i].gameObject.name).transform.position = GameObject.Find(P01W.getCards()[i].gameObject.name).GetComponent<CardVectors>().startPos;
                    }
                }

            }
            P01W.setAnswer1(this.gameObject);
        }
        else if (inAnsSlot2(this.gameObject))
        {
            this.transform.position = P01W.getAnsField2trans().position;

            //Look to see if another card is already there and returning the old card
            for (int i = 0; i < P01W.getCards().Count; i++)
            {
                //if this card is not the card we just dragged
                if (P01W.getCards()[i].gameObject.name.CompareTo(this.gameObject.name) != 0)
                {
                    //if this other card is already in AnsSlot2 return it to its start position
                    if (inAnsSlot2(P01W.getCards()[i]))
                    {
                        GameObject.Find(P01W.getCards()[i].gameObject.name).transform.position = GameObject.Find(P01W.getCards()[i].gameObject.name).GetComponent<CardVectors>().startPos;
                    }
                }

            }
            P01W.setAnswer2(this.gameObject);
        }
        else
            this.transform.position = startPos;
    }

    public Vector3 getCardVector()
    {
        return cardValues;
    }

    public void setCardVector(Vector3 val)
    {
        cardValues = val;
    }

    /// <summary>
    /// Check if this card has been dragged into AnswerSlot1 on the UI
    /// </summary>
    /// <returns> false if not in AnswerSlot1 boundaries
    /// otherwise returns true</returns>
    private bool inAnsSlot1(GameObject card)
    {
        Debug.Log("Dragged Card Pos: " + this.transform.position);

        Debug.Log("Ans Card1 Pos: " + P01W.getAnsField1trans().position);

        //Check if outside left boundary
        if (card.transform.position.x < P01W.getAnsField1trans().position.x - 40)
            return false;       
        //Check if outside right boundary
        else if (card.transform.position.x > P01W.getAnsField1trans().position.x + 40)
            return false;        
       //Check if outside bottom boundary
       else if (card.transform.position.y < P01W.getAnsField1trans().position.y - 60)
           return false;
       //Check if outside top boundary
       else if (card.transform.position.y > P01W.getAnsField1trans().position.y + 60)
           return false;

        else
            return true;
    }

    /// <summary>
    /// Check if this card has been dragged into AnswerSlot2 on the UI
    /// </summary>
    /// <returns> false if not in AnswerSlot2 boundaries
    /// otherwise returns true</returns>
    private bool inAnsSlot2(GameObject card)
    {
        Debug.Log("Dragged Card Pos: " + this.transform.position);

        Debug.Log("Ans Card2 Pos: " + P01W.getAnsField2trans().position);

        //Check if above left boundary
        if (card.transform.position.x < P01W.getAnsField2trans().position.x - 40)
            return false;
        //Check if below right boundary
        else if (card.transform.position.x > P01W.getAnsField2trans().position.x + 40)
            return false;
        //Check if above bottom boundary
        else if (card.transform.position.y < P01W.getAnsField2trans().position.y - 60)
            return false;
        //Check if below top boundary
        else if (card.transform.position.y > P01W.getAnsField2trans().position.y + 60)
            return false;
        else
            return true;
    }

    public Vector2 getStartPos()
    {
        return startPos;
    }


}
