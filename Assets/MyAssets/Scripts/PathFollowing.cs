using UnityEngine;
using System.Collections.Generic;

public class PathFollowing : MonoBehaviour {
    public List<Vector2> path; // Liste des coordonnées X et Y
    public float speed = 0.5f; // Vitesse de déplacement
    private int currentPointIndex = 0; 

    void Update() {
    if (path == null || path.Count == 0) return;

    Vector3 targetPosition = new Vector3(path[currentPointIndex].x, transform.position.y, path[currentPointIndex].y);
    Vector3 currentPosition = transform.position;
    Vector3 direction = (targetPosition - currentPosition).normalized;
    float distance = Vector3.Distance(currentPosition, targetPosition);

   
    transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);


    if (distance < 0.05f) {
        currentPointIndex++;
        if (currentPointIndex >= path.Count) {
            // L'ennemi a atteint la fin du chemin
            Destroy(gameObject);
        }
    }
}
}