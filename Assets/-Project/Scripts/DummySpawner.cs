using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    [SerializeField] Enemy Dummy;
    [SerializeField] int EnemyCount;

    void OnEnable() 
    { }

    void OnDisable() 
    { }

    void Awake() 
    { }

    void Start() 
    {
        for(int i = 0; i < EnemyCount; i++)
            Instantiate(Dummy, new Vector3(30, 0), Quaternion.identity).AsDummy();
    }

    void Update() 
    { }
}
