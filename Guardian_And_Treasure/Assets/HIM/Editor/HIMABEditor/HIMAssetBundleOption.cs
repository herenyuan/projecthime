using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class HIMAssetBundleOption
{
	public static BuildTarget Current
    {
        get
        {
#if UNITY_ANDROID
            return BuildTarget.Android;
#elif UNITY_STANDALONE_WIN
            return BuildTarget.StandaloneWindows;
#elif UNITY_IOS
            return BuildTarget.iOS;
#endif
        }
    }
}
