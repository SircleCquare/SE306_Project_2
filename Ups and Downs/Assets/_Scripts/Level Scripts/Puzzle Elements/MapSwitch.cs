using UnityEngine;
using System.Collections;

/** 
 * 
 * LOGIC IS COMMENTED OUT DUE TO SWITCH BEHAVIOUR CHANGING
 * REFACTOR IS NEEDED BEFORE CODE CAN BE USED
 * 
 */

public class MapSwitch : MonoBehaviour {

    private LineRenderer line;

    public Transform origin;
    public Transform[] destList; 

	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        
        GameObject parentObj = gameObject.transform.parent.gameObject;
        origin = parentObj.transform;

        ToggleSwitch switchObj = parentObj.GetComponent<ToggleSwitch>();
//        PressureField field = parentObj.GetComponent<PressureField>();
//        TriggerSphere sphere = parentObj.GetComponent<TriggerSphere>();
        if (switchObj != null)
        {
//            destList = new Transform[switchObj.targetList.Length];
//            Debug.Log("switch confirmed");
//            for (int i = 0; i < switchObj.targetList.Length; i++)
//            {
//                Switchable switchableObj = switchObj.targetList[i];
//                destList[i] = switchableObj.gameObject.transform;
//            }
        }
//        else if (field != null)
//        {
//            destList = new Transform[field.targetList.Length];
//            Debug.Log("field confirmed: " + destList.Length);
//            for (int i = 0; i < field.targetList.Length; i++)
//            {
//                Switchable switchableObj = field.targetList[i];
//                Debug.Log("Add Field " + switchableObj.gameObject.name);
//                destList[i] = switchableObj.gameObject.transform;
//            }
//        }
//        else if (sphere != null)
//        {
//            destList = new Transform[sphere.targetList.Length];
//            Debug.Log("sphere confirmed");
//            for (int i = 0; i < sphere.targetList.Length; i++)
//            {
//                Switchable switchableObj = sphere.targetList[i];
//                destList[i] = switchableObj.gameObject.transform;
//            }
//        }
        line.SetWidth(.2f, .2f);
        line.SetVertexCount(2 * destList.Length);
    }
	
	// Update is called once per frame
	void Update () {
        line.enabled = debugLinesOn();
        for (int i = 0; i < destList.Length; i++)
        {
            Transform dest = destList[i];
            line.SetPosition(2*i, origin.position);
            line.SetPosition(2*i + 1, dest.position);
        }
	}


    private bool debugLinesOn()
    {
        return GameController.Singleton.renderSwitchPaths;
    }
}