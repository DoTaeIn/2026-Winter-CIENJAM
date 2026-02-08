using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public string name;
    public WeaponData weapon;
    [Range(1, 100)] 
    public int dropChance;
}

public class Chest : MonoBehaviour
{
    [SerializeField] private List<LootItem> lootTable = new List<LootItem>();

    public void UnlockChest()
    {
        int totalWeight = 0;
        foreach (var item in lootTable)
        {
            totalWeight += item.dropChance;
        }
        
        int randomValue = Random.Range(0, totalWeight);
        
        foreach (var item in lootTable)
        {
            if (randomValue < item.dropChance)
            {
                //SpawnItem(item.prefab);
                return;
            }
            randomValue -= item.dropChance;
        }
    }

    void SpawnItem(GameObject itemPrefab)
    {
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Nothing here");
        }
    }
}
