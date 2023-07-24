using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Channel/SceneLoaderChannel")]
public class SceneLoaderChannel : ScriptableObject
{
    public void LoadScene(string SceneName) => OnSceneRequested?.Invoke(SceneName);
    public event Action<string> OnSceneRequested;
}
