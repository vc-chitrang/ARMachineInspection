using UnityEngine;
using UnityEngine.InputSystem;

public class PressInputBase:MonoBehaviour {
    protected InputAction m_PressAction;
    protected virtual void Awake() {
        m_PressAction = new InputAction("Press",binding: "<Pointer>/press");
        m_PressAction.started += ctx => {
            if (ctx.control.device is Pointer device) {
                OnPressBegan(device.position.ReadValue());
            }
        };

        m_PressAction.performed += ctx => {
            if (ctx.control.device is Pointer device) {
                OnPress(device.position.ReadValue());
            }
        };

        m_PressAction.canceled += _ => OnPressCancel();
    }

    protected virtual void OnEnable() {
        m_PressAction.Enable();
    }

    protected virtual void OnDisable() {
        m_PressAction.Disable();
    }

    protected virtual void OnDestroy() {
        m_PressAction.Dispose();
    }

    protected virtual void OnPress(Vector3 position) {
    }
    protected virtual void OnPressBegan(Vector3 positon) {
    }
    protected virtual void OnPressCancel() {
    }
}