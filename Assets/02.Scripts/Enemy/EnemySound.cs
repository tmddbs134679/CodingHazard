using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField]
    private float maxVolume = 0.01f;
    [SerializeField]    
    private float maxDistance = 15f;
    [SerializeField]
    private List<AudioClip> clipList = new List<AudioClip>();

    private AudioClip nowClip;
    public enum monSound {Run, Idle, Attack,Damaged,Dead}
    private AudioSource source;
    private AudioSource zombieBGM;
  
    private void Awake()
    {
        source = GetComponent<AudioSource>();   
        zombieBGM = new GameObject("AudioSource").AddComponent<AudioSource>();
        zombieBGM.spatialBlend = 1.0f;       
        zombieBGM.rolloffMode = AudioRolloffMode.Logarithmic; 
        zombieBGM.minDistance = 3f;            
        zombieBGM.maxDistance = 50f;
        zombieBGM.transform.SetParent(transform);
        zombieBGM.clip = clipList[4];
        zombieBGM.volume = 0.3f;
        StartCoroutine(growling());
    }
    public void StopMusic()
    {
        StopAllCoroutines();
        Destroy(source);
        Destroy(zombieBGM);

    }
    IEnumerator growling()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(6, 20));
            if (!zombieBGM.isPlaying)
            {
                zombieBGM.volume = Random.Range(0.25f, 0.35f);
                zombieBGM.pitch = Random.Range(0.8f, 1.2f);
                zombieBGM.Play();
            }
         
        }
    }
    private void Update()
    {
        
    }
    public void PlaySound(monSound monSound)
    {
        source.loop = false;
        source.clip = clipList[(int)monSound];
        source.Play();
    }
    public void PlayFootstep(monSound mod)
    {
        source.loop = true;
        if (clipList[(int)mod] == nowClip && source.isPlaying)
        {
            return;
        }
      source.clip=clipList[(int)mod];
      source.Play();
    }


}
