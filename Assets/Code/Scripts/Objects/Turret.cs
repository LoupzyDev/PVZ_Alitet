using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField] private float life;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject torretHead;
    [SerializeField] private Transform pivot;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float fireRate = 2f;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private List<GameObject> activeBullets = new List<GameObject>();

    private void Start() {
        for (int i = 0; i < poolSize; i++) {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }

        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine() {
        while (true) {
            yield return new WaitForSeconds(fireRate);
            CheckForTarget();
        }
    }

    private void CheckForTarget() {
        RaycastHit hit;
        Vector3 direction = pivot.forward;


        float maxDistance = Mathf.Abs(7f - pivot.position.x);

        if (Physics.Raycast(pivot.position, direction, out hit, maxDistance)) {
            float targetX = hit.point.x;


            if (targetX <= 7f && hit.collider.CompareTag("Enemy")) {
                Debug.DrawRay(pivot.position, direction * (targetX - pivot.position.x), Color.red);
                Debug.Log("Enemigo detectado dentro del rango. Disparando...");
                Shoot();
            }
        } else {
            Debug.DrawRay(pivot.position, direction * maxDistance, Color.green);
        }
    }

    private void Shoot() {
        if (bulletPool.Count == 0) {
            Debug.Log("No más balas disponibles en el pool");
            return;
        }

        GameObject bullet = GetBulletFromPool();
        if (bullet == null) return;

        bullet.transform.position = pivot.position;
        bullet.transform.rotation = pivot.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().linearVelocity = pivot.forward * bulletSpeed; 
        activeBullets.Add(bullet);
    }

    private GameObject GetBulletFromPool() {
        return bulletPool.Count > 0 ? bulletPool.Dequeue() : null;
    }

    public void ReturnBulletToPool(GameObject bullet) {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
        activeBullets.Remove(bullet);
    }
    public void TakeDamage(float damage) {

        life -= damage;
        if (life <= 0) {
            Die();
        }
    }
    private void Die() {
        Destroy(gameObject);
    }
    private IEnumerator RotateHead() {
        Quaternion initialRotation = torretHead.transform.rotation;
        Quaternion rightRotation = Quaternion.Euler(torretHead.transform.eulerAngles.x + 10, torretHead.transform.eulerAngles.y, torretHead.transform.eulerAngles.z);
        Quaternion leftRotation = Quaternion.Euler(torretHead.transform.eulerAngles.x - 10, torretHead.transform.eulerAngles.y, torretHead.transform.eulerAngles.z);

        yield return RotateToTarget(rightRotation);
        yield return RotateToTarget(leftRotation);
        yield return RotateToTarget(initialRotation);
    }

    private IEnumerator RotateToTarget(Quaternion targetRotation) {
        Quaternion startRotation = torretHead.transform.rotation;
        float elapsedTime = 0f;
        float rotationDuration = 0.2f;

        while (elapsedTime < rotationDuration) {
            torretHead.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        torretHead.transform.rotation = targetRotation;
    }
}
