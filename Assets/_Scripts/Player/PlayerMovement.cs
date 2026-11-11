using UnityEngine;

// player movement (Input System + Rigidbody)
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.6f;
    [SerializeField] private float turnSpeed = 12f;

    private Rigidbody rb;
    private Transform cameraTransform;
    private PlayerControls controls;
    private Vector3 moveDirection;
    private bool isSprinting;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void OnDestroy()
    {
        controls.Dispose();
    }

    void Update()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        isSprinting = controls.Player.Sprint.IsPressed();

        // camera direction, flat on the ground
        Vector3 forward = cameraTransform != null ? cameraTransform.forward : Vector3.forward;
        Vector3 right = cameraTransform != null ? cameraTransform.right : Vector3.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // not faster on diagonals
        moveDirection = Vector3.ClampMagnitude(forward * input.y + right * input.x, 1f);
    }

    void FixedUpdate()
    {
        // stop leftover speed after hits, or it slides
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

        float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        rb.MovePosition(rb.position + moveDirection * (speed * Time.fixedDeltaTime));

        // turn towards the move direction
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }
    }
}
