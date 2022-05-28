using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textName;
    [SerializeField]
    private TextMeshProUGUI textMoney;
    [SerializeField]
    private TextMeshProUGUI textUpgaradeCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button          buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private GameObject effectCarrot;
    [SerializeField]
    PlayerHP                playerHP;

    private TowerWeapon     currentTower;

    private bool dataOn = false;
    public bool isSuccess;

    private void Awake()
    {
        OffPanel();
    }

    private void Update()
    {
        if ( dataOn == false)
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        // 출력해야하는 타워 정보를 받아와서 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        // 타워 정보 Panel On
        gameObject.SetActive(true);
        dataOn = true;
        // 타워 정보 갱신
        UpdateTowerData();
        // 타워 오브젝트 주변에 표시되는 타워 공격범위 Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void OffPanel()
    {
        // 타워 정보 Panel Off
        gameObject.SetActive(false);
        dataOn = false;
        // 타워 공격범위 Sprite Off
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = "홍보력 : " + currentTower.Damage;
        textRate.text = "지연시간 : " + currentTower.Rate;
        textRange.text = "사거리 : " + currentTower.Range;
        textLevel.text = "레벨 : " + currentTower.Level;
        textName.text = currentTower.Name;
        textMoney.text = "일급 : " + currentTower.Money;
        if (currentTower.Level < currentTower.MaxLevel)
            textUpgaradeCost.text = "승진 비용 : " + currentTower.TowerTemplate.weapon[currentTower.Level].cost + "원";
        else
            textUpgaradeCost.text = "더 이상 승진할 수 없어요!";

        textSellCost.text = "해고 시 + " + currentTower.SellCost + "원";

        // 업그레이드가 불가능해지면 버튼 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        // 타워 업그레이드 시도 (성공:true, 실패:false)
        isSuccess = currentTower.Upgrade();

        if ( isSuccess == true)
        {
            // 타워가 업그레이드 되었기 때문에 타워 정보 갱신
            UpdateTowerData();
            // 타워 주변에 보이는 공격범위도 갱신
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            // 타워 업그레이드에 필요한 비용이 부족하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnclickEventTowerSell()
    {
        // 타워 판매
        currentTower.Sell();

        TextMeshProUGUI effectCarrotText = effectCarrot.GetComponent<TextMeshProUGUI>();
        //Debug.Log(currentTower.Money);
        effectCarrotText.text = "- " + (currentTower.Money * 0.2f) + "개";
        if (playerHP.CurrentHP >= currentTower.Money * 0.2f)
            playerHP.TakeDamage(currentTower.Money * -0.2f);
        else
            playerHP.TakeDamage(-1 * playerHP.CurrentHP);
        
        systemTextViewer.PrintText(SystemType.Fired);
        // 선택한 타워가 사라져서 Panel, 공격 범위 Off
        OffPanel();
    }

}
