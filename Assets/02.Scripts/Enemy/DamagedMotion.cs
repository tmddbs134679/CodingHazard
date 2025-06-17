using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedMotion : MonoBehaviour
{
    public Transform spineBone; // Çã¸® »À
    public float bendAmount = 20f;
    public float duration = 0.3f;

    private Quaternion originalRot;

    void Start()
    {
        originalRot = spineBone.localRotation;
    }

    public void PlayBend()
    {
        StopAllCoroutines();
        StartCoroutine(BendBack());
    }

    IEnumerator BendBack()
    {
        float time = 0f;
        Quaternion targetRot = originalRot * Quaternion.Euler(-bendAmount, 0, 0);
        while (time < duration)
        {
            spineBone.localRotation = Quaternion.Slerp(originalRot, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        // ¿ø»óº¹±Í
        time = 0f;
        while (time < duration)
        {
            spineBone.localRotation = Quaternion.Slerp(targetRot, originalRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
