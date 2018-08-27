using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 声道单元
/// </summary>
public class HIMAudio : MonoBehaviour
{
    public AudioSource mSource;
    public string OriginalName;
    public void Initialization()
    {
        mSource = gameObject.AddComponent<AudioSource>();
        OriginalName = name;
    }
    public void Play(AudioClip clip, ulong delay = 0, bool loop = false)
    {
        name = clip.name;
        mSource.clip = clip;
        mSource.loop = loop;
        mSource.Play(delay);
    }
    public void Stop()
    {
        mSource.Stop();
        mSource.loop = false;
        name = OriginalName;
    }
    public void Pause()
    {
        mSource.Pause();
    }
}
