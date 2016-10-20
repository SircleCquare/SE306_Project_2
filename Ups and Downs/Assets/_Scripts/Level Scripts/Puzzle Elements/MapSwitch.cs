using UnityEngine;
using System.Collections;

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