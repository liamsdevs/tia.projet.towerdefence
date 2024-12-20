using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private float speed;

    private GameObject target;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null)
        {
            // aller tout droit dans la direction actuelle du canon
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, distanceThisFrame);
    }

    void HitTarget()
    {
        Debug.Log("Hit target");
        Destroy(gameObject);
        target.GetComponent<EnemyManager>().TakeDamage(damage);
    }
}