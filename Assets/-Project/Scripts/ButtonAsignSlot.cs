using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonAsignSlot : MonoBehaviour
{
    [Header("Channel")]
    [SerializeField] InputReader InputReader;
    [SerializeField] VoidEvent DefaultControlls;

    [Header("Configuration")]
    [SerializeField] string isListeningSign;
    [SerializeField] string defaultPath;

    [Header("Others")]
    [SerializeField] TextMeshProUGUI ButtonText;
    [SerializeField] string AccordingInputActionName;
    InputAction accordingAction;

    public static event Action<MonoBehaviour, string> OnButtonRebinded;

    void OnEnable() 
    {
        OnButtonRebinded += CheckButtonRebind;
        DefaultControlls.OnEventRaised += DefaultSlot;
    }

    void OnDisable() 
    {
        OnButtonRebinded -= CheckButtonRebind;
    }

    void Awake() {}

    void Start() 
    {
        // Get Action
        accordingAction = InputReader.PlayerActionMap.FindAction(AccordingInputActionName);

        InitializeBindingSlot();
    }

    void Update() {

    }

    string prevButtonText;
    public void OnKeyBindRequested()
    {
        prevButtonText = ButtonText.text;
        ButtonText.text = isListeningSign;
        Rebind(AccordingInputActionName);
    }

    InputActionRebindingExtensions.RebindingOperation rebindOperation;
    public void Rebind(string actionName)
    {
        InputReader.DisableInput();
        if (accordingAction != null)
        {
            // Führe die interaktive Rebinding-Operation aus
            rebindOperation = accordingAction.PerformInteractiveRebinding()
                .WithTargetBinding(0) // Angenommen, es gibt nur ein Binding für die Aktion
                .WithCancelingThrough("<Keyboard>/escape") // Optional: Erlaube das Abbrechen der Rebinding-Operation mit der Escape-Taste
                .OnComplete(operation => RebindComplete(accordingAction))
                .OnCancel(operation => RebindCancel(accordingAction))
                .Start();
        }
    }

    void RebindComplete(InputAction action)
    {
        PlayerPrefs.SetString(AccordingInputActionName, action.bindings[0].effectivePath);
        ButtonText.text = InputControlPath.ToHumanReadableString(action.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        rebindOperation.Dispose();
        OnButtonRebinded?.Invoke(this, action.bindings[0].effectivePath);
        InputReader.EnableInput();
    }

    void RebindCancel(InputAction action)
    {
        ButtonText.text = prevButtonText;
        rebindOperation.Dispose();
        InputReader.EnableInput();
    }

    void InitializeBindingSlot()
    {
        // Get ButtonPath
        string buttonPath = PlayerPrefs.GetString(AccordingInputActionName, "Empty");
        if (buttonPath == "Empty") Debug.LogError("no buttonPath with name: " + AccordingInputActionName + " found in PlayerPrefs");

        RebindButton(buttonPath);
    }

    void RebindButton(string path)
    {
        // Apply the override
        accordingAction.ApplyBindingOverride(path);
        PlayerPrefs.SetString(AccordingInputActionName, path);
        prevButtonText = InputControlPath.ToHumanReadableString(path, InputControlPath.HumanReadableStringOptions.OmitDevice);
        ButtonText.text = prevButtonText;
    }

    void CheckButtonRebind(MonoBehaviour source, string asignedPath)
    {
        if (source == this) return;

        if (accordingAction.bindings[0].effectivePath == asignedPath)
        {
            accordingAction.RemoveBindingOverride(0);
            prevButtonText = "None";
            ButtonText.text = "None";
        }
    }

    void DefaultSlot() => RebindButton(defaultPath);
}
