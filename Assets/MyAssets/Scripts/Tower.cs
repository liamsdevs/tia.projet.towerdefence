using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    public string enemyTag = "Enemy";
    [SerializeField] private int cost;
    [SerializeField] private int damage;
    [SerializeField] private float fireRate; // in projectiles per minute
    [SerializeField] private float turnSpeed;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float range = 15f;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    private GameObject target;
    private float fireRateInSeconds = 60f / 1f; // Convert fire rate to seconds
    private float fireCountdown = 0f;

    public int GetCost()
    {
        return cost;
    }

    private void Start()
    {
        fireRateInSeconds = 60f / fireRate; // Convert fire rate to seconds
        InvokeRepeating("UpdateTarget", 0f, 0.5f); // Update target every 0.5 seconds
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }
        LookAtTarget();

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = fireRateInSeconds;
        }

        fireCountdown -= Time.deltaTime;
    }

    public void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Seek(target, damage, projectileSpeed);
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = target.transform.position - turret.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(turret.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        turret.rotation = Quaternion.Euler(-90f, rotation.y, 0f);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}