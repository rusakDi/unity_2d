using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour {
    
    private Button infoButton;
    public GameObject panelInfo;
    
    void Start() {
        panelInfo.SetActive(false);
        infoButton = GetComponent<Button>();
        infoButton.onClick.AddListener(Info);
    }

    void Info() {
        GetComponent<AudioSource>().Play();
        panelInfo.SetActive(true);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<AudioSource>().Play();
            panelInfo.SetActive(false);
        }
    }
}
