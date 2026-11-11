using System;
using UnityEngine;

// gathers the closest resource with E
[RequireComponent(typeof(PlayerInventory))]
public class PlayerGatherer : MonoBehaviour
{
    private const string ResourceTag = "Resource";

    [Header("Gathering")]
    [SerializeField] private float gatherRadius = 2f;
    [SerializeField] private float gatherCooldown = 0.25f;
    [SerializeField] private float scanInterval = 0.1f;
    [SerializeField] private LayerMask resourceLayers = ~0;

    // nearby resource (or null)
    public ResourceNode CurrentTarget { get; private set; }

    public event Action<ResourceNode> TargetChanged;
    public event Action<ResourceType> Gathered;

    private PlayerInventory inventory;
    private PlayerControls controls;
    private readonly Collider[] hits = new Collider[16];
    private float nextGatherTime;
    private float nextScanTime;

    void Awake()
    {
        // inventory is on the same object
        inventory = GetComponent<PlayerInventory>();
        controls = new PlayerControls();
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
        // look for resources 10 times per second, not every frame
        if (Time.time >= nextScanTime)
        {
            nextScanTime = Time.time + scanInterval;
            RefreshTarget();
        }

        if (controls.Player.Interact.WasPressedThisFrame())
        {
            RefreshTarget();
            TryGather();
        }
    }

    public bool TryGather()
    {
        if (CurrentTarget == null || Time.time < nextGatherTime)
        {
            return false;
        }

        if (!CurrentTarget.TryGather(out ResourceType type))
        {
            return false;
        }

        nextGatherTime = Time.time + gatherCooldown;
        inventory.Add(type, 1);
        Gathered?.Invoke(type);
        return true;
    }

    private void RefreshTarget()
    {
        ResourceNode closest = FindClosestResource();

        // ReferenceEquals also catches destroyed resources
        if (!ReferenceEquals(closest, CurrentTarget))
        {
            CurrentTarget = closest;
            TargetChanged?.Invoke(CurrentTarget);
        }
    }

    private ResourceNode FindClosestResource()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, gatherRadius, hits, resourceLayers,
            QueryTriggerInteraction.Collide);

        ResourceNode closest = null;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            Collider hit = hits[i];
            if (!hit.CompareTag(ResourceTag) || !hit.TryGetComponent(out ResourceNode node))
            {
                continue;
            }

            float distance = (hit.ClosestPoint(transform.position) - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = node;
            }
        }
        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, gatherRadius);
    }
}
