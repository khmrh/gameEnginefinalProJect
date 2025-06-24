using UnityEngine;
using TMPro;

public class PlayerItemEffects : MonoBehaviour
{

    public int GetZoneLevel() => zoneLevel;
    public int GetRotatorLevel() => rotatorLevel;
    public int GetHealingShotLevel() => healingShotLevel;

    [Header("1. ������ (Zone Effect)")]
    public GameObject zoneEffectPrefab;
    private GameObject zoneEffectInstance;
    private int zoneLevel = 0;
    public bool useZone = false;

    [Header("2. ȸ���� (Rotator Effect)")]
    public GameObject rotatorEffect;
    public float rotationSpeed = 180f;
    public float rotationRadius = 1.5f;
    private float rotationAngle = 0f;
    [SerializeField] GameObject rotatorPrefab;
    [SerializeField] RotatorManager rotatorManager;
    private int rotatorLevel = 0;
    public bool useRotator = false;

    [Header("3. ������ (Healing Shot)")]
    public GameObject healingShotPrefab;
    public float healingFireInterval = 1.5f;
    private float nextHealingFireTime = 0f;
    public float healChance = 0.3f;
    private int healingShotLevel = 0;
    public bool useHealingShot = false;

    [Header("4. �⺻ �ڵ� ����")]
    public GameObject basicShotPrefab;
    public float basicFireInterval = 0.5f;
    private float nextBasicFireTime = 0f;
    private int basicShotLevel = 1;

    [Header("���� �߻� ��ġ")]
    public Transform firePoint;

    [Header("UI ǥ��")]
    public TextMeshProUGUI zoneText;
    public TextMeshProUGUI rotatorText;
    public TextMeshProUGUI healingShotText;
    public TextMeshProUGUI basicShotText;

    public int baseDamage = 1;
    private Transform player;

    private void Start()
    {
        player = transform;
        UpdateUI();
    }

    private void Update()
    {
        // �⺻ ����
        if (Time.time >= nextBasicFireTime)
        {
            FireBasicShot();
            nextBasicFireTime = Time.time + basicFireInterval;
        }

        // ������
        if (useHealingShot && Time.time >= nextHealingFireTime)
        {
            FireHealingShot();
            nextHealingFireTime = Time.time + healingFireInterval;
        }

        // ���� ó��
        if (useZone && zoneEffectInstance != null)
        {
            zoneEffectInstance.transform.position = transform.position;
        }

        // ȸ�� ó��
        if (useRotator && rotatorEffect != null)
        {
            rotatorEffect.SetActive(true);
            rotationAngle += rotationSpeed * Time.deltaTime;
            float rad = rotationAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * rotationRadius;
            rotatorEffect.transform.position = player.position + offset;
        }
        else if (rotatorEffect != null)
        {
            rotatorEffect.SetActive(false);
        }
    }

    void FireBasicShot()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 direction = (mouseWorld - player.position).normalized;

        GameObject shot = Instantiate(basicShotPrefab, firePoint.position, Quaternion.identity);
        shot.GetComponent<BasicProjectile>().Initialize(direction, basicShotLevel);
    }

    void FireHealingShot()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 direction = (mouseWorld - player.position).normalized;

        GameObject shot = Instantiate(healingShotPrefab, firePoint.position, Quaternion.identity);
        shot.GetComponent<HealingShot>().Initialize(direction, healChance, healingShotLevel);
    }

    public void ActivateItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Zone:
                useZone = true;
                zoneLevel++;

                if (zoneEffectInstance == null)
                    zoneEffectInstance = Instantiate(zoneEffectPrefab);

                zoneEffectInstance.SetActive(true);
                ZoneEffectController z = zoneEffectInstance.GetComponent<ZoneEffectController>();
                if (z != null)
                    z.SetLevel(zoneLevel);
                break;

            case ItemType.Rotator:
                useRotator = true;
                rotatorLevel++;

                if (rotatorManager != null)
                    rotatorManager.SetLevel(rotatorLevel);
                break;

            case ItemType.HealingShot:
                useHealingShot = true;
                healingShotLevel++;
                break;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (zoneText != null)
            zoneText.text = $"����: {(useZone ? $"ON (LV{zoneLevel})" : "OFF")}";

        if (rotatorText != null)
            rotatorText.text = $"ȸ��: {(useRotator ? $"ON (LV{rotatorLevel})" : "OFF")}";

        if (healingShotText != null)
            healingShotText.text = $"����: {(useHealingShot ? $"ON (LV{healingShotLevel})" : "OFF")}";

        if (basicShotText != null)
            basicShotText.text = $"�⺻����: LV{basicShotLevel}";
    }
}
