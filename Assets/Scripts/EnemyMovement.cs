using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement: MonoBehaviour {
    public float speed = 10.0f;

    void Start() {
        Destroy(this.gameObject, 10);
    }

    void Update() {         
        transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
    }
}
