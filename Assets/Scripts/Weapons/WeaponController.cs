using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponData currentWeapon;

    public WeaponData mainWeapon; //주무기
    public WeaponData subWeapon; //보조무기

    public Transform firePos; 
    public LayerMask enemyLayer;

    private float nextAttackTime = 0f;

    void Update()
    {

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
                //((MagicWeaponData)currentWeapon).CastSpell(firePos, mousePos);

                nextAttackTime = Time.time + currentWeapon.attackRate;
            }
        }
    }

    void Attack()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentWeapon.Attack(firePos, mousePos, enemyLayer);
    }
}
