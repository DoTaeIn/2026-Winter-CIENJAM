using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public WeaponData currentWeapon;
    public WeaponData mainWeapon; //주무기
    public WeaponData subWeapon; //보조무기
    public Transform firePos;
    public SpriteRenderer weaponRenderer;

    [Header("Settings")]
    public LayerMask enemyLayer;

    private float nextAttackTime = 0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        EquipWeapon(mainWeapon);
    }

    void Update()
    {
        // 무기 교체 체크
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentWeapon == mainWeapon)
                EquipWeapon(subWeapon);
            else 
                EquipWeapon(mainWeapon);
        }

        // 공격 쿨타임 체크
        if (Time.time >= nextAttackTime)
        {
            // 좌클릭: 기본 공격 (근접 휘두르기 / 활 쏘기 / 지팡이 물리공격)
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + currentWeapon.attackRate;
            }

            // 우클릭: 특수 공격 (지팡이 마법 등)
            if (Input.GetMouseButtonDown(1) && currentWeapon is MagicWeaponData)
            {
                // 마우스 월드 좌표 가져오기
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // MagicWeapon으로 형변환해서 마법 함수 호출
                ((MagicWeaponData)currentWeapon).CastSpell(firePos, mousePos);

                nextAttackTime = Time.time + currentWeapon.attackRate;
            }
        }
    }

    void Attack()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentWeapon.Attack(firePos, mousePos, enemyLayer);
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        if (newWeapon.weaponSprite != null)
        {
            weaponRenderer.sprite = newWeapon.weaponSprite;
        }
        Debug.Log("Weapon changed.");
    }

    void OnDrawGizmos()
    {
        if (currentWeapon is not RangedWeaponData)
        {
            Gizmos.color = Color.red;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 플레이어보다 오른쪽인지 왼쪽인지 판단 (Y축 무시)
            bool isRight = mousePos.x > transform.position.x;

            // 방향 벡터 설정 (오른쪽이면 (1,0), 왼쪽이면 (-1,0))
            Vector2 facingDir = isRight ? Vector2.right : Vector2.left;

            Vector3 center = transform.position + (Vector3)(facingDir * 2.0f);

            // 회전 없이 그냥 그리기
            Gizmos.DrawWireCube(center, new Vector2(1.0f, 2.0f));
        }
    }
}
