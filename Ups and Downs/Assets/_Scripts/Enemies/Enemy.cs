using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

    private Renderer rend;
    private Vector3 originPosition;

    protected virtual void Start()
    {
        originPosition = transform.position;
    }

	// Use this for initialization
	
	protected virtual void Update ()
	{
	    if (GameController.Singleton.getSide() == Side.Dark)
	    {
	        UpdateActive();
            GetComponent<Renderer>().enabled = true;
	    }
	    else
	    {
            GetComponent<Renderer>().enabled = false;
	    }
	}

    /// <summary>
    /// An abstract Update method which is only processed when the Dark Side is active.
    /// </summary>
    protected abstract void UpdateActive();

    public virtual void ResetBehaviour()
    {
        Debug.Log("resetting behaviour");
        transform.position = originPosition;
    }
}
