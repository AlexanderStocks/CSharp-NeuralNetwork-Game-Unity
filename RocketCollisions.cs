using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RocketCollisions : RayController
{

    private const int DAMAGE = 10;

    public GameObject Rocket;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayStruct();

        CollisionsH();
        CollisionsV();
    }

    private void CollisionsH()
    {
        for (int i = -1; i < 1; i++)
        {
            if (i == 0)
            {
                i = 1;
            }
            
            float xPolarity = i;
            float lengthOfRay = 2 * indent;

            for (int j = 0; j < GetVRayCount(); j++)
            {
                Vector2 originOfRay = (xPolarity == -1) ? rayOrigin.bottomLeft : rayOrigin.bottomRight;

                originOfRay += Vector2.up * (GetHRayCount() * i);

                RaycastHit2D contact = Physics2D.Raycast(originOfRay, Vector2.right, lengthOfRay, rayLayer);

                if (contact)
                {
                    contact.collider.GetComponent<InitPlayer>().DamagePlayer(DAMAGE);
                }
            }
        }
    }

    private void CollisionsV()
    {
        float lengthOfRay = indent * 2;

        for (int i = 0; i < GetHRayCount(); i++)
        {
            Vector2 originOfRay = rayOrigin.bottomLeft;

            originOfRay += Vector2.up * GetHSpacing() * i;

            RaycastHit2D contact = Physics2D.Raycast(originOfRay, Vector2.right, lengthOfRay, rayLayer);

            if (contact)
            {
                contact.collider.GetComponent<InitPlayer>().DamagePlayer(DAMAGE);
                DestroyRocket(contact);
            }
        }
    }

    public void DestroyRocket(RaycastHit2D contact)
    {
        Rocket.GetComponent<Rocket>().Destoryrocket();
    }
}

