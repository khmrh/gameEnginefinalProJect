using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Transform center;
    private float radius;
    private float angle;
    private float speed;

    public int damage = 1;
    public float knockbackForce = 5f;

    public void Init(Transform centerPoint, float rad, float rotateSpeed, float angleOffset)
    {
        center = centerPoint;
        radius = rad;
        speed = rotateSpeed;
        angle = angleOffset;
    }

    void Update()
    {
        if (center == null) return;

        angle += speed * Time.deltaTime;
        float radian = angle * Mathf.Deg2Rad;
        transform.position = center.position + new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)) * radius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage);

                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir = (other.transform.position - transform.position).normalized;
                    rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
