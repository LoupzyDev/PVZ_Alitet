using UnityEngine;

public class Box : MonoBehaviour {

    [SerializeField] private float life;

    public void TakeDamage(float damage) {

        life -= damage;
        if (life <= 0) {
            Die();
        }
    }
    private void Die() {
        Destroy(gameObject);
    }

}
