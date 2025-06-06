using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LemonSkillUIController : MonoBehaviour
{

    [SerializeField] private Button _skillButton;
    [SerializeField] private GameObject _selectionFrame;
    [SerializeField] private LemonSkillsEnum _skill;

    //public static Action<LemonSkillsEnum> OnSkillButtonClicked;

    #region Unity Events

    private void Awake()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }
    

    // Start is called before the first frame update
    void Start()
    {

        _skillButton.onClick.AddListener(OnSkillClicked);

        _selectionFrame.SetActive(false);
    }

   

    // Update is called once per frame
    void Update()
    {
        if(SkillManager.Instance.SelectedSkill == _skill)
        {
            _selectionFrame.SetActive(true);
        }
        else
        {
            _selectionFrame.SetActive(false);
        }
    }

    private void OnDisable()
    {
        //_skillButton?.onClick.RemoveListener(OnSkillClicked);
    }


    #endregion

    #region Event Handlers

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (this == null) return;

        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            if(GameManager.Instance._currentLevelDef.Skills.Contains(_skill))
            {
                this.gameObject.SetActive(true);   
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnSkillClicked()
    {
        SkillManager.Instance.SelectedSkill = _skill;

       
    }

    #endregion
}
