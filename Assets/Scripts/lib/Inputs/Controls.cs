// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""12b9b29e-21ff-439f-bf4a-07c6dfe2d137"",
            ""actions"": [
                {
                    ""name"": ""Joystick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5e9e50f1-ecca-48b0-8423-56e45b3a663d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""STICk"",
                    ""id"": ""2772032a-595d-47d1-86ed-9fb95ffcf391"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Joystick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c69018d9-7bc6-4bb8-8c86-cfd1a02fb528"",
                    ""path"": ""<AndroidJoystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick & Joyattack"",
                    ""action"": ""Joystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""98491496-10aa-4a6e-b5e8-2e2c88824191"",
                    ""path"": ""<AndroidJoystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick & Joyattack"",
                    ""action"": ""Joystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""64a95528-af2a-4807-8b88-c36c4facc627"",
                    ""path"": ""<AndroidJoystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick & Joyattack"",
                    ""action"": ""Joystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""59fd747d-266a-4cbd-b002-f2f38342aeb9"",
                    ""path"": ""<AndroidJoystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick & Joyattack"",
                    ""action"": ""Joystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Joystick & Joyattack"",
            ""bindingGroup"": ""Joystick & Joyattack"",
            ""devices"": [
                {
                    ""devicePath"": ""<AndroidJoystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Joystick = m_Player.FindAction("Joystick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Joystick;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Joystick => m_Wrapper.m_Player_Joystick;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Joystick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJoystick;
                @Joystick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJoystick;
                @Joystick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJoystick;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Joystick.started += instance.OnJoystick;
                @Joystick.performed += instance.OnJoystick;
                @Joystick.canceled += instance.OnJoystick;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_JoystickJoyattackSchemeIndex = -1;
    public InputControlScheme JoystickJoyattackScheme
    {
        get
        {
            if (m_JoystickJoyattackSchemeIndex == -1) m_JoystickJoyattackSchemeIndex = asset.FindControlSchemeIndex("Joystick & Joyattack");
            return asset.controlSchemes[m_JoystickJoyattackSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnJoystick(InputAction.CallbackContext context);
    }
}
