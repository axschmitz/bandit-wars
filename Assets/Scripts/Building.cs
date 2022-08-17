using UnityEngine;

public class Building : MonoBehaviour
{
    public float attackRange = 8f;
    public int attackDamage = 50;
    public float attackSpeed = 0.25f;
    public int maxHealth = 200;

    int currentHealth;
    float nextAttackTime = 0f;

    public LayerMask enemyLayers;
    public GameObject arrow;

    public ParticleSystem particles;
    public TraumaInducer cameraShake;

    public Canvas UI;
    public Canvas EndScreen;

    void Start()
    {
        this.currentHealth = this.maxHealth;
    }

    void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            gameObject.transform.position, attackRange, enemyLayers);

        if (hitEnemies.Length >= 1)
        {
            if (Time.time >= nextAttackTime)
            {
                Attack(hitEnemies[0].gameObject.transform.position);
                nextAttackTime = Time.time + 1.0f / attackSpeed;
            }
        }
    }

    void Attack(Vector3 enemyPosition)
    {
        Vector2 position = gameObject.transform.position;
        float enemyDistance = Mathf.Abs(enemyPosition.x - position.x);

        if (enemyDistance > 3)
        {
            Arrow shotArrow = Instantiate(arrow, new Vector3(position.x, position.y + 6, -1f),
            Quaternion.identity).GetComponent<Arrow>();

            shotArrow.rb.AddForce(new Vector2(enemyDistance * 0.95f, 1), ForceMode2D.Impulse);
            Debug.Log("Arrow shot!");
        }
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;

        this.cameraShake.Play(.15f);

        if (this.currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(this.gameObject);

        this.cameraShake.Play(.4f);

        EndScreen.gameObject.SetActive(true);
        UI.gameObject.SetActive(false);
        FindObjectOfType<GameManager>().gameOver = true;
    }
}
