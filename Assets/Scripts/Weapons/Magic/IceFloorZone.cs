using System.Collections;
using System.Collections.Generic; // 리스트 사용을 위해 필요
using UnityEngine;

public class IceFloorZone : MonoBehaviour
{
    [Header("Zone Settings")]
    public float duration = 3.0f;       // 장판 지속 시간
    public float tickRate = 0.5f;       // 데미지 주는 간격 (0.5초마다)
    public float slowAmount = 0.5f;     // 적 이동속도 50% 감소

    private float zoneDamage;           // 투사체에서 받아올 데미지
    private List<Collider2D> enemiesInside = new List<Collider2D>(); // 현재 장판 위에 있는 적들

    // 투사체가 이 함수를 호출해서 데미지를 설정해줌
    public void Setup(float damage)
    {
        zoneDamage = damage; // (장판 데미지를 투사체보다 약하게 하려면 * 0.5f 등 추가)

        // 지속 시간이 지나면 장판 스스로 삭제
        Destroy(gameObject, duration);

        // 도트 데미지 코루틴 시작
        StartCoroutine(DealDamageOverTime());
    }

    // 영역에 들어옴
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInside.Add(other);

            // 이속 감소 적용 (Enemy 스크립트에 함수가 있다고 가정)
            // var enemyMove = other.GetComponent<EnemyMovement>();
            // if (enemyMove) enemyMove.ApplySlow(slowAmount);
        }
    }

    // 영역에서 나감
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInside.Remove(other);

            // 이속 감소 해제
            // var enemyMove = other.GetComponent<EnemyMovement>();
            // if (enemyMove) enemyMove.RemoveSlow();
        }
    }

    // 지속 피해 로직 (코루틴)
    IEnumerator DealDamageOverTime()
    {
        while (true) // 장판이 사라질 때까지 무한 반복
        {
            // 리스트를 거꾸로 돌면서 데미지 줌 (중간에 적이 죽어서 삭제될 경우 에러 방지)
            for (int i = enemiesInside.Count - 1; i >= 0; i--)
            {
                Collider2D enemy = enemiesInside[i];

                if (enemy == null) // 적이 죽어서 오브젝트가 사라졌으면 리스트에서 제거
                {
                    enemiesInside.RemoveAt(i);
                    continue;
                }

                Debug.Log($"{enemy.name} ice damage: {zoneDamage}");
                // enemy.GetComponent<EnemyHealth>()?.TakeDamage(zoneDamage);
            }

            // 설정한 간격만큼 대기
            yield return new WaitForSeconds(tickRate);
        }
    }
}