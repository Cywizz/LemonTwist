using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelDesign", menuName = "LemonTwist/LevelDesign", order = 1)]
public class LevelDefinition : ScriptableObject
{
    public string LevelName;
    public string SceneName;
    public int SpawnFrequencyInSeconds;
    public float LemonSpeed;

   
}
