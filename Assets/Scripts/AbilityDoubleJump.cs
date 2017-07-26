using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MegamanController))]
[RequireComponent(typeof(InputManager))]
public class AbilityDoubleJump : MonoBehaviour {
    private MegamanController megamanController;
    private InputManager inputManager;

    private void Awake()
    {
        megamanController = GetComponent<MegamanController>();
        inputManager = GetComponent<InputManager>();

        inputManager.OnJumpButtonPressed += OnJumpButtonPressed;
    }

    private void OnJumpButtonPressed() {
		if(!megamanController.playerStatus.isDead && !megamanController.playerStatus.isGrounded && !megamanController.playerStatus.isSliding
            && megamanController.playerStatus.airborneAbilityAvailable && !megamanController.playerStatus.isUpsized)
        {
            megamanController.PerformJump();
			megamanController.anim.SetBool ("isDJ",true);
            megamanController.playerStatus.airborneAbilityAvailable = false;
			StartCoroutine(DoubleJumpCoroutine());
        }
    }

	private IEnumerator DoubleJumpCoroutine()
	{
		yield return new WaitForSeconds(megamanController.doubleJumpTime);
		megamanController.anim.SetBool ("isDJ",false);
	}
}
