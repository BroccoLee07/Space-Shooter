using UnityEngine;

public class Asteroid : MonoBehaviour {
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _rotateSpeed = 20f;
    [SerializeField] private Boundary _movementBounds;

    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AnimationClip _explodeAnim;
    // [Tooltip("Additional time to wait before the asteroid is destroyed. Negative value decreases wait time")]
    // [SerializeField] private float _asteroidWaitTimeOffset;
    private float _explodeAnimLength;

    void Start() {
        _explodeAnimLength = _explodeAnim.length;
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
        if (other.tag == "Laser" || other.tag == "Enemy") {
            Destroy(other.gameObject);
            
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

        GameObject explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.localScale = this.transform.localScale;

        Destroy(this.gameObject);
        Destroy(explosion.gameObject, _explodeAnimLength);
    }

    private void Cleanup() {
        if (transform.position.y <= _movementBounds.minY) {
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
