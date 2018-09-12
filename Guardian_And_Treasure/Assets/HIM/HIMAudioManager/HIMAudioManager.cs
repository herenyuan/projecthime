using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// MONO单例
/// </summary>
public class HIMAudioManager : SingleMono<HIMAudioManager> {

    [Range(1,10)]
    public int Preload = 5;
    [Range(1, 20)]
    public int MaxCount = 20;
    private List<HIMAudio> HIMAudios = new List<HIMAudio>();
    private Dictionary<string, AudioClip> ClipsMapping = new Dictionary<string, AudioClip>();
    private List<AudioClip> Clips = new List<AudioClip>();

    List<string> folders = new List<string>()
    {
        "Sound/Audio/",
        "Sound/Giant/",
        "Sound/Panda/",
        "Sound/Princess/",
    };

    /// <summary>
    /// 模块初始化
    /// </summary>
    public override void Online()
    {
        gameObject.AddComponent<AudioListener>();
        for (int i = 0; i < Preload; i++)
        {
            GameObject source = new GameObject(string.Format("ADUIO[{0}]", i));
            HIMAudio ins = source.AddComponent<HIMAudio>();
            source.transform.SetParent(transform);
            ins.Initialization();
            HIMAudios.Add(ins);
        }
        this.PreloadSrc();
    }
    public override void Offline()
    {
        
    }
    private void PreloadSrc()
    {
        for (int i = 0; i < folders.Count; i++)
        {
            AudioClip[] tmp = Resources.LoadAll<AudioClip>(folders[i]);
            List<AudioClip> list = new List<AudioClip>(tmp);
            Clips.AddRange(list);
        }
        for (int i = 0; i < Clips.Count; i++)
        {
            ClipsMapping.Add(Clips[i].name, Clips[i]);
        }
    }

    public void Play(string clipName, ulong delay = 0, bool loop = false)
    {
        if (string.IsNullOrEmpty(clipName)) { return; }
        HIMAudio source = this.Search();
        if(source == null) { source = this.Create(); }
        if (source == null) { return; }//没有空闲 或者 到达上限
        AudioClip clip = this.Search(clipName);
        source.Play(clip, delay, loop);
    }
    public void Stop(string clipName)
    {
        for (int i = 0; i < HIMAudios.Count; i++)
        {
            HIMAudio source = HIMAudios[i];
            if (source == null) { continue; }
            if (source.mSource.clip == null) { continue; }
            if (source.name.Equals(clipName))
            {
                source.Stop();
            }
        }
    }
    public AudioClip Search(string clipName)
    {
        if(ClipsMapping.ContainsKey(clipName))
        {
            return ClipsMapping[clipName];
        }
        return null;
    }
    /// <summary>
    /// 获取一个空闲的 HIMAudio
    /// </summary>
    /// <returns></returns>
    public HIMAudio Search()
    {
        for (int i = 0; i < HIMAudios.Count; i++)
        {
            HIMAudio current = HIMAudios[i];
            if(current.mSource.clip == null || !current.mSource.isPlaying)
            {
                return current;
            }
        }
        return null;
    }
    private HIMAudio Create()
    {
        if (HIMAudios.Count <= MaxCount)
        {
            int index = Mathf.Max(0, HIMAudios.Count - 1);
            GameObject source = new GameObject(string.Format("ADUIO[{0}]", index));
            HIMAudio ins = source.AddComponent<HIMAudio>();
            source.transform.SetParent(transform);
            ins.Initialization();
            HIMAudios.Add(ins);
            return ins;
        }
        else
        {
            return null;
        }
    }
}
