using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    public Player player;
    Text text;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    private void Update()
    {
        text.text = "$"+player.Dinero.ToString();
    }
}
