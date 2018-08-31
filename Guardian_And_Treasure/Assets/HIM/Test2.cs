using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour {

    // Use this for initialization
    public bool Active = false;
    public bool Find = false;
    public Text txt;
	void Start () {

        string str = "";
        str += "GUID: "+ Application.buildGUID + "\n";
        str += "persistentDataPath: " + Application.persistentDataPath + "\n";
        //Debug.Log(Application.buildGUID);
        txt.text = str;
        //HIM.Online();
        //HIMResources.Ins.Online();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            Active = false;
            HIMResources.Ins.LoadPrefab(HIMPath.Prefab, "ball");
        }
        
    }
}
