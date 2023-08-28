using System;
using UnityEngine;

public enum PowerupType { 
    TRIPLESHOT,
    SHIELD,
    SPEED
}
public class Powerup : MonoBehaviour {

    [SerializeField] private float _movementSpeed;
    [Tooltip("Boundaries for spawning and destroying powerup")]
    [SerializeField] private Boundary _movementBoundary;
    [SerializeField] private PowerupType _powerupType;

    private GameManager _gameManager;
    void Start() { 
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Update() {
        Travel();
        Cleanup();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        ResolveCollision(other);
    }

    private void Travel() {
        try {
            // Move down at a given speed
            transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
        
    }

    private void Cleanup() {
        try {
            if (transform.position.y < _movementBoundary.minY || _gameManager.GetGameOver()) {
                Destroy(this.gameObject);
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
    }

    private void ResolveCollision(Collider2D other) {
        try {
            if (other.tag == "Player") {
                // Collect powerup and enable collected powerup for the player
                other.GetComponent<Player>().EnablePowerup(_powerupType);
                Destroy(this.gameObject);
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }
        
    }

    public Boundary GetMovementBoundary() {
        return _movementBoundary;
    }
    
}
