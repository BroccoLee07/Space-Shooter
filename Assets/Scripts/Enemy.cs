using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float _speed;
    [SerializeField] private Boundary _movementBoundary;
    public void Update() {
        // Move enemy down 4 per second        
        Travel();

        // If it goes past the bottom of the screen, respawn at the top with a random x position
        Cleanup();
    }

    private void Travel() {
        // Move enemy downward with speed of 4
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void Cleanup() {
        if (transform.position.y < _movementBoundary.minY) {
            transform.position = new Vector3(Random.Range(_movementBoundary.minX, _movementBoundary.maxX),_movementBoundary.maxY, transform.position.z);
        }
    }
}
