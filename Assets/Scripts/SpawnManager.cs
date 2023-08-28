using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [Header("Asteroid")]
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private GameObject _asteroidContainer;

    [Header("Enemy")]
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
    [Space(10)]

    [Header("Powerup spawn cooldown")]
    [Tooltip("Minimum spawn time for powerup (seconds)")]
    [SerializeField] private float _minPowerupSpawnTime = 10f;
    [Tooltip("Maximum spawn time for powerup (seconds)")]
    [SerializeField] private float _maxPowerupSpawnTime = 15f;
    [Space(10)]

    [Header("Asteroid spawn cooldown")]
    [Tooltip("Minimum spawn time for asteroid (seconds)")]
    [SerializeField] private float _minAsteroidSpawnTime = 3f;
    [Tooltip("Maximum spawn time for asteroid (seconds)")]
    [SerializeField] private float _maxAsteroidSpawnTime = 9f;
    [Tooltip("Minimum scaling of the asteroid when spawned")]
    [SerializeField] private float _minAsteroidSize = 0.2f;
    [Tooltip("Maximum scaling of the asteroid when spawned")]
    [SerializeField] private float _maxAsteroidSize = 1f;
    [Space(10)]

    private Asteroid _asteroid; 
    private Enemy _enemy;
    private Powerup _tripleShotPowerup;
    private Powerup _shieldPowerup;
    private Powerup _speedPowerup;

    private bool _isSpawningAsteroids;
    private bool _isSpawningEnemies;
    private bool _isSpawningPowerups;

    void Start() {
        _asteroid = _asteroidPrefab.GetComponent<Asteroid>();
        _enemy = _enemyPrefab.GetComponent<Enemy>();
        _tripleShotPowerup = _tripleShotPowerupPrefab.GetComponent<Powerup>();
        _shieldPowerup = _shieldPowerupPrefab.GetComponent<Powerup>();
        _speedPowerup = _speedPowerupPrefab.GetComponent<Powerup>();        

        SubscribeToEvents();
    }

    public void StartSpawning() { 
        _isSpawningAsteroids = true;
        _isSpawningEnemies = true;
        _isSpawningPowerups = true;

        StartCoroutine(SpawnAsteroidCoroutine());
        StartCoroutine(SpawnEnemyCoroutine());
        StartCoroutine(SpawnPowerupCoroutine());
    }

    private IEnumerator SpawnAsteroidCoroutine() {
        while (_isSpawningAsteroids) {
            SpawnAsteroid();

            yield return new WaitForSeconds(GetRandomAsteroidSpawnTime());
        }
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

    private void SpawnAsteroid() {
        
        GameObject asteroidObj = Instantiate(_asteroidPrefab, GetRandomAsteroidSpawnPosition(_asteroid), Quaternion.identity);

        float scale = GetRandomAsteroidScale();        
        asteroidObj.transform.localScale = new Vector3(scale, scale, scale);        
        // Affect asteroid's rotate speed with its scale
        asteroidObj.GetComponent<Asteroid>().SetRotateSpeed(_asteroid.GetRotateSpeed() / scale);

        asteroidObj.transform.SetParent(_asteroidContainer.transform);
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

    private Vector3 GetRandomAsteroidSpawnPosition(Asteroid asteroid) {
        float randomX = UnityEngine.Random.Range(asteroid.GetMovementBoundary().minX, asteroid.GetMovementBoundary().maxX);
        return new Vector3(randomX, asteroid.GetMovementBoundary().maxY, asteroid.transform.position.z);
    }
    private Vector3 GetRandomEnemySpawnPosition(Enemy enemy) {
        float randomX = UnityEngine.Random.Range(enemy.GetMovementBoundary().minX, enemy.GetMovementBoundary().maxX);
        return new Vector3(randomX, enemy.GetMovementBoundary().maxY, enemy.transform.position.z);
    }

    private Vector3 GetRandomPowerupSpawnPosition(Powerup powerup) {
        float randomX = UnityEngine.Random.Range(powerup.GetMovementBoundary().minX, powerup.GetMovementBoundary().maxX);
        return new Vector3(randomX, powerup.GetMovementBoundary().maxY, powerup.transform.position.z);
    }

    private float GetRandomAsteroidSpawnTime() {
        return UnityEngine.Random.Range(_minAsteroidSpawnTime, _maxAsteroidSpawnTime);
    }

    private float GetRandomEnemySpawnTime() {
        return UnityEngine.Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime);
    }

    private float GetRandomPowerupSpawnTime() {
        return UnityEngine.Random.Range(_minPowerupSpawnTime, _maxPowerupSpawnTime);
    }

    private float GetRandomAsteroidScale() {
        return UnityEngine.Random.Range(_minAsteroidSize, _maxAsteroidSize);
    }

    public void OnPlayerDeathEventHandler() {
        _isSpawningEnemies = false;
        _isSpawningPowerups = false;
        _isSpawningAsteroids = false;

        StopCoroutine(SpawnAsteroidCoroutine());
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
