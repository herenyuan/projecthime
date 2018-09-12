using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public void Awake()
    {
        HIMEDataManager.Ins.Online();
        HIMDebug.Ins.Online();
        HIMAudioManager.Ins.Online();
        HIMResources.Ins.Online();
        HIMHostManager.Ins.Online();
        HIMNetwork.Ins.Online();
        HIMResources.Ins.Online();
        HIMScenes.Ins.Online();

    }
}
