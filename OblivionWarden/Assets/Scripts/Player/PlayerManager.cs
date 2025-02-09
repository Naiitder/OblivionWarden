using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] GameObject prefabToSpawn;

    Animator animator;
    private int movementSpeedHash;
    private int basicAttackHash;
    private float attackInterval = 2.0f;
    [SerializeField] GameObject spawnPosition;

    [Header("PlayerFlags")]
    [SerializeField] bool isDead = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        movementSpeedHash = Animator.StringToHash("MovementSpeed");
        basicAttackHash = Animator.StringToHash("BasicAttack");
    }

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        playerMovement.HandleMovement();
        if (animator.GetBool(basicAttackHash)) playerMovement.HandleAimRotation(); else playerMovement.HandleRotation();
        UpdateMovementAnimationValues();
    }


    private IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(attackInterval);

            animator.SetBool(basicAttackHash, true); 

            yield return new WaitUntil(() => AnimationFinished(basicAttackHash));

            animator.SetBool(basicAttackHash, false);

            Instantiate(prefabToSpawn, spawnPosition.transform.position, transform.rotation);
        }
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
}
