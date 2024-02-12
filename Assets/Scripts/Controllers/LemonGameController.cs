using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonGameController : MonoBehaviour
{

    [SerializeField] private float _movementSpeed;
    public GameObject SelectorObject;

    private Rigidbody2D _rb;
    private EnvironmentCheckController _environmentCheckController;

    private LemonSkillsEnum _currentSkill;

    public Vector2 _primaryDirection;



    private bool _justSpawned;
    private bool _drowningStarted;


    #region Unity Events
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _environmentCheckController = GetComponentInChildren<EnvironmentCheckController>();

        _justSpawned = true;
        _drowningStarted = false;
        _currentSkill = LemonSkillsEnum.None;
        _primaryDirection = Vector2.right;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        

        //no skill, just walking
        if (_currentSkill == LemonSkillsEnum.None)
        {
            var direction = _primaryDirection;
            var currentPosition = new Vector2(transform.position.x, transform.position.y);

            if (_environmentCheckController.IsGrounded)
            {                
                //_rb.velocity = new Vector2(direction.x * _movementSpeed, _rb.velocity.y);                

                _justSpawned = false;
            }
            else if (_environmentCheckController.IsInWater)
            {

                direction = Vector2.down;
                _movementSpeed = 1f;

                //_rb.velocity = new Vector2(direction.x * _movementSpeed, _movementSpeed);

                if (_drowningStarted == false) StartDrowningProcess();
            }
            else
            {
                direction = Vector2.down;

                if (_justSpawned) //makes it jump side ways out of the tree
                {
                    direction = Vector2.right;
                }               

                //_rb.velocity = new Vector2(direction.x * _movementSpeed, _rb.velocity.y);
            }

            _rb.MovePosition(currentPosition + (direction * _movementSpeed * Time.deltaTime));
        }

        //blocker skill
        if(_currentSkill == LemonSkillsEnum.Blocker)
        {
            _rb.velocity = Vector2.zero;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null)
        {
            var otherLemon = collision.gameObject.GetComponent<LemonGameController>();

            if(otherLemon != null && otherLemon != this)
            {
                if (_currentSkill == LemonSkillsEnum.Blocker)
                {
                    otherLemon._primaryDirection = otherLemon._primaryDirection * -1;
                }
            }
        }
    }

    #endregion

    #region Local Members

    private void StartDrowningProcess()
    {
        _drowningStarted = true;
        StartCoroutine(StartDrowning());
    }


    private IEnumerator StartDrowning()
    {
        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);
    }


    #endregion

    #region Public Members

    public void SetSkill(LemonSkillsEnum skill)
    {
        _currentSkill = skill;
    }

    #endregion
}
