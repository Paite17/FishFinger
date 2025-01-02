using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    INTRO,
    PRE_WAVE,
    ACTIVE_WAVE,
    POST_WAVE,
    WIN,
    RESULTS
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // public modifiers
    [SerializeField] private MapData mapData_;
    [SerializeField] private List<Gun> gunPool;
    public float difficulty;
    public float[] gameSeed;
    public int maxNumberOfWaves;
    public int amountOfTimePerWave;
    public GameState currentState;
    public bool[] waveComplete;
    public bool xtraWave;
    public bool eggstraWork;
    public int smellStage;
    public bool endlessMode;
    public bool debugMode;    // temp thing just cus
    
    // game
    public int currentWave;
    public float currentWaveTimer;
    public float preWaveTimer;
    
    private bool tenSecondsLeft;

    // spawners (make sure these are set per map)
    // used for activating after wave starts
    // and deactivating after a wave briefly 
    [SerializeField] private List<EntitySpawner> spawners;

    // ui ref
    [SerializeField] private UIScript ui;

    // camera animator stuff
    [SerializeField] private Animator camAnimator;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject introCamera;
    [SerializeField] private GameObject resultsCam;
    [SerializeField] private Animator introCamAnim;

    public float[] totalScorePerWave;

    private Player plr;

    public MapData MapData_
    {
        get { return mapData_; }
    }

    public List<Gun> GunPool
    {
        get { return gunPool; }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // this will not fucking work if i add multiplayer
        plr = FindObjectOfType<Player>();
        int randomGunDecide = Random.Range(0, gunPool.Count);
        Debug.Log(randomGunDecide);
        WeaponSetUp(randomGunDecide);
        ui.GetComponent<MainMenuScoreLoader>().LoadData();
        eggstraWork = ui.GetComponent<MainMenuScoreLoader>().eggstraWork;
        endlessMode = ui.GetComponent<MainMenuScoreLoader>().endlessMode;

        if (spawners[0] == null)
        {
            spawners.Clear();
            FindAllSpawners();
        }

        if (!eggstraWork)
        {
            gameSeed = new float[9];
        }
        else
        {
            gameSeed = new float[15];
        }
       

        if (!eggstraWork)
        {
            if (!debugMode)
            {
                GenerateNewSeed();
            }            
        }
        else
        {
            LoadSeed();
            xtraWave = false;
            maxNumberOfWaves = 5;
        }
        
       

        currentWave = 1;
        preWaveTimer = 30;

        if (xtraWave)
        {
            maxNumberOfWaves = 4;
        }

        if (endlessMode)
        {
            maxNumberOfWaves = 999;
        }

        totalScorePerWave = new float[maxNumberOfWaves];
        waveComplete = new bool[maxNumberOfWaves];
        StartCoroutine(GameIntro());

    }

    public void IntroCorrection()
    {
        // intro cam
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        if (introCamAnim.isActiveAndEnabled)
        {
            switch (currentScene.name)
            {
                case "sr_spawninggrounds":
                    introCamAnim.Play("camIntro");
                    break;
                case "sr_fishfingerunderpass":
                    introCamAnim.Play("ffCamIntro");
                    break;
                case "sr_smokedsalmonspire":
                    introCamAnim.Play("SSSIntro");
                    break;
                default:
                    introCamAnim.Play("camIntro");
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.PRE_WAVE:
                PreWaveCounter();
                break;
            case GameState.ACTIVE_WAVE:
                ActiveWaveCountdown();
                break;
        }
    }

    // get spawners for if i forget to add them manually
    private void FindAllSpawners()
    {
        var spawner = FindObjectsOfType<EntitySpawner>();

        foreach (var current in spawner)
        {
            spawners.Add(current);
        }
    }

    /// <summary>
    /// Activates the correct weapon on the player, while deactivating all others
    /// </summary>
    private void WeaponSetUp(int newWep)
    {
        foreach (var current in gunPool)
        {
            if (current != gunPool[newWep])
            {
                current.gameObject.SetActive(false);
            }
        }

        gunPool[newWep].gameObject.SetActive(true);
        plr.CurrentlyEquippedGun = gunPool[newWep];
        FindObjectOfType<PlayerShoot>().SetGunShoot(gunPool[newWep]);
        UIScript.Instance.GunCorrection(gunPool[newWep]);
    }

    // generate a seed
    private void GenerateNewSeed()
    {
        // prolly a better Random fucntion out there for this sort of thing but eh
        // i'm doing this with random guesses and no google searches
        // indexes 0, 3 and 6 should represent how many enemies will spawn
        gameSeed[0] = Random.Range(2, 9);
        gameSeed[3] = Random.Range(2, 9);
        gameSeed[6] = Random.Range(2, 9);
        // indexes 1, 4 and 7 should represent what number the spawner will need in order to spawn enemies that pulse
        gameSeed[1] = Random.Range(1, 20);
        gameSeed[4] = Random.Range(1, 20);
        gameSeed[7] = Random.Range(1, 20);
        // indexes 2, 5 and 8 represent the amount of time between spawn attempts
        gameSeed[2] = Random.Range(3, 12);
        gameSeed[5] = Random.Range(3, 12);
        gameSeed[8] = Random.Range(3, 12);

        Debug.Log("Seed: " + gameSeed[0] + gameSeed[1] + gameSeed[2] + gameSeed[3] + gameSeed[4] + gameSeed[5] + gameSeed[6] + gameSeed[7] + gameSeed[8]);
    }

    // when in eggstra work mode you use a custom seed
    private void LoadSeed()
    {
        for (int i = 0; i < gameSeed.Length; i++)
        {
            ui.GetComponent<MainMenuScoreLoader>().LoadData();
            gameSeed[i] = ui.GetComponent<MainMenuScoreLoader>().eggstraSeed[i];
        }
    }

    // setting the values of the spawners to that of the seed
    private void SetSpawnerValues()
    {

        // activate spawners
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].AmountOfEnemiesPerPulse = Random.Range(2, 9);
            spawners[i].SpawnerDifficulty = Random.Range(1, 20);
            spawners[i].TimeBetweenPulses = Random.Range(3, 12);
            /*if (!endlessMode)
            {
                switch (currentWave)
                {
                    case 1:
                        spawners[i].AmountOfEnemiesPerPulse = gameSeed[0];
                        spawners[i].SpawnerDifficulty = gameSeed[1];
                        spawners[i].TimeBetweenPulses = gameSeed[2];
                        break;
                    case 2:
                        spawners[i].AmountOfEnemiesPerPulse = gameSeed[3];
                        spawners[i].SpawnerDifficulty = gameSeed[4];
                        spawners[i].TimeBetweenPulses = gameSeed[5];
                        break;
                    case 3:
                        spawners[i].AmountOfEnemiesPerPulse = gameSeed[6];
                        spawners[i].SpawnerDifficulty = gameSeed[7];
                        spawners[i].TimeBetweenPulses = gameSeed[8];
                        break;
                    case 4:
                        spawners[i].AmountOfEnemiesPerPulse = gameSeed[9];
                        spawners[i].SpawnerDifficulty = gameSeed[10];
                        spawners[i].TimeBetweenPulses = gameSeed[11];
                        break;
                    case 5:
                        spawners[i].AmountOfEnemiesPerPulse = gameSeed[12];
                        spawners[i].SpawnerDifficulty = gameSeed[13];
                        spawners[i].TimeBetweenPulses = gameSeed[14];
                        break;
                }
            }
            else
            {
                spawners[i].AmountOfEnemiesPerPulse = Random.Range(2, 9);
                spawners[i].SpawnerDifficulty = Random.Range(1, 20);
                spawners[i].TimeBetweenPulses = Random.Range(3, 12);
            } */
            

            // activate spawners
            if (currentState != GameState.RESULTS)
            {
                spawners[i].Activated = true;
            }
        }
    }

    public void DeactivateSpawners()
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            spawners[i].Activated = false;
        }
    }

    private void PreWaveCounter()
    {
        preWaveTimer -= Time.deltaTime;

        if (preWaveTimer < 0.9)
        {
            WaveStart();
            preWaveTimer = 10;
        }
    }

    private void EndOfWave()
    {
        currentState = GameState.POST_WAVE;
        ui.EndOfWave();
        // why was this hardcoded to 3??????
        if (currentWave != maxNumberOfWaves)
        {
            StartCoroutine(EndOfWaveLogic());
        }
        else
        {
            currentState = GameState.WIN;
            StartCoroutine(FinalWaveCleared());
        }
        
    }

    private IEnumerator EndOfWaveLogic()
    {
        if (currentWave < maxNumberOfWaves)
        {
            DeactivateSpawners();
            bool d = false;
            // restart timers
            currentWaveTimer = amountOfTimePerWave;
            tenSecondsLeft = false;
            // save current score to array
            totalScorePerWave[currentWave - 1] = GameObject.Find("Player").GetComponent<Player>().PlayerScore;
            // rest currentScore variable
            GameObject.Find("Player").GetComponent<Player>().PlayerScore = 0;
            GameObject.Find("Player").GetComponent<Player>().PlayerHealth = 100;
            // set currentWave's completion to true
            waveComplete[currentWave - 1] = true;
            // add 1 to currentWave
            if (!d)
            {
                currentWave++;
                d = true;
            }

            DespawnAllEnemies();

            yield return new WaitForSeconds(5f);

            // start countdown again
            currentState = GameState.PRE_WAVE;
            int randomGunDecide = Random.Range(0, gunPool.Count + 1);
            WeaponSetUp(randomGunDecide);
        }
    }

    private IEnumerator FinalWaveCleared()
    {
        DeactivateSpawners();
        currentState = GameState.POST_WAVE;
        // save current score to array
        totalScorePerWave[currentWave - 1] = GameObject.Find("Player").GetComponent<Player>().PlayerScore;
        DespawnAllEnemies();
        // set currentWave's completion to true
        waveComplete[currentWave - 1] = true;
        yield return new WaitForSeconds(5f);
        currentState = GameState.RESULTS;
        // results screen time
        ui.InitialiseResultsScreen();
        mainCamera.SetActive(false);
        resultsCam.SetActive(true);
        
    }

    private void ActiveWaveCountdown()
    {
        currentWaveTimer -= Time.deltaTime;

        if (currentWaveTimer <= 10)
        {
            if (!tenSecondsLeft)
            {
                tenSecondsLeft = true;
                ui.StartEndOfWaveCountdown();
            }
        }

        if (currentWaveTimer < 0.9)
        {
            EndOfWave();
        }
    }

    // setting everything for starting the wave
    private void WaveStart()
    {
        currentState = GameState.ACTIVE_WAVE;
        SetSpawnerValues();

        // ui anim coroutine
        ui.WaveStart();

        // set timers
        currentWaveTimer = amountOfTimePerWave;
    }

    // put the intro coroutine here???
    private IEnumerator GameIntro()
    {
        // set cam to intro cam
        mainCamera.SetActive(false);
        introCamera.SetActive(true);
        IntroCorrection();

        yield return new WaitForSeconds(12f);

        introCamera.SetActive(false);
        mainCamera.SetActive(true);
        ui.StartPreWaveCountdown();
        currentState = GameState.PRE_WAVE;
    }

    // basically the same as whatever will take you to the results screen but called prematurely
    public void GameOver()
    {
        StartCoroutine(GameOverWithDelay());
    }

    private IEnumerator GameOverWithDelay()
    {
        Debug.Log("guy died");
        ui.PlayerDies();
        // arrays are hard (originally didn't have the "- 1" bit)
        totalScorePerWave[currentWave - 1] = GameObject.Find("Player").GetComponent<Player>().PlayerScore;

        yield return new WaitForSeconds(2f);

        ui.InitialiseResultsScreen();
    }

    public void SetUpResultsLogic()
    {
        // cameras
        mainCamera.SetActive(false);
        resultsCam.SetActive(true);
        currentState = GameState.RESULTS;
        DespawnAllEnemies();
        DeactivateSpawners();
        //totalScorePerWave[currentWave - 1] = GameObject.Find("Player").GetComponent<Player>().PlayerScore;
    }

    // after wave/game over
    private void DespawnAllEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var current in enemies)
        {
            Destroy(current);
        }
    }
}
