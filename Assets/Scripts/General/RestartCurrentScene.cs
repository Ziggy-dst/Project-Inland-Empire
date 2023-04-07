using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityAtoms.FSM;

public class RestartCurrentScene : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(3)) RestartScene();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
