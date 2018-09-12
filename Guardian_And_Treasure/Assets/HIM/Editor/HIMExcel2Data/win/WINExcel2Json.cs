using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using Excel;
using System.Data;
using System.Text;

public class WINExcelToJson : EditorWindow
{
    public string SaveHead = "";
    /// <summary>
    /// 输出格式索引
    /// </summary>
    private int indexOfFormat = 0;

    /// <summary>
    /// 输出格式
    /// </summary>
    private string[] formatOption = new string[] { "JSON" };

    /// <summary>
    /// 编码索引
    /// </summary>
    private int indexOfEncoding = 0;

    /// <summary>
    /// 编码选项
    /// </summary>
    private string[] encodingOption = new string[] { "UTF-8", "GB2312" };
    public string SaveKey = "SavedPath";
    public string[] fullNames;
    public List<string> TotalPath;

    public void Initialization()
    {

    }
    public void OnGUI()
    {
        if(HIMEditorUtility.EdtConfig.ExcelFolder.Count == 0)
        {
            GUI.color = Color.red;
            EditorGUILayout.LabelField("请在HIM设置中添加配置路径.....");
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        EditorGUI.BeginDisabledGroup(true);
        for (int i = 0; i < HIMEditorUtility.EdtConfig.ExcelFolder.Count; i++)
        {
            EditorGUILayout.TextField(HIMEditorUtility.EdtConfig.ExcelFolder[i]);
        }
        EditorGUI.EndDisabledGroup();

        GUI.color = Color.green;
        bool import = GUILayout.Button("导入配置", GUILayout.ExpandWidth(true));
        GUI.color = Color.white;

        GUILayout.EndVertical();

        if (import)
        {
            this.SearchExcel();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        //获取所有路径
        if (TotalPath != null)
        {
            for (int i = 0; i < TotalPath.Count; i++)
            {
                EditorGUILayout.LabelField(TotalPath[i]);
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("请选择格式类型:", GUILayout.Width(85));
            indexOfFormat = EditorGUILayout.Popup(indexOfFormat, formatOption, GUILayout.Width(125));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("请选择编码类型:", GUILayout.Width(85));
            indexOfEncoding = EditorGUILayout.Popup(indexOfEncoding, encodingOption, GUILayout.Width(125));
            GUILayout.EndHorizontal();

            bool convert = GUILayout.Button("转换");
            if (convert)
            {
                //转换到json
                for (int i = 0; i < TotalPath.Count; i++)
                {
                    string fullName = TotalPath[i];
                    string name = "";
                    DataSet dataSet = this.Try(fullName, out name);
                    string folderName = HIMEditorUtility.ResPath + HIMEditorUtility.EdtConfig.ExportJsonFolder;
                    if (!Directory.Exists(folderName)) { Directory.CreateDirectory(folderName); }
                    string fileName = HIMEditorUtility.ResPath + HIMEditorUtility.EdtConfig.ExportJsonFolder + @"\" + name;
                    Debug.Log("output: " + fileName);
                    fileName = fileName.Replace(".xlsx", ".json");
                    Excel2Json.Convert(fileName, dataSet, Encoding.GetEncoding("utf-8"));
                }
                AssetDatabase.Refresh();
            }
        }
        GUILayout.EndVertical();
    }
    void SearchExcel()
    {
        TotalPath = new List<string>();
        for (int i = 0; i < HIMEditorUtility.EdtConfig.ExcelFolder.Count; i++)
        {
            if (this.CheckPath(HIMEditorUtility.EdtConfig.ExcelFolder[i]))
            {
                string[] path = Directory.GetFiles(HIMEditorUtility.EdtConfig.ExcelFolder[i]);
                List<string> temp = new List<string>(path);
                TotalPath.AddRange(temp);
            }
            else
            {
                Debug.LogError("请填写正确的文件路径: " + HIMEditorUtility.EdtConfig.ExcelFolder[i]);
            }
        }


    }
    public bool CheckPath(string path)
    {
        string pattern = @"^[a-zA-Z]:(((\\(?! )[^/:*?<>\""|\\]+)+\\?)|(\\)?)\s*$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(path);
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="excelFile">Excel file.</param>
    public DataSet Try(string excelFile, out string fileName)
    {
        FileInfo fileInfo = new FileInfo(excelFile);
        fileName = fileInfo.Name;
        FileStream mStream = fileInfo.Open(FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        return mExcelReader.AsDataSet();
    }
}
