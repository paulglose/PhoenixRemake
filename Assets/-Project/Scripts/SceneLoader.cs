using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] SceneLoaderChannel SceneLoaderChannel;
    [SerializeField] VoidEvent OnBattleSceneLoaded;
    [SerializeField] GameManagerChannel GameManagerChannel;
    Animator anim;

    [Header("Configurations")]
    [SerializeField] float TimeBetweenFinishedLoadingAndFadeOutStart;
    [SerializeField] float TimeBetweenFadeInAndLoadingStart;

    [Header("Other")]
    [SerializeField] GameObject PauseUI;
    [SerializeField] GameObject BattleUI;
    [SerializeField] GameObject MainMenuUI;

    void OnEnable()
    {
        SceneLoaderChannel.OnSceneRequested += (sceneName) => StartCoroutine(LoadScene(sceneName));
        SceneManager.sceneLoaded += (scene, sceneMode) => StartCoroutine(FadeOut(scene, sceneMode));
    }

    void OnDisable()
    {
        SceneLoaderChannel.OnSceneRequested -= (sceneName) => StartCoroutine(LoadScene(sceneName));
        SceneManager.sceneLoaded -= (scene, sceneMode) => StartCoroutine(FadeOut(scene, sceneMode));
    }

    void Awake() =>
        anim = GetComponent<Animator>();

    string previousScene;
    IEnumerator LoadScene(string sceneName)
    {
        anim.CrossFade("Show", 0);

        yield return new WaitForSecondsRealtime(TimeBetweenFadeInAndLoadingStart);
        
        // If existing
        if (!string.IsNullOrEmpty(previousScene))
            SceneManager.UnloadSceneAsync(previousScene);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        DisableAllUI();
        EnableSceneSpecificUI(sceneName);
        previousScene = sceneName;
    }

    IEnumerator FadeOut(Scene scene, LoadSceneMode scenemode)
    {
        SceneManager.SetActiveScene(scene);
        GameManagerChannel.RequestConstantInitialization();
        yield return new WaitForSecondsRealtime(TimeBetweenFinishedLoadingAndFadeOutStart);
        if (scene.name == previousScene)
        {
            anim.CrossFade("Hide", 0);
        }
    }

    void DisableAllUI()
    {
        BattleUI.SetActive(false);
        PauseUI.SetActive(false);
        MainMenuUI.SetActive(false);
    }

    void EnableSceneSpecificUI(string sceneName)
    {
        switch (sceneName)
        {
            case "BattleScene":
                BattleUI.SetActive(true);
                PauseUI.SetActive(true);
                OnBattleSceneLoaded.RaiseEvent();
                break;
            case "MainMenu":
                MainMenuUI.SetActive(true);
                break;
        }
    }
}