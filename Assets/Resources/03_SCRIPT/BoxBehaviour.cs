﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour {

    // Use this for initialization

    //public int size;
	void Start () {
        string spriteName = GetComponent<SpriteRenderer>().sprite.name;
        if (spriteName.Contains("small"))
        {
            size = 1;
            smallBoxPosition = transform.position;
        } else if (spriteName.Contains("medium"))
        {
            size = 2;
            mediumBoxPosition = transform.position;
        } else if (spriteName.Contains("big"))
        {
            size = 3;
            bigBoxPosition = transform.position;
        }
        Transform animationTransform = gameObject.transform.Find("BoxAnimation");
        if (animationTransform != null)
        {
            animation = animationTransform.gameObject;
            animation.GetComponent<Renderer>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (takenByPlayer)
        {
            positionOverPlayer();
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "PlayerHand")
        {
            playerInReach = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "PlayerHand")
        {
            playerInReach = false;
        }
    }

    GameObject animation = null;
    public Toy toy = null;
    public int size = 0;
    public bool isClosed = false;
    public bool isClosing = false;
    public bool playerInReach = false;
    public bool takenByPlayer = false;
    static public Vector3 smallBoxPosition;
    static public Vector3 mediumBoxPosition;
    static public Vector3 bigBoxPosition;

    public void packBox()
    {
        isClosing = true;
        float delay = 0;
        switch (size)
        {
            case 1:
                delay = 1;
                break;
            case 2:
                delay = 2.25f;
                break;
            case 3:
                delay = 4;
                break;
        }
        if (animation != null)
        {
            animation.GetComponent<Renderer>().enabled = true;
        }
        StartCoroutine(closeBoxAfterDelay(delay));
    }

    public void closeOrOpenBox(bool closing)
    {
        string boxState = (closing) ? "full" : "empty";
        switch (size)
        {
            case 1:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("09_TEXTURE/" + "props_box_small_" + boxState);
                break;
            case 2:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("09_TEXTURE/" + "props_box_medium_" + boxState);
                break;
            case 3:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("09_TEXTURE/" + "props_box_big_" + boxState);
                break;
        }
        isClosed = closing;
    }

    void positionOverPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        gameObject.transform.position = new Vector3(player.transform.position.x + (player.transform.lossyScale.x/3), player.transform.position.y + (2*player.transform.lossyScale.y), -5);
    }

    IEnumerator closeBoxAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        if (animation != null)
        {
            animation.GetComponent<Renderer>().enabled = false;
        }
        isClosing = false;
        closeOrOpenBox(true);
    }
}
