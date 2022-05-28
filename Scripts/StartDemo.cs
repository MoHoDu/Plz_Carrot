using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDemo : MonoBehaviour
{
    GameObject gameData;
    Data data;

    private void Awake()
    {
        gameData = GameObject.Find("GameData");
        data = gameData.GetComponent<Data>();
    }

    public void GoTown()
    {
        SceneManager.LoadScene("town");
    }

    public void ReGame()
    {
        int levelNumber = data.stageNumber + 1;
        SceneManager.LoadScene("game" + levelNumber);
    }

    public void MapSelect()
    {
        SceneManager.LoadScene("map");
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
