using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIManager : MonoBehaviour, IGameObjectConstant
{
    [Header("Channel")]
    [SerializeField] HealthUIChannel HealthChannel;

    [Header("Others")]
    [SerializeField] Animator HealthContainer;

    List<Animator> HealthContainers = new List<Animator>();

    public void Initialize()
    {
        // Destroy all existing Health Containers
        while(HealthContainers.Count > 0)
        {
            Animator objectToRemove = HealthContainers[HealthContainers.Count - 1];
            HealthContainers.Remove(objectToRemove);
            Destroy(objectToRemove.gameObject);
        }
    }

    private void OnEnable()
    {
        HealthChannel.OnHealthRegistered += OnHealthRegistered;
    }

    private void OnDisable()
    {
        HealthChannel.OnHealthRegistered -= OnHealthRegistered;
    }

    void OnHealthRegistered(int current, int max)
    {
        // If max not equal to the currently avaible spawn, or destroy remaining
        while (HealthContainers.Count < max)
        {
            HealthContainers.Add(Instantiate(HealthContainer, transform));
        }

        while(HealthContainers.Count > max)
        {
            Destroy(HealthContainers[HealthContainers.Count - 1].gameObject);
            HealthContainers.RemoveAt(HealthContainers.Count - 1);
        }

        for(int i = 0; i < current; i++)
        {
            if (!HealthContainers[i].GetCurrentAnimatorStateInfo(0).IsName("Filled"))
            {
                HealthContainers[i].CrossFade("Filled", 0f);
            }
        }

        for(int i = current; i < max; i++)
        {
            if (!HealthContainers[i].GetCurrentAnimatorStateInfo(0).IsName("Emptied"))
            {
                HealthContainers[i].CrossFade("Emptied", 0f);
            }
        }
    }
}
