using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BubbleSortNewManager : MonoBehaviour
{
    public BubbleSortNewItem cube;
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public List<int> cubeNumList;
    public List<BubbleSortNewItem> cubes;
    private Coroutine sortCoHandle;

    void Start()
    {
        StartSort();
    }
    IEnumerator SortCo()
    {
        // 토대 박스는 안보이게 하자
        cube.gameObject.SetActive(false);

        // list 이용하여 박스 만들자
        // item + 위치값 더한 곳에 생성하자
        for (int i = 0; i < cubeNumList.Count; i++)
        {
            var pos = cube.transform.position + offset * i;
            BubbleSortNewItem newItem = Instantiate(cube, pos, cube.transform.rotation);
            newItem.SetNumber(cubeNumList[i]);
            newItem.gameObject.SetActive(true);
            cubes.Add(newItem);
        }
        // 박스 생성하고 0.5초 멈추자
        yield return new WaitForSeconds(0.5f);

        // 버블정렬 
        for (int i = 0; i < cubeNumList.Count; i++)
        {
            for (int j = 0; j + 1 < cubeNumList.Count - i; j++)
            {
                // 박스 정보 가져오자
                var cube1 = cubes[j];
                var cube2 = cubes[j + 1];
                yield return StartCoroutine(ChangeColorCo(cube1, cube2, Color.yellow));

                if (cubeNumList[j] > cubeNumList[j + 1])
                {
                    // 리스트 숫자 바꾸기
                    SwapNum(cubeNumList, j, j + 1);

                    // 박스 위치 바꾸기
                    SwapBox(j, cube1, cube2);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    yield return StartCoroutine(ChangeColorCo(cube1, cube2, Color.green));
                }
                yield return StartCoroutine(ChangeColorCo(cube1, cube2, Color.white));
            }
            // 고정된 블럭 회색으로 표시
            cubes[cubeNumList.Count - 1 - i].ChangeColor(Color.gray);
        }

        void SwapNum(List<int> list, int idx1, int idx2)
        {
            int temp = list[idx1];
            list[idx1] = list[idx2];
            list[idx2] = temp;
        }

        void SwapBox(int j, BubbleSortNewItem cube1, BubbleSortNewItem cube2)
        {
            // 박스의 위치를 가져오자
            var cube1Pos = cube1.transform.position;
            var cube2Pos = cube2.transform.position;

            // 박스 배열, 박스 위치바꾸자
            cube1.ChangeColor(Color.red);
            cube2.ChangeColor(new Color(255, 0, 0));
            cubes[j] = cube2;
            cubes[j + 1] = cube1;
            cube2.transform.DOMove(cube1Pos, 0.5f).SetEase(Ease.InBounce);
            cube1.transform.DOMove(cube2Pos, 0.5f).SetEase(Ease.InBounce);
        }

        IEnumerator ChangeColorCo(
            BubbleSortNewItem cube1, BubbleSortNewItem cube2, Color color)
        {
            cube1.ChangeColor(color);
            cube2.ChangeColor(color);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartSort();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // 랜덤하게 숫자 할당
            // 배열에 있는 숫자를 랜덤하게 변경
            // 1) [완료] 배열에 있는 갯수와 같은 갯수로 만들자
            // 2) 숫자범위 :  -배열.카운트 ~ 배열.카운트
            for (int i = 0; i < cubeNumList.Count; i++)
            {
                int randInt =
                    UnityEngine.Random.Range(-cubeNumList.Count, cubeNumList.Count);
                cubeNumList[i] = randInt;
            }
        }
    }

    private void StartSort()
    {
        //기존에 있던거를 멈춰야한다
        //구현안하고 다시한다면 ?
        //기존에 있던 박스와 겹쳐진다
        // 기존 코루틴 정지
        if (sortCoHandle != null)
            StopCoroutine(sortCoHandle);

        // 기존박스를 부셔야함
        cubes.ForEach(x => Destroy(x.gameObject));
        // 배열에는 남아있으므로 배열도 비워줘야함
        cubes.Clear();

        // 다시 정렬 시작
        sortCoHandle = StartCoroutine(SortCo());
    }
}
