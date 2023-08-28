using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericExplosion : MonoBehaviour {
    [SerializeField] private AnimationClip _explosionAnim;
    private float _explosionAnimLength;

    void Start() {
        _explosionAnimLength = _explosionAnim.length;
        Destroy(this.gameObject, _explosionAnimLength);
    }
}
