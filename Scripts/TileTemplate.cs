using UnityEngine;

[CreateAssetMenu]
public class TileTemplate : ScriptableObject
{
    public RuleTile towerPrefab;  // 타워 생성을 위한 프리팹
    public GameObject followTowerPrefab; // 임시 타워 프리팹
    public Inform[] informs;         // 레벨별 타워(무기) 정보

    [System.Serializable]
    public struct Inform
    {
        public Sprite sprite;   // 보여지는 타워 이미지 (UI)
        public int cost;        // 필요 골드
        public int sell;        // 타워 판매 시 획득 골드
        public string name;
    }
}