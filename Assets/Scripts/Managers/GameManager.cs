using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private Camera _camera;

    private LemonGameController _lastSelectedLemon;

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;

        LemonSkillUIController.OnSkillButtonClicked += OnUISkillButtonClicked;
    }
  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Input.mousePosition));
            if (!rayHit.collider) return;

            var lemonSelected = rayHit.collider.gameObject.GetComponent<LemonGameController>();
            if(lemonSelected != null)
            {
               if(_lastSelectedLemon != null) _lastSelectedLemon.SelectorObject.SetActive(false);
                _lastSelectedLemon = lemonSelected;
                _lastSelectedLemon.SelectorObject.SetActive(true);                
            }
          

        }

       
    }

    private void LateUpdate()
    {
        //if (_lastSelectedLemon != null)
        //{
        //    var rb = _lastSelectedLemon.GetComponent<Rigidbody2D>();
            
        //    Debug.Log(rb.totalForce);
        //}
    }

    #endregion

    #region Event Handlers

    private void OnUISkillButtonClicked(LemonSkillsEnum skill)
    {
        if(_lastSelectedLemon != null)
        {
            _lastSelectedLemon.SetSkill(skill);
        }
    }

    #endregion
}
