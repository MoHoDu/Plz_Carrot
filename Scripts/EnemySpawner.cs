using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
	//[SerializeField]
	//private GameObject		enemyPrefab;			// 적 프리팹
	[SerializeField]
	private Button			buttonStartWave;
	[SerializeField]
	private SystemTextViewer systemTextViewer;


	[SerializeField]
	private GameObject		enemyHPSliderPrefab;    // 적 체력을 나타내는 Slider UI 프리팹
	[SerializeField]
	private GameObject		enemyHeartPrefab;		// 적 체력 하트 표시 프리팹
	[SerializeField]
	private Transform		canvasTransform;		// UI를 표현하는 Canvas 오브젝트의 Transform
	//[SerializeField]
	//private float			spawnTime;				// 적 생성 주기
	//[SerializeField]
	//private Transform[]		wayPoints;              // 현재 스테이지의 이동 경로
	[SerializeField]
	private Way[]			ways;
	[SerializeField]
	private PlayerHP		playerHP;               // 플레이어의 체력 컴포넌트
	[SerializeField]
	private PlayerGold		playerGold;             // 플레이어의 골드 컴포넌트
	[SerializeField]
  	private WaveSystem		waveSystem;

	[SerializeField]
	private TextMeshProUGUI plusEffect;             // 수입 이펙트
	[SerializeField]
	private TextMeshProUGUI textChanceCount;        // 잔여 대출 횟수

	[SerializeField]
	private GameObject panelLater;                  // 급여 패널
	[SerializeField]
	private GameObject buttonLater;                 // 급여 버튼
	[SerializeField]
	private Button buttonChance;                // 대출 버튼

	[SerializeField]
	private TextMeshProUGUI allMoney;
	[SerializeField]
	private TextMeshProUGUI willPayMoney;
	[SerializeField]
	private TextMeshProUGUI sumMoney;

	[SerializeField]
	private LevelStory		story;


	private Wave			currentWave;            // 현재 웨이브 정보
	private int				currentEnemyCount;		// 현재 웨이브에 남아있는 적 숫자 (웨이브 시작시 max로 설정, 적 사망 시 -1)
	private List<Enemy>		enemyList;              // 현재 맵에 존재하는 모든 적의 정보
	private int				enemyKillCount = 0;			// 적 킬 수
	private bool			goldGiven = false;
	private bool			goldPayed = true;
	//private bool			printOK = false;
	private int				chance = 1;
	private bool			panelOn = false;
	private bool			firstOn = true;

	// 적의 생성과 삭제는 EnemeySpawner에서 하기 때문에 Set은 필요 없다.
	public List<Enemy> EnemyList => enemyList;
	// 현재 웨이브의 남아있는 적, 최대 적 숫자
	public int CurrentEnemyCount => currentEnemyCount;
	public int MaxEnemyCount => currentWave.maxEnemyCount;
	public int EnemyKillCount => enemyKillCount;
	public bool GoldPayed => goldPayed;


	private void Awake()
	{
		// 적 리스트 메모리 할당
		enemyList = new List<Enemy>();
		// 적 생성 코루틴 함수 호출
		//StartCoroutine("SpawnEnemy");
	}

    private void Update()
	{ 
        if (enemyList.Count == 0 && goldGiven == false)
        {
			//systemTextViewer.PrintText(SystemType.NoFast);
			Time.timeScale = 1f;
			waveSystem.CheckDay();

			playerGold.CurrentGold += playerGold.PlusGold;
			playerGold.PrePlusGold = playerGold.PlusGold;
			playerGold.PlusGold = 0;
			goldGiven = true;
        }

		if (enemyList.Count == 0 && goldPayed == false)
        {
			//buttonStartWave.interactable = playerGold.CurrentGold >= playerGold.PayGold ? true : false;

			allMoney.text = playerGold.CurrentGold + " 원";
			willPayMoney.text = "- " + playerGold.PayGold + " 원";

			if (firstOn == true)
            {
				waveSystem.WaveText.text = (waveSystem.CurrentWave + 1) + "일차 홍보 시작!!";
				panelLater.SetActive(true);
				buttonLater.SetActive(true);
				panelOn = true;
				firstOn = false;
				if (story.gameLevel == 1)
                {
					story.PlusSceneNum(2);
                }
			}

			if (playerGold.CurrentGold >= playerGold.PayGold)
            {
				sumMoney.text = (playerGold.CurrentGold - playerGold.PayGold) + " 원";
				buttonChance.enabled = false;
            }
			else
            {
				sumMoney.text = (playerGold.CurrentGold - playerGold.PayGold) * -1 + "원 부족 (알바를 해고하거나 대출하세요.)";
				buttonChance.enabled = true;
            }
				

			//if (playerGold.CurrentGold >= playerGold.PayGold)
			//         {
			//	systemTextViewer.PrintText(SystemType.Payed);
			//	playerGold.CurrentGold -= playerGold.PayGold;
			//	goldPayed = true;
			//	printOK = false;
			//         }
			//else
			//         {
			//	if (printOK == false)
			//             {
			//		systemTextViewer.PrintText(SystemType.NotPay);
			//		printOK = true;
			//             }
			//	else
			//             {
			//		systemTextViewer.ThePanel.SetActive(true);
			//             }

				//         }
		}
    }

	public void StartWave(Wave wave)
    {
		// 매개변수로 받아온 웨이브 정보 저장
		currentWave			= wave;
		// 현재 웨이브의 최대 적 숫자를 저장
		currentEnemyCount	= currentWave.maxEnemyCount;
		// 현재 웨이브 시작
		StartCoroutine("SpawnEnemy");
    }

	private IEnumerator SpawnEnemy()
	{
		int spawnEnemyCount = 0;
		List<int>countEnemy = new List<int>();
		int i = 0;
		while (i < currentWave.enemyCounts.Length)
        {
			countEnemy.Add(0);
			i++;
        }

		//while (true)
		// 현재 웨이브에서 생성되어야 하는 적의 숫자만큼 적을 생성하고 코루틴 종료
		while (spawnEnemyCount < currentWave.maxEnemyCount)
		{
			// 첫 번째 적 숫자 변수에 0을 집어 넣으면 랜덤으로
			if (currentWave.enemyCounts[0] == 0)
            {
				//GameObject clone = Instantiate(enemyPrefab);
				// 웨이브에 등장하는 적의 종류가 여러 종류일 때 임의의 적이 등장하도록 설정하고, 적 오브젝트 생성
				int			enemyIndex	= Random.Range(0, currentWave.enemyPrefabs.Length);
				int			wayIndex = Random.Range(0, currentWave.selectWays.Length);
				GameObject	clone		= Instantiate(currentWave.enemyPrefabs[enemyIndex]);
				Enemy		enemy		= clone.GetComponent<Enemy>();  // 방금 생성된 적의 Enemy 컴포넌트

				// this는 나 자신 (자신의 EnemySpawner 정보)
				enemy.Setup(this, ways[currentWave.selectWays[wayIndex]].wayPoints, wayIndex); // wayPoint 정보를 매개변수로 Setup() 호출
				enemyList.Add(enemy);									// 리스트에 방금 생성된 적 정보 저장

				SpawnEnemyHPSlider(clone);								// 적 체력을 나타내는 Slider UI 생성 및 설정
            }
			// 아니면 정해진 숫자만큼만 생성
			else
			{
				int			enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
				int			wayIndex = Random.Range(0, currentWave.selectWays.Length);
				while (true)
                {
					if (countEnemy[enemyIndex] < currentWave.enemyCounts[enemyIndex])
                    {
						countEnemy[enemyIndex] += 1;
						break;
                    }

					enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
				}
				GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
				Enemy enemy = clone.GetComponent<Enemy>();  // 방금 생성된 적의 Enemy 컴포넌트

				// this는 나 자신 (자신의 EnemySpawner 정보)
				enemy.Setup(this, ways[currentWave.selectWays[wayIndex]].wayPoints, wayIndex); // wayPoint 정보를 매개변수로 Setup() 호출
				enemyList.Add(enemy);                                   // 리스트에 방금 생성된 적 정보 저장

				SpawnEnemyHPSlider(clone);                              // 적 체력을 나타내는 Slider UI 생성 및 설정
			}
			

			// 현재 웨이브에서 생성한 적의 숫자 +1
			spawnEnemyCount ++;

			if (spawnEnemyCount == 1)
            {
				goldGiven = false;
				goldPayed = false;
            }

			//yield return new WaitForSeconds(spawnTime); // spawnTime 시간 동안 대기
			// 각 웨이브마다 spawnTime이 다를 수 있기 때문에 현재 웨이브(currentWave)의 spawnTime 사용
			yield return new WaitForSeconds(currentWave.spawnTime); // spawnTime 시간 동안 대기
		}
	}

	public void DestroyEnemy(EnemyDestroyType type, Enemy enemy, float damage, int gold)
    {
		// 적이 목표지점까지 도착했을 때
		if (type == EnemyDestroyType.Arrive)
        {
			// 플레이어의 체력 -1
			//playerHP.TakeDamage(1);
        }
		// 적이 플레이어의 발사체에게 사망했을 때
		else if (type == EnemyDestroyType.Kill)
        {
			// 적의 종류에 따라 사망 시 골드 획득
			playerHP.TakeDamage(damage);

			enemyKillCount += 1;
			//playerGold.CurrentGold += gold;
			plusEffect.gameObject.SetActive(true);
			plusEffect.text = "+ " + gold + "원";
			playerGold.PlusGold += gold;
			new WaitForSeconds(2.0f * Time.deltaTime);
			
		}

		// 적이 사망할 때마다 현재 웨이브의 생존 적 숫자 감소 (UI 표시용)
		currentEnemyCount --;
		// 리스트에서 사망하는 적 정보 삭제
		enemyList.Remove(enemy);
		
		if (gold > 0)
        {
			StartCoroutine(WaitForDie(enemy));
        }
		else
			Destroy(enemy.gameObject);
		

	}

	private void SpawnEnemyHPSlider(GameObject enemy)
    {
		// 적 체력을 나타내는 Slider UI 생성
		GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
		GameObject heartClone = Instantiate(enemyHeartPrefab);
		// Slider UI 오브젝트를 parent("Canvas"오브젝트)의 자식으로 설정
		// Tip. UI는 캔버스의 자식오브젝트로 설정되어 있어야 화면에 보인다
		sliderClone.transform.SetParent(canvasTransform);
		heartClone.transform.SetParent(canvasTransform);
		// 계층 설정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
		sliderClone.transform.localScale = Vector3.one;
		heartClone.transform.localScale = Vector3.one;

		// Slider UI가 쫒아다닐 대상을 본인으로 설정
		sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
		// Slider UI에 자신의 체력 정보를 표시하도록 설정
		sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());

		heartClone.GetComponent<HeartPositionAutoSetter>().Setup(enemy.transform);
	}

	IEnumerator WaitForDie(Enemy enemy)
	{
		yield return new WaitForSeconds(2.0f);
		Destroy(enemy.gameObject);
		plusEffect.gameObject.SetActive(false);
	}

	public void PlusKillCount(int count)
    {
		enemyKillCount += 50;
    }

	public void CheckingMoney()
    {
		if (playerGold.CurrentGold >= playerGold.PayGold)
		{
			panelLater.SetActive(false);
			buttonLater.SetActive(false);
			panelOn = false;
			systemTextViewer.PrintText(SystemType.Payed);
			playerGold.CurrentGold -= playerGold.PayGold;
			goldPayed = true;
			//printOK = false;
			firstOn = true;
		}
		else
		{
			systemTextViewer.PrintText(SystemType.NotPay);
		}
	}

	public void ShowMeTheMoney()
    {
		if (chance > 0)
        {
			systemTextViewer.PrintText(SystemType.ShowMoney);
			playerGold.CurrentGold = playerGold.PayGold;
			chance -= 1;
			textChanceCount.text = "잔여 대출 : " + chance + "회";
			StartCoroutine(WaitingTime());
			CheckingMoney();
        }
		else
        {
			systemTextViewer.PrintText(SystemType.NoChance);
        }
    }

	public void PanelLaterOnOff()
    {
		if (panelOn == true)
        {
			panelLater.SetActive(false);
			panelOn = false;
        }
		else
        {
			panelLater.SetActive(true);
			panelOn = true;
        }
    }

	IEnumerator WaitingTime()
    {
		yield return new WaitForSeconds(3.0f * Time.deltaTime);
    }
}

[System.Serializable]   // 구조체 변수 직렬화
public struct Way  // 웨이브 구조체 생성
{
	public Transform[] wayPoints;              // 현재 스테이지의 이동 경로
}


/*
 * File : EnemySpawner.cs
 * Desc
 *	: 적 생성, 삭제 및 적 정보를 관리
 *	
 * Functions
 *	: StartWave() - wave 매개변수에 있는 정보를 바탕으로 웨이브 시작
 *	: SpawnEnemy() - 적 생성
 *	: DestroyEnemy() - 매개변수로 받은 적 삭제
 *	: SpawnEnemyHPSlider() - 적 체력 Slider UI 생성 및 설정
 *	
 */