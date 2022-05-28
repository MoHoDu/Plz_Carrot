using UnityEngine;
using System.Collections;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI     textPlayerHP;   // Text - TextMashPro UI [플레이어의 체력]
    [SerializeField]
    private TextMeshProUGUI     textPlayerGold; // Text - TextMashPro [플레이어의 골드]
    [SerializeField]
    private TextMeshProUGUI     textWave;       // Text - TextMashPro [현재 웨이브 / 총 웨이브]
    [SerializeField]
    private TextMeshProUGUI     textEnemyCount; // Text - TextMashPro [현재 적 숫자 / 최대 적 숫자]
    [SerializeField]
    private TextMeshProUGUI     textPlusMoney;
    [SerializeField]
    private TextMeshProUGUI     textPayMoney;
    [SerializeField]
    private TextMeshProUGUI     textWhen;
    [SerializeField]
    private PlayerHP            playerHP;       // 플레이어의 체력 정보
    [SerializeField]
    private PlayerGold          playerGold;     // 플레이어의 골드 정보
    [SerializeField]
    private WaveSystem          waveSystem;     // 웨이브 정보
    [SerializeField]
    private EnemySpawner        enemySpawner;   // 적 정보
    [SerializeField]
    private float lerpTime = 2.0f;

    private void Update()
    {
        if (playerHP.CurrentHP >= playerHP.MissionHP)
        {
            textPlayerHP.text = "<color=#FFE400>" + playerHP.CurrentHP + " / " + playerHP.MissionHP + "개</color>";
        }
        else
            textPlayerHP.text       = playerHP.CurrentHP + " / " + playerHP.MissionHP + "개";
        textPlayerGold.text     = playerGold.CurrentGold.ToString() + "원";
        textWave.text           = waveSystem.CurrentWave + " / " + waveSystem.MaxWave + "일";
        textEnemyCount.text     = enemySpawner.CurrentEnemyCount + " / " + enemySpawner.MaxEnemyCount + "명";
        textPlusMoney.text      = "+ " + playerGold.PlusGold + "원";
        textPayMoney.text       = "- " + playerGold.PayGold + "원";

        if (enemySpawner.GoldPayed == true)
            textWhen.text       = (waveSystem.CurrentWave + 1) + "일 마치고 : ";
        else
            textWhen.text       = waveSystem.CurrentWave + "일 마치고 : ";
    }
}
