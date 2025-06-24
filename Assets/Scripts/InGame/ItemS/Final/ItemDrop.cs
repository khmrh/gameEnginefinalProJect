using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Tooltip("드롭할 아이템 프리팹 목록 (Zone, Rotator, HealingShot 순서)")]
    public GameObject[] dropItems;

    [Range(0.05f, 0.10f)]
    public float dropChance = 0.08f; // 기본 8% 확률

    public int maxZoneLevel = 5;
    public int maxRotatorLevel = 5;
    public int maxHealingShotLevel = 3;

    private PlayerItemEffects player;

    private void Start()
    {
        player = FindObjectOfType<PlayerItemEffects>();
    }

    public void TryDropItem()
    {
        if (player == null || dropItems.Length < 3) return;
        if (Random.value > dropChance) return;

        int attempts = 10;
        while (attempts-- > 0)
        {
            int index = Random.Range(0, dropItems.Length);
            bool canDrop = false;

            switch ((ItemType)index)
            {
                case ItemType.Zone:
                    if (player.GetZoneLevel() < maxZoneLevel) canDrop = true;
                    break;
                case ItemType.Rotator:
                    if (player.GetRotatorLevel() < maxRotatorLevel) canDrop = true;
                    break;
                case ItemType.HealingShot:
                    if (player.GetHealingShotLevel() < maxHealingShotLevel) canDrop = true;
                    break;
            }

            if (canDrop)
            {
                Instantiate(dropItems[index], transform.position, Quaternion.identity);
                break;
            }
        }
    }
}