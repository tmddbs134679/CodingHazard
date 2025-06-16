using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    public float CurProgressRatio { get; private set; }

    private string loadingScene = "LoadingScene";
    
    private float minLoadingTime = 4;
    
    private WaitForSeconds startLoadingTime = new (2);
    
    
    
    public void LoadScene(string name)
    {
        CurProgressRatio = 0;
 
        SceneManager.LoadScene(loadingScene);

        StartCoroutine(LoadingAsync(name));
    }

    
    IEnumerator LoadingAsync(string name)
    {
        yield return startLoadingTime;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        
        asyncOperation.allowSceneActivation = false;

        float timer = 0f;
        float fakeProgress = 0f;
    
        while (!asyncOperation.isDone)
        {
            timer += Time.deltaTime;

            float targetProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            fakeProgress = Mathf.MoveTowards(fakeProgress, targetProgress, Time.deltaTime);

            CurProgressRatio = fakeProgress;

            if (fakeProgress >= 0.9f && timer >= minLoadingTime)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
  
}
