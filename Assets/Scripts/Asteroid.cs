using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour {
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _rotateSpeed = 20f;
    [SerializeField] private Boundary _movementBounds;
    void FixedUpdate() {
        Travel();
        RotateAnim();
        Cleanup();
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
