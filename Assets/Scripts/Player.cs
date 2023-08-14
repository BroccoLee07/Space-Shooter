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

    [Header("Player Movement Bounds")]
    // TODO: Replace bounds to be based on the actual device screen resolution
    [Tooltip("Movement limit for the top and right side of the screen")]
    [SerializeField] private Vector2 _playerMovementMaxBounds = new Vector2(11f, 0f);
    [Tooltip("Movement limit for the bottom and left side of the screen")]
    [SerializeField] private Vector2 _playerMovementMinBounds = new Vector2(-11f, -3.8f);


    public void Start() {
        transform.position = Vector3.zero;
    }

    public void Update() {
        CalculateMovement();

        FireLaser();
    }

    public void FireLaser() { 
        // spawn laser on space key press
        // delete after a distance outside the device's screen resolution
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3 laserSpawnPosition = _laserPrefab.GetComponent<Laser>().GetLaserSpawnPosition(transform.position, _laserOffest);
            Instantiate(_laserPrefab, laserSpawnPosition, Quaternion.identity);
        }
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
            Mathf.Clamp(transform.position.y, _playerMovementMinBounds.y, _playerMovementMaxBounds.y),
            transform.position.z
        );

        // Player horizontal movement restriction
        // If player object is moving horizontally beyond player bounds, set player position to the opposite limit 
        // as if teleporting or to have that effect where the ends of the screen are connected
        if (transform.position.x > _playerMovementMaxBounds.x) {
            transform.position = new Vector3(_playerMovementMinBounds.x, transform.position.y, transform.position.z);
        } else if (transform.position.x <= _playerMovementMinBounds.x) {
            transform.position = new Vector3(_playerMovementMaxBounds.x, transform.position.y, transform.position.z);
        }
    }
}
