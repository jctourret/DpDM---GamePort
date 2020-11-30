using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardObstacules : MonoBehaviour
{
    void Start()
    {
        if (!GameDifficulty.instance.hardModeOn)
        {
            Destroy(gameObject);
        }   
    }
}
