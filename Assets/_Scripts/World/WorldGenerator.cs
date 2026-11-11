using System.Collections.Generic;
using UnityEngine;

// places resources randomly on the map at start
public class WorldGenerator : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject prefab;
        [Min(0)] public int count = 10;
    }

    [Header("What to spawn")]
    [SerializeField] private SpawnEntry[] entries;

    [Header("Spawn area")]
    [SerializeField] private Vector2 areaSize = new Vector2(56f, 56f);
    [SerializeField] private float minDistance = 2.5f;
    [SerializeField] private float safeRadius = 5f; // free space at spawn
    [SerializeField] private LayerMask blockingLayers = ~0;
    [SerializeField] private int maxAttempts = 30;

    [Header("Random")]
    [Tooltip("0 = new map every run")]
    [SerializeField] private int seed = 0;

    private readonly List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        if (seed != 0)
        {
            Random.InitState(seed);
        }
        usedPositions.Clear();

        foreach (SpawnEntry entry in entries)
        {
            if (entry.prefab == null)
            {
                continue;
            }

            for (int i = 0; i < entry.count; i++)
            {
                if (!TryFindFreePosition(out Vector3 position))
                {
                    break; // no more room
                }
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                Instantiate(entry.prefab, position, rotation, transform);
                usedPositions.Add(position);
            }
        }
    }

    private bool TryFindFreePosition(out Vector3 position)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 candidate = new Vector3(
                Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
                0f,
                Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f));

            if (IsFree(candidate))
            {
                position = candidate;
                return true;
            }
        }
        position = Vector3.zero;
        return false;
    }

    private bool IsFree(Vector3 candidate)
    {
        if (candidate.magnitude < safeRadius)
        {
            return false;
        }

        foreach (Vector3 used in usedPositions)
        {
            if ((used - candidate).sqrMagnitude < minDistance * minDistance)
            {
                return false;
            }
        }

        // not inside walls
        return !Physics.CheckSphere(candidate + Vector3.up * 1.2f, 0.9f, blockingLayers, QueryTriggerInteraction.Ignore);
    }

    // spawn area in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize.x, 0.1f, areaSize.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, safeRadius);
    }
}
