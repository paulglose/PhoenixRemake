using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderEnabler : MonoBehaviour
{
    public void Start()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
