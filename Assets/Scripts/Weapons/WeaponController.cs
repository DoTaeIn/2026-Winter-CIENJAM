using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public WeaponData currentWeapon;
    public WeaponData mainWeapon; //�ֹ���
    public WeaponData subWeapon; //��������
    public Transform firePos;
    public SpriteRenderer weaponRenderer;
    public PlayerMove move;


    [Header("Settings")]
    public LayerMask enemyLayer;

    private float nextAttackTime = 0f;
    private Animator anim;

    private void Awake()
    {
        move = GetComponent<PlayerMove>();
    }

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
            SwapWeaopon();
        }

        // ���� ��Ÿ�� üũ
        if (Time.time >= nextAttackTime)
        {
            // ��Ŭ��: �⺻ ���� (���� �ֵθ��� / Ȱ ��� / ������ ��������)
            if (Input.GetMouseButtonDown(0))
            { 
                move.SwipeDown();
                Attack();
                nextAttackTime = Time.time + currentWeapon.attackRate;
            }

            // ��Ŭ��: Ư�� ���� (������ ���� ��)
            if (Input.GetMouseButtonDown(1) && currentWeapon is MagicWeaponData)
            {
                move.SwipeUp();
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
        if (currentWeapon != null)
        {
            //Destroy(currentWeapon);
        }
        currentWeapon = newWeapon;

        if (newWeapon.weaponPrefab != null)
        {
            GameObject currentWeaponObj = Instantiate(newWeapon.weaponPrefab, firePos);

            currentWeaponObj.transform.localPosition = Vector3.zero;
            currentWeaponObj.transform.localRotation = Quaternion.identity;
        }
        Debug.Log($"Weapon changed to {newWeapon.weaponName}");
    }

    public void SwapWeaopon()
    {
        if (currentWeapon == mainWeapon)
        {
            mainWeapon.weaponPrefab.SetActive(false);
            currentWeapon = subWeapon;
            subWeapon.weaponPrefab.SetActive(true);
        }

        else
        {
            subWeapon.weaponPrefab.SetActive(false);
            currentWeapon = mainWeapon;
            mainWeapon.weaponPrefab.SetActive(true);
        }
    }
}
