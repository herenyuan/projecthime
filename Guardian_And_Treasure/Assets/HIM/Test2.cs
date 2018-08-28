using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour {

    // Use this for initialization
    public bool Active = false;
    public bool Find = false;
	void Start () {

        HIMResources.Ins.Online();
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
