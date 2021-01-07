using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {
    Loader loader;
    private Button exitButton;
    
    void Start() {
        exitButton = GetComponent <Button> ();
        exitButton.onClick.AddListener (Play);
    }

    void Play(){
        GetComponent <AudioSource>().Play();
        SceneManager.LoadScene("Main");
    }
}
