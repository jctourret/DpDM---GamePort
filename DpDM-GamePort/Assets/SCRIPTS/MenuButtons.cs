using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("conduccion9");
    }
    public void onQuitButton()
    {
        Application.Quit();
    }
}
