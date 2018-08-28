using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIMPlatform
{
    public static RuntimePlatform Current
    {
        get {return Application.platform; }
    }
}
