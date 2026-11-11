using System.Collections;
using UnityEngine;

// resource on the map (tree, rock, bush)
public class ResourceNode : MonoBehaviour
{
    [Header("Resource")]
    [SerializeField] private ResourceType resourceType = ResourceType.Wood;
    [SerializeField] private string displayName = "Tree";
    [SerializeField, Min(1)] private int amount = 3;

    [Header("Gather feedback")]
    [SerializeField] private float punchScale = 0.15f;
    [SerializeField] private float punchDuration = 0.15f;

    private Vector3 initialScale;
    private Coroutine punchRoutine;

    public ResourceType Type => resourceType;
    public string DisplayName => displayName;
    public int Amount => amount;

    void Awake()
    {
        initialScale = transform.localScale;
    }

    // takes one unit, disappears when empty
    public bool TryGather(out ResourceType gatheredType)
    {
        gatheredType = resourceType;
        if (amount <= 0)
        {
            return false;
        }

        amount--;
        if (amount == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            PlayPunch();
        }
        return true;
    }

    // small effect when hit
    private void PlayPunch()
    {
        if (punchRoutine != null)
        {
            StopCoroutine(punchRoutine);
        }
        punchRoutine = StartCoroutine(Punch());
    }

    private IEnumerator Punch()
    {
        float elapsed = 0f;
        while (elapsed < punchDuration)
        {
            elapsed += Time.deltaTime;
            float factor = 1f + Mathf.Sin(elapsed / punchDuration * Mathf.PI) * punchScale;
            transform.localScale = initialScale * factor;
            yield return null;
        }
        transform.localScale = initialScale;
        punchRoutine = null;
    }
}
