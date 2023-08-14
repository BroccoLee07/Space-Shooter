using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float _speed = 4f;
    [SerializeField] private Boundary _movementBoundary;
    public void Update() {
        // Move enemy down 4 per second        
        Travel();

        // If it goes past the bottom of the screen, respawn at the top with a random x position
        Cleanup();
    }

    public void OnTriggerEnter(Collider other) {
        ResolveCollision(other);        
    }

    private void ResolveCollision(Collider other) { 
        // If enemy hit the player, damage player and destroy enemy
        // If enemy hit laser, destroy laser and destroy enemy
        if (other.tag == "Player") {
            // TODO: Reduce player HP by 1
            Destroy(this.gameObject);
        } else if (other.tag == "Laser") {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void Travel() {
        // Move enemy downward with speed of 4
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void Cleanup() {
        if (transform.position.y < _movementBoundary.minY) {
            float randomX = Random.Range(_movementBoundary.minX, _movementBoundary.maxX);
            transform.position = new Vector3(randomX, _movementBoundary.maxY, transform.position.z);
        }
    }
}
