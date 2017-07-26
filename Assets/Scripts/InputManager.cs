using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public KeyCode jumpButton;
    public KeyCode dashButton;
    public KeyCode resizeButton;

    public System.Action OnJumpButtonPressed;
    public System.Action OnJumpButtonReleased;
    public System.Action OnDashButtonPressed;
    public System.Action OnResizeButtonPressed;
    public System.Action<float> OnMoveDirectionChanged;

    public void Update()
    {
        if (Input.GetKeyDown(jumpButton) && OnJumpButtonPressed != null) OnJumpButtonPressed();
        if (Input.GetKeyUp(jumpButton) && OnJumpButtonReleased != null) OnJumpButtonReleased();
        if (Input.GetKeyDown(dashButton) && OnDashButtonPressed != null) OnDashButtonPressed();
        if (Input.GetKeyDown(resizeButton) && OnResizeButtonPressed != null) OnResizeButtonPressed();
        if (OnMoveDirectionChanged != null) OnMoveDirectionChanged(Input.GetAxis("Horizontal"));
    }
}
