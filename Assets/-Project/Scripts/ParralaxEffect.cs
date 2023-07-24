using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxEffect : MonoBehaviour
{
    [SerializeField] float ScrollSpeed;
    [SerializeField] Vector2 ScrollRange;

    private void Update()
    {
        transform.position -= new Vector3(ScrollSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x < ScrollRange.x) transform.position = new Vector3(ScrollRange.y, 0, 0);
    }
}
