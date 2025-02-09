using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController instance;
    
    PlayerControlls playerControlls;

    private float verticalInput;
    private float horizontalInput;
    private float moveAmount;

    private Vector2 movementInput;

    #region GettersAndSetters
    public float VerticalInput { get { return verticalInput; } }
    public float HorizontalInput { get { return horizontalInput; } }
    public float MoveAmount { get { return moveAmount; } }
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
}
