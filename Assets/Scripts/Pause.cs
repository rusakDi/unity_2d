using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {
    private Button infoButton;
    public GameObject pausePanel;

    void Start() {
        pausePanel.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<AudioSource>().Play();
            pausePanel.SetActive(true);
        }
    }
}
