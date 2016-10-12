using UnityEngine;
using System.Collections;

/**
	An implementation of the Switchable abstract class.
	A toggleable block changes transparency and collision when activated.
    
    Currently used in conjuction with a pressure plate. 
    Toggle block is 'active' when pressure plate is compressed   
*/
public class ToggleBlock : Switchable {
	
	/** The material which will be rendered when this object is active */
	public Material activeMaterial;
	/** The material which will be rendered when this object is deactive */
	public Material inactiveMaterial;

    public BlockState defaultState = BlockState.SOLID;
    private BlockState currentState;
    public enum BlockState { SOLID, TRANSPARENT }
	
	private Renderer rend;
	private Collider doorCollider;

    private void makeSolid()
    {
        doorCollider.enabled = true;
        rend.material = activeMaterial;
        currentState = BlockState.SOLID;
    }

    private void makeTransparent()
    {
        doorCollider.enabled = false;
        rend.material = inactiveMaterial;
        currentState = BlockState.TRANSPARENT;
    }
	

	// Use this for initialization
	void Start () {
		doorCollider = GetComponent<Collider>();
		rend = GetComponent<Renderer>();
        deactivate();
    }

    public override void activate() {
        switch (defaultState)
        {
            case (BlockState.SOLID):
                makeTransparent();
                break;
            case (BlockState.TRANSPARENT):
                makeSolid();
                break;
        }
    }

    public override void deactivate()
    {
        switch (defaultState)
        {
            case (BlockState.SOLID):
                makeSolid();
                break;
            case (BlockState.TRANSPARENT):
                makeTransparent();
                break;
        }
    }

    public override void toggle()
    {
        switch (currentState)
        {
            case (BlockState.SOLID):
                makeTransparent();
                break;
            case (BlockState.TRANSPARENT):
                makeSolid();
                break;
        }
    }
}
