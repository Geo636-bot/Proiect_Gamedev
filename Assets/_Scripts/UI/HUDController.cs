using TMPro;
using UnityEngine;

// HUD: resources and the action near the player
public class HUDController : MonoBehaviour
{
    [Header("Data sources")]
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerGatherer gatherer;

    [Header("Resources")]
    [SerializeField] private TMP_Text woodText;
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text berriesText;

    [Header("Interact hint")]
    [SerializeField] private GameObject interactHint;
    [SerializeField] private TMP_Text interactHintText;

    void OnEnable()
    {
        if (inventory != null)
        {
            inventory.Changed += OnInventoryChanged;
        }
        if (gatherer != null)
        {
            gatherer.TargetChanged += OnTargetChanged;
        }
    }

    void OnDisable()
    {
        if (inventory != null)
        {
            inventory.Changed -= OnInventoryChanged;
        }
        if (gatherer != null)
        {
            gatherer.TargetChanged -= OnTargetChanged;
        }
    }

    void Start()
    {
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            UpdateCount(type, inventory != null ? inventory.GetCount(type) : 0);
        }
        OnTargetChanged(gatherer != null ? gatherer.CurrentTarget : null);
    }

    private void OnInventoryChanged(ResourceType type, int count)
    {
        UpdateCount(type, count);
    }

    private void UpdateCount(ResourceType type, int count)
    {
        switch (type)
        {
            case ResourceType.Wood:
                woodText.text = $"Wood: {count}";
                break;
            case ResourceType.Stone:
                stoneText.text = $"Stone: {count}";
                break;
            case ResourceType.Berries:
                berriesText.text = $"Berries: {count}";
                break;
        }
    }

    private void OnTargetChanged(ResourceNode target)
    {
        bool hasTarget = target != null;
        interactHint.SetActive(hasTarget);
        if (hasTarget)
        {
            interactHintText.text = $"[E] {ActionFor(target.Type)}";
        }
    }

    // action text for each resource
    private static string ActionFor(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood:
                return "Chop";
            case ResourceType.Stone:
                return "Break";
            default:
                return "Gather";
        }
    }
}
