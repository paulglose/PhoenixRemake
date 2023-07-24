using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UpgradeEntry))]
public class UpgradeEntryScaleHandler : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader;
    
    [Header("Configuration")]
    [SerializeField] float HoveredScaleSpeed;
    [SerializeField] float HoveredScale;


    ButtonPhase currentCardPhase;

    bool hovered;
    private void OnEnable()
    {
        GetComponent<UpgradeEntry>().OnCardPhaseChanged += (cardPhase) => currentCardPhase = cardPhase;
    }

    private void OnDisable()
    {
        GetComponent<UpgradeEntry>().OnCardPhaseChanged += (cardPhase) => currentCardPhase = cardPhase;
    }

    private void Update()
    {
        if (currentCardPhase == ButtonPhase.Hovered) 
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(HoveredScale, HoveredScale, 0), HoveredScaleSpeed * Time.deltaTime);

        else if (currentCardPhase == ButtonPhase.Idle) transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 0), HoveredScaleSpeed * Time.deltaTime);
    }
}
