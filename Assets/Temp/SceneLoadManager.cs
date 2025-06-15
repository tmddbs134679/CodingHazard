using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private float time = 0;
    
    public void LoadScene(string name)
    {
        StartCoroutine(LoadingAsync(name));
    }

    IEnumerator LoadingAsync(string name)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        
        asyncOperation.allowSceneActivation = false; 
        
        while(!asyncOperation.isDone)
        { 
            time += Time.deltaTime; 
            
            print(asyncOperation.progress); 
           
            if(time > Constants.MinLoadingTime){ 
                asyncOperation.allowSceneActivation = true; 
            }
            yield return null;
        }
    }
}
