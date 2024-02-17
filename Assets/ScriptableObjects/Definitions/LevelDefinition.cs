using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelDesign", menuName = "LemonTwist/LevelDesign", order = 1)]
public class LevelDefinition : ScriptableObject
{
    public int LevelNo;
    public string LevelName;
    public string SceneName;
    public int SpawnFrequencyInSeconds;
    public float LemonSpeed;
    public Vector2 SpawnDirection;

    public bool SpawnLeftAndRight;

    public LemonSkillsEnum[] Skills;

    public bool HasReward;
    public Sprite RewardsItemSprite;
    public string RewardsItemText;
    public Sprite RewardSkillSprite;
    public string RewardSkillText;
    public SFXSoundsEnum RewardSkillSFX;

    public int NumberOfLemonsAdded;
    public bool IsLastLevel;


}
