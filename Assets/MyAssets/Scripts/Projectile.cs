using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private float speed = 15f;
    [SerializeField]
    private GameObject ImpactEffect;


    private GameObject target;

    public void Seek(GameObject _target, int _damage, float _speed)
    {
        target = _target;
        damage = _damage;
        speed = _speed;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        target.GetComponent<EnemyManager>().TakeDamage(damage);
        GameObject effectIns = Instantiate(ImpactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);
        Destroy(gameObject);
    }
}