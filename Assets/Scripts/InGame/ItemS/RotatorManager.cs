using UnityEngine;
using System.Collections.Generic;

public class RotatorManager : MonoBehaviour
{
    public GameObject rotatorPrefab;
    public float radius = 1.5f;
    public float rotationSpeed = 180f;

    private List<GameObject> rotators = new List<GameObject>();
    private int currentLevel = 0;



    void Update()
    {
        if (rotators.Count == 0) return;

        float angleStep = 360f / rotators.Count;

        for (int i = 0; i < rotators.Count; i++)
        {
            float angle = Time.time * rotationSpeed + angleStep * i;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;

            if (rotators[i] != null)
            {
                rotators[i].transform.localPosition = offset;
            }
        }
    }

    public void SetLevel(int level)
    {
        if (level == currentLevel) return;

        currentLevel = level;
        UpdateRotators();
        Debug.Log("SetLevel È£ÃâµÊ - level: " + level);
    }

    private void UpdateRotators()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        rotators.Clear();

        for (int i = 0; i < currentLevel; i++)
        {
            GameObject instance = Instantiate(rotatorPrefab);
            instance.SetActive(true);
            instance.transform.SetParent(transform, false);
            instance.transform.localRotation = Quaternion.identity;

            float angle = 360f / currentLevel * i;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            instance.transform.localPosition = offset;

            rotators.Add(instance);
            Debug.Log("Rotator »ý¼ºµÊ: " + instance.name);
        }
    }
}
