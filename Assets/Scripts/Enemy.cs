using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float _speed = 4f;
    [SerializeField] private Boundary _movementBoundary;
    [Tooltip("Points value rewarded to the player to add to their total score")]
    [SerializeField] private int _points = 10;

    // TODO: Create PlayerDTO to replace private instances such as this
    // TODO: Define values of DTOs required in the scene in a central scene manager
    private Player _player;

    public void Start() {
        // TODO: Get playerDTO instead of finding a gameobject with the certain script component
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void Update() {
        Travel();
        Cleanup();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        ResolveCollision(other);        
    }

    private void ResolveCollision(Collider2D other) {
        try {
            // If enemy hit the player, damage player and destroy enemy
            // If enemy hit laser, destroy laser and destroy enemy
            if (other.tag == "Player") {
                // TODO: Reduce player HP by 1
                Player player = other.GetComponent<Player>();
                if (player != null) { 
                    player.TakeDamage();
                } else {                    
                    throw new Exception("Missing Player script component");
                }
                Destroy(this.gameObject);
            } else if (other.tag == "Laser") {
                if (_player != null) { 
                    _player.UpdateScore(_points);
                }                
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        } catch (Exception e) {
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }
        
    }

    private void Travel() {
        try {
            // Move enemy downward with assigned speed
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        } catch (Exception e) {
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }
        
    }

    private void Cleanup() {        
        // If enemy goes past the bottom of the screen, respawn at the top with a random x position
        try {
            if (transform.position.y < _movementBoundary.minY) {
                float randomX = UnityEngine.Random.Range(_movementBoundary.minX, _movementBoundary.maxX);
                transform.position = new Vector3(randomX, _movementBoundary.maxY, transform.position.z);
            }
        } catch (Exception e) {
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }        
    }

    public Boundary GetMovementBoundary() {
        return _movementBoundary;
    }
}
