using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public float movementSpeed = 50.0f;
    public float gravity = 4.0f;
    public int gold = 15;

    public int attackDamage = 35;
    public float attackRange = 0.5f;
    public float attackSpeed = 0.6f;
    float nextAttackTime = 0.0f;

    bool canMove = false;
    bool isDead = false;

    public Rigidbody2D rb;
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public LayerMask friendlyLayers;
    public ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        this.currentHealth = this.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Physics2D.OverlapCircleAll(this.attackPoint.position, this.attackRange, this.enemyLayers).Length > 0
            || Physics2D.OverlapCircleAll(this.attackPoint.position, this.attackRange, this.friendlyLayers).Length > 0)
            && !this.isDead)
        {
            this.canMove = false;
            this.animator.SetFloat("Speed", 0);

            if (Time.time >= this.nextAttackTime
                && Physics2D.OverlapCircleAll(this.attackPoint.position, this.attackRange, this.friendlyLayers).Length == 0)
            {
                this.Attack();
                this.nextAttackTime = Time.time + 1.0f / this.attackSpeed;
            }
        }
        else
        {
            this.canMove = true;
            this.animator.SetFloat("Speed", 1);
        }

        if (this.gameObject.transform.position.y < -10 && !isDead)
        {
            Destroy(this.gameObject);
        }

        if (this.gameObject.transform.rotation.x != 0 || this.gameObject.transform.rotation.y != 0
            || this.gameObject.transform.rotation.z != 0 || this.gameObject.transform.rotation.w != 0)
        {
            this.gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (this.canMove && !this.isDead)
        {
            this.rb.MovePosition(this.rb.position + new Vector2(1.0f, Physics2D.gravity.y * Time.fixedDeltaTime * this.gravity)
                * this.movementSpeed * Time.fixedDeltaTime);
        }
    }

    void Attack()
    {
        this.animator.SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("HeroAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(this.attackPoint.position, this.attackRange, this.enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.name == "BanditTower")
            {
                enemy.GetComponent<Tower>().TakeDamage(this.attackDamage);
            }
            else
            {
                enemy.GetComponent<Bandit>().TakeDamage(this.attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (this.attackPoint != null)
        {
            Gizmos.DrawWireSphere(this.attackPoint.position, this.attackRange);
        }
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;

        this.animator.SetTrigger("Hurt");

        this.particles.Play();

        if (this.currentHealth < 1)
        {
            this.Die();
        }
    }

    void Die()
    {
        this.isDead = true;
        this.canMove = false;

        this.animator.SetTrigger("Die");
        Destroy(this.gameObject.GetComponent<Rigidbody2D>());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());

        Destroy(this.gameObject, 2.0f);

        FindObjectOfType<GameManager>().AddGold(gold);
    }
}
