using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;      // 전체화면을 덮는 빨간색 이미지
    [SerializeField]
    private float   maxHP = 20;     // 최대 체력
    [SerializeField]
    private float   missionHP = 160;    // 목표 당근
    [SerializeField]
    private int missionEnemy = 100;    // 목표 적 처리 수
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private float   currentHP;      // 현재 체력

    private float   takeDamagePoint;     // 당근 획득량
    

    public float    MaxHP => maxHP;
    public float    CurrentHP => currentHP;
    public float    MissionHP => missionHP;
    public int      MiaaionEnemy => missionEnemy;
    public float    TakeDamagePoint => takeDamagePoint;

    private void Awake()
    {
        currentHP   = maxHP;        // 현재 체력을 최대 체력과 같게 설정
    }

    public void TakeDamage(float damage)
    {
        // 현재 체력을 damage만큼 감소
        takeDamagePoint = damage;
        currentHP += damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        // 체력이 0이 되면 게임오버
        //if (currentHP <= 0)
        //{
        //    SceneManager.LoadScene("main");
        //}

        if (currentHP >= missionHP)
        {
            textPlayerHP.text = "<color=#0000ffff>" + currentHP + " / " + missionHP + "</color>";
            systemTextViewer.PrintText(SystemType.Clear);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        // 전체화면 크기로 배치된 imageScreen의 색상을 color 변수에 저장
        // imageScreen의 투명도를 40%로 설정
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        // 투명도가 0%가 될때까지 감소
        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}
