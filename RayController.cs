using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Super class to any instance that will be colliding
[RequireComponent (typeof (BoxCollider2D))]
public class RayController : MonoBehaviour
{
    private LayerMask rayLayer;

    private BoxCollider2D box;

    public const float indent = 0.3f;

    public RayOriginStruct rayOrigin;

    public int hRayCount = 4;
    public int vRayCount = 4;

    private float hSpacing;
    private float vSpacing;

    private const int NUMBER_TO_DECREASE_INDENT_BY = 2;

    public SideOfCollision sideOfCollision;
    // Start is called before the first frame update
    public virtual void Start()
    {
        box = GetComponent<BoxCollider2D>();
        SpacingCalc();
    }

    public void UpdateRayStruct() 
    {
        Bounds bounds = box.bounds;
        bounds.Expand(indent * NUMBER_TO_DECREASE_INDENT_BY);

        rayOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
        rayOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
    }

    public void SpacingCalc() 
    {
        Bounds bounds = box.bounds;
        bounds.Expand(indent * NUMBER_TO_DECREASE_INDENT_BY);

        hSpacing = bounds.size.x / (hRayCount - 1);
        vSpacing = bounds.size.y / (vRayCount - 1);
    }

    public struct RayOriginStruct
    {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    //Getters and setters
    public float GetHSpacing()
    {
        return hSpacing;
    }
    public void SetHSpacing(float hSpacing)
    {
        this.hSpacing = hSpacing;
    }

    public float GetVSpacing()
    {
        return vSpacing;
    }

    public void SetVSpacing(float vSpacing)
    {
        this.vSpacing = vSpacing;
    }

    public int GetHRayCount()
    {
        return hRayCount;
    }
    public void SetHRayCount(int hRayCount)
    {
        this.hRayCount = hRayCount;
    }

    public int GetVRayCount()
    {
        return vRayCount;
    }

    public void SetVRayCount(int vRayCount)
    {
        this.vRayCount = vRayCount;
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
