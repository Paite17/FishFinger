using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScoreLoader : MonoBehaviour
{
    public float highScore;
    [SerializeField] private Text highScoreText;
    [SerializeField] private GameObject eggstraMenu;
    public bool eggstraWork;
    public float[] eggstraSeed;
    public bool endlessMode;

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

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            eggstraWork = false;
            highScoreText.text = "Highscore: " + highScore.ToString("F0");
        }
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        highScore = data.highScore;
        eggstraWork = data.eggstraWork;
        eggstraSeed = data.gameSeed;
        endlessMode = data.endlessMode;
    }

    public void SaveData()
    {
        SaveSystem.SaveGame(this);
    }

    public void EggstraWorkToggle(bool toggle)
    {
        eggstraWork = toggle;
        eggstraMenu.SetActive(toggle);
    }
}
