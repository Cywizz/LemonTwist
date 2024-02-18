using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _difficultyText;
    [SerializeField] private TextMeshProUGUI _level1Text;
    [SerializeField] private TextMeshProUGUI _level2Text;
    [SerializeField] private TextMeshProUGUI _level3Text;
    [SerializeField] private TextMeshProUGUI _level4Text;
    [SerializeField] private TextMeshProUGUI _level5Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
       StartCoroutine(DelayedScoreUpdates());

    }

    private IEnumerator DelayedScoreUpdates()
    {
        yield return new WaitForSeconds(1f);

        var level1Score = PlayerPrefs.GetString("Level 1", "-1");
        var level2Score = PlayerPrefs.GetString("Level 2", "-1");
        var level3Score = PlayerPrefs.GetString("Level 3", "-1");
        var level4Score = PlayerPrefs.GetString("Level 4", "-1");
        var level5Score = PlayerPrefs.GetString("Level 5", "-1");

        _difficultyText.text = "Difficulty: " + PlayerPrefs.GetString("difficulty", "Normal");


        if (level1Score != "-1")
        {            
            _level1Text.text = "Level 1 - " + level1Score + " Lemons Alive";
        }
        else
        {
            _level1Text.text = "Level 1 - ???";
        }

        if (level2Score != "-1")
        {         
            _level2Text.text = "Level 2 - " + level2Score + " Lemons Alive";
        }
        else
        {
            _level2Text.text = "Level 2 - ???";
        }

        if (level3Score != "-1")
        {            
            _level3Text.text = "Level 3 - " + level3Score + " Lemons Alive";
        }
        else
        {
            _level3Text.text = "Level 3 - ???";
        }

        if (level4Score != "-1")
        {            
            _level4Text.text = "Level 4 - " + level4Score + " Lemons Alive";
        }
        else
        {
            _level4Text.text = "Level 4 - ???";
        }

        if (level5Score != "-1")
        {            
            _level5Text.text = "Level 5 - " + level5Score + " Lemons Alive";
        }
        else
        {
            _level5Text.text = "Level 5 - ???";
        }
    }

    public void ClearHighScore()
    {
        PlayerPrefs.DeleteKey("difficulty");

        PlayerPrefs.DeleteKey("Level 1");
        PlayerPrefs.DeleteKey("Level 2");
        PlayerPrefs.DeleteKey("Level 3");
        PlayerPrefs.DeleteKey("Level 4");
        PlayerPrefs.DeleteKey("Level 5");

        OnEnable();
    }
}
