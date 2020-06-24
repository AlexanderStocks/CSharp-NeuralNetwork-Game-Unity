using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Player : RayController
{
    public LayerMask playerLayer;
    public LayerMask ropeable;
    public LayerMask allowedToCollideWithPlayer;

    private bool hanging;
    private bool onRope;

    private InitPlayer initPlayer;

    private const float speedOfRope = 10;
    private float gravity;

    private Vector3 target;
    private Vector3 cameraDirection;

    private GameObject obstacles;
    private GameObject aiController;

    private const float maxAngleOfIncline = 65f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        initPlayer = GetComponent<InitPlayer>();

        gravity = initPlayer.getGravity();

        onRope = false;
        hanging = false;
    
    }

    // Update is called once per frame
    public void UpdateFromPlayer(Vector3 velocity)
    {
        UpdateRayStruct();
        sideOfCollision.clear();

        if (!onRope && !hanging)
        {
            if (velocity.x != 0)
            {
                collisionH(ref velocity);
            }

            if (velocity.y != 0)
            {
                collisionV(ref velocity);
            }
            transform.Translate(velocity);
        }
        
        if ((transform.position != target) && onRope && !hanging)
        {
            Launch(target);
        }

        if (hanging && Input.GetKeyDown(KeyCode.Space))
        {
            hanging = false;
            velocity.y = 0;
        }
    }

    public void LaunchSetup()
    {
        cameraDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cameraDirection.z = 0;
        Vector3 direction = cameraDirection - transform.position;

        RaycastHit2D contact = Physics2D.Raycast(transform.position, direction, Vector3.Magnitude(direction), ropeable);

        if (contact)
        {
            target = contact.point;
            target.z = 0;

            onRope = true;

            Launch(target);
        }
    }

    private void RopeCollisions(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);

        float horizontalRopeSpeed = (float)(speedOfRope * Math.Cos(angle) + indent);
        float verticalRopeSpeed = speedOfRope * Mathf.Sin(angle) + indent;

        for (int i = -1 ; i < 1; i++)
        {
            if (hanging)
            {
                break;
            }

            if (i == 0)
            {
                i = 1;
            }

            float lengthOfRay = speedOfRope * indent + indent;

            Vector3 ropeVelocity = new Vector3(horizontalRopeSpeed, verticalRopeSpeed);
            Vector3 negRopeVelocity = new Vector3(-horizontalRopeSpeed, -verticalRopeSpeed);
            
            collisionH(ref ropeVelocity);
            collisionH(ref negRopeVelocity);

            collisionV(ref ropeVelocity);
            collisionV(ref negRopeVelocity);
        }

    }

    void collisionH(ref Vector3 velocity) 
    {
        float xPolarity = Mathf.Sign(velocity.x);
        float lengthOfRay = Mathf.Abs(velocity.x) + indent;

        for (int i = 0; i < GetHRayCount(); i++)
        {
            Vector2 originOfRay = (xPolarity == -1) ? rayOrigin.bottomLeft : rayOrigin.bottomRight;

            originOfRay += Vector2.up * (GetHSpacing() * i);

            RaycastHit2D contact = Physics2D.Raycast(originOfRay, Vector2.right * xPolarity, lengthOfRay, playerLayer);

            if (contact) {
                velocity.x = (contact.distance - indent) * xPolarity;

                lengthOfRay = contact.distance;

                this.sideOfCollision.setLeft(xPolarity == -1);
                this.sideOfCollision.setRight(!sideOfCollision.getLeft());
            }
        }
    }

    void collisionV(ref Vector3 velocity) 
    {
        float yPolarity = Mathf.Sign(velocity.y);
        float lengthOfRay = Mathf.Abs(velocity.y) + indent;

        for (int i = 0; i < GetVRayCount(); i++)
        {
            Vector2 originOfRay = (yPolarity == -1) ? rayOrigin.bottomLeft : rayOrigin.topLeft;

            originOfRay += Vector2.right * (GetHSpacing() * i + velocity.x);

            RaycastHit2D contact = Physics2D.Raycast(originOfRay, Vector2.up * yPolarity, lengthOfRay, playerLayer);

            if (contact && yPolarity == 1) {
                velocity.y = 0.5f * ((contact.distance - indent) * yPolarity);
            } else {
                //check where stepped on different types of objects
                velocity.y = (contact.distance - indent) * yPolarity;

                lengthOfRay = contact.distance;

                this.sideOfCollision.setDown(yPolarity == -1);

                this.sideOfCollision.setUp(!sideOfCollision.getDown());
            }
        }
    }
}
