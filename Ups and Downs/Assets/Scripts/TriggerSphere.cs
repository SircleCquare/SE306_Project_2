using UnityEngine;
using System.Collections;

public class TriggerSphere : MonoBehaviour {

    public Switchable[] targetList;


    /**
        Activates the target list when a player enters the target sphere.
    */
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            setActive();
        }

    }

    /**
       Deactivates the target list when a player leaves the target sphere.
   */
    void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            setDeactive();
        }

    }

    /** Activates all targets in the target list. Effectively calls activate() on all switchables */
    private void setActive()
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            Switchable target = targetList[i];
            target.activate();
        }
    }

    /** Deactivates all targets in the target list. Effectively calls deactivate() on all switchables */
    private void setDeactive()
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            Switchable target = targetList[i];
            target.deactivate();
        }
    }

}
