using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {
    public EnemyPrefab prefab;
    public float intialDelay;
    public float enemyPeriod;
    public float enemyHeight;
    public Player player;

    void CreateEnemy() {
        var enemyPosition = Vector3.up * Random.Range(-30.0f, 30.0f);
        EnemyPrefab enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        enemy.transform.position += Vector3.up * Random.Range(-enemyHeight, enemyHeight);
        enemy.player = this.player;
    }

    void Activate() {
        InvokeRepeating("CreateEnemy", intialDelay, enemyPeriod);   
    }
}
