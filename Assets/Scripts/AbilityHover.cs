using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHover : MonoBehaviour {

    public float OnAirTime;

    private MegamanController megamanController;
    private InputManager inputManager;
    
    private bool isPress;

    private void Awake()
    {
        megamanController = GetComponent<MegamanController>();
        inputManager = GetComponent<InputManager>();

        inputManager.OnJumpButtonPressed += OnButtonPressed;
        inputManager.OnJumpButtonReleased += OnButtonRelease;
    }

    void OnButtonPressed()
    {
        isPress = true;
        if (!megamanController.playerStatus.isGrounded
            && megamanController.playerStatus.airborneAbilityAvailable)
        {
            megamanController.playerStatus.isAntiGravity = true;
            megamanController.playerStatus.airborneAbilityAvailable = false;
            StartCoroutine(OnAirCoroutine());
        }
    }

    void OnButtonRelease()
    {
        isPress = false;
    }

    private IEnumerator OnAirCoroutine()
    {
        float time = 0;

        while (time < OnAirTime && isPress)
        {
            time += Time.deltaTime;
            yield return null;
        }

        megamanController.playerStatus.isAntiGravity = false;
    }
}
