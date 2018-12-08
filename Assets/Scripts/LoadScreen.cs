using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour {

    public Text text;
    bool loadingScene = false;


    void Update()
    {
        if (!loadingScene)
        {
            loadingScene = true;
            //Need to figure out how to send which game mode is selected
            StartCoroutine(loadNewScene());
        }

        if (loadingScene)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.PingPong(Time.time, 1));
        }
    }

    // what actually loads the scene
    IEnumerator loadNewScene()
    {
        yield return new WaitForSeconds(2);

        AsyncOperation async = SceneManager.LoadSceneAsync(2);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
