using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    [SerializeField] protected PlayerManager player;

    public CharacterStats CharacterStats { get; set; }

    [SerializeField] float attackRange = 2f;
    [SerializeField] float movementSpeed = 3.5f;
    [SerializeField] int scoreDrop = 10;

    protected int walkingHash; 
    protected int basicAttackHash; 
    protected int deathHash; 

    [SerializeField] float dmgRate = 1f;

   protected NavMeshAgent enemyAgent;

    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        CharacterStats = GetComponent<CharacterStats>();
        enemyAgent = GetComponent<NavMeshAgent>();

        walkingHash = Animator.StringToHash("isWalking");
        basicAttackHash = Animator.StringToHash("isAttacking");
        deathHash = Animator.StringToHash("isDead");

        enemyAgent.stoppingDistance = attackRange;
        enemyAgent.speed = movementSpeed;

        CharacterStats.OnDeath += Die;
    }

    public virtual void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (isInRange() || CharacterStats.IsDead) return;

        enemyAgent.destination = player.transform.position;
        enemyAgent.stoppingDistance = attackRange;
        animator.SetBool(walkingHash, true);

    }

    void DealMeleeDmg()
    {
        player.CharacterStats.TakeDamage(CharacterStats.Dmg);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !CharacterStats.IsDead)
        {
            animator.SetBool(basicAttackHash, true);
            animator.SetBool(walkingHash, false);
            InvokeRepeating(nameof(DealMeleeDmg),0,dmgRate);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CancelInvoke(nameof(DealMeleeDmg));
            animator.SetBool(basicAttackHash, false);
        }
    }

    protected bool isInRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            return true;
        }
        else return false;
    }

    protected void Die()
    {
        if (CharacterStats.IsDead) return;
        CharacterStats.IsDead = true;

        enemyAgent.isStopped = true;
        enemyAgent.enabled = false;

        CancelInvoke(nameof(DealMeleeDmg));
        animator.SetBool(deathHash, true);

        GameController.instance.UpdateScore(scoreDrop);

        Destroy(gameObject, 2f);
    }


}
