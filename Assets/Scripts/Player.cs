using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerEvents { 
    public event Action OnPlayerDeath;
}

/// <summary>
/// Player class defines the behaviour and attributes of the player-controlled character.
/// </summary>
public class Player : MonoBehaviour, IPlayerEvents {

    // Player's attributes such as health points and speed
    [Header("Player Stats")]
    [SerializeField] private int _maxHP = 3;
    [SerializeField] private float _movementSpeed = 3.5f;
    [Tooltip("Cooldown for firing laser (seconds)")]
    [SerializeField] private float _attackSpeed = 0.5f;
    [Space(10)]

    // [Header("Player Ratings")]
    // [SerializeField] private int _score;
    // [Space(10)]

    [Header("Laser")]
    [SerializeField] private GameObject _defaultLaserPrefab;
    [SerializeField] private GameObject _tripleShotLaserPrefab;    

    [Tooltip("Laser spawn distance from player")]
    // Setting laser offset here instead on in Laser script 
    // because the offset might be different for different enemies and the player itself
    [SerializeField] private float _defaultLaserOffset;
    [SerializeField] private float _tripleShotLaserOffset;
    [Space(10)]

    [Header("Engine Fire")]
    [SerializeField] private GameObject[] _engineFires;

    [Header("Powerups")]
    [SerializeField] private bool _hasTripleShotPowerup;
    [SerializeField] private float _tripleShotPowerupActiveTime = 5f;
    [SerializeField] private bool _hasShieldPowerup;
    [SerializeField] private float _ShieldPowerupActiveTime = 3f;
    [SerializeField] private GameObject _shieldObject;
    [SerializeField] private bool _hasSpeedPowerup;    
    [SerializeField] private float _SpeedPowerupActiveTime = 4f;
    [SerializeField] private float _speedPowerupMovementSpeed = 5f;
    [Space(10)]

    [Header("Player Movement Bounds")]
    // TODO: Replace bounds to be based on the actual device screen resolution
    [SerializeField] private Boundary _movementBoundary;

    [Header("Player SFX")]
    // TODO: Move this under laser script?
    [SerializeField] private AudioClip _laserSfx;
    [SerializeField] private float _laserSfxVolume = 0.25f;
    // TODO: Re-assess if necessary because other objects already play the sound effect on death
    // TODO: Fix issue where laser or explosion sound gets cut off because either the laser is shot right after explosion or vice versa
    [SerializeField] private AudioClip _explosionSfx;
    [SerializeField] private float _explosionSfxVolume = 0.3f;

    // TODO: Move to a GameManager script to handle different events like player death or game over, etc
    // Player Events
    public event Action OnPlayerDeath;

    // TODO: Create userInterfaceDTO (or some other name) to replace private instances such as this
    // TODO: Define values of DTOs required in the scene in a central scene manager
    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioManager _audioManager;

    private float _nextLaserFireTime;
    private int _currentHP;
    private int _score;

    void Start() {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // Initialize player
        transform.position = Vector3.zero;
        foreach (GameObject engineFire in _engineFires) { 
            engineFire.SetActive(false);
        }
        _nextLaserFireTime = 0;
        _currentHP = _maxHP;
        _score = 0;

        // Initialize UI elements
        _uiManager.DisplayGameStartText(true);
        _uiManager.DisplayGameOverText(false);
        _uiManager.SetHPBarSprite(_currentHP);

        _gameManager.SetGameStart(false);
    }

    void Update() {
        if (_gameManager.GetGameStart()) { 
            CalculateMovement();
        }

        // Spawn laser on space key press and after cooldown
        if (Input.GetKeyDown(KeyCode.Space) && CanFireLaser() && _gameManager.GetGameStart()) {
            FireLaser();
        } else if (!_gameManager.GetGameStart() && Input.GetKeyDown(KeyCode.Return)) {
            StartGame();
        }
    }

    private void StartGame() {
        _gameManager.SetGameStart(true);
        _uiManager.DisplayGameStartText(false);
        StartCoroutine(_spawnManager.StartSpawning());
    }

    private void FireLaser() {
        try { 
            // Update next laser time to track cooldown before spawning the laser  
            _nextLaserFireTime = Time.time + _attackSpeed;

            // TODO: instantiate all lasers in a container for less clutter
            if (_hasTripleShotPowerup) {
                Vector3 laserSpawnPosition = _tripleShotLaserPrefab.GetComponentInChildren<Laser>().GetLaserSpawnPosition(transform.position, _tripleShotLaserOffset);
                Instantiate(_tripleShotLaserPrefab, laserSpawnPosition, Quaternion.identity);
            } else {
                Vector3 laserSpawnPosition = _defaultLaserPrefab.GetComponent<Laser>().GetLaserSpawnPosition(transform.position, _defaultLaserOffset);
                Instantiate(_defaultLaserPrefab, laserSpawnPosition, Quaternion.identity);
            }

            // Play SFX for laser
            _audioManager.PlaySoundEffect(_laserSfx, _laserSfxVolume);
        } catch (Exception e) { 
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }        
    }

    private bool CanFireLaser() {
        return Time.time > _nextLaserFireTime;
    }

    // Calculates movement of the player
    // Taking note of speed, direction, and movement boundaries
    private void CalculateMovement() { 
        try { 
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            // Move player object based on real time (time between frames), speed multiplier, and player input for direction
            // Speed changes based on whether or not speed powerup is enabled
            if (_hasSpeedPowerup) {
                transform.Translate(direction * _speedPowerupMovementSpeed * Time.deltaTime);
            } else {
                transform.Translate(direction * _movementSpeed * Time.deltaTime);
            }            

            // Player vertical movement restriction
            // If player object is moving vertically beyond player bounds, set player position to the limit as a restriction
            transform.position = new Vector3(
                transform.position.x, 
                Mathf.Clamp(transform.position.y, _movementBoundary.minY, _movementBoundary.maxY),
                transform.position.z
            );

            // Player horizontal movement restriction
            // If player object is moving horizontally beyond player bounds, set player position to the opposite limit 
            // as if teleporting or to have that effect where the ends of the screen are connected
            if (transform.position.x > _movementBoundary.maxX) {
                transform.position = new Vector3(_movementBoundary.minX, transform.position.y, transform.position.z);
            } else if (transform.position.x <= _movementBoundary.minX) {
                transform.position = new Vector3(_movementBoundary.maxX, transform.position.y, transform.position.z);
            }
        } catch (Exception e) { 
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }
    }

    public void TakeDamage() {
        try {
            // With shield powerup, player does not take damage for a limited time
            if (_hasShieldPowerup) {
                return;
            }

            // TODO: Handle different HP decrease for different enemies
            _currentHP -= 1;
            _uiManager.SetHPBarSprite(_currentHP);

            if (_currentHP <= 0) {  // Player lost all their HP
                ResolvePlayerDeath();
            } else {
                DisplayEngineFire();
                _audioManager.PlaySoundEffect(_explosionSfx, _explosionSfxVolume);
            }
        } catch (Exception e) { 
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }
    }

    private void DisplayEngineFire() {
        // Enable fire engine gameobject to show animation of player getting hurt
        bool _isDisplayingNewEngineFire = false;
        
        do { 
            int randomIndex = UnityEngine.Random.Range(0, _engineFires.Length);

            if (!_engineFires[randomIndex].activeInHierarchy) {
                _engineFires[randomIndex].SetActive(true);
                _isDisplayingNewEngineFire = true;
            }

        } while (!_isDisplayingNewEngineFire);
    }

    private void ResolvePlayerDeath() { 
        Destroy(this.gameObject);
        _audioManager.PlaySoundEffect(_explosionSfx, _explosionSfxVolume);
        OnPlayerDeath?.Invoke();

        _uiManager.DisplayGameOverText(true);
        _gameManager.SetGameOver(true);
    }

    public void UpdateScore(int points) {
        _score += points;
        _uiManager.SetScoreNumberText(_score);
    }

#region PLAYER POWERUPS
    public void EnablePowerup(PowerupType powerupType) { 
        switch (powerupType) { 
            case PowerupType.TRIPLESHOT:
                EnableTripleShotPowerup();
                break;
            case PowerupType.SHIELD:
                EnableShieldPowerup();
                break;
            case PowerupType.SPEED:
                EnableSpeedPowerup();
                break;
        }
    }

    private void EnableTripleShotPowerup() {
        _hasTripleShotPowerup = true;

        StartCoroutine(TripleShotPowerdownCoroutine());
    }

    // Disables TripleShot powerup after a set amount of time
    private IEnumerator TripleShotPowerdownCoroutine() {
        yield return new WaitForSeconds(_tripleShotPowerupActiveTime);

        _hasTripleShotPowerup = false;
    }

    private void EnableShieldPowerup() {
        _hasShieldPowerup = true;
        _shieldObject.SetActive(true);
        
        StartCoroutine(ShieldPowerdownCoroutine());
    }

    private IEnumerator ShieldPowerdownCoroutine() {
        yield return new WaitForSeconds(_ShieldPowerupActiveTime);

        _shieldObject.SetActive(false);
        _hasShieldPowerup = false;
    }

    private void EnableSpeedPowerup() {
        _hasSpeedPowerup = true;

        StartCoroutine(SpeedPowerdownCoroutine());
    }

    private IEnumerator SpeedPowerdownCoroutine() {
        yield return new WaitForSeconds(_SpeedPowerupActiveTime);

        _hasSpeedPowerup = false;
    }

#endregion
}
