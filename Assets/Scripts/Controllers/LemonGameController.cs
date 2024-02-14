using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonGameController : MonoBehaviour
{

    private float _movementSpeed;    
    public LemonSkillsEnum CurrentSkill;
    public Vector2 _primaryDirection;

    private Rigidbody2D _rb;

    [HideInInspector]
    public EnvironmentCheckController _environmentCheckController;
    private bool _justSpawned;
    private bool _drowningStarted;
    private bool _buildingStarted;
    private bool _diggingStarted;
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
        _primaryDirection = Vector2.right;
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

            var currentPosition = new Vector2(transform.position.x, transform.position.y);

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
                    direction = (Vector2.right + Vector2.up) * 0.5f;

                    _rb.AddForce(direction, ForceMode2D.Impulse);
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
       

    private IEnumerator StartDrowning()
    {
        yield return new WaitForSeconds(3);

        KillLemon();
    }



    #endregion

    #region Public Members

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
    }

    public void LemonHitOtherLemon(DirectionLemonHitEnum hitOn, LemonGameController otherLemon)
    {
        if(otherLemon.CurrentSkill == LemonSkillsEnum.Blocker && _lastLemonHit != otherLemon)
        {
            _lastLemonHit = otherLemon;
            _primaryDirection = hitOn == DirectionLemonHitEnum.Left ? Vector2.left : Vector2.right;
        }
    }

    public void LemonPickedUpKey(KeyController keyController)
    {
        _hasKey = true;
        _capturedKeyController = keyController;
    }

    public void LemonEntersDoor()
    {
        if(_hasKey)
        {
            Destroy(_capturedKeyController.gameObject);            
        }

        GameManager.Instance.LemonsAtHomeForLevelCount++;
        Destroy(this.gameObject);

    }

    #endregion
}

public enum DirectionLemonHitEnum
{
    Left,
    Right
}
