using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] SceneLoaderChannel SceneLoadChannel;

    private void Start()
    {
        SceneLoadChannel.LoadScene("MainMenu");
    }
}
