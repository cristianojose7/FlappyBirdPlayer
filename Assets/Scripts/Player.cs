using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Projectile types
 1 = Default 
 2 = Dream
 3 = Plasma
 */

/**
 * Animation types
 1 = Hurt
 2 = Cure
 */

public class Player : MonoBehaviour {
    public bool started;
    public bool gameOver;
    public int health;
    public int maxHealth;
    public Projectile projectilePrefab;
    public float blinkCycleSeconds;
    public ParticleSystem deathParticleSystem;
    public int projectileType;
    public float timeBetweenDefaultBullets;
    public EnemySpawn enemySpawn;
    public PowerUpSpawn powerUpSpawn;
    public int points;
    public int phase;
    public Image[] hearts;
    private int animationType;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private float invisibilityTimer;
    private float animationTimer;
    private float launchTimer;

    void Start () {
        projectileType = 1;
        invisibilityTimer = 0;
        animationTimer = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.isKinematic = true;
    }

    void StartAllEvents() {
        enemySpawn.SendMessage("Activate");
        powerUpSpawn.SendMessage("Activate");
        started = true;
        rigidBody.isKinematic = false;
    }

    void Update() {
        for (int i = 0; i < hearts.Length; ++i) {
            if (i < health) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;            
            }
        }   

        if (gameOver) {
            rigidBody.velocity = Vector3.right * 5.5f;  
            return;
        }

        if (Input.GetButtonDown ("Fire1")) { 
            if (!started) {
                StartAllEvents();
            }

            if (rigidBody.velocity.y <= 0.0f) {
                rigidBody.velocity = Vector3.up * 5.5f;  
            } 
        }

        if (projectileType != 1 && Input.GetButtonDown ("Fire2")) { 
            LaunchProjectile();
        }

        if (projectileType == 1 && Input.GetButton("Fire2")) { 
            if (launchTimer <= 0.0f) {
                launchTimer = timeBetweenDefaultBullets;            
                LaunchProjectile();
            }
        }

        if (launchTimer > 0.0f) {
            launchTimer -= Time.deltaTime;
        }

        if (invisibilityTimer > 0.0f) {
            invisibilityTimer -= Time.deltaTime;
        }

        if (animationTimer > 0.0f) {
            animationTimer -= Time.deltaTime;
            if (animationTimer <= 0.0f) {
                animationType = 0;
            }
        }
    }

    void StopAllTimers() {
        animationType = 0;
        animationTimer = 0.0f;
        launchTimer = 0.0f;
        invisibilityTimer = 0.0f;
    }

    void LaunchProjectile() {
        Projectile first = null;
        Projectile second = null;
        switch (this.projectileType) {
            case 3: // Plasma 
                first = Instantiate(projectilePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                second = Instantiate(projectilePrefab, transform.position + Vector3.down * 0.5f, Quaternion.identity);
                first.SetPlasma();
                second.SetPlasma();   
                break;   
            case 2: // Dream
                first = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                second = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                first.SetDream(1.0f); // Inverted direction
                second.SetDream(-1.0f);
                break;   
            default:
            case 1: // Default
                first = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                first.SetDefault();
                break;     
        }
    }

    public void Hurt(int damageAmount) {
        if (invisibilityTimer > 0.0f) {
            return;        
        }
        health -= damageAmount;
        if (health <= 0) {
            Kill();
            return;
        }
        SetInvisible();
    }
    
    void SetCureSprite() {
        spriteRenderer.color = Color.green;
        Invoke("SetNormalSprite", blinkCycleSeconds);
    }

    void SetHurtSprite() {
        spriteRenderer.color = Color.red;
        Invoke("SetNormalSprite", blinkCycleSeconds);
    }

    void SetNormalSprite() {
        spriteRenderer.color = Color.white;
        if (animationTimer > 0) {
            switch (animationType) {
                case 1:
                    Invoke("SetHurtSprite", blinkCycleSeconds);
                    break;
                case 2:
                    Invoke("SetCureSprite", blinkCycleSeconds);
                    break;
                default:
                    break;
            }
        }
    }
    
    public void SetInvisible() {
        invisibilityTimer = 1.0f / 10.0f;
        animationTimer = 1.0f / 10.0f;
        animationType = 1;
        SetHurtSprite();
    }

    public void Kill() {
        health = 0;
        gameOver = true;
        deathParticleSystem.gameObject.SetActive(true);
        deathParticleSystem.transform.position = this.transform.position;
        deathParticleSystem.Play(); 
        this.gameObject.SetActive(false);
    }

    public void Upgrade(int type) {
        this.projectileType = type;
        animationTimer = 1.0f / 20.0f;
        animationType = 2;
        SetCureSprite();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Hurt(1);
        }

        if (other.gameObject.CompareTag("InstantDeath")) {
            Kill();
        }

        if (other.gameObject.CompareTag("PowerUp")) {
            other.gameObject.SendMessage("OnPlayerContact", this);
            Destroy(other.gameObject);
        }
    }

    public void OnEnemyDestroy() {
        points += 1;
        if (points >= 15) {
            Destroy(enemySpawn.gameObject);      
            Destroy(powerUpSpawn.gameObject);      
            //enemySpawn.SendMessage("Activate"); 
            animationTimer = 1.0f / 20.0f;
            animationType = 2;
            SetCureSprite();
            gameOver = true;
        }    
    }
}
