using UnityEngine;

public class EnemyManager : MonoBehaviour {
    
    [SerializeField] private PathFollowing pathFollowing;
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private int enemySpeed = 1;

    void Start() {}

    public void TakeDamage(int damage) {
        enemyHealth -= damage;
        if (enemyHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}