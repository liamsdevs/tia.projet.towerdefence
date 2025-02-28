using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    
    [SerializeField] private PathFollowing pathFollowing;
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private float enemySpeed = 1f;
    [SerializeField] private int reward = 10;

    void Start() {
        pathFollowing.SetSpeed(enemySpeed);
    }

    public void TakeDamage(int damage) {
        enemyHealth -= damage;
        if (enemyHealth <= 0) {
            Die();
        }
    }

    public void SetPath(List<Vector2> path) {
        pathFollowing.SetPath(path);
    }

    void Die() {
        GameManager.instance.GainMoney(reward);
        Destroy(gameObject);
    }
}