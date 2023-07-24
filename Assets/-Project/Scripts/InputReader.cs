using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = ("Channel/InputReader"))]
public class InputReader : UnityEngine.ScriptableObject
{
    [SerializeField] public InputActionAsset InputAsset;

    InputAction Up;
    InputAction Down;
    InputAction Left;
    InputAction Right;
    public event Action<Vector2> MovementPerformed;

    InputAction Mobility;
    public event Action MobilityPerformed;
    public event Action MobilityCanceled;

    InputAction Pause;
    InputAction UIPause;
    public event Action PausePerformed;

    InputAction Click;
    public event Action ClickPerformed;
    public event Action ClickCanceled;

    [SerializeField] public InputActionMap activeActionMap;

    [Header("Channel")]
    [SerializeField] WaveChannel WaveChannel;

    public InputActionMap PlayerActionMap;
    public InputActionMap UIActionMap;

    private void OnEnable()
    {
        PlayerActionMap = InputAsset.FindActionMap("Player");
        if (PlayerActionMap == null) Debug.LogError("No PlayerActionMap found");

        UIActionMap = InputAsset.FindActionMap("UI");
        if (UIActionMap == null) Debug.LogError("No UIActionMap found");

        currentWeapon = InputReaderWeapon.None;

        WaveChannel.OnWaveCompleted += SwitchToUI;
        WaveChannel.OnUpgradeSelected += SwitchToPlayer;

        Up = PlayerActionMap["MoveUp"];
        Up.performed += (x) => currentDirection += new Vector2(0, 1);
        Up.performed += RaiseDirectionPerformed;
        Up.canceled += (x) => currentDirection -= new Vector2(0, 1);
        Up.canceled += RaiseDirectionPerformed;

        Right = PlayerActionMap["MoveRight"];
        Right.performed += (x) => currentDirection += new Vector2(1, 0);
        Right.performed += RaiseDirectionPerformed;
        Right.canceled += (x) => currentDirection -= new Vector2(1, 0);
        Right.canceled += RaiseDirectionPerformed;

        Left = PlayerActionMap["MoveLeft"];
        Left.performed += (x) => currentDirection += new Vector2(-1, 0);
        Left.performed += RaiseDirectionPerformed;
        Left.canceled += (x) => currentDirection -= new Vector2(-1, 0);
        Left.canceled += RaiseDirectionPerformed;

        Down = PlayerActionMap["MoveDown"];
        Down.performed += (x) => currentDirection += new Vector2(0, -1);
        Down.performed += RaiseDirectionPerformed;
        Down.canceled += (x) => currentDirection -= new Vector2(0, -1);
        Down.canceled += RaiseDirectionPerformed;

        Special = PlayerActionMap["Special"];
        Special.performed += RaiseSpecialPerformed;
        Special.canceled += RaiseSpecialCanceled;

        Primary = PlayerActionMap["Primary"];
        Primary.performed += RaisePrimaryPerformed;
        Primary.canceled += RaisePrimaryCanceled;

        Ultimate = PlayerActionMap["Ultimate"];
        Ultimate.performed += RaiseUltimatePerformed;
        Ultimate.canceled += RaiseUltimateCanceled;

        Mobility = PlayerActionMap["Mobility"];
        Mobility.performed += RaiseMobilityPerformed;
        Mobility.canceled += RaiseMobilityCanceled;

        UIPause = UIActionMap["Pause"];
        Pause = PlayerActionMap["Pause"];
        Pause.performed += RaisePausePerformed;
        UIPause.performed += RaisePausePerformed;

        Click = UIActionMap["Click"];
        Click.performed += RaiseClickPerformed;
        Click.canceled += RaiseClickCanceled;
    }

    private void OnDisable()
    {
        if (activeActionMap != null)
        {
            activeActionMap.Disable();
        }
    }

    InputActionMap previousActionMap = null;
    public bool IsPlayerInput => activeActionMap?.name == "Player";
    public bool IsUIInput => activeActionMap?.name == "UI";
    public void SwitchToUI() => SwitchActionMap("UI");
    public void SwitchToPlayer() => SwitchActionMap("Player");

    public void DisableInput()
    {
        previousActionMap = activeActionMap;
        SwitchActionMap("Disabled");
    }

    public void EnableInput()
    {
        if (previousActionMap == null) { Debug.LogWarning("EnableInput is used before DisableInput was used"); return; }

        SwitchActionMap(previousActionMap.name);
        previousActionMap = null;
    }

    void SwitchActionMap(string mapName)
    {
        if (activeActionMap != null)
            activeActionMap.Disable();

        activeActionMap = InputAsset.FindActionMap(mapName);

        if (activeActionMap != null)
            activeActionMap.Enable();
    }

    Vector2 currentDirection;
    void RaiseDirectionPerformed(InputAction.CallbackContext context)
    {
        MovementPerformed?.Invoke(currentDirection.normalized);
    }

    #region Special
    InputAction Special;
    // Used by Special Weapons to send the confirmation of the weapon usage
    public void RegisterSpecial() => currentWeapon = InputReaderWeapon.Special;

    // Used by Special Weapons to interrupt the Weapon Use, mid use
    public void InterruptSpecial() => currentWeapon = InputReaderWeapon.None;

    public event Action SpecialPerformed;
    void RaiseSpecialPerformed(InputAction.CallbackContext context)
    {
        if (currentWeapon == InputReaderWeapon.None)
        {
            SpecialPerformed?.Invoke();
        }
    }

    public event Action SpecialCanceled;
    void RaiseSpecialCanceled(InputAction.CallbackContext context)
    {
        if (currentWeapon == InputReaderWeapon.Special)
        {
            SpecialCanceled?.Invoke();
        }
    }
    #endregion

    #region Primary
    InputAction Primary;
    // Used by Primary Weapons to send the confirmation of the weapon usage
    public void RegisterPrimary() => currentWeapon = InputReaderWeapon.Primary;
    // Used by Primary Weapons to interrupt the Weapon Use, mid use
    public void InterruptPrimary() => currentWeapon = InputReaderWeapon.None;

    public event Action PrimaryPerformed;
    void RaisePrimaryPerformed(InputAction.CallbackContext context)
    {
        if (currentWeapon == InputReaderWeapon.None)
        {
            PrimaryPerformed?.Invoke();
        }
    }

    public event Action PrimaryCanceled;
    void RaisePrimaryCanceled(InputAction.CallbackContext context)
    {
        if (currentWeapon == InputReaderWeapon.Primary)
        {
            PrimaryCanceled?.Invoke();
            InterruptPrimary();
        }
    }
    #endregion

    #region Primary
    InputAction Ultimate;
    // Used by Primary Weapons to send the confirmation of the weapon usage
    public void RegisterUltimate() => currentWeapon = InputReaderWeapon.Ultimate;
    // Used by Primary Weapons to interrupt the Weapon Use, mid use
    public void InterruptUltimate() => currentWeapon = InputReaderWeapon.None;

    public event Action UltimatePerformed;
    void RaiseUltimatePerformed(InputAction.CallbackContext context)
    {
        if (currentWeapon == InputReaderWeapon.None)
        {
            UltimatePerformed?.Invoke();
        }
    }

    public event Action UltimateCanceled;
    void RaiseUltimateCanceled(InputAction.CallbackContext context)
    {
        if (currentWeapon == InputReaderWeapon.Ultimate)
        {
            UltimateCanceled?.Invoke();
        }
    }
    #endregion

    void RaiseMobilityPerformed(InputAction.CallbackContext context) => MobilityPerformed?.Invoke();
    void RaiseMobilityCanceled(InputAction.CallbackContext context) => MobilityCanceled?.Invoke();

    void RaisePausePerformed(InputAction.CallbackContext context) => PausePerformed?.Invoke();

    void RaiseClickPerformed(InputAction.CallbackContext context) => ClickPerformed?.Invoke();
    void RaiseClickCanceled(InputAction.CallbackContext contest) => ClickCanceled?.Invoke();

    InputReaderWeapon currentWeapon;
}