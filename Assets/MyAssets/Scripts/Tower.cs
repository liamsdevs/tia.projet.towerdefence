using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost;
    [SerializeField] private int damage;
    [SerializeField] private float fireRate; // in projectiles per minute
    [SerializeField] private float turnSpeed;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float range;
    [SerializeField] private GameObject turret;
    [SerializeField] private Transform projectileSocket;
    [SerializeField] private GameObject projectilePrefab;

    private GameObject currentTarget;
    private Coroutine shootingCoroutine;

    public int GetCost()
    {
        return cost;
    }

    private void Start()
    {
        shootingCoroutine = StartCoroutine(ShootingRoutine());
    }

    private void Update()
    {
        CheckForTarget();
        if (currentTarget != null)
        {
            LookAtTarget();
        }
    }

    private IEnumerator ShootingRoutine()
    {
        float fireInterval = 60f / fireRate; // convert fireRate to seconds per shot
        while (true)
        {
            if (currentTarget != null && IsTargetInRange(currentTarget))
            {
                Shoot();
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }

    public void Shoot()
    {
        Debug.Log("Shooting");
        GameObject projectile = Instantiate(projectilePrefab, projectileSocket.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetDamage(damage);
        projectile.GetComponent<Projectile>().SetSpeed(projectileSpeed);
        projectile.GetComponent<Projectile>().SetTarget(currentTarget);
    }

    private void CheckForTarget()
    {
        if (currentTarget == null || !IsTargetInRange(currentTarget))
        {
            currentTarget = FindNewTarget();
        }
    }

    private bool IsTargetInRange(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) <= range;
    }

    private GameObject FindNewTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log("Found target");
                return collider.gameObject;
            }
        }
        return null;
    }

    private void LookAtTarget()
    {
        Vector3 direction = currentTarget.transform.position - turret.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(turret.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        turret.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}