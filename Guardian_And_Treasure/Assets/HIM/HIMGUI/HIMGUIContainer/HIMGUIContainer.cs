using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HIMGUIContainer : MonoBehaviour
{
    public Canvas mainCanvas;
    public CanvasScaler mainScaler;
    public Camera uiCamera;
    public Transform uiLayer;
    private List<Transform> roots;
    private void Awake()
    {
        roots = new List<Transform>();
        for (int i = 0; i < uiLayer.childCount; i++)
        {
            string rootName = string.Format("[{0}]", i);
            roots.Add(uiLayer.Find(rootName));
        }
    }
    public void Push(HIMUIBase target, int layerIndex)
    {
        if (layerIndex >= roots.Count) { }
    }
}
