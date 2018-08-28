using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIM
{
    private static HIMSOProject m_Project;
    public static HIMSOProject Project
    {
        get
        {
            if(m_Project == null)
            {
                m_Project = HIMResources.Ins.LoadSO<HIMSOProject>("/SO/", "HIMSOProject");
            }
            return m_Project;
        }
    }
    public static void Online()
    {
        
    }
}
