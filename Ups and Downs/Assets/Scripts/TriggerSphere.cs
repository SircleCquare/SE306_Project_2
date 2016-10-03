using UnityEngine;
using System.Collections;

public class TriggerSphere : MonoBehaviour {

    public Switchable[] targetList;


    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            setActive();
        }

    }

    void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            setDeactive();
        }

    }

    private void setActive()
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            Switchable target = targetList[i];
            target.activate();
        }
    }

    private void setDeactive()
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            Switchable target = targetList[i];
            target.deactivate();
        }
    }

}
