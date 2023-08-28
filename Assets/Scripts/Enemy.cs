using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private float _speed = 4f;
    [SerializeField] private Boundary _movementBoundary;
    [Tooltip("Points value rewarded to the player to add to their total score")]
    [SerializeField] private int _points = 10;

    [SerializeField] private Animator _enemyAnimator;
    [SerializeField] private AnimationClip _enemyExplodeAnim;
    [Tooltip("Additional time to wait before the enemy is destroyed. Negative value decreases wait time")]
    [SerializeField] private float _enemyDestroyWaitOffset;
    [SerializeField] private AudioClip _explosionSfx;
    [SerializeField] private float _explosionSfxVolume = 0.15f;
    private float _enemyExplodeAnimLength;
    // TODO: Create PlayerDTO to replace private instances such as this
    // TODO: Define values of DTOs required in the scene in a central scene manager
    private Player _player;
    private GameManager _gameManager;
    private AudioManager _audioManager;

    void Start() {
        // TODO: Get playerDTO instead of finding a gameobject with the certain script component
        _player = GameObject.Find("Player").GetComponent<Player>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        _enemyExplodeAnimLength = _enemyExplodeAnim.length;
    }
    void Update() {
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
                // Disable script to avoid any triggers when player gets hit and the enemy is already dead
                // GetComponent<Enemy>().enabled = false;
                // Disable collider to avoid any further triggers
                // GetComponent<Collider2D>().enabled = false;

                ExplodeEnemy();              
            } else if (other.tag == "Laser") {
                if (_player != null) { 
                    _player.UpdateScore(_points);
                }
                // Disable collider to avoid any further triggers
                // GetComponent<Collider2D>().enabled = false;              
                Destroy(other.gameObject);

                ExplodeEnemy();               
            } else if(other.tag == "Asteroid") {
                ExplodeEnemy();
            }
        } catch (Exception e) {
            // TODO: Display error message on screen
            Debug.Log(e.Message);
        }
        
    }

    private void ExplodeEnemy() {
        // Disable collider to avoid any further triggers
        GetComponent<Collider2D>().enabled = false;

        // Disable enemy movement since it is already dead and avoid colliding with the player
        // If player collides with the explosion, they can still lose HP
        // TODO: Try slowly reducing speed instead of suddenly pausing? Something with just stopping there and exploding looks off
        _speed = 0;

        // Trigger animation for enemy death
        _enemyAnimator.SetTrigger("OnEnemyDeath");
        _audioManager.PlaySoundEffect(_explosionSfx, _explosionSfxVolume);
        Destroy(this.gameObject, _enemyExplodeAnimLength + _enemyDestroyWaitOffset);
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
            } else if (_gameManager.GetGameOver()) {
                Destroy(this.gameObject);
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
