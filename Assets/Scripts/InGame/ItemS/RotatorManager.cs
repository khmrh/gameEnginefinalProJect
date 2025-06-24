using UnityEngine;
using System.Collections.Generic;

public class RotatorManager : MonoBehaviour
{
    public GameObject rotatorPrefab;
    public int level = 1;

    public float baseSpeed = 180f;
    public float radius = 1.5f;

    private List<GameObject> rotators = new List<GameObject>();

    void Start()
    {
        UpdateRotators();
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        UpdateRotators();
    }

    void UpdateRotators()
    {
        // 扁粮 昏力
        foreach (GameObject r in rotators)
            Destroy(r);
        rotators.Clear();

        // 货肺 积己
        for (int i = 0; i < level; i++)
        {
            GameObject r = Instantiate(rotatorPrefab, transform);
            Rotator logic = r.GetComponent<Rotator>();
            float angleOffset = (360f / level) * i;

            if (logic != null)
            {
                logic.Init(this.transform, radius, baseSpeed + (level * 50f), angleOffset);
            }

            rotators.Add(r);
        }
    }
}
