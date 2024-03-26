using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private float maxTeleportDistance = 5f;
    [SerializeField] private LayerMask teleportLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 teleportDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                teleportDirection += Vector3.up;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                teleportDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                teleportDirection += Vector3.right;
            }

            if (teleportDirection != Vector3.zero)
            {
                Vector3 teleportPosition = transform.position + teleportDirection.normalized * maxTeleportDistance;

                // Perform raycast to check for collision with teleport layer
                RaycastHit2D hit = Physics2D.Raycast(teleportPosition, Vector2.down, Mathf.Infinity, teleportLayer);

                if (hit.collider != null)
                {
                    // Check if the teleport position is inside the collider of the hit object
                    Collider2D hitCollider = hit.collider;
                    if (hitCollider.OverlapPoint(teleportPosition))
                    {
                        return;
                    }

                    // Check if the distance to the ground is less than the maximum teleport distance
                    float distanceToGround = hit.distance;
                    if (distanceToGround < maxTeleportDistance)
                    {
                        transform.position = new Vector3(teleportPosition.x, hit.point.y + GetComponent<BoxCollider2D>().size.y / 2, 0f);
                    }
                    else
                    {
                        transform.position = teleportPosition;
                    }
                }
                else
                {
                    transform.position = teleportPosition;
                }
            }
        }
    }
}