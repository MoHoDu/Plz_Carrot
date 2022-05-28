using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    // 게임이 시작 -> TowerDataViewer 클래스에 의해 해당 오브젝트 비활성화
    // Awake() 함수 호출이 불가 --> 최초 1회는 범위가 활성화 안 됨
/*    private void Awake()
    {
        OffAttackRange();
    }*/

    public void OnAttackRange(Vector3 position, float range)
    {
        gameObject.SetActive(true);

        // 공격 범위 크기
        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        // 공격 범위 위치
        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
