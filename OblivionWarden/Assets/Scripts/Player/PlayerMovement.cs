using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7.5f;
    [SerializeField] private float rotationSpeed = 10f;
    CharacterController characterController;

    [SerializeField]LayerMask groundMask;
    Camera mainCamera;
    GameObject cameraObject;

    Transform myTransform;

    private void Awake()
    {
        cameraObject = GameObject.Find("CinemachineCamera");
        characterController = GetComponent<CharacterController>();
        myTransform = transform;
        mainCamera = Camera.main;
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



    public void HandleAimRotation()
    {
        Vector3 targetDir = Vector3.zero;

        if (InputController.instance.LookInput == Vector2.zero)
        {
            targetDir = GetMouseWorldPosition() - myTransform.position;
            targetDir.y = 0; 
        }
        else 
        {
            targetDir = new Vector3(InputController.instance.LookHorizontalInput,
                0, InputController.instance.LookVerticalInput);
        }

        if (targetDir.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            return hit.point; 
        }
        return myTransform.position; 
    }

}
