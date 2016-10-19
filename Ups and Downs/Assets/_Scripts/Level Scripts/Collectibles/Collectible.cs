using UnityEngine;
using System.Collections;

/// <summary>
/// The Collectible class provides a framework for all collectible items.
/// When colliding with the player the onPickup() method is invoked.
/// </summary>
public class Collectible : MonoBehaviour {

    private Vector3 originPosition;
    private Vector3 originRotation;
    private Vector3 originScale;

    /// <summary>
    /// Controls whether or not this collectible will be replaced on death (defaults to true)
    /// </summary>
    public bool isResetOnDeath = true;
    private bool pickedUp = false;

    /// <summary>
    /// Set position, rotation and scale at runtime from the collectble's world position
    /// (Used to replace the collectible).
    /// </summary>
    protected virtual void Start()
    {
        originPosition = transform.position;
        originRotation = transform.eulerAngles;
        originScale = transform.localScale;
    }

  /// <summary>
  /// When the active player collides with the collectible call onPickup().
  /// </summary>
	protected void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == GameController.PLAYER_TAG && !pickedUp)
    {
      onPickup();
    }

  }

  /// <summary>
  /// Called when the collectible is picked up by the active player character.
  /// </summary>
  public virtual void onPickup() {

    // Destroy the object if it's not due to be replaced on death.
    if (!isResetOnDeath)
    {
      Destroy(this.gameObject);
    }

    // Otherwise, simply hide the collectible.
    GetComponent<Renderer>().enabled = false;
    pickedUp = true;
  }

  /// <summary>
  /// Reset the collectible's position state and visiblility.
  /// </summary>
  public virtual void ResetBehaviour()
  {
    transform.position = originPosition;
    transform.eulerAngles = originRotation;
    transform.localScale = originScale;
    GetComponent<Renderer>().enabled = true;
    pickedUp = false;
  }
}
