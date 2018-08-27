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
    public List<string> ExcelRoot;
    public string[] fullNames;
    public List<string> TotalPath;
    public void Initialization()
    {
        string SaveData = PlayerPrefs.GetString(SaveKey);
        string[] path = SaveData.Split(';');
        ExcelRoot = new List<string>(path);
        //至少会存在一条空路径
        for (int i = 0; i < path.Length; i++)
        {
            Debug.Log("读取路径：" + path[i]);
        }
    }
    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        for (int i = 0; i < ExcelRoot.Count; i++)
        {
            string inputString = EditorGUILayout.TextField(ExcelRoot[i]);
            if (!inputString.Equals(ExcelRoot[i]))
            {
                ExcelRoot[i] = inputString;
                Debug.Log("路径输入：" + inputString);
            }
        }
        GUILayout.BeginHorizontal();
        bool AddPath = GUILayout.Button("[+]~点击这里增加配置路径",GUILayout.ExpandWidth(true));
        GUI.color = Color.red;
        bool DeletePath = GUILayout.Button("[-]~删除一条配置路径", GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        GUILayout.EndHorizontal();
        if (AddPath)
        {
            ExcelRoot.Add("");
        }
        if(DeletePath)
        {
            int lastIndex = ExcelRoot.Count - 1;
            if(lastIndex>=0)
            {
                ExcelRoot.RemoveAt(lastIndex);
            }
        }
        GUI.color = Color.green;
        bool import = GUILayout.Button("导入配置", GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        GUILayout.EndVertical();

        if (import)
        {
            string SaveData = "";
            for (int i = 0; i < ExcelRoot.Count; i++)
            {
                if (i < ExcelRoot.Count - 1)
                {
                    SaveData += ExcelRoot[i]+";";
                }
                else
                {
                    SaveData += ExcelRoot[i];
                }
            }
            Debug.Log("Save: "+SaveData);
            PlayerPrefs.SetString(SaveKey, SaveData);
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
                    DataSet dataSet = this.Try(fullName,out name);
                    string output = Application.dataPath + "/HOA/Resources/Config/" + name;

                    Debug.Log("output: " + output);
                    output = output.Replace(".xlsx", ".json");
                    Excel2Json.Convert(output, dataSet, Encoding.GetEncoding("utf-8"));
                }
                AssetDatabase.Refresh();
            }

        }
        GUILayout.EndVertical();
       
    }
    void SearchExcel()
    {
        if(ExcelRoot == null || ExcelRoot.Count == 0)
        {
            Debug.LogError("请填写路径");
            return;
        }
        TotalPath = new List<string>();
        for (int i = 0; i < ExcelRoot.Count; i++)
        {
            if (this.CheckPath(ExcelRoot[i]))
            {
                string []path = Directory.GetFiles(ExcelRoot[i]);
                List<string> temp = new List<string>(path);
                TotalPath.AddRange(temp);
            }
            else
            {
                Debug.LogError("请填写正确的文件路径: " + ExcelRoot[i]);
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
