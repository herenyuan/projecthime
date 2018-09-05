using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour {

    // Use this for initialization
    public bool Active = false;
    public bool Find = false;
    public Text txt;
    public Image IMGTest;
    TextAsset txtAsset;
    string str = "";
    public Sprite sp;
    void Start () {

       
        str += "GUID: "+ Application.buildGUID + "\n";
        str += "persistentDataPath: " + Application.persistentDataPath + "\n";
        str += "HIMPath.Src: " + HIMPath.Src + "\n";
        txt.text = str;
        HIMResources.Ins.onErrorCallBack = onError;
        HIMResources.Ins.onMessageCallBack = onError;
        HIMResources.Ins.Online();
        HIMResources.Ins.LoadPrefab("Prefab/Item/", "Ball");
        //HIMResources.Ins.LoadPrefab("Prefab/Item/", "Ball2");
        //HIMResources.Ins.LoadPrefab("Prefab/Item/", "Ball3");
        
        //txtAsset = HIMResources.Ins.LoadText("config","buff",".json");
        //IMGTest.sprite = HIMResources.Ins.LoadImage("sprite", "icon10004");
        //Debug.Log(txtAsset.text);
    }
    void onError(string msg)
    {
        str += msg+"\n";
        txt.text = str;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
