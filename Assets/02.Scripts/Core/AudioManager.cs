using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<AudioID, AudioEntry> _audioEntries = new();

    private List<AudioSource> _audioSources = new();
    
    private AudioMixer _audioMixer;

    private AudioMixerGroup _sfxGroup;
    private AudioMixerGroup _bgmGroup;
    
    private void Awake()
    {
        _audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
        
        var audioData = Resources.Load<AudioData>("Audio/AudioData");

        if (audioData == null || _audioMixer == null)
        {
            Debug.LogError("잘못된 리소스");
            
            return;
        }
        
        _sfxGroup = _audioMixer.FindMatchingGroups(AudioType.SFX.ToString())[0];
        _bgmGroup = _audioMixer.FindMatchingGroups(AudioType.BGM.ToString())[0];

        if (_sfxGroup == null || _bgmGroup == null)
        {
            Debug.Log("잘못된 오디오 그룹");

            return;
        }
        
        foreach (var item in audioData.Entries)
        {
            _audioEntries[item.id] = item;
        }        
    }
    
    public void PlayAudio(AudioID id)
    {
        if (_audioEntries.TryGetValue(id, out AudioEntry entry))
        {
            AudioSource audioSource = GetAudioSource();

            audioSource.outputAudioMixerGroup = entry.audioType == AudioType.SFX ? _sfxGroup : _bgmGroup;
            
            audioSource.loop = entry.isLoop;

            audioSource.clip = entry.clip;

            audioSource.volume = entry.volume;

            audioSource.Play();
        }
    }
    
    public void StopAudio(AudioID id)
    {
        if (_audioEntries.TryGetValue(id, out var entry))
        {
            foreach (var source in _audioSources)
            {
                if (source.isPlaying && source.clip == entry.clip)
                {
                    source.Stop();
                    break;
                }
            }
        }
    }
    

    public void SetVolume(AudioType type,  float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        
        _audioMixer.SetFloat(type.ToString(), dB);
    }
    
    
    private AudioSource GetAudioSource()
    {
        foreach (AudioSource source in _audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        
        AudioSource createdSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        
        createdSource.transform.SetParent(transform);
            
        createdSource.playOnAwake = false;
        
        _audioSources.Add(createdSource);
        
        return createdSource;
    }
}
