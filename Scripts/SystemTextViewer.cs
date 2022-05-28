using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public enum SystemType { Money = 0, Build, NotPay, ShowMoney, NoChance, Payed, NewDay, Fast, NoFast, Clear, AllClear, Fired,}

public class SystemTextViewer : MonoBehaviour
{
    [SerializeField]
    private GameObject      PanelSystem;
    [SerializeField]
    private WaveSystem      waveSystem;
    [SerializeField]
    private PlayerGold      playerGold;
    [SerializeField]
    private PlayerHP      playerHP;

    [SerializeField]
    private TextMeshProUGUI laterTitle;
    [SerializeField]
    private GameObject effectCarrot;

    private TextMeshProUGUI textSystem;
    //private TMPAlpha        tmpAlpha;

    public GameObject ThePanel => PanelSystem;

    private void Awake()
    {
        textSystem  = GetComponent<TextMeshProUGUI>();
        //tmpAlpha    = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        switch ( type)
        {
            case SystemType.Money:
                PanelSystem.SetActive(true);
                textSystem.text = "System : 돈이 부족해요...";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.Build:
                PanelSystem.SetActive(true);
                textSystem.text = "System : 배치할 수 없는 곳이에요...";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.NotPay:
                PanelSystem.SetActive(true);
                textSystem.text = "System : 일급 " + playerGold.PayGold + "원을 먼저 지급하세요!!";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.ShowMoney:
                PanelSystem.SetActive(true);
                textSystem.text = "System : " + playerGold.PayGold + "원을 대출했어요.";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.NoChance:
                PanelSystem.SetActive(true);
                textSystem.text = "System : 대출 한도가 초과되었어요...";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.Payed:
                PanelSystem.SetActive(true);
                textSystem.text = "System : 일급을 지불했어요.";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.NewDay:
                if (waveSystem.CurrentWave != 0)
                {
                    PanelSystem.SetActive(true);
                    textSystem.text = waveSystem.CurrentWave + "일째 홍보 끝";
                    laterTitle.text = waveSystem.CurrentWave + "일 급여 정산";
                    StartCoroutine("Wait");
                    StartCoroutine("WaitActiveFalse");
                }
                break;
            case SystemType.Fast:
                PanelSystem.SetActive(true);
                StartCoroutine("WaitX4");
                textSystem.text = "System : 4배속";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.NoFast:
                PanelSystem.SetActive(true);
                textSystem.text = "System : 기본속도";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.Clear:
                PanelSystem.SetActive(true);
                textSystem.text = "홍보 목표치 달성!!!";
                StartCoroutine("WaitActiveFalse");
                break;
            case SystemType.AllClear:
                PanelSystem.SetActive(true);
                if (playerHP.CurrentHP >= playerHP.MissionHP)
                    textSystem.text = "홍보를 성공적으로 마쳤다!!!";
                else
                    textSystem.text = "홍보에 실패했다!!!";
                StartCoroutine("WaitActiveFalse");
                SceneManager.LoadScene("result");
                break;
            case SystemType.Fired:
                PanelSystem.SetActive(true);
                textSystem.text = "해고한 알바를 브라운씨에게 보냈어요.";
                StartCoroutine(effectOn(effectCarrot));
                StartCoroutine("WaitActiveFalse");
                break;

        }
        
        //tmpAlpha.FadeOut();
        
        
    }

    private IEnumerator WaitActiveFalse()
    {
        yield return new WaitForSeconds(2.0f);
        PanelSystem.SetActive(false);
    }

    private IEnumerator effectOn(GameObject effectPanel)
    {
        effectPanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        effectPanel.SetActive(false);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
    }

    private IEnumerator WaitX4()
    {
        yield return new WaitForSeconds(8.0f);
    }
}
