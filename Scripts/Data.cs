using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Tiles
{
    public int TileID;
    public int X;
    public int Y;
    public int Z;

    public Tiles(int id, int x, int y, int z)
    {
        TileID = id;
        X = x;
        Y = y;
        Z = z;
    }

}

public class Score
{
    public int MapID;
    public int HighScore;

    public Score(int id, int score)
    {
        MapID = id;
        HighScore = score;
    }
}

public class Data : MonoBehaviour
{
    [SerializeField]
    public Datas[] datas;
    [SerializeField]
    public ResultDatas[] results;


    public int stageNumber;
    public Vector3Int[ , ] tileNumber;
    public string[] tileText;

    public List<Tiles> TileList = new List<Tiles>();
    public List<Score> ScoreList = new List<Score>();

    private void Start()
    {

    }

    //private void Awake()
    //{
    //    LoadTileData();
    //}

    public void TileListAdd(int tileID, int x, int y, int z)
    {
        int a = 0;
        for (int i = 0; i < TileList.Count; i++)
        {
            if (TileList[i].X == x && TileList[i].Y == y && TileList[i].Z == z)
            {
                if (TileList[i].TileID == tileID)
                {
                    a += 1;
                    break;
                }
                else
                {
                    a += 1;
                    TileList[i] = new Tiles(tileID, x, y, z);
                    break;
                }
            }
        }
        //Debug.Log(a);
        if (a == 0)
            TileList.Add(new Tiles(tileID, x, y, z));
    }

    public void ScoreListAdd(int mapID, int score)
    {
        int a = 0;
        for (int i = 0; i < ScoreList.Count; i++)
        {
            if (ScoreList[i].MapID == mapID)
            {
                ScoreList[i] = new Score(mapID, score);
                a += 1;
                break;
            }
        }
        //Debug.Log(a);
        if (a == 0)
            ScoreList.Add(new Score(mapID, score));

    }

    public void UpdateStageData(int stageNum, int kill, float carrot, int money, bool clear01, bool clear02, float totalPoints, int M01, int M02)
    {
        datas[stageNum -1].stageKill = kill;
        datas[stageNum -1].stageCarrot = carrot;
        datas[stageNum -1].stageMoney = money;
        datas[stageNum -1].otherMission01 = clear01;
        datas[stageNum -1].otherMission02 = clear02;
        datas[stageNum -1].totalCarrot = totalPoints;
        datas[stageNum -1].totalM01 = M01;
        datas[stageNum -1].totalM02 = M02;

        stageNumber = stageNum - 1;
    }

    public void Save()
    {
        //Debug.Log("Save");

        //for (int i = 0; i < TileList.Count; i++)
        //{
        //    Debug.Log(TileList[i].TileID);
        //}

        //for (int i = 0; i < ScoreList.Count; i++)
        //{
        //    Debug.Log(ScoreList[i].MapID);
        //}

        // 모바일
        #if !WEB_BUILD
        JsonData TileJson = JsonMapper.ToJson(TileList);
        JsonData ScoreJson = JsonMapper.ToJson(ScoreList);

        //Debug.Log("scoreList : ");
        //Debug.Log(ScoreJson.ToString());

        string tilePath = pathForDocumentsFile("TileData.json");
        string scorePath = pathForDocumentsFile("ScoreData.json");

        FileStream tileFile = new FileStream(tilePath, FileMode.Create, FileAccess.Write);
        FileStream scoreFile = new FileStream(scorePath, FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(tileFile);
        StreamWriter sw2 = new StreamWriter(scoreFile);
        sw.WriteLine(TileJson.ToString());
        sw2.WriteLine(ScoreJson.ToString());

        sw.Close();
        sw2.Close();
        tileFile.Close();
        scoreFile.Close();

            //File.WriteAllText(Application.persistentDataPath
            //                + "/Assets/Data/TileData.json"
            //                , TileJson.ToString());
            //File.WriteAllText(Application.persistentDataPath
            //                + "/Assets/Data/ScoreData.json"
            //                , ScoreJson.ToString());
        #endif
        //JsonData TileJson = JsonMapper.ToJson(TileList);
        //JsonData ScoreJson = JsonMapper.ToJson(ScoreList);

        //File.WriteAllText(Application.dataPath
        //                + "/Data/TileData.json"
        //                , TileJson.ToString());
        //File.WriteAllText(Application.dataPath
        //                + "/Data/ScoreData.json"
        //                , ScoreJson.ToString());




    }

    public string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }

        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }

    public void Load()
    {
        #if !WEB_BUILD
        string tilePath = pathForDocumentsFile("TileData.json");
        string scorePath = pathForDocumentsFile("ScoreData.json");

        if (File.Exists(tilePath))
        {
            FileStream tileFile = new FileStream(tilePath, FileMode.Open, FileAccess.Read);
            StreamReader tileSr = new StreamReader(tileFile);
            FileStream scoreFile = new FileStream(scorePath, FileMode.Open, FileAccess.Read);
            StreamReader scoreSr = new StreamReader(scoreFile);

            string tileStr = null;
            tileStr = tileSr.ReadLine();
            string scoreStr = null;
            scoreStr = scoreSr.ReadLine();

            tileSr.Close();
            tileFile.Close();
            scoreSr.Close();
            scoreFile.Close();

            JsonData tileData = JsonMapper.ToObject(tileStr);
            JsonData scoreData = JsonMapper.ToObject(scoreStr);

            TileList.Clear();
            ScoreList.Clear();

            for (int i = 0; i < tileData.Count; i++)
            {
                //Debug.Log(tileData[i]["TileID"].ToString());
                //Debug.Log(tileData[i]["X"].ToString());
                //Debug.Log(tileData[i]["Y"].ToString());
                //Debug.Log(tileData[i]["Z"].ToString());
                TileList.Add(new Tiles(int.Parse(tileData[i]["TileID"].ToString()),
                                        int.Parse(tileData[i]["X"].ToString()),
                                        int.Parse(tileData[i]["Y"].ToString()),
                                        int.Parse(tileData[i]["Z"].ToString())));
            }
            for (int i = 0; i < scoreData.Count; i++)
            {
                //Debug.Log(scoreData[i]["MapID"].ToString());
                //Debug.Log(scoreData[i]["HighScore"].ToString());
                ScoreList.Add(new Score(int.Parse(scoreData[i]["MapID"].ToString()),
                                        int.Parse(scoreData[i]["HighScore"].ToString())));

                results[i].highScore = ScoreList[i].HighScore;
            }

        }

        else
        {

        }
        #else
        
        #endif

        //if (Application.platform == RuntimePlatform.Android)
        //{

        //    WWW www = new WWW("fill://"
        //                    + Application.persistentDataPath
        //                    + "/Assets/Data/TileData.json");
        //    while (!www.isDone) { }

        //    string TileJsonstring = www.text;
        //    JsonData tileData = JsonMapper.ToObject(TileJsonstring);

        //    WWW vvv = new WWW("fill://"
        //                    + Application.persistentDataPath
        //                    + "/Assets/Data/ScoreData.json");

        //    while (!vvv.isDone) { }

        //    string ScoreJsonstring = www.text;
        //    JsonData scoreData = JsonMapper.ToObject(ScoreJsonstring);

        //    TileList.Clear();
        //    ScoreList.Clear();

        //    for (int i = 0; i < tileData.Count; i++)
        //    {
        //        //Debug.Log(tileData[i]["TileID"].ToString());
        //        //Debug.Log(tileData[i]["X"].ToString());
        //        //Debug.Log(tileData[i]["Y"].ToString());
        //        //Debug.Log(tileData[i]["Z"].ToString());
        //        TileList.Add(new Tiles(int.Parse(tileData[i]["TileID"].ToString()),
        //                                int.Parse(tileData[i]["X"].ToString()),
        //                                int.Parse(tileData[i]["Y"].ToString()),
        //                                int.Parse(tileData[i]["Z"].ToString())));
        //    }
        //    for (int i = 0; i < scoreData.Count; i++)
        //    {
        //        //Debug.Log(scoreData[i]["MapID"].ToString());
        //        //Debug.Log(scoreData[i]["HighScore"].ToString());
        //        ScoreList.Add(new Score(int.Parse(scoreData[i]["MapID"].ToString()),
        //                                int.Parse(scoreData[i]["HighScore"].ToString())));

        //        results[i].highScore = ScoreList[i].HighScore;
        //    }
        //}
        //else
        //{
        //    Debug.Log("Load");

        //    string TileJsonstring = File.ReadAllText(Application.dataPath
        //                                                + "/Data/TileData.json");
        //    string ScoreJsonstring = File.ReadAllText(Application.dataPath
        //                                                + "/Data/ScoreData.json");

        //    //Debug.Log(TileJsonstring);
        //    //Debug.Log(ScoreJsonstring);


        //    JsonData tileData = JsonMapper.ToObject(TileJsonstring);
        //    JsonData scoreData = JsonMapper.ToObject(ScoreJsonstring);

        //    TileList.Clear();
        //    ScoreList.Clear();

        //    for (int i = 0; i < tileData.Count; i++)
        //    {
        //        //Debug.Log(tileData[i]["TileID"].ToString());
        //        //Debug.Log(tileData[i]["X"].ToString());
        //        //Debug.Log(tileData[i]["Y"].ToString());
        //        //Debug.Log(tileData[i]["Z"].ToString());
        //        TileList.Add(new Tiles(int.Parse(tileData[i]["TileID"].ToString()),
        //                                int.Parse(tileData[i]["X"].ToString()),
        //                                int.Parse(tileData[i]["Y"].ToString()),
        //                                int.Parse(tileData[i]["Z"].ToString())));
        //    }
        //    for (int i = 0; i < scoreData.Count; i++)
        //    {
        //        //Debug.Log(scoreData[i]["MapID"].ToString());
        //        //Debug.Log(scoreData[i]["HighScore"].ToString());
        //        ScoreList.Add(new Score(int.Parse(scoreData[i]["MapID"].ToString()),
        //                                int.Parse(scoreData[i]["HighScore"].ToString())));

        //        results[i].highScore = ScoreList[i].HighScore;
        //    }
        //}

    }



}

[System.Serializable]   // 구조체 변수 직렬화
public struct Datas  // 웨이브 구조체 생성
{ 
    public int stageKill;
    public float stageCarrot;
    public int stageMoney;
    public bool otherMission01;
    public bool otherMission02;

    public float totalCarrot;
    public int totalM01;
    public int totalM02;
}

[System.Serializable]
public struct ResultDatas
{
    public int highScore;
}