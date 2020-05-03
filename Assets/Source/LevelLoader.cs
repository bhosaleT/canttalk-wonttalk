using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    [SerializeField]
    Animator transitionAnimator;

    [SerializeField]
    float transitionTime = 1f;

    // Load next scene
    public void LoadMapScene(string sceneToLoad) {
        StartCoroutine(ActuallyLoadScene(sceneToLoad));
    }

    IEnumerator ActuallyLoadScene(string sceneName) {
        transitionAnimator.SetTrigger("OnStart");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }

}