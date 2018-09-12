using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class Excel2Json
{
    public static Dictionary<string, string> TypeEx = new Dictionary<string, string>() {
        { "int","System.Int32" },
        { "string","System.String"},
        { "float","System.Single"},
    };

    public static List<string> sift = new List<string>() {
        "//",
        "##",
    };
    public static string savePath = "";
    public static int filedRow = 0;
    public static int valueRow = 0;
    
    /// <summary>
    /// 转换为Json
    /// </summary>
    /// <param name="JsonPath">Json文件路径</param>
    /// <param name="Header">表头行数</param>
    public static void Convert(string JsonPath, DataSet dataSet, Encoding encoding)
    {
        //判断Excel文件中是否存在数据表
        if (dataSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = dataSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表存储整个表的数据
        List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();
        //读取第2行数据的字段名


        //读取数据当前表的第 4 行开始才是数据
        for (int i = 3; i < rowCount; i++)
        {
            //准备一个字典存储每一行的数据
            Dictionary<string, object> row = new Dictionary<string, object>();
            for (int j = 0; j < colCount; j++)
            {
                string field = mSheet.Rows[1][j].ToString();
                string fieldType = mSheet.Rows[2][j].ToString();
                if (!Valid(field)) { continue; }
                field = Format(field);
                fieldType = Format(fieldType);
                object value = mSheet.Rows[i][j];
                Type valueType = value.GetType();
                if (valueType == typeof(System.DBNull)) { continue; }
                if (fieldType.Equals("int"))
                {
                    row[field] = System.Convert.ToInt32(value);
                }
                else
                {
                    row[field] = mSheet.Rows[i][j];
                }
                //Key-Value对应

            }

            if (row != null && row.Count > 0)
            {
                //添加到表数据中
                table.Add(row);
            }
            
        }
     
        //生成Json字符串
        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        //string json = JsonMapper.ToJson(table);
        //写入文件
        using (FileStream fileStream = new FileStream(JsonPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(json);
            }
        }
    }

    public static string ExcelPath = "";
    /// <summary>
    /// 把配置转移到Assets指定目录
    /// </summary>
    public static void MoveExcelToAssets()
    {

    }

    public static void Convert2Object(string JsonPath, DataSet dataSet, Encoding encoding)
    {
        if (dataSet.Tables.Count == 0) { Debug.LogError("Excel 文件中不存在表"); return; }

        DataTable sheet = dataSet.Tables[0];//获取第一张表
        if (sheet.Rows.Count == 0) { Debug.LogError("表中不存在数据"); return; }

        //读取数据表行数和列数
        int rowCount = sheet.Rows.Count;
        int colCount = sheet.Columns.Count;

        if (rowCount < 3) { return; }

        DataRow memoes = sheet.Rows[0];//备注名、注释
        DataRow fieldNames = sheet.Rows[1];//字段名
        DataRow fieldType = sheet.Rows[2]; //字段类型




        string json = JsonConvert.SerializeObject(null, Newtonsoft.Json.Formatting.Indented);
        //写入文件
        using (FileStream fileStream = new FileStream(JsonPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(json);
            }
        }
    }
    /// <summary>
    /// 对于表格中的空值，找到一列中的非空值，并构造一个同类型的默认值
    /// </summary>
    private object getColumnDefault(DataTable sheet, DataColumn column, int firstDataRow)
    {
        for (int i = firstDataRow; i < sheet.Rows.Count; i++)
        {
            object value = sheet.Rows[i][column];
            Type valueType = value.GetType();
            if (valueType != typeof(System.DBNull))
            {
                if (valueType.IsValueType)
                    return Activator.CreateInstance(valueType);
                break;
            }
        }
        return "";
    }
    /// <summary>
    /// 获取列的对象
    /// </summary>
    private static object GetColumnObject(string fieldName, object value)
    {
        object final;
        Type valueType = Type.GetType(fieldName);
        if (valueType != typeof(System.DBNull))
        {
            if (valueType.IsValueType) { final = Activator.CreateInstance(valueType); }
        }
        return null;
    }
    public static bool Valid(string field)
    {
        for(int i = 0;i<sift.Count;i++)
        {
            if(field.StartsWith(sift[i]))
            {
                return false;
            }
        }
        return true;
    }
    public static string Format(string field)
    {
        string result = field;
        if (field.StartsWith("**")) { result = field.Replace("**", ""); }
        return result;
    }
}

public class JSONTable
{
    public object Data;
}

