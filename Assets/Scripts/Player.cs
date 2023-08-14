using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float _speed = 3.5f;

    [Header("Player Laser")]
    [SerializeField] private GameObject _laserPrefab;
    [Tooltip("Laser spawn distance from player")]
    // Setting laser offest here instead on in Laser script 
    // because the offset might be different for different enemies and the player itself
    [SerializeField] private float _laserOffest;
    [Tooltip("Cooldown for firing laser (seconds)")]
    [SerializeField] private float _laserFireRate = 0.5f;

    [Header("Player Movement Bounds")]
    // TODO: Replace bounds to be based on the actual device screen resolution
    // [Tooltip("Movement limit for the top and right side of the screen")]
    // [SerializeField] private Vector2 _playerMovementMaxBounds = new Vector2(11f, 0f);
    // [Tooltip("Movement limit for the bottom and left side of the screen")]
    // [SerializeField] private Vector2 _playerMovementMinBounds = new Vector2(-11f, -3.8f);
    [SerializeField] private Boundary _movementBoundary;

    private float _nextLaserFireTime;


    public void Start() {
        transform.position = Vector3.zero;
    }

    public void Update() {
        CalculateMovement();

        // spawn laser on space key press and after cooldown
        if (Input.GetKeyDown(KeyCode.Space) && CanFireLaser()) {
            FireLaser();
        }
    }

    public void FireLaser() { 
        // update next laser time to track cooldown before spawning the laser  
        _nextLaserFireTime = Time.time + _laserFireRate;
        Vector3 laserSpawnPosition = _laserPrefab.GetComponent<Laser>().GetLaserSpawnPosition(transform.position, _laserOffest);
        Instantiate(_laserPrefab, laserSpawnPosition, Quaternion.identity);
    }

    public bool CanFireLaser() {
        return Time.time > _nextLaserFireTime;
    }

    // Calculates movement of the player
    // Taking note of speed, direction, and movement boundaries
    private void CalculateMovement() { 
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // Move player object based on real time (time between frames), 
        // speed multiplier, and player input for direction
        transform.Translate(direction * _speed * Time.deltaTime);

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
    }
}
