using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SceneLoaderChannel SceneLoaderChannel;
    [SerializeField] GameManagerChannel GameManagerChannel;

    void OnEnable() 
    {
        GameManagerChannel.OnConstantInitializeRequested += InitializeConstants;
    }

    void OnDisable() 
    {
        GameManagerChannel.OnConstantInitializeRequested -= InitializeConstants;
    }

    private void Start()
    {
        SceneLoaderChannel.LoadScene("MainMenu");
    }

    void Update() {}

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InitializeConstants()
    {
        IGameObjectConstant[] constants = FindObjectsOfType<MonoBehaviour>()
            .OfType<IGameObjectConstant>()
            .ToArray();

        foreach (IGameObjectConstant constant in constants)
        {
            constant.Initialize();
        }
    }
}
