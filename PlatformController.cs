using System;
using UnityEngine;


public class PlatformController : RayController
{
    private const float speedOfPlatform = 10f;

    private float percentThroughPath;

    private int startPoint;

    private Vector3[] localPoints;
    private Vector3[] globalPoints;

    private const int timeToWait = 10;
    private int timeWaited = 0;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetGlobalPoints();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayStruct();

        Vector3 velocity = PlatformVelocity();

        Push(velocity);

        transform.Translate(velocity);
    }

    private void SetGlobalPoints()
    {
        globalPoints = new Vector3[localPoints.Length];

        for (int i = 0; i < localPoints.Length; i++)
        {
            globalPoints[i] = localPoints[i] + transform.position;
        }
    }

    private Vector3 PlatformVelocity()
    {
        if (Time.time < timeWaited)
        {
            return Vector3.zero;
        }
        
        int destPoint = startPoint + 1;

        float distanceOfPoints = Vector3.Distance(globalPoints[startPoint], globalPoints[destPoint]);

        percentThroughPath += (speedOfPlatform / distanceOfPoints) * Time.deltaTime;

        Vector3 position = Vector3.Lerp(globalPoints[startPoint], globalPoints[destPoint], percentThroughPath);

        if (percentThroughPath == 1)
        {
            percentThroughPath = 0;
            startPoint++;
            if (startPoint >= globalPoints.Length - 1)
            {
                startPoint = 0;
                
                System.Array.Reverse(globalPoints);
            }

            timeWaited = (int)Time.time + timeToWait;
        }

        return position - transform.position;
    }

    private void Push(Vector3 velocity)
    {
        float yPolarity = Mathf.Sign(velocity.y);
        float xPolarity = Mathf.Sign(velocity.x);

        if (velocity.y != 0)
        {
            float lengthOfRay = Mathf.Abs(velocity.y) + indent;

            for (int i = 0; i < GetVRayCount(); i++)
            {
                Vector2 originOfRay = (yPolarity == 1) ? rayOrigin.topLeft : rayOrigin.bottomLeft;

                originOfRay += Vector2.right * GetVSpacing() * i;

                RaycastHit2D contact = Physics2D.Raycast(originOfRay, Vector2.up * yPolarity, lengthOfRay, rayLayer);
                
                if (contact)
                {
                    float yMove = velocity.y - yPolarity * (contact.distance - indent);
                    float xMove = velocity.x;

                    contact.transform.Translate(new Vector3(xMove, yMove));
                }
            }
        }

        if (velocity.x != 0)
        {
            float lengthOfRay = Math.Abs(velocity.x);

            for (int i = 0; i < GetHRayCount(); i++)
            {
                Vector2 originOfRay = (xPolarity == -1) ? rayOrigin.bottomLeft : rayOrigin.bottomRight;

                originOfRay += Vector2.up * GetHRayCount() * i;

                RaycastHit2D contact = Physics2D.Raycast(originOfRay, Vector2.right * xPolarity, lengthOfRay, rayLayer);

                if (contact)
                {
                    float xMove = velocity.x - xPolarity * (contact.distance - indent);
                    float yMove = 0;

                    contact.transform.Translate(new Vector3(xMove, yMove));
                }
            }
        }

        if (yPolarity == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float lengthOfRay =indent * 2;

            for (int i = 0; i < GetVRayCount(); i++)
            {
                Vector2 originOfRay = rayOrigin.topLeft;

                originOfRay += Vector2.right * GetVRayCount() * i;

                RaycastHit2D contact = Physics2D.Raycast(originOfRay, Vector2.up, lengthOfRay, rayLayer);

                if (contact)
                {
                    contact.transform.Translate(new Vector3(velocity.x, velocity.y));
                    
                    break;
                }
            }
        }
    }
}
