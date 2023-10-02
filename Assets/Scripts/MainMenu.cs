using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Dropdown mapDropdown;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingProgress;


    private List<string> allSceneNames;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        allSceneNames = new List<string>();
        /*int sceneCount = SceneManager.sceneCountInBuildSettings;

        // get scene names
        for (int i = 0; i < sceneCount; i++)
        {
            allSceneNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
        mapDropdown.AddOptions(allSceneNames); */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetDropdown();
        }
    }


    private void ResetDropdown()
    {
        mapDropdown.ClearOptions();
        allSceneNames = new List<string>();
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        // get scene names
        for (int i = 0; i < sceneCount; i++)
        {
            allSceneNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
        mapDropdown.AddOptions(allSceneNames);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    // on start button
    public void LoadSelectedScene()
    {
        string sceneName = mapDropdown.options[mapDropdown.value].text;
        StartCoroutine(LoadingManager(sceneName));
    }

    private IEnumerator LoadingManager(string scene)
    {
        loadingScreen.SetActive(true);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene);

        while (!loadOperation.isDone)
        {
            // loading bar
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingProgress.fillAmount = progress;

            yield return null;
        }
    }
}
