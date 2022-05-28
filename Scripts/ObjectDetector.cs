﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner    towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera          mainCamera;
    private Ray             ray;
    private RaycastHit      hit;
    private Transform       hitTransform = null;    // 마우스 픽킹으로 선택한 오브젝트 임시 저장
    private Touch           tempTouchs;
    //private Vector3         touchedPos;

    private void Awake()
    {
        // "MainCamera" 태그를 가지고 있는 오브젝트 탐색 후 Camera 컴포넌트 정보 전달
        // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();와 동일
        mainCamera = Camera.main;

    }

    private void Update()
    {
        if (Application.platform != RuntimePlatform.Android)
        {

            //마우스가 UI에 머물러 있을 때는 아래 코드가 실행되지 않도록 함(모바일로 할 시 마우스 부분 지워야함)
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                return;
            }


            // 마우스 왼쪽 버튼을 눌렀을 때
            if (Input.GetMouseButtonDown(0))
            {
                // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
                // ray.origin : 광선의 시작위치(=카메라 위치)
                // ray.direction : 광선의 진행방향
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
                // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    hitTransform = hit.transform;

                    // 광선에 부딪힌 오브젝트의 태그가 "file"이면
                    if (hit.transform.CompareTag("Tile"))
                    {
                        // 타워를 생성하는 SpawnTower() 호출
                        towerSpawner.SpawnTower(hit.transform);
                    }
                    // 타워를 선택하면 해당 타워 정보를 출력하는 타워 정보창 On
                    else if (hit.transform.CompareTag("Tower"))
                    {
                        towerDataViewer.OnPanel(hit.transform);
                    }

                }
                else
                {
                    towerSpawner.CancelTower();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                // 마우스를 눌렀다 때었을 때 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
                if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
                {
                    // 타워 정보 패널을 비활성화 한다
                    towerDataViewer.OffPanel();
                }

                hitTransform = null;
            }

            // 윗 부분까지가 마우스 부분
        }
        else
        {

            if (Input.touchCount > 0)
            {
                //for (int i = 0; i < Input.touchCount; i++)
                //{
                //    tempTouchs = Input.GetTouch(i);
                //    if (tempTouchs.phase == TouchPhase.Ended)
                //    {
                //        // 카메라 위치에서 화면의 마우스 위치를 관통하는 광선 생성
                //        // ray.origin : 광선의 시작위치(=카메라 위치)
                //        // ray.direction : 광선의 진행방향
                //        ray = mainCamera.ScreenPointToRay(tempTouchs.position);

                //        break;
                //    }
                //}
                tempTouchs = Input.touches[0];

                //if (IsPointerOverUIObject(tempTouchs.position))
                //{
                //Debug.Log("pointer1");
                //    return; 
                //}

                if (EventSystem.current.IsPointerOverGameObject(tempTouchs.fingerId))
                {
                    //Debug.Log("pointer2");
                    return;
                }


                if (tempTouchs.phase == TouchPhase.Ended)
                {


                    ray = mainCamera.ScreenPointToRay(tempTouchs.position);
                    // 2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법
                    // 광선에 부딪히는 오브젝트를 검출해서 hit에 저장
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        hitTransform = hit.transform;

                        // 광선에 부딪힌 오브젝트의 태그가 "file"이면
                        if (hit.transform.CompareTag("Tile"))
                        {
                            // 타워를 생성하는 SpawnTower() 호출
                            towerSpawner.SpawnTower(hit.transform);
                        }
                        // 타워를 선택하면 해당 타워 정보를 출력하는 타워 정보창 On
                        else if (hit.transform.CompareTag("Tower"))
                        {
                            towerDataViewer.OnPanel(hit.transform);
                        }
                    }
                    //touchOn = true;
                    else
                    {
                        towerSpawner.CancelTower();
                    }

                }
                else if (tempTouchs.phase == TouchPhase.Began)
                {
                    // 터치를 눌렀다 때었을 때 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
                    if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
                    {
                        // 타워 정보 패널을 비활성화 한다
                        towerDataViewer.OffPanel();
                        //touchOn = false;

                    }
                    hitTransform = null;
                }
            }

        }
    }

    //public bool IsPointerOverUIObject(Vector2 touchPos)
    //{
    //    PointerEventData eventDataCurrentPosition
    //        = new PointerEventData(EventSystem.current);

    //    eventDataCurrentPosition.position = touchPos;

    //    List<RaycastResult> results = new List<RaycastResult>();


    //    EventSystem.current
    //    .RaycastAll(eventDataCurrentPosition, results);

    //    return results.Count > 0;
    //}
}