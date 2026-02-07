using UnityEngine;
using UnityEngine.Pool;

public class ArrowPoolManager : MonoBehaviour
{
    // Singleton 처리
    public static ArrowPoolManager Instance;
    [Header("Settings")]
    public ArrowBehavior arrowPrefab; // 화살 프리팹
    public int defaultCapacity = 10;    // 초기 개수
    public int maxSize = 20;           // 최대 개수

    private IObjectPool<ArrowBehavior> _pool;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 풀 초기화
        _pool = new ObjectPool<ArrowBehavior>(
            CreateArrow,        // 없으면 새로 생성하는 함수
            OnTakeFromPool,     // 풀에서 꺼낼 때 실행할 함수
            OnReturnToPool,     // 풀에 반납할 때 실행할 함수
            OnDestroyArrow,     // 풀이 꽉 찼는데 반납되면 삭제하는 함수
            true,               // 에러 체크 (동일한거 두번 반납 방지)
            defaultCapacity,
            maxSize
        );
    }

    // 화살 생성
    private ArrowBehavior CreateArrow()
    {
        ArrowBehavior arrow = Instantiate(arrowPrefab);
        arrow.SetPool(_pool);
        return arrow;
    }

    // 풀에서 대여
    private void OnTakeFromPool(ArrowBehavior arrow)
    {
        arrow.gameObject.SetActive(true);
    }

    // 화살 반납
    private void OnReturnToPool(ArrowBehavior arrow)
    {
        arrow.gameObject.SetActive(false);
    }

    // 화살 삭제
    private void OnDestroyArrow(ArrowBehavior arrow)
    {
        Destroy(arrow.gameObject);
    }

    // 외부에서 화살을 요청시
    public ArrowBehavior GetArrow()
    {
        return _pool.Get();
    }
}
