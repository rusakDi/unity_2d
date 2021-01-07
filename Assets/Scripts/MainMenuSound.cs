using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSound : MonoBehaviour {
    void Start() {
        GetComponent<AudioSource>().Play();
    }
}
