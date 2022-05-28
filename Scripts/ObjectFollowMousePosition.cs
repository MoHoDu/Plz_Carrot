using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{
    private Camera  mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 모바일인 경우 터치 좌표를 기준으로 게임 월드 상의 좌표를 구한다.
        if (Input.touchCount > 0)
        {
            Vector3 position = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            transform.position = mainCamera.ScreenToWorldPoint(position);
            // z 위치를 0으로 설정
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        // 화면의 마우스 좌표를 기준으로 게임 월드 상의 좌표를 구한다.
        else
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            transform.position = mainCamera.ScreenToWorldPoint(position);
            // z 위치를 0으로 설정
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }


    }
}
