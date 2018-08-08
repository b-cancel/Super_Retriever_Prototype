using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartBtn : MonoBehaviour {

    // When "Start" btn is clicked, the game is loaded; change "mod" to correct name of scene.
    public void onReStartClick()
    {
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }
}
