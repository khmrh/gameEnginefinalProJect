using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireInterval = 0.4f;

    private float timer;

    void Update()
    {
        // 캐릭터가 마우스를 바라보도록 회전
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        Vector3 direction = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 일정 시간마다 자동 발사
        timer += Time.deltaTime;
        if (timer >= fireInterval)
        {
            Shoot(direction);
            timer = 0f;
        }
    }

    void Shoot(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(direction);
    }
}
