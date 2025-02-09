using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7.5f;
    [SerializeField] private float rotationSpeed = 10f;
    CharacterController characterController;

    GameObject cameraObject;
    Transform myTransform;

    Animator animator;
    private int movementSpeedHash;

    private void Awake()
    {
        cameraObject = GameObject.Find("CinemachineCamera");
        characterController = GetComponent<CharacterController>();
        myTransform = transform;
        animator = GetComponent<Animator>();
        movementSpeedHash = Animator.StringToHash("MovementSpeed");
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        UpdateMovementAnimationValues();
    }

    public void HandleMovement()
    {
        Vector3 movement = new Vector3(InputController.instance.HorizontalInput*movementSpeed, 0,
            InputController.instance.VerticalInput*movementSpeed);
        characterController.Move(movement * Time.deltaTime);
    }

    public void HandleRotation()
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = InputController.instance.MoveAmount;

        targetDir = cameraObject.transform.forward * InputController.instance.VerticalInput;
        targetDir += cameraObject.transform.right * InputController.instance.HorizontalInput;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero) targetDir = myTransform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * Time.deltaTime);

        myTransform.rotation = targetRotation;
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
