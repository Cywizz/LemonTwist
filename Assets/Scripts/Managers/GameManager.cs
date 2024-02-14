using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] LevelDefinition[] _levels;   

    private Camera _camera;
    private LemonGameController _lastSelectedLemon;
    private int _currentSceneIndex = -1;


    [HideInInspector]
    public LevelDefinition _currentLevelDef;

    [HideInInspector]
    public int LemonCount;

    [HideInInspector]
    public int LemonsAtHomeForLevelCount;

    private GameDifficultyEnum _currentDifficulty;

    public static GameManager Instance { get; private set; }

    #region Unity Events

    private void Awake()
    {
        #region Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion

        _camera = Camera.main;

       // LemonSkillUIController.OnSkillButtonClicked += OnUISkillButtonClicked;

        //default
        SetDifficulty(GameDifficultyEnum.Normal);
        _currentLevelDef = _levels[0];


    }

    // Start is called before the first frame update
    void Start()
    {
       
        

    }
  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var rayHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Input.mousePosition));
            if (!rayHit.collider) return;

            var lemonSelected = rayHit.collider.gameObject.GetComponent<LemonGameController>();
            if (lemonSelected != null && SkillManager.Instance.SelectedSkill != LemonSkillsEnum.None)
            {
                //key carrier can not get a skill
                if (lemonSelected.LemonHasKey == false)
                {
                    _lastSelectedLemon = lemonSelected;
                    _lastSelectedLemon.SetSkill(SkillManager.Instance.SelectedSkill);

                    AudioManager.Instance.PlaySFX(SFXSoundsEnum.LemonSelected);
                }
            }
          

        }

       
    }

    private void LateUpdate()
    {
       
    }

    #endregion

    #region Event Handlers

    //private void OnUISkillButtonClicked(LemonSkillsEnum skill)
    //{
    //    if(_lastSelectedLemon != null)
    //    {
    //        _lastSelectedLemon.SetSkill(skill);
    //    }
    //}

    public void SetDifficulty(GameDifficultyEnum difficulty)
    {
        _currentDifficulty = difficulty;

        if(difficulty == GameDifficultyEnum.Easy)
        {
            LemonCount = 20;
        }
        else if (difficulty == GameDifficultyEnum.Normal)
        {
            LemonCount = 15;
        }
        else
        {
            LemonCount = 1;
        }
    }

    public void LoadNextScene()
    {
        _currentSceneIndex++;

        _currentLevelDef = _levels[_currentSceneIndex];

        SceneManager.LoadScene(_currentLevelDef.SceneName);

        LemonsAtHomeForLevelCount = 0;
    }

    public void RestartLevel()
    {
        SetDifficulty(_currentDifficulty);

        LemonsAtHomeForLevelCount = 0;

        SceneManager.LoadScene(_currentLevelDef.SceneName);

        
    }

    public void RestartGame()
    {
        _currentSceneIndex = -1;

        SetDifficulty(_currentDifficulty);


      

        SceneManager.LoadScene("MainMenu");

        LemonsAtHomeForLevelCount = 0;

       
    }

    #endregion
}

public enum GameDifficultyEnum
{
    Easy,
    Normal,
    Hard
}

