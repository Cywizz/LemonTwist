using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonSpawner : MonoBehaviour
{

    [SerializeField] private GameObject _lemonPrefab;

    private int _numberLemonsToSpawn;

    private int _spawnFrequencySeconds;
    private LevelDefinition _levelDef;

    private bool _lastSpawnLeft;


    private int _lemonsSpawned;

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {
        _levelDef = GameManager.Instance._currentLevelDef;
        _spawnFrequencySeconds = _levelDef.SpawnFrequencyInSeconds;
        _numberLemonsToSpawn = GameManager.Instance.LemonCount;

        StartCoroutine(SpawnLemons());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #endregion


    #region Local Members

    private IEnumerator SpawnLemons()
    {

        yield return new WaitForSeconds(2f);
        
        _lemonsSpawned = 0;

        while (_lemonsSpawned < _numberLemonsToSpawn)
        {
            var newLemon = Instantiate(_lemonPrefab, this.transform);
            var lemonController = newLemon.GetComponent<LemonGameController>();

            if (_levelDef.SpawnLeftAndRight == false)
            {
                lemonController.SetPrimaryDirection(GameManager.Instance._currentLevelDef.SpawnDirection);
            }
            else
            { //alternate spawn direction
                if(_lastSpawnLeft)
                {
                    lemonController.SetPrimaryDirection(Vector2.right);
                    _lastSpawnLeft = false;
                }
                else
                {
                    lemonController.SetPrimaryDirection(Vector2.left);
                    _lastSpawnLeft = true;
                }
                    
            }

            _lemonsSpawned++;

            AudioManager.Instance.PlaySFX(SFXSoundsEnum.LemonSpawns);

            yield return new WaitForSeconds(_spawnFrequencySeconds);
        }

        



        
    }

    #endregion
}
