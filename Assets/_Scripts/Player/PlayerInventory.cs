using System;
using UnityEngine;

// how many resources the player has
public class PlayerInventory : MonoBehaviour
{
    public event Action<ResourceType, int> Changed;

    private readonly int[] counts = new int[Enum.GetValues(typeof(ResourceType)).Length];

    public int GetCount(ResourceType type)
    {
        return counts[(int)type];
    }

    public void Add(ResourceType type, int amount)
    {
        counts[(int)type] += amount;
        Changed?.Invoke(type, counts[(int)type]);
    }
}
