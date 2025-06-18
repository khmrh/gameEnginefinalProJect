using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] dropItems; // 아이템 프리팹 배열
    public float dropChance = 0.3f;

    public void TryDropItem()
    {
        if (Random.value <= dropChance)
        {
            int index = Random.Range(0, dropItems.Length);
            Instantiate(dropItems[index], transform.position, Quaternion.identity);
        }
    }
}
