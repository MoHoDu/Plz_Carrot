using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    Level level;

    public void GoGame()
    {
        int levelNumber = level.levelNum + 1;
        SceneManager.LoadScene("game" + levelNumber);
    }
}
