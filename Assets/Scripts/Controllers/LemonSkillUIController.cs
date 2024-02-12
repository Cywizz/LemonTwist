using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LemonSkillUIController : MonoBehaviour
{

    [SerializeField] private Button _skillButton;

    public static Action<LemonSkillsEnum> OnSkillButtonClicked;

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {

        _skillButton.onClick.AddListener(OnSkillClicked);
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        _skillButton?.onClick.RemoveListener(OnSkillClicked);
    }

    #endregion

    #region Event Handlers

    private void OnSkillClicked()
    {
        OnSkillButtonClicked?.Invoke(LemonSkillsEnum.Blocker);
        //public Action<string, string, string> OnNotificationReadyToSend;
        //notificationService.OnNotificationReadyToSend += SendNotice;
    }

    #endregion
}
