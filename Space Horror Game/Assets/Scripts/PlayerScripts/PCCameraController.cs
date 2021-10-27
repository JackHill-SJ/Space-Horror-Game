using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCCameraController : MonoBehaviour
{
    [SerializeField] InputActionAsset playerControls;
    InputAction LookAction;
    Vector3 lookDirection;

    public float RotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;

    private void Awake()
    {
        var PlayerActionMap = playerControls.FindActionMap("Player");
        LookAction = PlayerActionMap.FindAction("Look");

        LookAction.performed += LookInput;
        LookAction.canceled += LookInput;
    }
    private void OnEnable()
    {
        LookAction.Enable();
    }
    private void OnDisable()
    {
        LookAction.Disable();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        mouseX += lookDirection.x * (RotationSpeed * .5f);
        mouseY -= lookDirection.z * (RotationSpeed * .5f);
        mouseY = Mathf.Clamp(mouseY, -35, 60);

        transform.LookAt(Target);

        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Player.rotation = Quaternion.Euler(0, mouseX, 0);
    }

    public void LookInput(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        lookDirection = new Vector3(direction.x, 0, direction.y);
    }
}
