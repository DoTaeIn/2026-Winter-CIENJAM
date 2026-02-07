using UnityEngine;

public class IceFloor : SpellBehavior
{
    [Header("Settings")]
    public float speed = 8f;
    public float floorDuration = 3f;
    public float floorDmg = 2f;
    public GameObject floorPrefab;

    private float damage;
    public override void Initialize(Vector2 targetPos, float dmg)
    {
        Debug.Log("Cast IceFloor!");
        base.Initialize(targetPos, dmg);
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            GameObject createdZone = Instantiate(floorPrefab, contactPoint, Quaternion.Euler(0, 0, 90));
            IceFloorZone zoneScript = createdZone.GetComponent<IceFloorZone>();
            if (zoneScript != null)
            {
                zoneScript.Setup(floorDmg);
            }

            Destroy(gameObject);
        }
    }
}
