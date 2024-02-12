using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnvironmentCheckController : MonoBehaviour
{

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _waterLayerMask;
    [SerializeField] private LayerMask _lemonLayerMask;


    public bool IsGrounded;
    public bool IsInWater;
   

    private void OnTriggerStay2D(Collider2D collision)
    {
        var isCollidingWithGroundLayer = collision != null && (((1 << collision.gameObject.layer) & _groundLayerMask) != 0);
        IsGrounded = isCollidingWithGroundLayer;

        var isCollidingWithWaterLayer = collision != null && (((1 << collision.gameObject.layer) & _waterLayerMask) != 0);
        IsInWater = isCollidingWithWaterLayer;

       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var wasCollidingWithGroundLayer = collision != null && (((1 << collision.gameObject.layer) & _groundLayerMask) != 0);
        if(wasCollidingWithGroundLayer)
        {
            IsGrounded = false;
        }        

        var wasCollidingWithWaterLayer = collision != null && (((1 << collision.gameObject.layer) & _waterLayerMask) != 0);
        if(wasCollidingWithWaterLayer ) 
        {
            IsInWater = false;
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //var isCollidingWithOtherLemon = collision != null && (((1 << collision.gameObject.layer) & _lemonLayerMask) != 0);

        //if (isCollidingWithOtherLemon)
        //{
        //    string s = string.Empty;
        //}
    }



}
