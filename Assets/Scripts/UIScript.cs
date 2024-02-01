using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Text waveText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeLeft;
    [SerializeField] private Text preWaveCountdown;
    [SerializeField] private Text endOfWaveCountdown;
    [SerializeField] private Text waveClear;
    [SerializeField] private GameObject waveStartLabel;
    [SerializeField] private Text waveStartsInText;
    [SerializeField] private GameObject sideBarStatus;
    [SerializeField] private GameObject healthSideBar;
    [SerializeField] private Text introMapName;
    [SerializeField] private GameObject blackFadeOut;
    [SerializeField] private GameObject whiteFade;
    [SerializeField] private Text healthLabel;

    [SerializeField] private GameManager gm;

    private bool timeRunningOut;

    // ResultsScreen
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private GameObject waveTagObj;
    [SerializeField] private GameObject eggstraWaveTagObj;
    [SerializeField] private List<GameObject> waveTick;
    [SerializeField] private List<GameObject> eggstraWaveTick;
    [SerializeField] private List<Text> waveTagScoreLabel;
    [SerializeField] private List<Text> eggstraTickScoreLabel;
    [SerializeField] private Text totalScore;
    [SerializeField] private GameObject newHighScoreLabel;
    [SerializeField] private List<GameObject> waveTags;
    [SerializeField] private List<GameObject> eggstraWaveTags;
    [SerializeField] private GameObject FadeIn;

    //gameover
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject youDiedText;
    [SerializeField] private Animator youDiedAnim;
    [SerializeField] private GameObject fadeInBackgroundGameOver;

    [SerializeField] private GameObject crosshair;
    [SerializeField] private Image cooldownIndicator;

    private Gun currentGun;

    private Player player;

    private bool fuck;


    private void Awake()
    {
        blackFadeOut.SetActive(true);
        introMapName.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGun = FindObjectOfType<Gun>();
        crosshair.SetActive(false);
        StartCoroutine(IntroFade());
        UpdateSidebarWave();

        player = GameObject.Find("Player").GetComponent<Player>();
        
        // Get map name
        Scene currentScene = SceneManager.GetActiveScene();

        switch (currentScene.name)
        {
            case "sr_spawninggrounds":
                introMapName.text = "Spawning Grounds";
                break;
            case "sr_smokedsalmonspire":
                introMapName.text = "Smoked Salmon Spire";
                break;
            case "sr_fishfingerunderpass":
                introMapName.text = "Fish Finger Underpass";
                break;
            default:
                // if i don't make a specific case for a map then use this
                introMapName.text = currentScene.name;
                break;
        }

    }

    private IEnumerator IntroFade()
    {
        yield return new WaitForSeconds(11.5f);
        whiteFade.SetActive(true);
        introMapName.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.currentState == GameState.PRE_WAVE)
        {
            string s_ = gm.preWaveTimer.ToString("F0");
            preWaveCountdown.text = s_; ;
        }

        if (timeRunningOut && gm.currentState == GameState.ACTIVE_WAVE)
        {
            string s = gm.currentWaveTimer.ToString("F0");
            endOfWaveCountdown.text = s;
        }

        if (gm.currentState == GameState.ACTIVE_WAVE)
        {
            string _s = gm.currentWaveTimer.ToString("F0");
            timeLeft.text = _s;
        }

        if (gm.currentWaveTimer <= 15 && gm.currentState == GameState.ACTIVE_WAVE)
        {
            timeLeft.color = new Color32(255, 234, 0, 255);
        }
        else
        {
            timeLeft.color = new Color32(255, 255, 255, 255);
        }

        healthLabel.text = player.PlayerHealth.ToString();
        cooldownIndicator.fillAmount = currentGun.cooldownTimer;

        if (gm.xtraWave)
        {
            if (gm.currentWave == 4)
            {
                return;
            }
            else
            {
                scoreText.text = player.PlayerScore.ToString("F0");
            }
        }
        else
        {
            scoreText.text = player.PlayerScore.ToString("F0");
        }


        // i hate
        if (gm.currentState == GameState.RESULTS)
        {
            waveStartsInText.gameObject.SetActive(false);
            preWaveCountdown.gameObject.SetActive(false);
            crosshair.gameObject.SetActive(false);
        }

    }

    // update the sidebars wave number
    private void UpdateSidebarWave()
    {
        // setup text lables now
        Debug.Log(gm.currentWave);
        if (gm.currentWave != gm.maxNumberOfWaves)
        {
            waveStartsInText.text = "Wave " + gm.currentWave + " starts in";
        }
        else
        {
            waveStartsInText.text = "Final wave starts in";
        }

        
        waveText.text = "Wave " + gm.currentWave;
        if (gm.currentWave == 4 && gm.xtraWave)
        {
            waveText.text = "XTRAWAVE";
            scoreText.text = "Defeat the Giant Boss!";
        }

        timeLeft.text = "" + gm.amountOfTimePerWave;
    }

    // UI stuff at the start of the wave
    public void WaveStart()
    {
        StartCoroutine(WaveStartAnim());
    }

    private IEnumerator WaveStartAnim()
    {
        preWaveCountdown.gameObject.SetActive(false);
        waveStartsInText.gameObject.SetActive(false);
        waveStartLabel.SetActive(true);
        sideBarStatus.SetActive(true);
        healthSideBar.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        waveStartLabel.SetActive(false);
    }

    public void StartPreWaveCountdown()
    {
        if (gm.currentState != GameState.WIN)
        {
            crosshair.SetActive(true);
            introMapName.gameObject.SetActive(false);
            preWaveCountdown.gameObject.SetActive(true);
            waveStartsInText.gameObject.SetActive(true);
        }
    }

    // the ten second countdown at the end of a wave
    public void StartEndOfWaveCountdown()
    {
        
        timeRunningOut = true;
        endOfWaveCountdown.gameObject.SetActive(true);
    }

    public void EndOfWave()
    {
        // show UI
        endOfWaveCountdown.gameObject.SetActive(false);

        if (gm.currentWave != gm.maxNumberOfWaves)
        {
            waveClear.text = "Wave " + gm.currentWave + " clear!";
        }
        else
        {
            waveClear.text = "Final wave clear!";
        }
        waveClear.gameObject.SetActive(true);
        sideBarStatus.SetActive(false);
        healthSideBar.SetActive(false);
        StartCoroutine(WaveClearDisappear());
    }

    private IEnumerator WaveClearDisappear()
    {
        yield return new WaitForSeconds(1.5f);

        // reset hud
        waveClear.gameObject.SetActive(false);
        UpdateSidebarWave();
        timeRunningOut = false;

        yield return new WaitForSeconds(3.5f);
        StartPreWaveCountdown();
    }

    // load this right upon dying/winning to get the deets
    public void InitialiseResultsScreen()
    {

        int wavesCompleted = 0;
        // find out how many waves were beat
        for (int i = 0; i < gm.waveComplete.Length; i++)
        {
            if (gm.waveComplete[i])
            {
                wavesCompleted++;
            }
        }

        Debug.Log("Waves completed: " + wavesCompleted);

        // score text on a tag
        for (int i = 0; i < wavesCompleted; i++)
        {
            Debug.Log("i = " + i);
            waveTagScoreLabel[i].text = gm.totalScorePerWave[i].ToString("F0");

            if (gm.xtraWave)
            {
                // replace this with a check on the name of the boss
                waveTagScoreLabel[3].text = "Robozuna";
            }

            if (gm.eggstraWork)
            {
                eggstraTickScoreLabel[i].text = gm.totalScorePerWave[i].ToString("F0");
            }
        }

        // for when you haven't completed any wave
        if (wavesCompleted == 0)
        {
            waveTagScoreLabel[0].text = gm.totalScorePerWave[0].ToString("F0");
        }
       
        //float fullScore = gm.totalScorePerWave[0] + gm.totalScorePerWave[1] + gm.totalScorePerWave[2];
        float fullScore = GetAllScores();
        
        
        totalScore.text = fullScore.ToString("F0");

        sideBarStatus.SetActive(false);
        healthSideBar.SetActive(false);
        StartCoroutine(ResultsScreenLogic());
    }

    // get the full score of the current attempt
    private float GetAllScores()
    {
        float stored = 0;
        for (int i = 0; i < gm.totalScorePerWave.Length; i++)
        {
            stored += gm.totalScorePerWave[i];
        }

        return stored;
    }

    // i hate everything
    private void WaveTagPosCorrection()
    {
        if (!fuck)
        {
            Debug.LogWarning("change pos please :)))))))");
            if (gm.currentWave == 1)
            {
                waveTagObj.GetComponent<RectTransform>().position = new Vector3(waveTagObj.transform.position.x + 725, waveTagObj.transform.position.y, waveTagObj.transform.position.z);
            }

            if (gm.currentWave == 2)
            {
                waveTagObj.GetComponent<RectTransform>().position = new Vector3(waveTagObj.transform.position.x + 475, waveTagObj.transform.position.y, waveTagObj.transform.position.z);
            }

            if (gm.currentWave == 3)
            {
                waveTagObj.GetComponent<RectTransform>().position = new Vector3(waveTagObj.transform.position.x + 234, waveTagObj.transform.position.y, waveTagObj.transform.position.z);
            }

            if (gm.currentWave == 4 && gm.xtraWave)
            {
                waveTagObj.GetComponent<RectTransform>().position = new Vector3(waveTagObj.transform.position.x + 0, waveTagObj.transform.position.y, waveTagObj.transform.position.z);
            }

            fuck = true;
        }


    }

    private IEnumerator ResultsScreenLogic()
    {
        // get some other stuff outta here
        gm.DeactivateSpawners();
        crosshair.SetActive(false);
        cooldownIndicator.gameObject.SetActive(false);
        waveStartsInText.gameObject.SetActive(false);
        preWaveCountdown.gameObject.SetActive(false);
        endOfWaveCountdown.gameObject.SetActive(false);
        WaveTagPosCorrection();
        Debug.Log("ResultsScreenLogic() started!");

        // new high score check
        // WHY WAS I USING THE VALUE FROM THE FUCKING STRING????  AND NOT THE ACTUAL FLOAT
        if (GetAllScores() >= GetHighScore())
        {
            newHighScoreLabel.SetActive(true);
            SaveScore(GetAllScores());
        }

        resultsScreen.SetActive(true);

        // spawn the funnies
        yield return new WaitForSeconds(0.2f);
        
        // eggstrawork check 
        if (!gm.eggstraWork)
        {

            waveTags[0].SetActive(true);

            if (gm.currentWave > 1)
            {
                yield return new WaitForSeconds(0.5f);
                waveTags[1].SetActive(true);

            }

            if (gm.currentWave > 2)
            {            
                yield return new WaitForSeconds(0.5f);
                waveTags[2].SetActive(true);
            }

            if (gm.currentWave > 4 && gm.xtraWave)
            {                
                yield return new WaitForSeconds(0.5f);
                waveTags[3].SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);

            // check what was complete
            for (int i = 0; i < gm.waveComplete.Length; i++)
            {
                if (gm.waveComplete[i])
                {
                    waveTick[i].SetActive(true);
                    waveTick[i].GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                }
                else
                {
                    waveTick[i].SetActive(true);
                    waveTick[i].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
            }


            yield return new WaitForSeconds(3f);

            // Quit to menu
            FadeIn.SetActive(true);

            yield return new WaitForSeconds(3f);


            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            eggstraWaveTags[0].SetActive(true);

            if (gm.currentWave > 1)
            {
                yield return new WaitForSeconds(0.5f);
                eggstraWaveTags[1].SetActive(true);

            }

            if (gm.currentWave > 2)
            {
                yield return new WaitForSeconds(0.5f);
                eggstraWaveTags[2].SetActive(true);
            }

            if (gm.currentWave > 4)
            {
                yield return new WaitForSeconds(0.5f);
                eggstraWaveTags[3].SetActive(true);
            }

            if (gm.currentWave >= 5)
            {
                yield return new WaitForSeconds(0.5f);
                eggstraWaveTags[4].SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);

            // check what was complete
            // (bad) (terrible) (delete)
            for (int i = 0; i < gm.waveComplete.Length; i++)
            {
                if (gm.waveComplete[i])
                {
                    eggstraWaveTick[i].SetActive(true);
                    eggstraWaveTick[i].GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                }
                else
                {
                    eggstraWaveTick[i].SetActive(true);
                    eggstraWaveTick[i].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
            }


            yield return new WaitForSeconds(3f);

            // Quit to menu
            FadeIn.SetActive(true);

            yield return new WaitForSeconds(3f);


            SceneManager.LoadScene("MainMenu");
        }


       
    }

    private void SaveScore(float score)
    {
        player.PlayerScore = score;
        SaveSystem.SaveGame(player);
    }

    private float GetHighScore()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        return data.highScore;
    }

    public void PlayerDies()
    {
        StartCoroutine(PlayerDeathUI());
    }

    private IEnumerator PlayerDeathUI()
    {
        // make gameover screen visible
        gameOverScreen.SetActive(true);
        // make text visible
        youDiedText.SetActive(true);
        // wait for animation and slightly more
        yield return new WaitForSeconds(0.8f);
        // fade background to black
        fadeInBackgroundGameOver.SetActive(true);
        // fade text
        yield return new WaitForSeconds(0.5f);

        youDiedAnim.SetBool("Fade", true);
        // activate results UI.

        yield return new WaitForSeconds(0.5f);

        fadeInBackgroundGameOver.SetActive(false);
        gm.SetUpResultsLogic();
        InitialiseResultsScreen();
        
    }
}
