using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PermanentUI : MonoBehaviour
{
    //player stats
    public int diamond = 0;
    public int health = 5;
    public TextMeshProUGUI diamondNumber;
    public TextMeshProUGUI healthAmount;

    public static PermanentUI perm;
    
    private void Start()
    {
        if(SceneManager.GetActiveScene().name=="Menu")
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
        if (!perm)
            perm = this;
        else Destroy(gameObject);
    }

    public void Reset()
    {
        diamond = 0;
        diamondNumber.text = diamond.ToString();
        health -= 1;
        healthAmount.text = health.ToString();
    }
    
    public void GameOverReset()
    {
        diamond = 0;
        diamondNumber.text = diamond.ToString();
        health = 5;
        healthAmount.text = health.ToString();
    }

}
