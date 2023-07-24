using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    Animator Animator;

    [Header("Configuration")]
    public string IdleName;
    public string HoveredName;
    public string SelectedName;


    [SerializeField] float TransitionTime;
    [SerializeField] float SelectedFadeInTime;
    public UnityEvent OnButtonConfirmed;

    AnimationClip Selected;

    void OnEnable() {}

    void OnDisable() {}

    void Awake() 
    {
        Animator = GetComponent<Animator>();
    }

    void Start() 
    {
        // Get the duration of the Seleted
        if (SelectedName != "")
            Selected = GetComponent<Animator>().runtimeAnimatorController.animationClips.First(clip => clip.name == SelectedName);
    }

    void Update() {}


    public ButtonPhase CurrentButtonPhase;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentButtonPhase != ButtonPhase.Selected)
        {
            Animator.CrossFade(HoveredName, TransitionTime);
            CurrentButtonPhase = ButtonPhase.Hovered;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CurrentButtonPhase != ButtonPhase.Selected)
        {
            Animator.CrossFade(IdleName, TransitionTime);
            CurrentButtonPhase = ButtonPhase.Idle;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {


        if (CurrentButtonPhase == ButtonPhase.Hovered)
        {
            if (SelectedName != "") Animator.CrossFade(SelectedName, SelectedFadeInTime);
            CurrentButtonPhase = ButtonPhase.Selected;

            StartCoroutine(ResetButton());
        }
    }
    
    IEnumerator ResetButton()
    {
        OnButtonConfirmed?.Invoke();
        if (SelectedName != "") yield return new WaitForSecondsRealtime(Selected.length);
        Animator.CrossFade(IdleName, TransitionTime);
        CurrentButtonPhase = ButtonPhase.Idle;
    }
}