using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public Button playButton;
    public Button continueButton;

    public GameObject panelCarta;

    private void Start() {
        playButton.Select();
    }

    public void StartGame() {
        StartCoroutine(LoadAsyncScene("escenafinal"));
    }

    private IEnumerator LoadAsyncScene(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowCard() {
        panelCarta.SetActive(true);
        continueButton.Select();
    }
}
