using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{    

    [SerializeField] GameObject _mainMenuUI;
    [SerializeField] GameObject _levelUI;
    [SerializeField] GameObject _deathUI;
    [SerializeField] GameObject _successUI;

    [SerializeField] TextMeshProUGUI _lemonCountText;
    [SerializeField] TextMeshProUGUI _lemonsAtHomeText;

    [SerializeField] TextMeshProUGUI _difficultyDescriptionText;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            _mainMenuUI.SetActive(true);
            _levelUI.SetActive(false);
            _deathUI.SetActive(false);
            _successUI.SetActive(false);

            _difficultyDescriptionText.text = "NORMAL - You start with " + GameManager.Instance.LemonCount + " Lemons";

            
        }
        else
        {
            _mainMenuUI.SetActive(false);
            _levelUI.SetActive(true);
            _deathUI.SetActive(false);
            _successUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _lemonCountText.text = GameManager.Instance.LemonCount.ToString();
        _lemonsAtHomeText.text = GameManager.Instance.LemonsAtHomeForLevelCount.ToString();

        if(GameManager.Instance.LemonCount == 0)
        {//no lemons left  - game over
            _deathUI.SetActive(true);
        }
        else
        {
            _deathUI.SetActive(false);
        }

        if (GameManager.Instance.LemonCount == GameManager.Instance.LemonsAtHomeForLevelCount)
        {//all lemons at home. - next level
            _successUI.SetActive(true);
        }
        else
        {
            _successUI.SetActive(false);
        }



        //var allLemons = FindObjectsOfType<MonoBehaviour>(true).OfType<LemonGameController>().ToArray();
        //if(allLemons.Length == 0 && GameManager.Instance.LemonCount > 0)
        //{

        //}

    }


    #region Event Handlers

    public void SetGameEasy()
    {
        GameManager.Instance.SetDifficulty(GameDifficultyEnum.Easy);  
        AudioManager.Instance.PlaySFX(SFXSoundsEnum.ButtonPop);

        _difficultyDescriptionText.text = "EASY - You start with " + GameManager.Instance.LemonCount + " Lemons";
    }

    public void SetGameNormal()
    {
        GameManager.Instance.SetDifficulty(GameDifficultyEnum.Normal); 
        AudioManager.Instance.PlaySFX(SFXSoundsEnum.ButtonPop);

        _difficultyDescriptionText.text = "NORMAL - You start with " + GameManager.Instance.LemonCount + " Lemons";
    }

    public void SetGameHard()
    {
        GameManager.Instance.SetDifficulty(GameDifficultyEnum.Hard);
        AudioManager.Instance.PlaySFX(SFXSoundsEnum.ButtonPop);

        _difficultyDescriptionText.text = "HARD - You start with " + GameManager.Instance.LemonCount + " Lemons";
    }

    public void StartGame()
    {
        GameManager.Instance.LoadNextScene();
        _levelUI.SetActive(true);
        _mainMenuUI.SetActive(false);
        _deathUI.SetActive(false);
        _successUI.SetActive(false);

        PlayerPrefs.Save(); 

        AudioManager.Instance.PlaySFX(SFXSoundsEnum.ButtonPop);
    }

    public void RestartLevel()
    {
        _levelUI.SetActive(true);
        _mainMenuUI.SetActive(false);
        _deathUI.SetActive(false);
        _successUI.SetActive(false);

        GameManager.Instance.RestartLevel();
    }

    public void RestartGame()
    {
        _levelUI.SetActive(false);
        _mainMenuUI.SetActive(true);
        _deathUI.SetActive(false);
        _successUI.SetActive(false);

        GameManager.Instance.RestartGame();
    }

    public void NextLevel()
    {

        _levelUI.SetActive(true);
        _mainMenuUI.SetActive(false);
        _deathUI.SetActive(false);
        _successUI.SetActive(false);

        GameManager.Instance.LoadNextScene();

    }

    public void PauseClicked()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }



    #endregion
}
