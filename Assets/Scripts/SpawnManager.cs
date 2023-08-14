using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    // [Tooltip("Horizontal spawn position offset from enemy's movement boundary")]
    // [Min(0)]
    // [SerializeField] private float _enemySpawnHorizontalOffset = 2;

    [Header("Enemy spawn cooldown")]
    [Tooltip("Minimum spawn time for enemy (seconds)")]
    [SerializeField] private float _minEnemySpawnTime = 5f;
    [Tooltip("Maximum spawn time for enemy (seconds)")]
    [SerializeField] private float _maxEnemySpawnTime = 8f;

    private Enemy enemy;

    public void Start() { 
        enemy = _enemyPrefab.GetComponent<Enemy>();

        SubscribeToEvents();

        StartCoroutine(SpawnEnemyCoroutine());
    }
    public void Update() {
    }

    private IEnumerator SpawnEnemyCoroutine() {
        while (true) {
            SpawnEnemy();

            yield return new WaitForSeconds(GetRandomEnemySpawnTime());
        }
    }

    private void SpawnEnemy() { 
        GameObject enemyObj = Instantiate(_enemyPrefab, GetRandomEnemySpawnPosition(enemy), Quaternion.identity);
        enemyObj.transform.SetParent(_enemyContainer.transform);
    }

    private Vector3 GetRandomEnemySpawnPosition(Enemy enemy) {
        // float randomX = Random.Range(enemy.GetMovementBoundary().minX + _enemySpawnHorizontalOffset, enemy.GetMovementBoundary().maxX - _enemySpawnHorizontalOffset);
        float randomX = UnityEngine.Random.Range(enemy.GetMovementBoundary().minX, enemy.GetMovementBoundary().maxX);
        return new Vector3(randomX, enemy.GetMovementBoundary().maxY, enemy.transform.position.z);
    }

    private float GetRandomEnemySpawnTime() {
        return UnityEngine.Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime);
    }

    // TODO: Trigger when player HP is <= 0
    public void OnPlayerDeathEventHandler() {
        StopCoroutine(SpawnEnemyCoroutine());
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
}
