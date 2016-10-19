using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

    private Renderer rend;
    private Vector3 originPosition;
    private Vector3 originRotation;
    private Vector3 originScale;

    protected virtual void Start()
    {
        originPosition = transform.position;
        originRotation = transform.eulerAngles;
        originScale = transform.localScale;
    }

	protected virtual void Update ()
	{
	    if (GameController.Singleton.getSide() == Side.DARK)
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
        transform.position = originPosition;
        transform.eulerAngles = originRotation;
        transform.localScale = originScale;
    }
}
