using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    public PlayerStats CharacterStats;

    Animator animator;
    private int movementSpeedHash;
    private int basicAttackHash;
    private int deathHash;

    [Header("Projectile Settings")]
    [SerializeField] private int currentProjectileCount = 1;
    [SerializeField] private float projectileSpreadAngle = 15f;
    private float attackInterval = 2.0f;
    [SerializeField] GameObject spawnPosition;
    [SerializeField] GameObject prefabToSpawn;

    [Header("PlayerFlags")]
    [SerializeField] bool isDead = false;

    AudioSource audioSource;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        movementSpeedHash = Animator.StringToHash("MovementSpeed");
        basicAttackHash = Animator.StringToHash("BasicAttack");
        deathHash = Animator.StringToHash("isDead");
        CharacterStats = GetComponent<PlayerStats>();

        audioSource = GetComponent<AudioSource>();

        CharacterStats.OnDeath += Die;
    }

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        if (CharacterStats.IsDead) return;
        playerMovement.HandleMovement();
        playerMovement.HandleAimRotation();
        if (!animator.GetBool(basicAttackHash)) playerMovement.HandleRotation(); else playerMovement.HandleAttackRotation();
        UpdateMovementAnimationValues();
    }


    private IEnumerator AttackRoutine()
    {
        while (!CharacterStats.IsDead)
        {
            yield return new WaitForSeconds(attackInterval);

            animator.SetBool(basicAttackHash, true); 

            yield return new WaitUntil(() => AnimationFinished(basicAttackHash));

            animator.SetBool(basicAttackHash, false);
            float startAngle = -(projectileSpreadAngle * (currentProjectileCount - 1)) / 2f;

            for (int i = 0; i < currentProjectileCount; i++)
            {
                Quaternion rotation = transform.rotation * Quaternion.Euler(0, startAngle + (projectileSpreadAngle * i), 0);
                GameObject projectile = Instantiate(prefabToSpawn, spawnPosition.transform.position, rotation);


                Projectile projectileScript = projectile.GetComponent<Projectile>();
                if (projectileScript != null)
                {
                    projectileScript.Damage = CharacterStats.Dmg;
                }
            }
            audioSource.Play();
        }
    }

    public void UpdateAttackInterval(float change)
    {
        attackInterval = Mathf.Max(0.2f, attackInterval + change);
    }

    public void UpdateProjectileCount(int increase)
    {
        currentProjectileCount += increase;
    }


    private bool AnimationFinished(int animationHash)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        return stateInfo.shortNameHash == animationHash && stateInfo.normalizedTime >= 1f;
    }

    public void UpdateMovementAnimationValues()
    {
        float v = 0;

        if (InputController.instance.MoveAmount > 0 && InputController.instance.MoveAmount < 0.55f) v = 0.5f;
        else if (InputController.instance.MoveAmount > 0.55f) v = 1;
        else if (InputController.instance.MoveAmount < 0 && InputController.instance.MoveAmount > -0.55f) v = -0.5f;
        else if (InputController.instance.MoveAmount < -0.55f) v = -1;
        else v = 0;

        animator.SetFloat(movementSpeedHash, v, 0.1f, Time.deltaTime);
    }

    void Die()
    {
        if (CharacterStats.IsDead) return;
        CharacterStats.IsDead = true;

        animator.SetBool(deathHash, true);

        GameController.instance.ActiveDeadPanel();
        GameController.instance.PlayAudioClip(GameController.instance.dieSound);
    }
}
