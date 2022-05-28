using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class RailOn : MonoBehaviour
{
    [SerializeField]
    int levelNum;
    [SerializeField]
    private float lerpTime = 3.0f;
    [SerializeField]
    float startAlpha = 0;
    [SerializeField]
    float endAlpha = 255;

    GameObject gameData;
    ResultDatas[] gmResult;

    Tilemap spriteRenderer;

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    private void Awake()
    {
        gameData = GameObject.Find("GameData");
        gmResult = gameData.GetComponent<Data>().results;

        spriteRenderer = GetComponent<Tilemap>();

        if (gmResult[levelNum - 1].highScore > 0)
        {
            StartCoroutine(TileAlphaLerp(startAlpha, endAlpha));
            
        }

        
    }

    private IEnumerator TileAlphaLerp(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            // lerpTime 시간동안 while() 반복문 실행
            currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;

            // Text = TextMeshPro의 폰트 투명도를 start에서 end로 변경
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(start, end, percent);
            spriteRenderer.color = color;

            yield return null;
        }
        
    }

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

}
