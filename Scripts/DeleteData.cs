using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class DeleteData : MonoBehaviour
{
    GameObject gameData;
    Data data;

    public void Awake()
    {
        gameData = GameObject.Find("GameData");
        data = gameData.GetComponent<Data>();
    }

    public void DataClear()
    {
        data.TileList.Clear();
        data.ScoreList.Clear();

        if (Application.platform == RuntimePlatform.Android)
        {

            File.Delete(Application.persistentDataPath
                            + "/TileData.json"
                            );
            File.Delete(Application.persistentDataPath
                            + "/ScoreData.json"
                            );

    }
        else
        {

            File.Delete(Application.dataPath
                                + "/TileData.json"
                                );
            File.Delete(Application.dataPath
                                + "/ScoreData.json"
                                );
        }

data.Save();

        for (int i = 0; i < data.results.Length; i++)
        {
            data.results[i].highScore = 0;
        }
    }
}
