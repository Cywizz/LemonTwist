using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MainMenu")
        {
            _currentLevelDef = _levels[0];
            _currentSceneIndex = -1;
        }
        else
        {
            _currentLevelDef = _levels.Where(l => l.name == sceneName).FirstOrDefault();
            _currentSceneIndex = _currentLevelDef.LevelNo - 1;
        }

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

    #region Private Members

  

    #endregion

    #region Event Handlers


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
            LemonCount = 5;
        }

        PlayerPrefs.SetString("difficulty", difficulty.ToString());
    }

    

    public void LoadNextScene(UIController ui)
    {
        if (_currentSceneIndex > -1)
        {
            //save how many lemons left per level
            PlayerPrefs.SetString(_currentLevelDef.LevelName, LemonCount.ToString());
        }

        PlayerPrefs.Save(); //save all prefs so far

        if (_currentLevelDef.IsLastLevel)
        {
            RestartGame();
            return;
        }

        _currentSceneIndex++;

        _currentLevelDef = _levels[_currentSceneIndex];        

        SceneManager.LoadScene(_currentLevelDef.SceneName);

        LemonsAtHomeForLevelCount = 0;

        if (_currentLevelDef.LevelNo == 1)
            ui.ShowTutorial();
    }

    public void RestartLevel()
    {
        //SetDifficulty(_currentDifficulty);

        LemonsAtHomeForLevelCount = 0;

        var lookUpIndex = 0;

        if(_currentSceneIndex == 0)
        {
            SetDifficulty(_currentDifficulty); //reset
        }
        else
        {
            //check completed previous level for the amount of lemons
            lookUpIndex = _currentSceneIndex - 1;
            var previousLevel = _levels[lookUpIndex];
            LemonCount = int.Parse(PlayerPrefs.GetString(previousLevel.LevelName));
        }        

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

