/*
Date : 2016/9/17
Author : liucan
Describe : 
xml read/load
*/

using System;
using System.IO;
using System.Xml;
using UnityEngine;

public class XmlManager
{
    // 配置文件保存路径
    private static string path = System.AppDomain.CurrentDomain.BaseDirectory + "Config\\";

    private XmlDocument xmldoc;
    private XmlNode rootNode;

    public XmlManager(string fileName)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string xmlName = path + fileName;
        Debug.Log("读取xml:" + xmlName);
        rootNode = null;
        xmldoc = new XmlDocument();
        xmldoc.Load(xmlName);

        if (xmldoc != null)
        {
            rootNode = xmldoc.SelectSingleNode("root");
        }
    }

    public XmlNode getRootNode()
    {
        return rootNode;
    }

    public XmlNode getChildNode(XmlNode parentNode, string nodeName)
    {
        XmlNode node = parentNode.SelectSingleNode(nodeName);
        return node;
    }

    public XmlNodeList getChildNodes(XmlNode parentNode, string nodeName)
    {
        XmlNodeList nodeList = parentNode.SelectNodes(nodeName);
        return nodeList;
    }

    public int GetValueInt(XmlNode node, string valueName)
    {
        XmlElement ele = (XmlElement)node;
        if (ele.HasAttribute(valueName))
        {
            return Convert.ToInt32(ele.GetAttribute(valueName).ToString());
        }
        return 0;
    }

    public float GetValueFloat(XmlNode node, string valueName)
    {
        XmlElement ele = (XmlElement)node;
        if (ele.HasAttribute(valueName))
        {
            return Convert.ToSingle(ele.GetAttribute(valueName).ToString());
        }
        return 0f;
    }

    public string GetValueString(XmlNode node, string valueName)
    {
        XmlElement ele = (XmlElement)node;
        if (ele.HasAttribute(valueName))
        {
            return ele.GetAttribute(valueName).ToString();
        }
        return "";
    }

    public bool GetValueBool(XmlNode node, string valueName)
    {
        XmlElement ele = (XmlElement)node;
        if (ele.HasAttribute(valueName))
        {
            return ele.GetAttribute(valueName) == "true" ? true : false;
        }
        return false;
    }
}
