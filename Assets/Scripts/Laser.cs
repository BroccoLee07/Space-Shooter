using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum LaserType {
//     DEFAULT,
//     TRIPLESHOT
// }

// TODO: Rename to bullet
public class Laser : MonoBehaviour {

    [SerializeField] private float _movementSpeed = 4f;

    [Header("Laser Travel Bounds")]
    // TODO: Replace bounds to be based on the actual device screen resolution
    [Tooltip("Travel limit for the top of the screen")]
    [SerializeField] private float _laserTravelTopLimit = 9f;

    // private LaserType laserType;

    public void Update() {
        Travel();        
        Cleanup();
    }

    // public void Initialize(LaserType laserType) {
    //     this.laserType = laserType;
    // }

    private void Travel() {
        try { 
            // Move laser upward
            transform.Translate(Vector3.up * _movementSpeed * Time.deltaTime);
        } catch (Exception e) { 
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }
    }

    private void Cleanup() {
        try { 
            // Delete after a going out of the screen
            if (transform.position.y >= _laserTravelTopLimit) {
                Destroy(this.gameObject);
            }
        } catch (Exception e) { 
            // TODO: Display error message on screen
            Debug.Log(e.Message);
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
