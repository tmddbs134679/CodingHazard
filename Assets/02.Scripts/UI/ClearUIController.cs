using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUIController : MonoBehaviour
{
    [SerializeField] private UI_ClearPopup _popup;
    [SerializeField] private UI_FailPopup _failpopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StageManager.Instance.OnClearStage += Play;
        StageManager.Instance.OnFailStage += Fail;
    }

    private void OnDisable()
    {
        StageManager.Instance.OnClearStage -= Play;
        StageManager.Instance.OnFailStage -= Fail;
    }
    private void Play()
    {
        StartCoroutine(PlaySequence());
    }

    private void Fail()
    {
        StartCoroutine(FailSequence());
    }

    private IEnumerator FailSequence()
    {
        yield return _failpopup.FadeInCoroutine();             
        yield return _failpopup.ToastCoroutine();
        SceneManager.LoadScene(3);
    }
    private IEnumerator PlaySequence()
    {
        yield return _popup.FadeInCoroutine();             // 1. FadeIn 완료 대기
        yield return _popup.ToastCoroutine();               // 2. Toast 애니메이션 완료 대기
        yield return _popup.StartCreditScrollCoroutine(); // 3. 크레딧 스크롤 완료 대기
    }



}
