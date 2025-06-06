using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonAnimationController : MonoBehaviour
{
    private Animator _animator;
    private LemonGameController _controller;

    #region Unity Events


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<LemonGameController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_controller.CurrentSkill == LemonSkillsEnum.None)
        {            
            _animator.SetTrigger("IsWalking");


            if (_controller._environmentCheckController.IsInWater)
            {
                _animator.ResetTrigger("IsWalking");
                _animator.SetTrigger("IsDrowning");
            }
            else
            {

                if (_controller._environmentCheckController.IsGrounded == false)
                {
                    _animator.ResetTrigger("IsWalking");
                    _animator.SetTrigger("IsFalling");
                }                
            }
        }
        else
        {
            if(_controller.CurrentSkill == LemonSkillsEnum.Blocker)
            {
                _animator.SetTrigger("IsBlocking");
            }
            if(_controller.CurrentSkill == LemonSkillsEnum.Builder) 
            {
                _animator.ResetTrigger("IsFalling");
                _animator.ResetTrigger("IsWalking");
                _animator.SetTrigger("IsBuilding");               
            }
            if(_controller.CurrentSkill == LemonSkillsEnum.Juicer)
            {
                _animator.SetTrigger("IsJuicing");
                
            }
            if(_controller.CurrentSkill == LemonSkillsEnum.Digger)
            {
                _animator.ResetTrigger("IsFalling");
                _animator.ResetTrigger("IsWalking");
                _animator.SetTrigger("IsDigging");
            }
            if (_controller.CurrentSkill == LemonSkillsEnum.Basher)
            {
                _animator.ResetTrigger("IsFalling");
                _animator.ResetTrigger("IsWalking");
                _animator.SetTrigger("IsBashing");
            }

        }


        if(_controller._environmentCheckController.IsHitByKillObstacle)
        {
            _animator.SetTrigger("IsJuicing");
            
        }


    }

    #endregion

    


    #region Animation Event Handlers

    public void AnimationStarted(string animationName)
    {
        if(animationName == "Builder")
            AudioManager.Instance.PlaySFX(SFXSoundsEnum.Building);


        if (animationName == "Juice")
            AudioManager.Instance.PlaySFX(SFXSoundsEnum.Juicing);

        if (animationName == "Digger")
            AudioManager.Instance.PlaySFX(SFXSoundsEnum.Digging);

        if(animationName == "Basher")
            AudioManager.Instance.PlaySFX(SFXSoundsEnum.Bashing);

    }

    public void AnimationEnded(string animationName)
    {

        if (animationName == "Juice")
            _controller.KillLemon();

    }

    #endregion
}
