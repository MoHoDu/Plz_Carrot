using UnityEngine;
using System.Collections;
using TMPro;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[]   towerTemplate;              // 타워 정보 (공격력, 공격 속도 등)
    //[SerializeField]
    //private GameObject      towerPrefab;
    //[SerializeField]
    //private int             towerBuildGold = 50;        // 타워 건설에 사용되는 골드
    [SerializeField]
    private EnemySpawner    enemySpawner;				// 현재 맵에 존재하는 적 리스트 정보를 얻기 위해..
    [SerializeField]
    private PlayerGold      playerGold;                 // 타워 건설 시 골드 감소를 위해 컴포넌트 불러옴
    [SerializeField]
    private SystemTextViewer systemTextViewer;          // 논 부속, 건설 불가와 같은 시스템 메시지 출력
    [SerializeField]
    public TextMeshProUGUI MinusEffect;                // 일급 이펙트
    [SerializeField]
    public TextMeshProUGUI payPrice;
    [SerializeField]
    public TowerDataViewer towerData;
    [SerializeField]
    private LevelStory      story;

    public bool             isOnTowerButton = false;    // 타워 건설 버튼을 눌렀는지 체크
    private GameObject      followTowerClone = null;    // 임시 타워 사용 완료 시 삭제를 위해 저장하는 변수
    private int             towerType;                  // 타워 속
    private bool            cancelOn = false;
    private bool            effectActive = false;  

    [SerializeField]
    private GameObject trashZone;   // 쓰레기 버튼

    //private Ray ray;
    //private RaycastHit hit;
    //private Camera mainCamera;

    private GameObject[] prefabTiles;

    private void Awake()
    {
        prefabTiles = GameObject.FindGameObjectsWithTag("Tile");
    }

    public void ReadytoSpawnTower(int type)
    {
        towerType = type;

        // 버튼을 중복해서 누르는 것을 방지하기 위해 필요
        if ( isOnTowerButton == true )
        {
            return;
        }

        // 타워 건설 가능 여부 확인
        // 타워를 건설할 만큼 돈이 없으면 타워 건설X
        if ( towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold )
        {
            // 골드가 부족해서 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        // 타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;

        trashZone.gameObject.SetActive(true);

        // 마우스를 혹은 터치를 따라다니는 임시 타워 생성
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        // 타워 설치 가능 지역 표시
        for (int i = 0; i <prefabTiles.Length; i++)
        {
            prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(100 / 255f, 200 / 255f, 80 / 255f);
        }
        
        // 타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {        
        // 타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if (isOnTowerButton ==false)
        {
            return;
        }

        // 타워 건설 가능 여부 확인
        // 1. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 X
        //if ( towerBuildGold > playerGold.CurrentGold)
        /*if ( towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            // 골드가 부족해서 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }*/

        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 X
        if ( tile.IsBuildTower == true)
        {
            // 현재 위치에 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        // 다시 타워 건설 버튼을 눌러서 타워를 건설하도록 변수 설정
        isOnTowerButton = false;

        // 타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        //타워 건설에 필요한 골드만큼 플레이어 골드 감소
        //playerGold.CurrentGold -= towerBuildGold;
        MinusEffect.text = "+ " + towerTemplate[towerType].weapon[0].money + "원";
        StartCoroutine(EffectOn());
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        playerGold.PayGold += towerTemplate[towerType].weapon[0].money;
        // 선택한 타일의 위치에 타워 건설 (타일보다 z축 -1의 위치에 배치)
        Vector3 position = tileTransform.position + Vector3.back;
        //GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        // 타워 무기에 enemySpawner 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold, tile);

        // 타워를 배치했기 때문에 마우스, 터치를 따라다니는 임시 타워 삭제
        Destroy(followTowerClone);
        // 타워 설치 가능 지역 표시 해제
        for ( int i = 0; i < prefabTiles.Length; i++)
        {
            prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
        }

        // 타워 건설을 취소할 수 있는 코루틴 함수 중지
        StopCoroutine("OnTowerCancelSystem");
        trashZone.gameObject.SetActive(false);
        if (story.gameLevel == 1)
        {
            story.PlusSceneNum(0);
        }
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true) {
            // ESC 또는 마우스 오른쪽 버튼을 눌렀을 때 타워 건설 취소
            if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                //isOnTowerButton = false;
                //// 마우스를 따라다니는 임시 타워 삭제
                //Destroy(followTowerClone);
                //for (int i = 0; i < prefabTiles.Length; i++)
                //{
                //    prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                //}
                //break;
                cancelOn = true;
            }
            else if (Input.touchCount >= 2)
            {
                //    isOnTowerButton = false;
                //    // 마우스를 따라다니는 임시 타워 삭제
                //    Destroy(followTowerClone);
                //    // 타워 설치 가능 지역 표시 해제
                //    for ( int i = 0; i < prefabTiles.Length; i++)
                //    {
                //        prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                //    }
                //    break;
                cancelOn = true;
            }

            if (cancelOn == true)
            {
                isOnTowerButton = false;
                // 마우스를 따라다니는 임시 타워 삭제
                Destroy(followTowerClone);
                // 타워 설치 가능 지역 표시 해제
                for (int i = 0; i < prefabTiles.Length; i++)
                {
                    prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                }
                cancelOn = false;
                trashZone.gameObject.SetActive(false);
                break;
            }
            

            yield return null;
        }
    }

    public void CancelTower()
    {
        isOnTowerButton = false;
        // 마우스를 따라다니는 임시 타워 삭제
        if (followTowerClone != null)
            Destroy(followTowerClone);
        // 타워 설치 가능 지역 표시 해제
        for (int i = 0; i < prefabTiles.Length; i++)
        {
            prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
        }
        cancelOn = false;
        trashZone.gameObject.SetActive(false);
        StopCoroutine(OnTowerCancelSystem());
    }
    public void SellEffect()
    {
        string a = payPrice.text;
        a = a.Replace("일급 : ", "");
        MinusEffect.text = "- " + a + " 원";
        StartCoroutine(EffectOn());
    }

    public void UpgradeEffect()
    {
        bool upgradeOk = towerData.isSuccess;
        if (upgradeOk == true)
            StartCoroutine(EffectOn());
    }

    IEnumerator EffectOn()
    {
        if (effectActive == false)
        {
            MinusEffect.gameObject.SetActive(true);
            effectActive = true;
            yield return new WaitForSeconds(2.0f);
            MinusEffect.gameObject.SetActive(false);
            effectActive = false;
        }
    }
}
