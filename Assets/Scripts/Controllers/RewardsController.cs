using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardsController : MonoBehaviour
{
    [SerializeField] private Image _rewardsItemImage;
    [SerializeField] private TextMeshProUGUI _rewardsItemText;
    [SerializeField] private Image _rewardsSkillImage;
    [SerializeField] private TextMeshProUGUI _rewardsSkillText;
    [SerializeField] private GameObject _finalLevelPanel;
    [SerializeField] private GameObject _nextLevelButtonContainer;
    [SerializeField] private GameObject _headerText;

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {
        _rewardsItemImage.gameObject.SetActive(false);
        _rewardsItemText.gameObject.SetActive(false);
        _rewardsSkillImage.gameObject.SetActive(false);
        _rewardsSkillText.gameObject.SetActive(false);
        _finalLevelPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (GameManager.Instance == null) return;
        var levelDef = GameManager.Instance._currentLevelDef;

        if ((levelDef.IsLastLevel == false))
        {
            _finalLevelPanel.SetActive(false);

            if (levelDef.HasReward)
            {
                _rewardsItemImage.sprite = levelDef.RewardsItemSprite;
                _rewardsItemText.text = levelDef.RewardsItemText;
                _rewardsSkillImage.sprite = levelDef.RewardSkillSprite;
                _rewardsSkillText.text = levelDef.RewardSkillText;

                AudioManager.Instance.MusicSource.volume = AudioManager.Instance.MusicSource.volume * 50 / 100;

                StartCoroutine(DelayShowRewardItem(levelDef));
                StartCoroutine(DelayShowRewardSkill(levelDef));
            }
            else
            {
                if (levelDef.NumberOfLemonsAdded > 0)
                {
                    _rewardsItemImage.sprite = levelDef.RewardsItemSprite;
                    _rewardsItemText.text = levelDef.RewardsItemText;

                    AudioManager.Instance.MusicSource.volume = AudioManager.Instance.MusicSource.volume * 50 / 100;

                    StartCoroutine(DelayShowRewardItem(levelDef));

                    GameManager.Instance.LemonCount += levelDef.NumberOfLemonsAdded;
                    GameManager.Instance.LemonsAtHomeForLevelCount = GameManager.Instance.LemonCount;
                }
            }

        }
        else
        {
            _finalLevelPanel.SetActive(true);
            _nextLevelButtonContainer.SetActive(false);
            _headerText.SetActive(false);
            this.gameObject.SetActive(true);
        }
    }


    private void OnDisable()
    {
        _rewardsItemImage.gameObject.SetActive(false);
        _rewardsItemText.gameObject.SetActive(false);
        _rewardsSkillImage.gameObject.SetActive(false);
        _rewardsSkillText.gameObject.SetActive(false);
        _finalLevelPanel.SetActive(false);
        _nextLevelButtonContainer.SetActive(true);
        _headerText.SetActive(true);
    }

    #endregion



    #region Local Members

    private IEnumerator DelayShowRewardItem(LevelDefinition def)
    {
        yield return new WaitForSeconds(1f);

        AudioManager.Instance.PlaySFX(SFXSoundsEnum.RewardItem);


        _rewardsItemImage.gameObject.SetActive(true);
        _rewardsItemText.gameObject.SetActive(true);
    }

    private IEnumerator DelayShowRewardSkill(LevelDefinition def)
    {
        yield return new WaitForSeconds(3f);

        AudioManager.Instance.PlaySFX(def.RewardSkillSFX);

        _rewardsSkillImage.gameObject.SetActive(true);
        _rewardsSkillText.gameObject.SetActive (true);

        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlaySFX(def.RewardSkillSFX);
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlaySFX(def.RewardSkillSFX);
    }

    #endregion

}
