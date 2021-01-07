using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
    public void RestartLevel() {
        var gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
        gameManager.RestartLevel();
    }
}