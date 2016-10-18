using UnityEngine;
using System.Linq;
using System.Collections;

/**
    Switches manipulate switchable objects

    Switches will affect 
        - anything that is set as a child of the switch (in scene layout)
        - anything that is manually connected to the switch
            (allows for a switchable to have multiple behaviours)  
*/

public abstract class Switch : MonoBehaviour {

    public Switchable[] externalTargets;
    protected Switchable[] targetList;

    protected virtual void Start()
    {
        Switchable[] existing = GetComponentsInChildren<Switchable>();
        targetList = existing.Concat(externalTargets).ToArray();
    }
}
