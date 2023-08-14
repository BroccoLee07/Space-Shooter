using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    // speed variable
    [SerializeField] private float _speed;

    [Header("Laser Travel Bounds")]
    // TODO: Replace bounds to be based on the actual device screen resolution
    [Tooltip("Travel limit for the top of the screen")]
    [SerializeField] private float _laserTravelTopLimit = 9f;
    
    public void Update() { 
        // Add movement to laser on spawn
        Travel();
        // Destroy laser object if it goes over the limit
        Cleanup();
    }

    private void Travel() {
        // Move laser upward
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    private void Cleanup() {
        // Delete after a going out of the screen
        if (transform.position.y >= _laserTravelTopLimit) {
            Destroy(this.gameObject);
        }
    }

    public Vector3 GetLaserSpawnPosition(Vector3 playerPosition, float laserOffset) {
        // Calculate spawn position and return value
        return CalculateLaserSpawnPosition(playerPosition, laserOffset);
    }

    private Vector3 CalculateLaserSpawnPosition(Vector3 playerPosition, float laserOffset) {
        // TODO: have checking for types of laser
        Vector3 _laserSpawnPosition = playerPosition + new Vector3(0, laserOffset, 0);
        return _laserSpawnPosition;
    }
}
