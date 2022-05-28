using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class SpawnTiles : MonoBehaviour
{
    [SerializeField]
    private TileTemplate[] tileTemplate;              // 타워 정보 (공격력, 공격 속도 등)
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Sprite select;
    [SerializeField]
    private Sprite nonSelect;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private GameObject trashZone;
    //[SerializeField]
    //private GameObject      towerPrefab;
    //[SerializeField]
    //private int             towerBuildGold = 50;        // 타워 건설에 사용되는 골드
    [SerializeField]
    private PlayerGold playerGold;                 // 타워 건설 시 골드 감소를 위해 컴포넌트 불러옴
    //[SerializeField]
    //private SystemTextViewer systemTextViewer;          // 논 부속, 건설 불가와 같은 시스템 메시지 출력
    private bool isOnTileButton = false;    // 타워 건설 버튼을 눌렀는지 체크
    private GameObject followTileClone = null;    // 임시 타워 사용 완료 시 삭제를 위해 저장하는 변수
    private int tileType;                  // 타워 속성
    private bool cancelOn = false;

    //private Ray ray;
    //private RaycastHit hit;
    //private Camera mainCamera;

    private GameObject[] prefabTiles;
    private GameObject gameData;

    private void Awake()
    {
        prefabTiles = GameObject.FindGameObjectsWithTag("Tile");
        gameData = GameObject.Find("GameData");
        Data data = gameData.GetComponent<Data>();

        //data.LoadTileData();
        data.Load();

        for (int i = 0; i < data.TileList.Count; i++)
        {
            if (data.TileList[i] != null)
            {
                Vector3Int point = new Vector3Int();
                point.x = data.TileList[i].X;
                point.y = data.TileList[i].Y;
                point.z = data.TileList[i].Z;
                tilemap.SetTile(point, tileTemplate[data.TileList[i].TileID].towerPrefab);
            }
        }      
    }

    public void ReadytoSpawnTower(int type)
    {
        tileType = type;

        // 버튼을 중복해서 누르는 것을 방지하기 위해 필요
        if (isOnTileButton == true)
        {
            return;
        }

        // 타워 건설 가능 여부 확인
        // 타워를 건설할 만큼 돈이 없으면 타워 건설X
        //if (tileTemplate[tileType].informs[0].cost > playerGold.CurrentGold)

        trashZone.gameObject.SetActive(true);

        // 타워 건설 버튼을 눌렀다고 설정
        isOnTileButton = true;
        // 마우스를 혹은 터치를 따라다니는 임시 타워 생성
        followTileClone = Instantiate(tileTemplate[tileType].followTowerPrefab);
        // 타워 설치 가능 지역 표시
        for (int i = 0; i < prefabTiles.Length; i++)
        {
            //prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(100 / 255f, 200 / 255f, 80 / 255f);
            prefabTiles[i].GetComponent<SpriteRenderer>().sprite = select;
        }

        // 타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        // 타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if (isOnTileButton == false)
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        // 2. 현재 타일의 위치에 이미 타워가 건설되어 있으면 타워 건설 X
        if (tile.IsBuildTower == true)
        {
            // 현재 위치에 타워 건설이 불가능하다고 출력
            //systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        // 다시 타워 건설 버튼을 눌러서 타워를 건설하도록 변수 설정
        isOnTileButton = false;

        // 타워가 건설되어 있음으로 설정
        tile.IsBuildTower = true;
        //타워 건설에 필요한 골드만큼 플레이어 골드 감소
        //playerGold.CurrentGold -= towerBuildGold;
        //playerGold.CurrentGold -= tileTemplate[tileType].weapon[0].cost;
        //playerGold.PayGold += tileTemplate[tileType].weapon[0].money;
        // 선택한 타일의 위치에 타워 건설 (타일보다 z축 -1의 위치에 배치)
        Vector3Int position = grid.WorldToCell(tileTransform.position + Vector3.back);

        Data data = gameData.GetComponent<Data>();
        //data.SaveTileData(tileType, position.x, position.y, position.z);
        data.TileListAdd(tileType, position.x, position.y, position.z);
        data.Save();

        //GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        //RuleTile clone = Instantiate(tileTemplate[tileType].towerPrefab, position, Quaternion.identity);
        tilemap.SetTile(position, tileTemplate[tileType].towerPrefab);

        // 타워를 배치했기 때문에 마우스, 터치를 따라다니는 임시 타워 삭제
        Destroy(followTileClone);
        // 타워 설치 가능 지역 표시 해제
        for (int i = 0; i < prefabTiles.Length; i++)
        {
            //prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
            prefabTiles[i].GetComponent<SpriteRenderer>().sprite = nonSelect;
        }

        // 타워 건설을 취소할 수 있는 코루틴 함수 중지
        StopCoroutine("OnTowerCancelSystem");
        trashZone.gameObject.SetActive(false);
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            // ESC 또는 마우스 오른쪽 버튼을 눌렀을 때 타워 건설 취소
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                //isOnTileButton = false;
                //// 마우스를 따라다니는 임시 타워 삭제
                //Destroy(followTileClone);
                //for (int i = 0; i < prefabTiles.Length; i++)
                //{
                //    //prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                //    prefabTiles[i].GetComponent<SpriteRenderer>().sprite = nonSelect;
                //}
                cancelOn = true;
                //break;
            }
            else if (Input.touchCount >= 2)
            {
                //isOnTileButton = false;
                //// 마우스를 따라다니는 임시 타워 삭제
                //Destroy(followTileClone);
                //// 타워 설치 가능 지역 표시 해제
                //for (int i = 0; i < prefabTiles.Length; i++)
                //{
                //    //prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                //    prefabTiles[i].GetComponent<SpriteRenderer>().sprite = nonSelect;
                //}
                cancelOn = true;
                //break;
            }

            if (cancelOn == true)
            {
                isOnTileButton = false;
                // 마우스를 따라다니는 임시 타워 삭제
                Destroy(followTileClone);
                // 타워 설치 가능 지역 표시 해제
                for (int i = 0; i < prefabTiles.Length; i++)
                {
                    //prefabTiles[i].GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f);
                    prefabTiles[i].GetComponent<SpriteRenderer>().sprite = nonSelect;
                }
                cancelOn = false;
                trashZone.gameObject.SetActive(false);
                break;
            }


            yield return null;
        }
    }


    public void CancelTile()
    {
        if (cancelOn == false)
        {
            cancelOn = true;
        }
    }
}
