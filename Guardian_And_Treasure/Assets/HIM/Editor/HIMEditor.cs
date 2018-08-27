using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HIMEditor
{
    /// <summary>
    /// 显示当前窗口	
    /// </summary>
    [MenuItem("游戏设计器/导表工具")]
    public static void Tool1()
    {
        WINExcelToJson win = EditorWindow.GetWindow<WINExcelToJson>("导表工具");
        win.Initialization();
        win.Show();
    }
    [MenuItem("游戏设计器/资源打包工具")]
    public static void Tool2()
    {
        HIMABEditorWindow win = EditorWindow.GetWindow<HIMABEditorWindow>("资源打包工具");
        win.Initialization();
        win.Show();
    }
}
