using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIMPlatform
{
#if UNITY_ANDROID
    public static readonly string Current = "Android";
#elif UNITY_STANDALONE_WIN
    public static readonly string Current = "StandaloneWindows";    
#elif UNITY_IOS
    public static readonly string Current = "iOS";  
#endif
}
