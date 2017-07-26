using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(CustomCharacterController))]
[RequireComponent (typeof(InputManager))]
public class MegamanController : MonoBehaviour
{
	[SerializeField] AudioClip soundOnJump, soundOnDie, soundOnDash, soundOnZoom;

	public LayerMask deadCollisionMask;
	public float runSpeed;
	public float jumpSpeed;
	public float dashMultiplier;
	public float dashTime;
	public float doubleJumpTime;
	public float wallJumpTime;
	public float resizeRate;
	public float resizeScale;
	public float ResizeCooldownTime;
	public Vector2 wallSlidingDeviant;
	public Vector2 moveVector;
	public bool airAbility;
	public PlayerStatus playerStatus;
	public Animator anim;
	public string nextLevel;
	private CustomCharacterController characterController;
	private InputManager inputManager;

	private float currentFacing;

	private void Awake ()
	{
		anim = GetComponent<Animator> ();
		characterController = GetComponent<CustomCharacterController> ();
		inputManager = GetComponent<InputManager> ();
		playerStatus.currentVelocity = Vector2.zero;
		playerStatus.isDashing = false;
		playerStatus.speedMultiplier = 1;
		playerStatus.isUpsized = true;
		playerStatus.resizeAvailable = true;
		playerStatus.moveAllowed = true;

		inputManager.OnJumpButtonPressed += OnJumpButtonPressed;
		inputManager.OnDashButtonPressed += OnDashButtonPressed;
		inputManager.OnMoveDirectionChanged += OnMoveDirectionChanged;
	}

	private void OnJumpButtonPressed ()
	{
		if (!playerStatus.isDead && playerStatus.isGrounded || playerStatus.isSliding) {
			PerformJump ();
			if (playerStatus.isSliding && !playerStatus.isUpsized) {
				
				playerStatus.currentVelocity.x = -playerStatus.currentVelocity.x;
				playerStatus.isWallJumping = true;
				StartCoroutine (WallJumpCoroutine ());
			}
		}
	}

	public void PerformJump ()
	{		
		SoundManager.instance.Play (soundOnJump);
		playerStatus.currentVelocity.y = jumpSpeed * Time.fixedDeltaTime;
	}

	private IEnumerator WallJumpCoroutine ()
	{
		yield return new WaitForSeconds (wallJumpTime);
		playerStatus.isWallJumping = false;
	}

	private void OnDashButtonPressed ()
	{
		if (!playerStatus.isDead && playerStatus.isGrounded && !playerStatus.isDashing && !playerStatus.isUpsized) {
			PerformDash ();
		}
	}

	public void PerformDash ()
	{		
		SoundManager.instance.Play (soundOnDash);
		playerStatus.isDashing = true;
		anim.SetBool ("isDashing", true);
		playerStatus.speedMultiplier = dashMultiplier;
		StartCoroutine (DashCoroutine ());
	}

	private IEnumerator DashCoroutine ()
	{
		yield return new WaitForSeconds (dashTime);
		playerStatus.isDashing = false;
		anim.SetBool ("isDashing", false);
	}

	private void OnMoveDirectionChanged (float direction)
	{
		
		if (playerStatus.moveAllowed == true) {
			if (playerStatus.isWallJumping) {
				return;
			}
			if (direction != 0) {
				currentFacing = Mathf.Sign (direction);
			}

			if (playerStatus.isDashing) {
				playerStatus.currentVelocity.x = currentFacing * runSpeed * playerStatus.speedMultiplier * Time.fixedDeltaTime;
				anim.SetFloat ("runSpeed", Mathf.Abs (playerStatus.currentVelocity.x));
				Flip (playerStatus.currentVelocity.x);
			} else {
				playerStatus.currentVelocity.x = direction * runSpeed * playerStatus.speedMultiplier * Time.fixedDeltaTime;
				anim.SetFloat ("runSpeed", Mathf.Abs (playerStatus.currentVelocity.x));
				Flip (playerStatus.currentVelocity.x);
			}
		}
	}

	private void FixedUpdate ()
	{
		moveVector = playerStatus.currentVelocity;
		airAbility = playerStatus.airborneAbilityAvailable;
		playerStatus.isGrounded = false;
		playerStatus.isSliding = false;
		playerStatus.isAntiGravity = false;
		anim.SetBool ("isGrounded", playerStatus.isGrounded);

		if (characterController.collisionInfo.collideBottom) {
			playerStatus.isGrounded = true;
			anim.SetBool ("isGrounded", true);
			anim.SetBool ("isDJ", false);
			anim.SetBool ("isWallSliding", false);
			anim.SetFloat ("jumpSpeed", playerStatus.currentVelocity.y);

		} else if (((characterController.collisionInfo.collideRight && playerStatus.currentVelocity.x > 0)
		           || (characterController.collisionInfo.collideLeft && playerStatus.currentVelocity.x < 0))
		           && playerStatus.isUpsized == false) {
			playerStatus.airborneAbilityAvailable = true;
			playerStatus.isSliding = true;
			anim.SetBool ("isWallSliding", true);
			anim.SetBool ("isDJ", false);
			characterController.Move (wallSlidingDeviant);
			playerStatus.isAntiGravity = true;
		} else
			anim.SetBool ("isWallSliding", false);
		
		if (playerStatus.isGrounded) {
			playerStatus.airborneAbilityAvailable = true;
			if (playerStatus.currentVelocity.y < 0)
				playerStatus.currentVelocity.y = 0;
			if (!playerStatus.isDashing && !playerStatus.isUpsized)
				playerStatus.speedMultiplier = 1;
			if (playerStatus.isUpsized)
				playerStatus.speedMultiplier = 0.7f;			
		}

		if (characterController.collisionInfo.collideTop) {
			playerStatus.currentVelocity.y = 0;
		}


		if (playerStatus.isAntiGravity) {
			playerStatus.currentVelocity.y = 0;
		}
		else playerStatus.currentVelocity += Physics2D.gravity * Time.fixedDeltaTime;  


		characterController.Move (playerStatus.currentVelocity);

		// Spike Collision
		if (characterController.collisionInfo.isSpike) {
			playerStatus.moveAllowed = false;
			StartCoroutine (Die ());
		}
		// Exit Door
		if (characterController.collisionInfo.DoorCollide == true) {
			TKSceneManager.ChangeScene (nextLevel);
		}

	}

	public IEnumerator Die ()
	{
		playerStatus.isDead = true;
		SoundManager.instance.PlaySoundEffect (soundOnDie);
		characterController.collisionMask = deadCollisionMask;
		playerStatus.currentVelocity.x = -runSpeed * Time.fixedDeltaTime * 0.5f;
		playerStatus.currentVelocity.y = runSpeed * Time.fixedDeltaTime * 0.5f;
		anim.SetBool ("isDead",true);
		playerStatus.isAntiGravity = true;
		playerStatus.moveAllowed = false;
		yield return new WaitForSeconds (0.6f);
		TKSceneManager.ChangeScene ("GameOverScene");
	}

	private void Flip (float currentVelX)
	{
		if (currentVelX > 0) {
			transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
		} else if (currentVelX < 0)
			transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
	}

	public void Lose ()
	{		
		StartCoroutine (Die ());
	}
}

public struct PlayerStatus
{
	public bool isGrounded;
	public bool isDashing;
	public bool isSliding;
	public bool isWallJumping;
	public bool airborneAbilityAvailable;
	public bool isAntiGravity;
	public bool isUpsized;
	public bool isDead;
	public bool moveAllowed;
	public bool resizeAvailable;
	public float speedMultiplier;
	public Vector2 currentVelocity;
    
}
