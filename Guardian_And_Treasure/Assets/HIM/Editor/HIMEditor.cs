using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HIMEditor
{
    [MenuItem("游戏设计器/基本设置")]
    public static void Tool0()
    {
        HIMProjectSettingWindow win = EditorWindow.GetWindow<HIMProjectSettingWindow>("工程设置");
        win.Initialization();
        win.Show();
    }

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
