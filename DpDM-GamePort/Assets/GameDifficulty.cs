using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDifficulty : MonoBehaviour
{
    public static GameDifficulty instance;
    public bool hardModeOn = false;
    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void OnNormalButton()
    {
        hardModeOn = false;
    }
    public void OnHardButton()
    {
        hardModeOn = true;
    }
}
