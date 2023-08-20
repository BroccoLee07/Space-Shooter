using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    [Header("Powerups")]
    [SerializeField] private GameObject _tripleShotPowerupPrefab;
    [SerializeField] private GameObject _shieldPowerupPrefab;
    [SerializeField] private GameObject _speedPowerupPrefab;
    // Creating container for powerup to reduce clutter esp if multiple powerups will be allowed to spawn
    // Multiple powerups can give some depth to the game to make the player choose what powerup to get
    [SerializeField] private GameObject _powerupContainer;

    [Header("Enemy spawn cooldown")]
    [Tooltip("Minimum spawn time for enemy (seconds)")]
    [SerializeField] private float _minEnemySpawnTime = 5f;
    [Tooltip("Maximum spawn time for enemy (seconds)")]
    [SerializeField] private float _maxEnemySpawnTime = 8f;

    [Header("Powerup spawn cooldown")]
    [Tooltip("Minimum spawn time for powerup (seconds)")]
    [SerializeField] private float _minPowerupSpawnTime = 10f;
    [Tooltip("Maximum spawn time for powerup (seconds)")]
    [SerializeField] private float _maxPowerupSpawnTime = 15f;

    private Enemy _enemy;
    private Powerup _tripleShotPowerup;
    private Powerup _shieldPowerup;
    private Powerup _speedPowerup;

    private bool _isSpawningEnemies;
    private bool _isSpawningPowerups;

    void Start() { 
        _enemy = _enemyPrefab.GetComponent<Enemy>();
        _tripleShotPowerup = _tripleShotPowerupPrefab.GetComponent<Powerup>();
        _shieldPowerup = _shieldPowerupPrefab.GetComponent<Powerup>();
        _speedPowerup = _speedPowerupPrefab.GetComponent<Powerup>();

        _isSpawningEnemies = true;
        _isSpawningPowerups = true;

        SubscribeToEvents();

        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnPowerupCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine() {
        while (_isSpawningEnemies) {
            SpawnEnemy();

            yield return new WaitForSeconds(GetRandomEnemySpawnTime());
        }
    }

    private IEnumerator SpawnPowerupCoroutine() {
        while (_isSpawningPowerups) {
            SpawnRandomPowerup();

            yield return new WaitForSeconds(GetRandomPowerupSpawnTime());
        }
    }

    private void SpawnEnemy() { 
        GameObject enemyObj = Instantiate(_enemyPrefab, GetRandomEnemySpawnPosition(_enemy), Quaternion.identity);
        enemyObj.transform.SetParent(_enemyContainer.transform);
    }

    private void SpawnRandomPowerup() {
        PowerupType powerupType = GetRandomPowerupType();
        GameObject powerupObj = null;

        switch (powerupType) { 
            case PowerupType.TRIPLESHOT:
                powerupObj = Instantiate(_tripleShotPowerupPrefab, GetRandomPowerupSpawnPosition(_tripleShotPowerup), Quaternion.identity);
                break;
            case PowerupType.SHIELD:
                powerupObj = Instantiate(_shieldPowerupPrefab, GetRandomPowerupSpawnPosition(_shieldPowerup), Quaternion.identity);
                break;
            case PowerupType.SPEED:
                powerupObj = Instantiate(_speedPowerupPrefab, GetRandomPowerupSpawnPosition(_speedPowerup), Quaternion.identity);
                break;
        }

        powerupObj.transform.SetParent(_powerupContainer.transform);        
    }

    private Vector3 GetRandomEnemySpawnPosition(Enemy enemy) {
        float randomX = UnityEngine.Random.Range(enemy.GetMovementBoundary().minX, enemy.GetMovementBoundary().maxX);
        return new Vector3(randomX, enemy.GetMovementBoundary().maxY, enemy.transform.position.z);
    }

    private Vector3 GetRandomPowerupSpawnPosition(Powerup powerup) {
        float randomX = UnityEngine.Random.Range(powerup.GetMovementBoundary().minX, powerup.GetMovementBoundary().maxX);
        return new Vector3(randomX, powerup.GetMovementBoundary().maxY, powerup.transform.position.z);
    }

    private float GetRandomEnemySpawnTime() {
        return UnityEngine.Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime);
    }

    private float GetRandomPowerupSpawnTime() {
        return UnityEngine.Random.Range(_minPowerupSpawnTime, _maxPowerupSpawnTime);
    }

    public void OnPlayerDeathEventHandler() {
        _isSpawningEnemies = false;
        _isSpawningPowerups = false;

        StopCoroutine(SpawnEnemyCoroutine());
        StopCoroutine(SpawnPowerupCoroutine());
        
        Cleanup();
    }

    private void Cleanup() {
        foreach (Transform enemyObj in _enemyContainer.transform) {
            Destroy(enemyObj.gameObject);
        }
    }

    private void SubscribeToEvents() {
        try {
            FindObjectOfType<Player>().OnPlayerDeath += OnPlayerDeathEventHandler;
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }

    public PowerupType GetRandomPowerupType() { 
        PowerupType[] values = (PowerupType[])Enum.GetValues(typeof(PowerupType));
        int randomIndex = UnityEngine.Random.Range(0, values.Length);
        return values[randomIndex];
    }

    
}
