using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Projectile types
 1 = Default 
 2 = Dream
 3 = Plasma
 */

public class PowerUp: MonoBehaviour {
    public float speed;
    public int powerUpType;
    public Color defaultColor;
    public Color dreamColor;
    public Color plasmaColor;
    private SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 10);
        this.powerUpType = Random.Range(1, 4);
        switch (this.powerUpType) {
            case 1:
                spriteRenderer.color = defaultColor;   
                break;
            case 2:
                spriteRenderer.color = dreamColor;   
                break;
            case 3:
                spriteRenderer.color = plasmaColor;   
                break;
            default:
                spriteRenderer.color = defaultColor;  
                break;
        }
    }

    void Update() {         
        transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
    }

    void OnPlayerContact(Player player) {
        player.Upgrade(this.powerUpType);    
    }
}
