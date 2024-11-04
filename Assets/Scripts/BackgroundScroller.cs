using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller: MonoBehaviour {
    public float speed;
    public Player player;

    void Update() {
        if (player.started) {
            transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= -18.0f) {
                transform.position = new Vector3(transform.position.x + 36.0f, 0.0f, 0.0f);        
            }
        }
    }
}