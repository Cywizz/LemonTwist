using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class LemonGameController : MonoBehaviour
{

    [HideInInspector]
    public EnvironmentCheckController _environmentCheckController;
    public LemonSkillsEnum CurrentSkill;
    public Vector2 _primaryDirection;
    public Texture2D HoverCursor_Ok;
    public Texture2D HoverCursor_Invalid;

    private float _movementSpeed;    
    private Rigidbody2D _rb;
    private bool _justSpawned;
    private bool _drowningStarted;
    private bool _buildingStarted;
    private bool _diggingStarted;
    private bool _bashingStarted;
    private LemonGameController _lastLemonHit;
    private bool _hasKey;
    private KeyController _capturedKeyController;

    


    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _environmentCheckController = GetComponentInChildren<EnvironmentCheckController>();
       

        _justSpawned = true;
        _drowningStarted = false;
        CurrentSkill = LemonSkillsEnum.None;       
        _movementSpeed = GameManager.Instance._currentLevelDef.LemonSpeed;
    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        

        //no skill, just walking
        if (CurrentSkill == LemonSkillsEnum.None)
        {           

            var direction = _primaryDirection;         

            if (_environmentCheckController.IsGrounded)
            {
                _rb.velocity = new Vector2(direction.x * _movementSpeed, direction.y * _movementSpeed);

                _justSpawned = false;
            }
            else if (_environmentCheckController.IsInWater)
            {

                direction = Vector2.down + _primaryDirection;                
                _rb.velocity = new Vector2(0, 0.5f);               

                if (_drowningStarted == false) StartDrowningProcess();
            }
            else
            {
                direction = Vector2.down;

                if (_justSpawned) //makes it jump side ways out of the tree
                {
                    //direction = (_primaryDirection) * _movementSpeed;
                    direction = (direction + Vector2.up) * 0.5f;

                    _rb.AddForce(direction, ForceMode2D.Force);
                    //_rb.velocity = direction;
                }
                else
                {
                    _rb.velocity = new Vector2(direction.x * _movementSpeed, direction.y * _movementSpeed);

                }
            }

            if (_environmentCheckController.IsHittingObstacleToLeft)
            {
                _primaryDirection = Vector2.right;
                
            }

            if (_environmentCheckController.IsHittingObstacleToRight)
            {
                _primaryDirection = Vector2.left;
                
            }

            if(_environmentCheckController.IsOverCrumbleTile)
            {
                //_rb.velocity = new Vector2(direction.x * _movementSpeed, direction.y * _movementSpeed);
                SkillManager.Instance.LemonOverCrumbleTile(this);
            }

        }

        //movement and action of different skills
        if(CurrentSkill == LemonSkillsEnum.Blocker)
        {
            _rb.velocity = Vector2.zero;
        }
        if(CurrentSkill == LemonSkillsEnum.Juicer)
        {
            _rb.velocity = Vector2.zero;
        }
        if(CurrentSkill == LemonSkillsEnum.Builder)
        {
            _rb.velocity = Vector2.zero;
            if (_buildingStarted == false)
            {
                _buildingStarted = true;
                StartCoroutine(StartBuildProcess());
            }
        }
        if(CurrentSkill == LemonSkillsEnum.Digger)
        {
            _rb.velocity = Vector2.zero;
            if(_diggingStarted == false)
            {
                _diggingStarted = true;
                StartCoroutine(StartDiggingProcess());
            }

        }

        if(CurrentSkill == LemonSkillsEnum.Basher)
        {
            _rb.velocity = Vector2.zero;

            if(_bashingStarted == false)
            {
                _bashingStarted = true;
                StartCoroutine(StartBashingProcess());
            }
        }

    }

    private void OnMouseEnter()
    {
        if(SkillManager.Instance.SelectedSkill == LemonSkillsEnum.None)
        {
            Cursor.SetCursor(HoverCursor_Invalid, Vector2.zero, CursorMode.ForceSoftware);            
        }
        else
        {
            if (CurrentSkill == LemonSkillsEnum.Blocker && SkillManager.Instance.SelectedSkill != LemonSkillsEnum.Juicer)
            {
                Cursor.SetCursor(HoverCursor_Invalid, Vector2.zero, CursorMode.ForceSoftware);
            }
            else
            {
                if (LemonHasKey)
                {
                    Cursor.SetCursor(HoverCursor_Invalid, Vector2.zero, CursorMode.ForceSoftware);
                }
                else
                {
                    Cursor.SetCursor(HoverCursor_Ok, Vector2.zero, CursorMode.ForceSoftware);
                }
            }
        }

        
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(GameManager.Instance.GameDefaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }


    #endregion

    #region Local Members



    private IEnumerator StartDiggingProcess()
    {
        yield return new WaitForSeconds(1f);

        SkillManager.Instance.Dig(this);
    }

   private IEnumerator StartBuildProcess()
    {
        yield return new WaitForSeconds(1f);
                
        SkillManager.Instance.BuildPlatform(this);
    }

    private void StartDrowningProcess()
    {
        _drowningStarted = true;
        StartCoroutine(StartDrowning());
    }

    private IEnumerator StartBashingProcess()
    {
        yield return new WaitForSeconds(1f);

        SkillManager.Instance.Bash(this);
    }


    private IEnumerator StartDrowning()
    {
        yield return new WaitForSeconds(3);

        KillLemon();
    }



    #endregion

    #region Public Members

    public void SetPrimaryDirection(Vector2 direction)
    {
        _primaryDirection = direction;
    }

    public bool LemonHasKey
    {
        get
        {
            return _hasKey;
        }
    }

    public void KillLemon()
    {
        GameManager.Instance.LemonCount--;

        if (_hasKey) _capturedKeyController.KeyHolderDied();

        Destroy(this.gameObject);
    }

    public void SetSkill(LemonSkillsEnum skill)
    {
        CurrentSkill = skill;

        //reset possible flags
        _buildingStarted = false;
        _diggingStarted = false;
        _bashingStarted = false;
    }

    public void LemonHitOtherLemon(DirectionLemonHitEnum sideOtherLemonhitOn, LemonGameController otherLemon)
    {
        if(otherLemon.CurrentSkill == LemonSkillsEnum.Blocker)
        {
            _primaryDirection = sideOtherLemonhitOn == DirectionLemonHitEnum.Left ? Vector2.left : Vector2.right;

            _lastLemonHit = otherLemon;

            
        }
    }

    public void LemonPickedUpKey(KeyController keyController)
    {
        _hasKey = true;
        _capturedKeyController = keyController;

        AudioManager.Instance.PlaySFX(SFXSoundsEnum.KeyPickup);
    }

    public void LemonEntersDoor()
    {
        if(_hasKey)
        {
            Destroy(_capturedKeyController.gameObject);            
        }

        GameManager.Instance.LemonsAtHomeForLevelCount++;

        AudioManager.Instance.PlaySFX(SFXSoundsEnum.EnterDoor);

        Destroy(this.gameObject);

    }

    #endregion
}

public enum DirectionLemonHitEnum
{
    Left,
    Right
}
