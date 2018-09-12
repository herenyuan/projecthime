using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// scene 文件管理
/// </summary>
public class HIMScenes : SingleMono<HIMScenes>
{
    private static AsyncOperation CurrentAsync;

    //--------------------------------------------Load 系列方法
    //--------------------------------------------这个是最常用的
    /// <summary>
    /// 加载一个场景（小型场景）阻塞加载
    /// </summary>
    public static void Load(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
    /// <summary>
    /// 异步加载一个场景
    /// </summary>
    public static void LoadAsync(string name)
    {
        if (Application.isPlaying)
        {
            //COROUTINE.Ins.Begin(LoadProgress(name, null));
        }
        else
        {
            Debug.LogError("异步加载方法只能在Play模式下运行");
        }
    }
    private static IEnumerator LoadProgress(string name, object p)
    {
        CurrentAsync = SceneManager.LoadSceneAsync(name);
        CurrentAsync.allowSceneActivation = false;
        Debug.Log("step1");
        // 不允许加载完毕自动切换场景，因为有时候加载太快了就看不到加载进度条UI效果了
        //ao.allowSceneActivation = true;
        // mAsyncOperation.progress测试只有0和0.9(其实只有固定的0.89...)
        // 所以大概大于0.8就当是加载完成了
        while (true)
        {

            if (CurrentAsync.progress < 0.9f)
            {
                Debug.Log("Progress: " + CurrentAsync.progress);
            }
            else
            {
                CurrentAsync.allowSceneActivation = true;

                Debug.Log("场景加载：" + CurrentAsync.isDone);
                break;
            }

            yield return null;
        }
        Debug.Log("final: " + CurrentAsync.progress);

    }

    //----------------------------------------Add 系列方法
    //----------------------------------------容易导致场景烘培物件失效

    public static void Add(string name)
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogError("叠加场景，只能在 Play 模式下运行");
        }
    }


    /// <summary>
    /// AddAsync 【暂不使用】
    /// </summary>
    /// <param name="name"></param>
    public static void AddAsync(string name)
    {
        if (Application.isPlaying)
        {
            //COROUTINE.Ins.Begin(AddProgress(name, null));
        }
        else
        {
            Debug.LogError("异步叠加场景，只能在Play模式下运行");
        }
    }
    private static IEnumerator AddProgress(string name, object p)
    {
        CurrentAsync = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        CurrentAsync.allowSceneActivation = false;
        Debug.Log("step1");
        // 不允许加载完毕自动切换场景，因为有时候加载太快了就看不到加载进度条UI效果了
        //ao.allowSceneActivation = true;
        // mAsyncOperation.progress测试只有0和0.9(其实只有固定的0.89...)
        // 所以大概大于0.8就当是加载完成了
        while (true)
        {

            if (CurrentAsync.progress < 0.9f)
            {
                Debug.Log("Progress: " + CurrentAsync.progress);
            }
            else
            {
                CurrentAsync.allowSceneActivation = true;
                Debug.Log("场景加载：" + CurrentAsync.isDone);
                break;
            }

            yield return null;
        }
        Debug.Log("final: " + CurrentAsync.progress);
    }

    public override void Online()
    {
        
    }

    public override void Offline()
    {
        
    }
    //------------------------------------------------------回调
}


