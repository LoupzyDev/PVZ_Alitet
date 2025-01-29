using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    [SerializeField] private float speed = 10f; 
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        rb.MovePosition(transform.position + -transform.right * speed * Time.deltaTime);
    }


    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            collision.gameObject.GetComponent<Box>().TakeDamage(5);
        }
        if (collision.gameObject.CompareTag("Turret")) {
            collision.gameObject.GetComponent<Turret>().TakeDamage(5);
        }
    }
}
