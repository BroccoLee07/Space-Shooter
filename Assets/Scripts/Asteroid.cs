using UnityEngine;

public class Asteroid : MonoBehaviour {
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _rotateSpeed = 20f;
    [SerializeField] private Boundary _movementBounds;
    [Tooltip("Points value rewarded to the player to add to their total score")]
    [SerializeField] private int _points = 5;

    [SerializeField] private GameObject _explosionPrefab;
    
    [Tooltip("Additional time to wait before the asteroid is destroyed. Negative value decreases wait time")]
    [SerializeField] private float _asteroidWaitTimeOffset;
    [SerializeField] private AudioClip _explosionSfx;
    [SerializeField] private float _explosionSfxVolume = 0.15f;
    
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
    }

    void FixedUpdate() {
        Travel();
        RotateAnim();
        Cleanup();
    }

    public void OnTriggerEnter2D(Collider2D other) {                
        ResolveCollision(other);        
    }

    private void Travel() {
        // Move downward with fixedDeltaTime because FixedUpdate is used and ensures physics simulations are consistent
        Vector2 movement = Vector2.down * _movementSpeed * Time.fixedDeltaTime;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // MovePosition instead of Transform.Translate for better physics interaction
        rb.MovePosition(rb.position + movement);
    }

    private void RotateAnim() {
        transform.RotateAround(GetComponent<Renderer>().bounds.center, Vector3.forward, _rotateSpeed * Time.fixedDeltaTime);
    }

    private void ResolveCollision(Collider2D other) {
        // TODO: Set these tag names in a Constants file
        if (other.tag == "Laser") {
            if (_player != null) { 
                _player.UpdateScore(_points);
            }
            // TODO: Handle laser destruction in Laser script itself
            Destroy(other.gameObject);

            ExplodeAsteroid();
        } else if (other.tag == "Enemy") { 
            ExplodeAsteroid();
        } else if (other.tag == "Player") {
            // TODO: Get player script component and make player take damage
            Player player = other.GetComponent<Player>();
            player.TakeDamage();

            ExplodeAsteroid();
        }
    }

    private void ExplodeAsteroid() { 
        // TODO: Try slowly reducing speed instead of suddenly pausing? Something with just stopping there and exploding looks off
        _movementSpeed = 0;
        GetComponent<Collider2D>().enabled = false;
        // GetComponent<Animator>().SetTrigger("OnAsteroidCollisoin");
        // Destroy(this.gameObject, _explodeAnimLength + _asteroidWaitTimeOffset);

        // TODO: Create separate script for explosion itself
        GameObject explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.localScale = this.transform.localScale;

        _audioManager.PlaySoundEffect(_explosionSfx, _explosionSfxVolume);

        Destroy(this.gameObject, _asteroidWaitTimeOffset);
    }

    private void Cleanup() {
        if (transform.position.y <= _movementBounds.minY || _gameManager.GetGameOver()) {
            Destroy(this.gameObject);
        }
    }

    public Boundary GetMovementBoundary() {
        return _movementBounds;
    }

    public float GetRotateSpeed() {
        return _rotateSpeed;
    }

    public void SetRotateSpeed(float newRotateSpeed) {
        _rotateSpeed = newRotateSpeed;
    }
}
