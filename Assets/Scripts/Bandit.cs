using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : MonoBehaviour
{

    public int maxHealth = 100;
    int currentHealth;
    public float movementSpeed = 60.0f;
    public float gravity = 4.0f;

    public int attackDamage = 20;
    public float attackRange = 0.5f;
    public float attackSpeed = 0.8f;
    float nextAttackTime = 0.0f;

    bool canMove = false;
    bool isDead = false;
    static bool warCryPlaying = false;

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
            || Physics2D.OverlapCircleAll(this.attackPoint.position, this.attackRange, this.friendlyLayers).Length > 1)
            && !this.isDead)
        {
            this.canMove = false;
            this.animator.SetFloat("Speed", 0);
            this.animator.SetBool("inCombat", true);
            //this.audioManager.Pause("FootStepsBandit");

            if (Time.time >= this.nextAttackTime 
                && Physics2D.OverlapCircleAll(this.attackPoint.position, this.attackRange, this.friendlyLayers).Length == 1)
            {
                this.Attack();
                this.nextAttackTime = Time.time + 1.0f / this.attackSpeed;
            }
        }
        else
        {
            this.canMove = true;
            this.animator.SetFloat("Speed", 1);
            this.animator.SetBool("inCombat", false);
        }

        // Destroy GameObject if it is not visible
        if (this.gameObject.transform.position.y < -10 && !isDead)
        {
            Destroy(this.gameObject);
        }

        if (!warCryPlaying)
        {
            FindObjectOfType<AudioManager>().Play("WarCry");
            warCryPlaying = true;
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
            this.rb.MovePosition(this.rb.position + new Vector2(-1.0f, Physics2D.gravity.y * Time.fixedDeltaTime * this.gravity)
                * this.movementSpeed * Time.fixedDeltaTime);
        }
    }

    void Attack()
    {
        this.animator.SetTrigger("Attack");
        FindObjectOfType<AudioManager>().Play("BanditAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(this.attackPoint.position, this.attackRange, this.enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {

            if (enemy.name == "HeroTower")
            {
                enemy.GetComponent<Building>().TakeDamage(this.attackDamage);
            }
            else
            {
                enemy.GetComponent<Hero>().TakeDamage(this.attackDamage);
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
        warCryPlaying = false;

        this.animator.SetTrigger("Die");
        //StartCoroutine(ExecuteAfterTime(0.5f));
        Destroy(this.gameObject.GetComponent<Rigidbody2D>());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());

        Destroy(this.gameObject, 2.0f);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        this.animator.SetTrigger("Hurt");
    }
}
