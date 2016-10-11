using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
        Renderer renderer = gameObject.GetComponent<Renderer>();

	    if (GameController.Singleton.getSide() == Side.Dark)
	    {
	        UpdateActive();
            renderer.enabled = true;
	    }
	    else
	    {
            renderer.enabled = false;
	    }
	}

    protected abstract void UpdateActive();
}
