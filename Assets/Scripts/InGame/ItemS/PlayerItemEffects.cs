using UnityEngine;
using TMPro;

public class PlayerItemEffects : MonoBehaviour
{
    public int GetZoneLevel() => zoneLevel;
    public int GetRotatorLevel() => rotatorLevel;
    public int GetHealingShotLevel() => healingShotLevel;

    [Header("아이템 최대 레벨 제한")]
    [SerializeField] int maxZoneLevel = 5;
    [SerializeField] int maxRotatorLevel = 5;
    [SerializeField] int maxHealingShotLevel = 5;

    [Header("1. 존 이펙트 (Zone Effect)")]
    public GameObject zoneEffectPrefab;
    private GameObject zoneEffectInstance;
    private int zoneLevel = 0;
    public bool useZone = false;

    [Header("2. 회전체 (Rotator Effect)")]
    public float rotationSpeed = 180f;
    public float rotationRadius = 1.5f;
    private float rotationAngle = 0f;

    [SerializeField] GameObject rotatorPrefab;
    [SerializeField] RotatorManager _rotatorManager;
    private int rotatorLevel = 0;
    public bool useRotator = false;

    [Header("3. 힐링샷 (Healing Shot)")]
    public GameObject healingShotPrefab;
    public float healingFireInterval = 1.5f;
    private float nextHealingFireTime = 0f;
    public float healChance = 0.3f;
    private int healingShotLevel = 0;
    public bool useHealingShot = false;

    [Header("4. 기본 자동 발사")]
    public GameObject basicShotPrefab;
    public float basicFireInterval = 0.5f;
    private float nextBasicFireTime = 0f;
    private int basicShotLevel = 1;

    [Header("발사 위치")]
    public Transform firePoint;

    [Header("UI 표시")]
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
        if (Time.time >= nextBasicFireTime)
        {
            FireBasicShot();
            nextBasicFireTime = Time.time + basicFireInterval;
        }

        if (useHealingShot && Time.time >= nextHealingFireTime)
        {
            FireHealingShot();
            nextHealingFireTime = Time.time + healingFireInterval;
        }

        if (useZone && zoneEffectInstance != null)
        {
            if (!zoneEffectInstance.activeSelf)
                zoneEffectInstance.SetActive(true);

            zoneEffectInstance.transform.position = transform.position;
        }
    }

    void FireBasicShot()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 direction = (mouseWorld - player.position).normalized;

        GameObject shot = Instantiate(basicShotPrefab, firePoint.position, Quaternion.identity);
        var proj = shot.GetComponent<ProjectileBase>();
        if (proj != null) proj.Initialize(direction, basicShotLevel, false);
    }

    void FireHealingShot()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2 direction = (mouseWorld - player.position).normalized;

        GameObject shot = Instantiate(healingShotPrefab, firePoint.position, Quaternion.identity);
        var proj = shot.GetComponent<ProjectileBase>();
        if (proj != null) proj.Initialize(direction, healingShotLevel, true, healChance);
    }

    public void ActivateItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.Zone:
                if (zoneLevel < maxZoneLevel)
                {
                    useZone = true;
                    zoneLevel++;

                    if (zoneEffectInstance == null)
                        zoneEffectInstance = Instantiate(zoneEffectPrefab);

                    zoneEffectInstance.SetActive(true);
                    var z = zoneEffectInstance.GetComponent<ZoneEffectController>();
                    if (z != null) z.SetLevel(zoneLevel);

                    ApplyZoneEffects();
                }
                break;

            case ItemType.Rotator:
                if (rotatorLevel < maxRotatorLevel)
                {
                    useRotator = true;
                    rotatorLevel++;

                    if (_rotatorManager != null)
                        _rotatorManager.SetLevel(rotatorLevel);
                }
                break;

            case ItemType.HealingShot:
                if (healingShotLevel < maxHealingShotLevel)
                {
                    useHealingShot = true;
                    healingShotLevel++;
                    ApplyHealingShotEffects();
                }
                break;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (zoneText != null) zoneText.text = $"존: {(useZone ? $"ON (LV{zoneLevel})" : "OFF")}";
        if (rotatorText != null) rotatorText.text = $"회전: {(useRotator ? $"ON (LV{rotatorLevel})" : "OFF")}";
        if (healingShotText != null) healingShotText.text = $"힐링: {(useHealingShot ? $"ON (LV{healingShotLevel})" : "OFF")}";
        if (basicShotText != null) basicShotText.text = $"기본공격: LV{basicShotLevel}";
    }

    void ApplyZoneEffects()
    {
        var moveScript = GetComponent<Players>();
        if (moveScript != null)
        {
            moveScript.moveSpeed = moveScript.baseMoveSpeed + zoneLevel * 0.5f;
            moveScript.currentAttackInterval = moveScript.baseAttackInterval / (1f + zoneLevel * 0.2f);
        }

        basicFireInterval = Mathf.Max(0.1f, 0.5f - zoneLevel * 0.05f); // 기본샷 속도도 따로 적용하고 싶다면
    }


    void ApplyHealingShotEffects()
    {
        healChance = 0.3f + healingShotLevel * 0.05f;
        healingFireInterval = Mathf.Max(0.3f, 1.5f - healingShotLevel * 0.2f);
    }

    public int GetRotatorDamage()
    {
        return baseDamage + rotatorLevel * 2;
    }
}
