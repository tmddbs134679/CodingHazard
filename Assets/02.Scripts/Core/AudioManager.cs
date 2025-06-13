using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<AudioID, AudioClip> _audios = new();
    private void Awake()
    {
        var audioData = Resources.Load<AudioData>("Data/AudioData");

        if (audioData == null)
        {
            Debug.LogError("잘못된 오디오 데이터");
            return;
        }
        
        foreach (var item in audioData.Entries)
        {
            _audios[item.id] = item.clip;
        }        
    }
}
