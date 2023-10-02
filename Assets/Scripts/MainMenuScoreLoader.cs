using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScoreLoader : MonoBehaviour
{
    public float highScore;
    [SerializeField] private Text highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        if (!SaveSystem.DoesPlayerFileExist())
        {
            SaveSystem.CreatePlayerFile(this);
        }
        else
        {
            LoadData();
        }

        highScoreText.text = "Highscore: " + highScore.ToString("F0");
    }

    private void LoadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        highScore = data.highScore;
    }
}
