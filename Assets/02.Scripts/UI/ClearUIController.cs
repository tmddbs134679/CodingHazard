using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearUIController : MonoBehaviour
{
    [SerializeField] private UI_ClearPopup _popup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StageManager.Instance.OnClearStage += Play;
    }

    private void OnDisable()
    {
        StageManager.Instance.OnClearStage -= Play;
    }
    private void Play()
    {
        StartCoroutine(PlaySequence());
    }
    private IEnumerator PlaySequence()
    {
        yield return _popup.FadeInCoroutine();             // 1. FadeIn 완료 대기
        yield return _popup.ToastCoroutine();               // 2. Toast 애니메이션 완료 대기
        yield return _popup.StartCreditScrollCoroutine(); // 3. 크레딧 스크롤 완료 대기
    }

}
