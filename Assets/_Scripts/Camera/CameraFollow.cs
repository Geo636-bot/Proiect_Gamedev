using UnityEngine;

// top-down camera that follows the player, with zoom
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float lookHeight = 1f;

    [Header("Zoom: offset from player (near, mid, far)")]
    [SerializeField] private Vector3[] zoomOffsets =
    {
        new Vector3(0f, 8f, -6f),
        new Vector3(0f, 12f, -9f),
        new Vector3(0f, 18f, -13f)
    };
    [SerializeField] private int startZoomLevel = 1;
    [SerializeField] private float zoomCooldown = 0.2f;

    [Header("Smoothing")]
    [SerializeField] private float followSharpness = 8f;

    private PlayerControls controls;
    private int zoomLevel;
    private Vector3 currentOffset;
    private float nextZoomTime;

    void Awake()
    {
        controls = new PlayerControls();
        zoomLevel = Mathf.Clamp(startZoomLevel, 0, zoomOffsets.Length - 1);
        currentOffset = zoomOffsets[zoomLevel];
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

    void Start()
    {
        // right position from the first frame
        if (target != null)
        {
            transform.position = target.position + currentOffset;
            transform.LookAt(target.position + Vector3.up * lookHeight);
        }
    }

    void Update()
    {
        float scroll = controls.Player.Zoom.ReadValue<float>();

        // one zoom step per scroll
        if (scroll == 0f || Time.unscaledTime < nextZoomTime)
        {
            return;
        }
        nextZoomTime = Time.unscaledTime + zoomCooldown;
        zoomLevel = scroll > 0f
            ? Mathf.Max(zoomLevel - 1, 0)
            : Mathf.Min(zoomLevel + 1, zoomOffsets.Length - 1);
    }

    // after the player has moved
    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        float k = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
        currentOffset = Vector3.Lerp(currentOffset, zoomOffsets[zoomLevel], k);

        Vector3 focus = target.position + Vector3.up * lookHeight;
        Vector3 desiredPosition = target.position + currentOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, k);

        // fixed rotation, so it does not turn when moving sideways
        transform.rotation = Quaternion.LookRotation(focus - desiredPosition);
    }
}
