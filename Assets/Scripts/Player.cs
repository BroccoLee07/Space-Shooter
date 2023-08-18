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

    [Header("Player Laser")]
    [SerializeField] private GameObject _laserPrefab;
    [Tooltip("Laser spawn distance from player")]
    // Setting laser offset here instead on in Laser script 
    // because the offset might be different for different enemies and the player itself
    [SerializeField] private float _laserOffset;  

    [Header("Player Movement Bounds")]
    // TODO: Replace bounds to be based on the actual device screen resolution
    [SerializeField] private Boundary _movementBoundary;

    // Player Events
    public event Action OnPlayerDeath;

    private float _nextLaserFireTime;
    private int _currentHP;

    public void Start() {
        transform.position = Vector3.zero;
        _currentHP = _maxHP;
    }

    public void Update() {
        CalculateMovement();

        // Spawn laser on space key press and after cooldown
        if (Input.GetKeyDown(KeyCode.Space) && CanFireLaser()) {
            FireLaser();
        }
    }

    public void FireLaser() {
        try { 
            // update next laser time to track cooldown before spawning the laser  
            _nextLaserFireTime = Time.time + _attackSpeed;
            Vector3 laserSpawnPosition = _laserPrefab.GetComponent<Laser>().GetLaserSpawnPosition(transform.position, _laserOffset);
            Instantiate(_laserPrefab, laserSpawnPosition, Quaternion.identity);
        } catch (Exception e) { 
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }        
    }

    public bool CanFireLaser() {
        return Time.time > _nextLaserFireTime;
    }

    // Calculates movement of the player
    // Taking note of speed, direction, and movement boundaries
    private void CalculateMovement() { 
        try { 
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            // Move player object based on real time (time between frames), 
            // speed multiplier, and player input for direction
            transform.Translate(direction * _movementSpeed * Time.deltaTime);

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
            // TODO: Handle different HP decrease for different enemies
            _currentHP -= 1;
            Debug.Log($"Current player HP: {_currentHP}");
            if (_currentHP <= 0) {  // Player lost all their HP
                Debug.Log("Game over");
                Destroy(this.gameObject);

                OnPlayerDeath?.Invoke();
            }
        } catch (Exception e) { 
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }
    }
}
