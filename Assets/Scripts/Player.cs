using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] private float _speed = 3.5f;

    [Header("Player Movement Bounds")]
    [Tooltip("Movement limit for the top and right side of the screen")]
    [SerializeField] private Vector2 playerPositiveBounds;
    [Tooltip("Movement limit for the bottom and left side of the screen")]
    [SerializeField] private Vector2 playerNegativeBounds;


    public void Start() {
        transform.position = Vector3.zero;
    }

    public void Update() {
        CalculateMovement();
    }

    // Calculates movement of the player
    // Including speed, direction, and movement boundaries
    private void CalculateMovement() { 
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // Move player object based on real time (time between frames), 
        // speed multiplier, and player input for direction
        transform.Translate(direction * _speed * Time.deltaTime);

        // Player vertical movement restriction
        // If player object is moving vertically beyond player bounds, set player position to the limit as a restriction
        if (transform.position.y > playerPositiveBounds.y) {
            transform.position = new Vector3(transform.position.x, playerPositiveBounds.y, transform.position.z);
        } else if (transform.position.y <= playerNegativeBounds.y) {
            transform.position = new Vector3(transform.position.x, playerNegativeBounds.y, transform.position.z);
        }

        // Player horizontal movement restriction
        // If player object is moving horizontally beyond player bounds, set player position to the opposite limit 
        // as if teleporting or to have that effect where the ends of the screen are connected
        if (transform.position.x > playerPositiveBounds.x) {
            transform.position = new Vector3(playerNegativeBounds.x, transform.position.y, transform.position.z);
        } else if (transform.position.x <= playerNegativeBounds.x) {
            transform.position = new Vector3(playerPositiveBounds.x, transform.position.y, transform.position.z);
        }
    }
}
