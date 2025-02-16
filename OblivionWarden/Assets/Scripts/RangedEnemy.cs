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
        if (isInRange() && !isAttacking && !CharacterStats.IsDead) StartRangedAttack();
        else if ((!isInRange() || CharacterStats.IsDead) && isAttacking) StopRangedAttack();
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
        ShootProjectile();
        InvokeRepeating(nameof(ShootProjectile), attackRate, attackRate);
    }

    void ShootProjectile()
    {
        if (player == null) return;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition.position, transform.rotation);
    }
}
