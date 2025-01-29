using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("TwoBullets") || collision.gameObject.CompareTag("FourBullets"))
        {
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {   
            Destroy(collision.gameObject);
        }
    }
}
