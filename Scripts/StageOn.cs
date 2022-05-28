using UnityEngine;
using UnityEngine.UI;

public class StageOn : MonoBehaviour
{
    [SerializeField]
    int levelNum;

    GameObject gameData;
    ResultDatas[] gmResult;

    Button button;

    public void Awake()
    {
        gameData = GameObject.Find("GameData");
        gmResult = gameData.GetComponent<Data>().results;

        button = GetComponent<Button>();

        if (gmResult[levelNum - 2].highScore > 0)
        {
            button.interactable = true;

        }


    }
}
