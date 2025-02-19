using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] float attackRate = 1f;

    bool isAttacking = false;

    int shottingHash;

    public override void Awake()
    {
        base.Awake();
        shottingHash = Animator.StringToHash("isShooting");
    }

    public override void Update()
    {
        base.Update();
        if (isInRange() && !isAttacking && !CharacterStats.IsDead && !animator.GetBool(basicAttackHash)) StartRangedAttack();
        else if ((!isInRange() || CharacterStats.IsDead) && isAttacking || animator.GetBool(basicAttackHash)) StopRangedAttack();
        if(isAttacking) RotateTowardsPlayer();
    }


    void StopRangedAttack()
    {
        isAttacking = false;
        animator.SetBool(shottingHash, false);
        CancelInvoke(nameof(ShootProjectile));
    }

    void StartRangedAttack()
    {
        isAttacking = true;
        animator.SetBool(walkingHash, false);
        animator.SetBool(shottingHash, true);
        InvokeRepeating(nameof(ShootProjectile), attackRate, attackRate);
    }

    void ShootProjectile()
    {
        if (player == null) return;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, transform.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Damage = CharacterStats.Dmg;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; 

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); 
    }
}
