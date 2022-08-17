using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int enemyDamage = 110;
    public int friendlyDamage = 40;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (this.gameObject.transform.position.y <= -3.7)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.Equals(8))
        {
            collision.gameObject.GetComponent<Hero>().TakeDamage(enemyDamage);

            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            Debug.Log("Hero damaged!");
        }
        else if (collision.gameObject.layer.Equals(9))
        {
            collision.gameObject.GetComponent<Bandit>().TakeDamage(friendlyDamage);

            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            Debug.Log("Bandit damaged!");
        }
    }
}
