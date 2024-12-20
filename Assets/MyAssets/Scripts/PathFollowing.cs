using UnityEngine;
using System.Collections.Generic;
public class PathFollowing : MonoBehaviour
{
    private List<Vector2> path; // Liste des coordonnées X et Y
    public float speed = 0.5f; // Vitesse de déplacement
    private int currentPointIndex = 0;

    public void SetPath(List<Vector2> path)
    {
        this.path = path;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (path == null || path.Count == 0) return;

        Vector3 targetPosition = new Vector3(path[currentPointIndex].x, transform.localPosition.y, path[currentPointIndex].y);
        Vector3 currentPosition = transform.localPosition;
        Vector3 direction = (targetPosition - currentPosition).normalized;
        float distance = Vector3.Distance(currentPosition, targetPosition);

        transform.localPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        // Rotation fluide vers la direction du mouvement
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }

        if (distance < 0.0001f)
        {
            currentPointIndex++;
            if (currentPointIndex >= path.Count)
            {
                // L'ennemi a atteint la fin du chemin
                Destroy(gameObject);
            }
        }
    }
}