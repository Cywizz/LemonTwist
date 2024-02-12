using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonClickedController : MonoBehaviour
{
    [SerializeField] private GameObject _selector;

    private Camera _camera;
    


    #region Unity Events

    private void Awake()
    {
        _camera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            var rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Input.mousePosition));
            if (!rayHit.collider) return;

            if(rayHit.collider.gameObject.transform.parent == this.transform.parent)
            {
                _selector.gameObject.SetActive(true);
            }
            else
            {
                _selector.gameObject.SetActive(false);
            }

        }
    }

    #endregion
}
