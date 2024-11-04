using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour {
    public PowerUp prefab;
    public float intialDelay;
    public float period;
    public float screenRange;

    void CreatePowerUp() {
        var position = transform.position + Vector3.up * Random.Range(-screenRange, screenRange);
        PowerUp good = Instantiate(prefab, position, Quaternion.identity);
    }

    void Activate() {
        InvokeRepeating("CreatePowerUp", intialDelay, period); 
    }
}
