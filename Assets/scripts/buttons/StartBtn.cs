using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// NOTE: I originally intended this script for just the start btn, but more btns were needed so I included them here

public class StartBtn : MonoBehaviour {

	// When "Start" btn is clicked, the game is loaded; change "mod" to correct name of scene.
	public void onStartClick(){
		SceneManager.LoadScene("OFFICIAL_DEMO", LoadSceneMode.Single);
	}
}
