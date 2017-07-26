using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

	public int numberOfRayHorizontal;
	public int numberOfRayVertical;

	public LayerMask collisionMask;
	public Vector2 currentVelocity;

	private BoxCollider2D coll;
	public Bounds colliderBounds;
	private float skinWidth = 0.2f;
	private RaycastOrigins raycastOrigins;


	private float raySpacingHorizontal;
	private float raySpacingVertical;

	public CollisionInfo collisionInfo;

	private void Awake()
	{
		coll = GetComponent<BoxCollider2D>();
		CalculateBounds();
		CalculateRaySpacing();
		currentVelocity =new Vector2 (0,0);
	}

	public void CalculateBounds() {
		colliderBounds = coll.bounds;
		colliderBounds.Expand(-skinWidth * 2);

		raycastOrigins = new RaycastOrigins();
		raycastOrigins.topLeft = new Vector2(colliderBounds.min.x, colliderBounds.max.y);
		raycastOrigins.topRight = new Vector2(colliderBounds.max.x, colliderBounds.max.y);
		raycastOrigins.bottomLeft = new Vector2(colliderBounds.min.x, colliderBounds.min.y);
		raycastOrigins.bottomRight = new Vector2(colliderBounds.max.x, colliderBounds.min.y);
	}

	private void CalculateRaySpacing() {
		numberOfRayHorizontal = numberOfRayHorizontal < 2 ? 2 : numberOfRayHorizontal;
		numberOfRayVertical = numberOfRayVertical < 2 ? 2 : numberOfRayVertical;

		raySpacingVertical = (colliderBounds.extents.x * 2) / (numberOfRayHorizontal - 1);
		raySpacingHorizontal = (colliderBounds.extents.y * 2) / (numberOfRayVertical - 1);
	}

	private void RaycastHorizontal(ref Vector2 velocity)
	{
		float direction = Mathf.Sign(velocity.x);
		Vector2 raycastBase = direction < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

		for (int i=0; i < numberOfRayHorizontal; i++)
		{
			float distance = Mathf.Abs(velocity.x);
			Vector2 raycastOrigin = raycastBase + raySpacingHorizontal * i * Vector2.up;

			RaycastHit2D[] hits = 
				Physics2D.RaycastAll(raycastOrigin, new Vector2(direction, 0), distance + skinWidth, collisionMask);
			Debug.DrawRay(raycastOrigin, new Vector2(velocity.x, 0), Color.red, Time.fixedDeltaTime, false);

			foreach(RaycastHit2D hit in hits)
			{
				if (hit.collider.isTrigger)
				{
					hit.collider.SendMessage("OnTriggerEnter2D", coll);
				}
				else
				{
					
					if (Mathf.Abs(velocity.x) > Mathf.Abs((hit.distance - skinWidth) * direction))
					{
						velocity.x = (hit.distance - skinWidth) * direction;
					}
					collisionInfo.collideLeft = direction < 0;
					collisionInfo.collideRight = direction > 0;
				}
			}
		
		}
	}

	private void RaycastVertical(ref Vector2 velocity)
	{
		float direction = Mathf.Sign(velocity.y);

		Vector2 raycastBase = direction < 0 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
		//Debug.Log(direction);
		for (int i = 0; i < numberOfRayVertical; i++)
		{
			float distance = Mathf.Abs(velocity.y);
			Vector2 raycastOrigin = raycastBase + raySpacingVertical * i * Vector2.right;

			RaycastHit2D[] hits =
				Physics2D.RaycastAll(raycastOrigin, new Vector2(0, direction), distance + skinWidth, collisionMask);
			Debug.DrawRay(raycastOrigin, new Vector2(0, velocity.y), Color.red, Time.fixedDeltaTime, false);

			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider.isTrigger)
				{
					hit.collider.SendMessage("OnTriggerEnter2D", coll);
				}
				else
				{
					
					if(Mathf.Abs(velocity.y) > Mathf.Abs((hit.distance - skinWidth) * direction))
					{
						velocity.y = (hit.distance - skinWidth) * direction;
					}
					collisionInfo.collideTop = direction > 0;
					collisionInfo.collideBottom = direction < 0;
				}
			}           
		}
	}

	public void Move(Vector2 velocity)
	{
		CalculateBounds();
		CalculateRaySpacing();
		collisionInfo.Reset();

		if(velocity.x != 0)
		{
			RaycastHorizontal(ref velocity);
		}

		if(velocity.y != 0)
		{
			RaycastVertical(ref velocity);
		}

		transform.position += (Vector3)velocity;
	}
	private void FixedUpdate(){
		
		currentVelocity += Physics2D.gravity * Time.fixedDeltaTime;
		if (collisionInfo.collideBottom) {
			if (currentVelocity.y < 0)
				currentVelocity.y = 0;
		}

		Move (currentVelocity);     
	}

	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo
	{
		public bool collideTop, collideBottom;
		public bool collideLeft, collideRight;
		public bool isBox;
		public bool isPushed;
		public void Reset()
		{
			collideTop = collideBottom = collideLeft = collideRight = isBox = false;
		}
	}
}
