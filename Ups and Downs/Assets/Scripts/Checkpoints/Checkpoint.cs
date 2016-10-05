﻿using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    /** Indictates which player can activate and use this checkpoint */
    public Side checkpointSide;

    /** purely visible for Debug purposes, DO NOT CHANGE */
    public bool active = false;

    public Vector3 getPosition()
    {
        return transform.position;
    }

    /** Called when a player activates this checkpoint */
    public void activate()
    {
        active = true;
    }

    public bool isActive()
    {
        return active;
    }

    /** Activates the checkpoint when the player enters its collision sphere. */
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerController controller = col.gameObject.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.addCheckPoint(gameObject.GetComponent<Checkpoint>());
            }
        }

    }
}