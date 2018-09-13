using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class UIDepth : MonoBehaviour
{
    public int order;
    public bool isUI = true;
    public bool Check = false;
    void Start()
    {

    }
    private void Update()
    {
        if(Check)
        {
            Check = false;
            if (isUI)
            {
                Canvas canvas = GetComponent<Canvas>();
                if (canvas == null)
                {
                    canvas = gameObject.AddComponent<Canvas>();
                }
                canvas.overrideSorting = true;
                canvas.sortingOrder = order;
            }
            else
            {
                Renderer[] renders = GetComponentsInChildren<Renderer>();
                foreach (Renderer render in renders)
                {
                    render.sortingOrder = order;
                }
            }
        }
    }
}
