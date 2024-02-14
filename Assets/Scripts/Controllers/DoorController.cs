using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool _isOpen;
    private Animator _animator;

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var lemonController = collision.gameObject.GetComponent<LemonGameController>();

        if (lemonController != null)
        {
            if (_isOpen == false)
            {
                if (lemonController.LemonHasKey)
                {
                    _isOpen = true;
                    _animator.SetTrigger("IsOpening");
                    lemonController.LemonEntersDoor();
                }
            }
            else
            {
                lemonController.LemonEntersDoor();
            }
            
        }
        else
        {

        }

    }

    #endregion
}
