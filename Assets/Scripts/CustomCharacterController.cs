using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CustomCharacterController : MonoBehaviour {
    public int numberOfRayHorizontal;
    public int numberOfRayVertical;

    public LayerMask collisionMask;

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

    public void Move(Vector2 velocity)
    {
        CalculateBounds();
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

    private void RaycastHorizontal(ref Vector2 velocity)
    {
        float direction = Mathf.Sign(velocity.x);
        Vector2 raycastBase = direction < 0 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
        //Debug.Log(direction);
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
					if (hit.collider.tag == "isBox")
						collisionInfo.isBox = true;
					if (hit.collider.gameObject.tag == "isSpike")
						collisionInfo.isSpike = true;
					if (hit.collider.gameObject.tag == "ExitDoor")
						collisionInfo.DoorCollide = true;
					
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
					if (hit.collider.gameObject.tag == "isBox")
						collisionInfo.isBox = true;

					if (hit.collider.gameObject.tag == "isSpike")
						collisionInfo.isSpike = true;

					if (hit.collider.gameObject.tag == "ExitDoor")
						collisionInfo.DoorCollide = true;


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

    public void RaycastTouchHorizontal(ref Vector2 velocity)
    {

        Vector2 raycastBaseLeft = raycastOrigins.bottomLeft;
        Vector2 raycastBaseRight = raycastOrigins.bottomRight;
        //Debug.Log(direction);
        for (int i = 0; i < numberOfRayHorizontal; i++)
        {
            float distance = Mathf.Abs(velocity.x);
            Vector2 raycastOriginLeft = raycastBaseLeft + raySpacingHorizontal * i * Vector2.up;
            Vector2 raycastOriginRight = raycastBaseRight + raySpacingHorizontal * i * Vector2.up;

            RaycastHit2D[] hitsLeft =
                Physics2D.RaycastAll(raycastOriginLeft, new Vector2(-1, 0), distance + skinWidth, collisionMask);
            RaycastHit2D[] hitsRight =
                Physics2D.RaycastAll(raycastOriginRight, new Vector2(1, 0), distance + skinWidth, collisionMask);
            Debug.DrawRay(raycastOriginLeft, new Vector2(velocity.x, 0), Color.red, Time.fixedDeltaTime, false);
            Debug.DrawRay(raycastOriginRight, new Vector2(velocity.x, 0), Color.red, Time.fixedDeltaTime, false);

            foreach (RaycastHit2D hit in hitsLeft)
            {
                if (hit.collider.isTrigger)
                {
                    hit.collider.SendMessage("OnTriggerEnter2D", coll);
                }
                else
                {
                    if (Mathf.Abs(velocity.x) > Mathf.Abs(hit.distance - skinWidth))
                    {
                        velocity.x = (hit.distance - skinWidth) * -1;
                    }
                    collisionInfo.touchLeft = true;
                }
            }

            foreach (RaycastHit2D hit in hitsRight)
            {
                if (hit.collider.isTrigger)
                {
                    hit.collider.SendMessage("OnTriggerEnter2D", coll);
                }
                else
                {
                    if (Mathf.Abs(velocity.x) > Mathf.Abs(hit.distance - skinWidth))
                    {
                        velocity.x = (hit.distance - skinWidth) * 1;
                    }
                    collisionInfo.touchRight = true;
                }
            }
        }
    }

    public void RaycastTouchVertical(ref Vector2 velocity)
    {

        Vector2 raycastBaseTop = raycastOrigins.topLeft;
        Vector2 raycastBaseBottom = raycastOrigins.bottomLeft;
        //Debug.Log(direction);
        for (int i = 0; i < numberOfRayVertical; i++)
        {
            float distance = Mathf.Abs(velocity.y);
            Vector2 raycastOriginTop = raycastBaseTop + raySpacingVertical * i * Vector2.right;
            Vector2 raycastOriginBottom = raycastBaseBottom + raySpacingHorizontal * i * Vector2.right;

            RaycastHit2D[] hitsBottom =
                Physics2D.RaycastAll(raycastOriginBottom, new Vector2(0, -1), distance + skinWidth, collisionMask);
            RaycastHit2D[] hitsTop =
                Physics2D.RaycastAll(raycastOriginTop, new Vector2(0, 1), distance + skinWidth, collisionMask);
            Debug.DrawRay(raycastOriginBottom, new Vector2(0, velocity.y), Color.red, Time.fixedDeltaTime, false);
            Debug.DrawRay(raycastOriginTop, new Vector2(0, velocity.y), Color.red, Time.fixedDeltaTime, false);

            foreach (RaycastHit2D hit in hitsBottom)
            {
                if (hit.collider.isTrigger)
                {
                    hit.collider.SendMessage("OnTriggerEnter2D", coll);
                }
                else
                {
                    if (Mathf.Abs(velocity.y) > Mathf.Abs(hit.distance - skinWidth))
                    {
                        velocity.y = (hit.distance - skinWidth) * -1;
                    }
                    collisionInfo.touchBottom = true;
                }
            }

            foreach (RaycastHit2D hit in hitsTop)
            {
                if (hit.collider.isTrigger)
                {
                    hit.collider.SendMessage("OnTriggerEnter2D", coll);
                }
                else
                {
                    if (Mathf.Abs(velocity.y) > Mathf.Abs(hit.distance - skinWidth))
                    {
                        velocity.y = (hit.distance - skinWidth) * 1;
                    }
                    collisionInfo.touchTop = true;
                }
            }
        }
    }
}
public struct RaycastOrigins {
    public Vector2 topLeft, topRight;
    public Vector2 bottomLeft, bottomRight;
}

public struct CollisionInfo
{
    public bool collideTop, collideBottom;
    public bool collideLeft, collideRight;
    public bool touchLeft, touchRight;
    public bool touchBottom, touchTop;
	public bool isBox;
	public bool isSpike;
	public bool DoorCollide;
    public void Reset()
    {
		collideTop = collideBottom = collideLeft = collideRight = isBox = isSpike =  false;
    }
}
