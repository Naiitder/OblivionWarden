using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    PlayerControlls playerControlls;

    private float verticalInput;
    private float horizontalInput;
    private float moveAmount;

    private float lookVerticalInput;
    private float lookHorizontalInput;

    private Vector2 movementInput;
    private Vector2 lookInput;

    #region GettersAndSetters
    public float VerticalInput { get { return verticalInput; } }
    public float HorizontalInput { get { return horizontalInput; } }
    public float MoveAmount { get { return moveAmount; } }
    public float LookVerticalInput { get { return lookVerticalInput; } }
    public float LookHorizontalInput { get { return lookHorizontalInput; } }
    public Vector2 LookInput { get { return lookInput; } }
    #endregion

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

    }

    private void OnEnable()
    {
        if (playerControlls == null)
        {
            playerControlls = new PlayerControlls();

            playerControlls.Locomotion.Movement.started += onMovementInput;
            playerControlls.Locomotion.Movement.canceled += onMovementInput;
            playerControlls.Locomotion.Movement.performed += onMovementInput;
            playerControlls.Locomotion.Look.performed += onLookInput;
        }
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }

    void onLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
        lookHorizontalInput = lookInput.x;
        lookVerticalInput = lookInput.y;
    }
}