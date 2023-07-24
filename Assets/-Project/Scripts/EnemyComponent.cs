using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    public EnemyConfiguration Configuration;
    [HideInInspector] public Enemy Enemy;

    protected virtual void OnEnable() {}

    protected virtual void OnDisable() {}

    protected virtual void Awake() {}

    protected virtual void Start()
    {
        Enemy = GetComponent<Enemy>();
        Configuration = Enemy.Configuration;
    }
    
    protected virtual void Update() {}
}
