using UnityEngine;
using TMPro;

public class TextMissionViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMissionEnemy;   // Text - TextMashPro UI [플레이어의 체력]
    [SerializeField]
    private TextMeshProUGUI textMissionMoney; // Text - TextMashPro [플레이어의 골드]
    [SerializeField]
    private PlayerGold      playerGold;     // 플레이어의 골드 정보
    [SerializeField]
    private PlayerHP        playerHP;     // 플레이어의 당근 정보
    [SerializeField]
    private EnemySpawner enemySpawner;   // 적 정보
    [SerializeField]
    public int         MissionKill = 100;

    public bool        clear1 = false;
    public bool        clear2 = false;

    private void Update()
    {
        if (playerGold.CurrentGold < playerGold.MissionGold && clear1 == false)
        {
            textMissionMoney.text = playerGold.CurrentGold + " / " + playerGold.MissionGold.ToString() + " 원";
        }
        else if (playerGold.CurrentGold >= playerGold.MissionGold && clear1 == false)
        {
            textMissionMoney.text = "조건 완료!";
            playerHP.TakeDamage(20);
            clear1 = true;
        }
        else
        {
            textMissionMoney.text = "조건 완료!";
        }

        if (enemySpawner.EnemyKillCount < MissionKill)
        {
            textMissionEnemy.text = enemySpawner.EnemyKillCount + " / " + MissionKill + " 명";
        }
        else if (clear2 == false)
        {
            textMissionEnemy.text = "조건 완료!";
            playerHP.TakeDamage(20);
            clear2 = true;
        }
        else
        {
            textMissionEnemy.text = "조건 완료!";
        }

        
    }
}
