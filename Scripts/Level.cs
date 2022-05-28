using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    [SerializeField]
    private level[] levels;
    [SerializeField]
    TextMeshProUGUI enemyName;
    [SerializeField]
    TextMeshProUGUI enemyTip;
    [SerializeField]
    TextMeshProUGUI levelTitle;
    [SerializeField]
    TextMeshProUGUI totalMissionNum;
    [SerializeField]
    TextMeshProUGUI otherMission01Num;
    [SerializeField]
    TextMeshProUGUI otherMission02Num;
    [SerializeField]
    Image enemy01Sprite;
    [SerializeField]
    Image enemy02Sprite;
    [SerializeField]
    Image enemy03Sprite;
    [SerializeField]
    Image enemy04Sprite;
    [SerializeField]
    Image[] starImage;
    [SerializeField]
    Sprite carrotStar;
    [SerializeField]
    Sprite carrotNone;

    GameObject gameData;
    //Datas[] gmData;
    ResultDatas[] gmResult;

    private int star = 0;

    public int levelNum = 0;

    //public EnemyTips PreEnemy01 => preEnemy01;
    //public EnemyTips PreEnemy02 => preEnemy02;
    //public EnemyTips PreEnemy03 => preEnemy03;
    //public EnemyTips PreEnemy04 => preEnemy04;

    private void Awake()
    {
        gameData = GameObject.Find("GameData");
        //gmData = gameData.GetComponent<Data>().datas;
        gmResult = gameData.GetComponent<Data>().results;

    }

    public void UpdateLevelTips(int num)
    {
        levelTitle.text = levels[num].LevelName;
        totalMissionNum.text = "x " + levels[num].TotalMission;
        otherMission01Num.text = "x " + levels[num].OtherMission01;
        otherMission02Num.text = "x " + levels[num].OtherMission02;
        enemy01Sprite.sprite = levels[num].PreEnemy01.EnemySprite;
        enemy02Sprite.sprite = levels[num].PreEnemy02.EnemySprite;
        enemy03Sprite.sprite = levels[num].PreEnemy03.EnemySprite;
        enemy04Sprite.sprite = levels[num].PreEnemy04.EnemySprite;

        if (levels[num].PreEnemy01.name == "Enemy04")
        {
            RectTransform enemysprite = enemy01Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 80);
        }
        else
        {
            RectTransform enemysprite = enemy01Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 100);
        }
      
        if (levels[num].PreEnemy02.name == "Enemy04")
        {
            RectTransform enemysprite = enemy02Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 80);
        }
        else
        {
            RectTransform enemysprite = enemy02Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 100);
        }

        if (levels[num].PreEnemy03.name == "Enemy04")
        {
            RectTransform enemysprite = enemy03Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 80);
        }
        else
        {
            RectTransform enemysprite = enemy03Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 100);
        }

        if (levels[num].PreEnemy04.name == "Enemy04")
        {
            RectTransform enemysprite = enemy04Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 80);
        }
        else
        {
            RectTransform enemysprite = enemy04Sprite.GetComponent<RectTransform>();
            enemysprite.sizeDelta = new Vector2(100, 100);
        }

        star = gmResult[num].highScore;
        levels[num].highScore = star;

        starImage[0].sprite = carrotNone;
        starImage[1].sprite = carrotNone;
        starImage[2].sprite = carrotNone;
        starImage[3].sprite = carrotNone;
        starImage[4].sprite = carrotNone;
        while (star > 0)
        {
            starImage[star - 1].sprite = carrotStar;
            star -= 1;
        }

        levelNum = num;
    }

    public void UpdateData01()
    {
        enemyName.text = levels[levelNum].PreEnemy01.EnemyName;
        enemyTip.text = levels[levelNum].PreEnemy01.EnemyToolTip;
    }

    public void UpdateData02()
    {
        enemyName.text = levels[levelNum].PreEnemy02.EnemyName;
        enemyTip.text = levels[levelNum].PreEnemy02.EnemyToolTip;
    }

    public void UpdateData03()
    {
        enemyName.text = levels[levelNum].PreEnemy03.EnemyName;
        enemyTip.text = levels[levelNum].PreEnemy03.EnemyToolTip;
    }

    public void UpdateData04()
    {
        enemyName.text = levels[levelNum].PreEnemy04.EnemyName;
        enemyTip.text = levels[levelNum].PreEnemy04.EnemyToolTip;
    }
}

[System.Serializable]   // 구조체 변수 직렬화
public struct level  // 웨이브 구조체 생성
{
    [SerializeField]
    string levelName;
    [SerializeField]
    int totalMission;
    [SerializeField]
    int otherMission01;
    [SerializeField]
    int otherMission02;
    [SerializeField]
    EnemyTips preEnemy01;
    [SerializeField]
    EnemyTips preEnemy02;
    [SerializeField]
    EnemyTips preEnemy03;
    [SerializeField]
    EnemyTips preEnemy04;

    public string LevelName => levelName;
    public int TotalMission => totalMission;
    public int OtherMission01 => otherMission01;
    public int OtherMission02 => otherMission02;
    public EnemyTips PreEnemy01 => preEnemy01;
    public EnemyTips PreEnemy02 => preEnemy02;
    public EnemyTips PreEnemy03 => preEnemy03;
    public EnemyTips PreEnemy04 => preEnemy04;

    public int highScore;
}