using Unity.VisualScripting;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private GameObject laser;
    [SerializeField] private float speed = 2f;
    private bool isCollision = false;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (isCollision)
        {
            speed = 0;
        } else
        {
            speed = 2;
        }

        rb.MovePosition(transform.position + -transform.right * speed * Time.deltaTime);
    }
    public void TakeDamage(float damage)
    {

        life -= damage;
        if (life <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            isCollision = true;
            collision.gameObject.GetComponent<Box>().TakeDamage(0.1f);
            isCollision = false;
        }
        if (collision.gameObject.CompareTag("Turret"))
        {
            isCollision = true;
            collision.gameObject.GetComponent<Turret>().TakeDamage(0.1f);
            isCollision = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TwoBullets"))
        {
            other.gameObject.GetComponentInParent<Turret>().ReturnBulletToPool(other.gameObject);
            TakeDamage(4);
        }
        if (other.gameObject.CompareTag("FourBullets"))
        {
            other.gameObject.GetComponentInParent<Turret>().ReturnBulletToPool(other.gameObject);
            TakeDamage(8);
        }
    }
}
