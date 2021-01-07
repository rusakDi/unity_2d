using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour {
    
    private Button continueButton;
    public GameObject pausePanel;
    
    void Start() {
        continueButton = GetComponent <Button> ();
        continueButton.onClick.AddListener (Quit);
    }

    void Quit() {
        GetComponent <AudioSource>().Play ();
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
