using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour {

    public Rigidbody rigidbody;
    public MeshRenderer mr;
	// Use this for initialization
	void Start () {
        rigidbody.isKinematic = true;
        mr.sharedMaterial = HIMResources.Ins.LoadMaterial("material", "shine_green_highlight");
    }
	
	// Update is called once per frame
	void Update ()
    {
        float anglex = Random.Range(0, 10);
        float angley = Random.Range(0, 10);
        float anglez = Random.Range(0, 10);
        transform.Rotate(anglex, angley, 0);
	}
}
