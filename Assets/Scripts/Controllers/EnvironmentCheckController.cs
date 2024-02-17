using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnvironmentCheckController : MonoBehaviour
{

    [SerializeField] private BoxCollider2D _physicsCollider;
    [SerializeField] private BoxCollider2D _leftTriggerCollider;
    [SerializeField] private BoxCollider2D _rightTriggerCollider;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _waterLayerMask;
    [SerializeField] private LayerMask _lemonLayerMask;
    [SerializeField] private LayerMask _bounceLayerMask;
    [SerializeField] private LayerMask _killObstacleLayerMask;
    [SerializeField] private LayerMask _crumbleLayerMask;


    public LemonGameController ParentController;


    public bool IsGrounded;
    
    public bool IsInWater;

    public bool IsHittingLemonOnLeft;

    public bool IsHittingLemonOnRight;

    public bool IsHittingObstacleToLeft;

    public bool IsHittingObstacleToRight;

    public bool IsHitByKillObstacle;

    public bool IsOverCrumbleTile;

    private void Awake()
    {
        ParentController = GetComponentInParent<LemonGameController>();
    }


    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        IsGrounded = CheckGrounding();
        IsInWater = CheckIfInWater();

        IsHittingLemonOnLeft = CheckLemonOnLeft();
        IsHittingLemonOnRight = CheckLemonOnRight();

        IsHittingObstacleToLeft = CheckForObstacleHitLeft();
        IsHittingObstacleToRight = CheckForObstacleHitRight();

        IsHitByKillObstacle = CheckForKillObstacleHit();

        IsOverCrumbleTile = CheckIfOverCrumbleTile();
    }

    

    private bool CheckGrounding()
    {
        float extraHeight = 0.1f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_physicsCollider.bounds.center, _physicsCollider.bounds.size, 0f, Vector2.down, extraHeight, _groundLayerMask);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(_physicsCollider.bounds.center + new Vector3(_physicsCollider.bounds.extents.x, 0), Vector2.down * (_physicsCollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(_physicsCollider.bounds.center - new Vector3(_physicsCollider.bounds.extents.x, 0), Vector2.down * (_physicsCollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(_physicsCollider.bounds.center - new Vector3(_physicsCollider.bounds.extents.x, _physicsCollider.bounds.extents.y + extraHeight), Vector2.right * (_physicsCollider.bounds.extents.x*2), rayColor);

        return raycastHit.collider != null;
    }


    private bool CheckIfInWater()
    {
        float extraHeight = 0.1f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_physicsCollider.bounds.center, _physicsCollider.bounds.size, 0f, Vector2.down, extraHeight, _waterLayerMask);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.blue;

            Debug.DrawRay(_physicsCollider.bounds.center + new Vector3(_physicsCollider.bounds.extents.x, 0), Vector2.down * (_physicsCollider.bounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(_physicsCollider.bounds.center - new Vector3(_physicsCollider.bounds.extents.x, 0), Vector2.down * (_physicsCollider.bounds.extents.y + extraHeight), rayColor);
            Debug.DrawRay(_physicsCollider.bounds.center - new Vector3(_physicsCollider.bounds.extents.x, _physicsCollider.bounds.extents.y + extraHeight), Vector2.right * (_physicsCollider.bounds.extents.x * 2), rayColor);
        }

        return raycastHit.collider != null;
    }

    private bool CheckForObstacleHitLeft()
    {
        float exstraWidth = 0.1f;

        RaycastHit2D raycastHit = Physics2D.Raycast(_physicsCollider.bounds.center, Vector2.left, _physicsCollider.bounds.extents.x + exstraWidth, _bounceLayerMask);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.red;
        }
        else
        {
            rayColor = Color.green;
        }

        Debug.DrawRay(_physicsCollider.bounds.center, Vector2.left * (_physicsCollider.bounds.extents.x + exstraWidth), rayColor);

        return raycastHit.collider != null;

    }

    private bool CheckForObstacleHitRight()
    {
        float exstraWidth = 0.1f;

        RaycastHit2D raycastHit = Physics2D.Raycast(_physicsCollider.bounds.center, Vector2.right, _physicsCollider.bounds.extents.x + exstraWidth, _bounceLayerMask);

        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.red;
        }
        else
        {
            rayColor = Color.green;
        }

        Debug.DrawRay(_physicsCollider.bounds.center, Vector2.right * (_physicsCollider.bounds.extents.x + exstraWidth), rayColor);

        return raycastHit.collider != null;
    }

    private bool CheckLemonOnLeft()
    {
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_lemonLayerMask);
        
        List<Collider2D> otherLemonsHitList = new List<Collider2D>();

        int lemonCount = _leftTriggerCollider.OverlapCollider(contactFilter, otherLemonsHitList);

        if (lemonCount > 0)
        {
            foreach (var collider in otherLemonsHitList) 
            {
                //it could hit the lemoncontroller direct, or the left or right trigger colliders
                var otherLemonController = collider.gameObject.GetComponent<LemonGameController>();
                if(otherLemonController == null)
                {
                    //it might be the left or right triggers
                    var triggerEnvironment = collider.gameObject.GetComponentInParent<EnvironmentCheckController>();
                    if(triggerEnvironment != null)
                    {
                        otherLemonController = triggerEnvironment.ParentController;
                    }
                    
                }

                if (otherLemonController != ParentController)
                {
                    otherLemonController.LemonHitOtherLemon(DirectionLemonHitEnum.Left, ParentController);

                    return true;                    
                }
            }
        }

        return false;
    }

    private bool CheckLemonOnRight()
    {
        var contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(_lemonLayerMask);

        List<Collider2D> otherLemonsHitList = new List<Collider2D>();

        int lemonCount = _rightTriggerCollider.OverlapCollider(contactFilter, otherLemonsHitList);

        if (lemonCount > 1) //if it is just 1 then the lemon is colliding with itself
        {  

            foreach (var collider in otherLemonsHitList)
            {
                //it could hit the lemoncontroller direct, or the left or right trigger colliders
                var otherLemonController = collider.gameObject.GetComponent<LemonGameController>();
                if (otherLemonController == null)
                {
                    //it might be the left or right triggers
                    var triggerEnvironment = collider.gameObject.GetComponentInParent<EnvironmentCheckController>();
                    if (triggerEnvironment != null)
                    {
                        otherLemonController = triggerEnvironment.ParentController;
                    }
                }

                if (otherLemonController != ParentController)
                {
                    otherLemonController.LemonHitOtherLemon(DirectionLemonHitEnum.Right, ParentController);

                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckForKillObstacleHit()
    {
        float extraHeight = 0.1f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_physicsCollider.bounds.center, _physicsCollider.bounds.size, 0f, Vector2.down, extraHeight, _killObstacleLayerMask);
        
        return raycastHit.collider != null;
    }

    private bool CheckIfOverCrumbleTile()
    {
        float extraHeight = 0.1f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(_physicsCollider.bounds.center, _physicsCollider.bounds.size, 0f, Vector2.down, extraHeight, _crumbleLayerMask);
               

        return raycastHit.collider != null;
    }





}
