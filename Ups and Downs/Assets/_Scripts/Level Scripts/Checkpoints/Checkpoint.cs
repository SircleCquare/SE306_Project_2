using UnityEngine;
using System.Collections;

/// <summary>
/// A Checkpoint defines a location where a given player character should respawn at
/// in their untimely death. This is used to enable easy level resetting.
/// and could be used to provide save-points within a level (currently unused).
/// </summary>
public class Checkpoint : MonoBehaviour {

    /** Indictates which player can activate and use this checkpoint */
    public Side checkpointSide;

    /// <summary>
    /// The order represents which checkpoint in the sequence of checkpoints for
    /// the level side this is. 0 represents the first checkpoint, 1 represents
    /// the second checkpoint (unused) and so forth.
    /// Matching nth checkpoints on both sides need to be on the same screen otherwise
    /// the characters will be moed around upon respawning.
    /// </summary>
    public int order = 0;

    /** purely visible for Debug purposes, DO NOT CHANGE */
    public bool active = false;

    void Awake()
    {
        GameController.Singleton.RegisterCheckpoint(this);
        //Debug.Log(GameController.Singleton.getCheckpoint(checkpointSide, 0));
    }

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
        if (col.gameObject.tag == GameController.PLAYER_TAG)
        {
            PlayerController controller = col.gameObject.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.addCheckPoint(gameObject.GetComponent<Checkpoint>());
            }
        }

    }
}
