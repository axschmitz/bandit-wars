using UnityEngine;
using UnityEngine.UI;

public class BanditTower : Tower
{
    public Text endScreenText;

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

    public override void Attack(Vector3 enemyPosition)
    {
        Vector2 position = gameObject.transform.position;
        float enemyDistance = Mathf.Abs(enemyPosition.x - position.x);

        if (enemyDistance > 3)
        {
            Arrow shotArrow = Instantiate(arrow, new Vector3(position.x, position.y + 6, -1f),
            Quaternion.identity).GetComponent<Arrow>();

            shotArrow.rb.AddForce(new Vector2(enemyDistance * -0.95f, 1), ForceMode2D.Impulse);
            Debug.Log("Arrow shot!");
        }
    }

    public override void TakeDamage(int damage)
    {
        this.currentHealth -= damage;

        this.cameraShake.Play(.15f);

        if (this.currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Destroy(this.gameObject);

        this.cameraShake.Play(.4f);

        endScreen.gameObject.SetActive(true);
        endScreenText.text = "YOU LOST!!!";
        UI.gameObject.SetActive(false);
        FindObjectOfType<GameManager>().gameOver = true;
    }
}
