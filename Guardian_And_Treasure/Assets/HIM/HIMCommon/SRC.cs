using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CORE
{
    /// <summary>
    /// CORE 核心组件
    /// 资源管理类
    /// 引擎资源综合调度
    /// </summary>
    public class RESOURCESEx
    {
        public enum Error
        {
            none = 0,
            noPrefab,
            noFile,
        }
        //处理范围
        //Prefab
        //Texture
        //Material
        //Text
        //etc...

        /// <summary>
        /// 制作一个预设 GameObject 物体，克隆到场景中
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">资源名字</param>
        /// <param name="parent">绑定父节点</param>
        public static Error MakeGo(string path, string name, out GameObject clone_, Transform parent = null)
        {
            string fullName = path + name;
            GameObject original = Resources.Load<GameObject>(fullName);
            if (original == null) { clone_ = null; return Error.noPrefab; }
            clone_ = GameObject.Instantiate(original);
            return Error.none;
        }



        /// <summary>
        /// 创建一个GO并设置显示状态
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="clone_"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static Error MakeGo(string path, string name, out GameObject clone_, bool active = false)
        {
            string fullName = path + name;
            GameObject original = Resources.Load<GameObject>(fullName);
            original.SetActive(active);
            if (original == null) { clone_ = null; return Error.noPrefab; }
            clone_ = GameObject.Instantiate(original);
            return Error.none;
        }

        /// <summary>
        /// 制作一个纹理，返回纹理的内存引用
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">资源名字</param>
        public static void MakeTex(string path, string name, out Texture Texture_)
        {
            Texture_ = null;
        }
        /// <summary>
        /// 制作材质
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="Mat_"></param>
        public static void MakeMat(string path, string name, out Material Mat_)
        {
            Mat_ = null;
        }
        public static void MakeTxt(string path, string name, out string Txt_)
        {
            Txt_ = null;
        }

        /// <summary>
        /// 制作一个预设 GameObject 物体，克隆到场景中
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">资源名字</param>
        /// <param name="parent">绑定父节点</param>
        public static GameObject LoadGo(string fullName, out Error code, Transform parent,  bool active = false, bool instalInWorldSpace = false)
        {
            GameObject original = Resources.Load<GameObject>(fullName);
            if (original == null) { code = Error.noPrefab; return null; }
            original = GameObject.Instantiate(original, parent, instalInWorldSpace);
            original.SetActive(active);
            code = Error.none;
            return original;
        }
        public static TextAsset LoadText(string fullName, out Error code)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(fullName);
            if (textAsset == null) { code = Error.noFile; return null; }
            code = Error.none;
            return textAsset;
        }

        public static Texture LoadTexture(string fullName, out Error code)
        {
            Texture tex = Resources.Load<Texture>(fullName);
            if(tex == null) { code = Error.noFile; return null; }
            code = Error.none;
            return tex;
        }
        public static Sprite LoadSprite(string _Path, string _Name,out Error code)
        {
            string fullName = _Path + _Name;
            Sprite sp = Resources.Load<Sprite>(fullName);
            if (sp == null)
            {
                Debug.Log(string.Format("{0} is not exist...", fullName));
                fullName = _Path + "NONE";
                sp = Resources.Load<Sprite>(fullName);
            }
            if (sp == null)
            {
                Debug.Log(string.Format("{0} is not exist, please check out resources in path -> [ {1} ]", _Name, fullName));
            }
            code = Error.none;
            return sp;
        }
        public static Material LoadMat(string _Path,string _Name,out Error error)
        {
            string fullName = _Path + _Name;
            Material mat = Resources.Load<Material>(fullName);
            if (mat == null)
            {
                Debug.Log(string.Format("{0} is not exist...", fullName));
                fullName = _Path + "NONE";
                mat = Resources.Load<Material>(fullName);
            }
            if (mat == null) { throw new System.Exception(string.Format("{0} is not exist, please check out resources in path -> [ {1} ]", _Name, fullName)); }
            error = Error.none;
            return mat;
        }
    }
}

