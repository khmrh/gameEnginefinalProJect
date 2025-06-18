using UnityEngine;
using TMPro;

public class PlayerItemEffects : MonoBehaviour
{
    [Header("1. ������ (Zone Effect)")]
    public GameObject zoneEffect;
    public float zoneDamageInterval = 1f;

    [Header("2. ȸ���� (Rotator Effect)")]
    public GameObject rotatorEffect;
    public float rotationSpeed = 180f;
    public float rotationRadius = 1.5f;
    private float rotationAngle = 0f;

    [Header("3. �߻��� (Healing Shot)")]
    public GameObject healingShotPrefab;
    public Transform firePoint;
    public float fireInterval = 1.5f;
    public float healChance = 0.3f;
    private float nextFireTime = 0f;

    [Header("UI ǥ��")]
    public TextMeshProUGUI zoneText;
    public TextMeshProUGUI rotatorText;
    public TextMeshProUGUI projectileText;

    [Header("������ Ȱ��ȭ ����")]
    public bool useZone = true;
    public bool useRotator = true;
    public bool useProjectile = true;

    private int zoneLevel = 0;
    private int rotatorLevel = 0;
    private int projectileLevel = 0;

    public int baseDamage = 1;


    private Transform player;

    private void Start()
    {
        player = transform;
        UpdateUI();
    }

    private void Update()
    {
        // ���� ó��
        if (useZone)
        {
            zoneEffect.SetActive(true);
            zoneEffect.transform.position = player.position;
        }
        else
        {
            zoneEffect.SetActive(false);
        }

        // ȸ�� ó��
        if (useRotator)
        {
            rotatorEffect.SetActive(true);
            rotationAngle += rotationSpeed * Time.deltaTime;
            float rad = rotationAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * rotationRadius;
            rotatorEffect.transform.position = player.position + offset;
        }
        else
        {
            rotatorEffect.SetActive(false);
        }

        // �߻� ó��
        if (useProjectile && Time.time >= nextFireTime)
        {
            FireHealingShot();
            nextFireTime = Time.time + fireInterval;
        }
    }

    void FireHealingShot()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 direction = (mouseWorld - player.position).normalized;

        GameObject shot = Instantiate(healingShotPrefab, firePoint.position, Quaternion.identity);
        shot.GetComponent<HealingShot>().Initialize(direction, healChance);
    }

    public void UpdateUI()
    {
        if (zoneText != null)
        {
            string zoneStatus = useZone ? $"ON (LV{zoneLevel})" : "OFF";
            zoneText.text = $"����: {zoneStatus}";
        }

        if (rotatorText != null)
        {
            string rotatorStatus = useRotator ? $"ON (LV{rotatorLevel})" : "OFF";
            rotatorText.text = $"ȸ��: {rotatorStatus}";
        }

        if (projectileText != null)
        {
            string projectileStatus = useProjectile ? $"ON (LV{projectileLevel})" : "OFF";
            projectileText.text = $"�߻�: {projectileStatus}";
        }
    }

    public void ActivateItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Zone:
                useZone = true;
                zoneLevel++;
                zoneEffect.GetComponent<EnemyZoneDamage>().damage = baseDamage + zoneLevel;
                break;

            case ItemType.Rotator:
                useRotator = true;
                rotatorLevel++;
                rotatorEffect.GetComponent<RotatorDamage>().damage = baseDamage + rotatorLevel;
                break;

            case ItemType.HealingShot:
                useProjectile = true;
                projectileLevel++;
                break;
        }

        UpdateUI(); // �� ���� �ݿ��� ���·� UI ����
    }
}
