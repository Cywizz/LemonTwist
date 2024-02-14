using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonSpawner : MonoBehaviour
{

    [SerializeField] private GameObject _lemonPrefab;

    private int _numberLemonsToSpawn;

    private int _spawnFrequencySeconds;


    private int _lemonsSpawned;

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {
        _spawnFrequencySeconds = GameManager.Instance._currentLevelDef.SpawnFrequencyInSeconds;
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

            _lemonsSpawned++;

            yield return new WaitForSeconds(_spawnFrequencySeconds);
        }

        



        
    }

    #endregion
}
