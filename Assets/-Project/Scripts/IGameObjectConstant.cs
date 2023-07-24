using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an interface for GameObjects that are consistent through multeple scenes, and need to be initialized at some point
/// </summary>
public interface IGameObjectConstant
{
    public void Initialize();
}
