using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIM
{
    public static HIMSOProject Project;
    public static void Online()
    {
        Project = HIMResources.Ins.LoadSO<HIMSOProject>(HIMPath.SO, "HIMSOProject");
    }
}
