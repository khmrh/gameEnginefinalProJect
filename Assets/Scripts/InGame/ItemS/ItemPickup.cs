using UnityEngine;

public enum ItemType { Zone, Rotator, HealingShot }

public class ItemPickup : MonoBehaviour
{
    public ItemType itemType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerItemEffects effects = other.GetComponent<PlayerItemEffects>();
            if (effects != null)
            {
                effects.ActivateItem(itemType);
            }

            Destroy(gameObject);
        }
    }
}
