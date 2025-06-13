using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/Data/Audio Data")]
public class AudioData : ScriptableObject
{
    [field: SerializeField] public AudioEntry[] Entries { get; private set; }

}
