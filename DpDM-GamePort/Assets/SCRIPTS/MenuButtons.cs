﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public void onSinglePlayerButton()
    {
        SceneManager.LoadScene("conduccion10");
    }
    public void OnMultiplayerButton()
    {
        SceneManager.LoadScene("conduccion9");
    }
    public void onQuitButton()
    {
        Application.Quit();
    }
}
