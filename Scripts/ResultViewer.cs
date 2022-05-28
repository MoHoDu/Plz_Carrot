using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultViewer : MonoBehaviour
{
    GameObject gameData;
    Datas[] gmData;
    ResultDatas[] gmResults;
    int stageNum;
    int star = 0;

    [SerializeField]
    Image[] starImage;
    [SerializeField]
    Sprite carrotStar;
    [SerializeField]
    Sprite carrotNone;
    [SerializeField]
    TextMeshProUGUI totalMissionRNum;
    [SerializeField]
    TextMeshProUGUI otherMissionR01Num;
    [SerializeField]
    TextMeshProUGUI otherMissionR02Num;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData");
        gmData = gameData.GetComponent<Data>().datas;
        gmResults = gameData.GetComponent<Data>().results;
        stageNum = gameData.GetComponent<Data>().stageNumber;

        totalMissionRNum.text = "x " + gmData[stageNum].stageCarrot + " / " + gmData[stageNum].totalCarrot;
        otherMissionR01Num.text = "x " + gmData[stageNum].stageKill + " / " + gmData[stageNum].totalM01;
        otherMissionR02Num.text = "x " + gmData[stageNum].stageMoney + " / " + gmData[stageNum].totalM02;

        if (gmData[stageNum].stageCarrot >= gmData[stageNum].totalCarrot)
        {
            if (gmData[stageNum].otherMission01 & gmData[stageNum].otherMission02)
            {
                star = 3;
            }
            else if (gmData[stageNum].otherMission01)
            {
                star = 2;
            }
            else if (gmData[stageNum].otherMission02)
            {
                star = 2;
            }
            else
            {
                star = 1;
            }

            if (gmData[stageNum].stageCarrot - gmData[stageNum].totalCarrot >= gmData[stageNum].totalCarrot * 0.4f)
            {
                star += 2;
            }
            else if (gmData[stageNum].stageCarrot - gmData[stageNum].totalCarrot >= gmData[stageNum].totalCarrot * 0.2f)
            {
                star += 1;
            }

        }
        else
        {
            star = 0;
        }

        starImage[0].sprite = carrotNone;
        starImage[1].sprite = carrotNone;
        starImage[2].sprite = carrotNone;
        starImage[3].sprite = carrotNone;
        starImage[4].sprite = carrotNone;

        if (gmResults[stageNum].highScore < star)
        {
            gmResults[stageNum].highScore = star;
            Data data = gameData.GetComponent<Data>();

            data.ScoreListAdd(stageNum, star);
            //data.SaveHighScore();
            data.Save();
        }

        while (star > 0)
        {
            starImage[star - 1].sprite = carrotStar;
            star -= 1;
        }


    }

}
