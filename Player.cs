using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : RayController
{
    public LayerMask playerLayer;

    public SideOfCollision sideOfCollision;

    private InitPlayer initPlayer;

    private float gravity;
    
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        initPlayer = GetComponent<InitPlayer>();
        gravity = initPlayer.getGravity();
    }

    // Update is called once per frame
    public void UpdateFromInitPlayer(Vector3 velocity)
    {
        UpdateRayStruct();
        sideOfCollision.clear();

        gravity = initPlayer.getGravity();

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

    public struct SideOfCollision
    {
        private bool up, down, left, right;

        public void clear() 
        {
            up = down = left = right = false;
        }

        public bool getUp()
        {
            return up;
        }

        public bool getDown()
        {
            return down;
        }

        public bool getLeft()
        {
            return left;
        }

        public bool getRight()
        {
            return right;
        }

        public void setUp(bool value) 
        {
            up = value;
        }

        public void setDown(bool value) 
        {
            down = value;
        }

        public void setLeft(bool value) 
        {
            left = value;
        }

        public void setRight(bool value) 
        {
            right = value;
        }
    }
}
