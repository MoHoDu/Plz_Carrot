using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[]          waves;                  // 현재 스테이지의 모든 웨이브 정보
    [SerializeField]
    private EnemySpawner    enemySpawner;
    [SerializeField]
    private PlayerGold      playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private TextMissionViewer mission;
    [SerializeField]
    private TextMeshProUGUI waveText;

    private int             currentWaveIndex = -1;  // 현재 웨이브 인덱스
    private bool            timeFast = false;
    private bool            timeStop = false;

    // 웨이브 정보 출력을 위한 Get 프로퍼티 (현재 웨이브, 총 웨이브)
    public int              CurrentWave => currentWaveIndex + 1; // 시작이 0이기 때문에 +1
    public int              MaxWave => waves.Length;
    public TextMeshProUGUI  WaveText => waveText;

    private float             gameSpeed = 1f;

    GameObject gameData;

    private void Awake()
    {
        gameData = GameObject.Find("GameData");
    }

    public void StartWave()
    {
        //if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex == waves.Length)
        //{
        //    systemTextViewer.PrintText(SystemType.AllClear);
        //    return;
        //}

        if (enemySpawner.EnemyList.Count == 0 && enemySpawner.GoldPayed == false && enemySpawner.GoldPayed == false)
        {
            if (enemySpawner.EnemyList.Count == 0)
            {
                systemTextViewer.PrintText(SystemType.NotPay);
                
            }

            return;
        }

        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length -1)
        {
            //systemTextViewer.PrintText(SystemType.NoFast);
            //Time.timeScale = 1f;
            timeFast = false;
            waveText.text = "4배속으로";
            // 인덱스의 시작이 -1이기 때문에 웨이브 인덱스 증가를 먼저 함
            currentWaveIndex ++;
            // EnemySpawner의 StartWave() 함수 호출, 현재 웨이브 정보 제공
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
        else if (enemySpawner.EnemyList.Count > 0)
        {
            if (timeFast == false)
            {
                systemTextViewer.PrintText(SystemType.Fast);
                waveText.text = "1배속으로";
                Time.timeScale = 4f;
                gameSpeed = 4f;
                timeFast = true;
            }
            else if (timeFast == true)
            {
                systemTextViewer.PrintText(SystemType.NoFast);
                waveText.text = "4배속으로";
                Time.timeScale = 1f;
                gameSpeed = 1f;
                timeFast = false;
            }
        }
    }

    public void CheckDay()
    {
        if (currentWaveIndex < waves.Length - 1)
        {
            systemTextViewer.PrintText(SystemType.NewDay);
        }
        else if (currentWaveIndex == waves.Length - 1)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int gameNum = Int32.Parse(sceneName.Replace("game",""));
            gameData.GetComponent<Data>().UpdateStageData(gameNum, enemySpawner.EnemyKillCount, playerHP.CurrentHP, playerGold.TopGold, mission.clear1, mission.clear2, playerHP.MissionHP, mission.MissionKill, playerGold.MissionGold);
            systemTextViewer.PrintText(SystemType.AllClear);
            
        }
    }

    public void StopAndGoGame()
    {
        if (timeStop == false)
        {
            Time.timeScale = 0;
            timeStop = true;
        }
        else
        {
            Time.timeScale = gameSpeed;
            timeStop = false;
        }
        
    }

    public void OneDay()
    {
        Array.Resize<Wave>(ref waves, 1);
    }
}

[System.Serializable]   // 구조체 변수 직렬화
public struct Wave  // 웨이브 구조체 생성
{
    public float        spawnTime;      // 현재 웨이브 적 생성 주기
    public int          maxEnemyCount;  // 현재 웨이브 적 등장 숫자
    public GameObject[] enemyPrefabs;   // 현재 웨이브 적 등장 종류
    public int[]        enemyCounts;    // 현재 웨이브 적 등장 숫자 지정
    public int[]        selectWays;     // 현재 웨이브에 필요한 경로 지정
}
