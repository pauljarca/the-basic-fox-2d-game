using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI pointsText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        
        pointsText.text = score.ToString() + " DIAMONDS";

    }
    public void PlayGame()
    {
        Time.timeScale = 1f;
        PermanentUI.perm.GameOverReset();
        SceneManager.LoadScene(1);
    }
}
