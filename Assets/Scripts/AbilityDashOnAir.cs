using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MegamanController))]
[RequireComponent(typeof(InputManager))]
public class AbilityDashOnAir : MonoBehaviour
{
    private MegamanController megamanController;
    private InputManager inputManager;

    private void Awake()
    {
        megamanController = GetComponent<MegamanController>();
        inputManager = GetComponent<InputManager>();

        inputManager.OnDashButtonPressed += OnDashButtonPressed;
    }

    private void OnDashButtonPressed()
    {
		if (!megamanController.playerStatus.isDead && !megamanController.playerStatus.isGrounded
            && megamanController.playerStatus.airborneAbilityAvailable
            && !megamanController.playerStatus.isUpsized)
        {            
            megamanController.playerStatus.isAntiGravity = true;
            megamanController.PerformDash();       

            megamanController.playerStatus.airborneAbilityAvailable = false;            
            StartCoroutine(DashOnAirCoroutine());
        }
    }

    private IEnumerator DashOnAirCoroutine()
    {
        yield return new WaitForSeconds(megamanController.dashTime);
        megamanController.playerStatus.isAntiGravity = false;
        megamanController.playerStatus.moveAllowed = true;
		megamanController.playerStatus.speedMultiplier = 1;
    }
}