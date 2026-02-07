using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public WeaponData currentWeapon;
    public WeaponData mainWeapon; //�ֹ���
    public WeaponData subWeapon; //��������
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
        // ���� ��ü üũ
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentWeapon == mainWeapon)
                EquipWeapon(subWeapon);
            else 
                EquipWeapon(mainWeapon);
        }

        // ���� ��Ÿ�� üũ
        if (Time.time >= nextAttackTime)
        {
            // ��Ŭ��: �⺻ ���� (���� �ֵθ��� / Ȱ ��� / ������ ��������)
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + currentWeapon.attackRate;
            }

            // ��Ŭ��: Ư�� ���� (������ ���� ��)
            if (Input.GetMouseButtonDown(1) && currentWeapon is MagicWeaponData)
            {
                // ���콺 ���� ��ǥ ��������
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // MagicWeapon���� ����ȯ�ؼ� ���� �Լ� ȣ��
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


}
