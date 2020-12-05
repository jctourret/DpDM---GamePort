using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Inventory : MonoBehaviour
{
    public Player player;
    public GameObject[] images;
    void Start()
    {
        
    }
    void Update()
    {
        switch (player.Bolasas.Count(x => x !=null))
        {
            case 0:
                images[0].SetActive(false);
                images[1].SetActive(false);
                images[2].SetActive(false);
                break;
            case 1:
                images[0].SetActive(true);
                images[1].SetActive(false);
                images[2].SetActive(false);
                break;
            case 2:
                images[1].SetActive(true);
                images[2].SetActive(false);
                break;
            case 3:
                images[2].SetActive(true);
                break;
        }
    }
}
