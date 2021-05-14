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
        // ��� �ڽ��� �Ⱥ��̰� ����
        cube.gameObject.SetActive(false);

        // list �̿��Ͽ� �ڽ� ������
        // item + ��ġ�� ���� ���� ��������
        for (int i = 0; i < cubeNumList.Count; i++)
        {
            var pos = cube.transform.position + offset * i;
            BubbleSortNewItem newItem = Instantiate(cube, pos, cube.transform.rotation);
            newItem.SetNumber(cubeNumList[i]);
            newItem.gameObject.SetActive(true);
            cubes.Add(newItem);
        }
        // �ڽ� �����ϰ� 0.5�� ������
        yield return new WaitForSeconds(0.5f);

        // �������� 
        for (int i = 0; i < cubeNumList.Count; i++)
        {
            for (int j = 0; j + 1 < cubeNumList.Count - i; j++)
            {
                // �ڽ� ���� ��������
                var cube1 = cubes[j];
                var cube2 = cubes[j + 1];
                yield return StartCoroutine(ChangeColorCo(cube1, cube2, Color.yellow));

                if (cubeNumList[j] > cubeNumList[j + 1])
                {
                    // ����Ʈ ���� �ٲٱ�
                    SwapNum(cubeNumList, j, j + 1);

                    // �ڽ� ��ġ �ٲٱ�
                    SwapBox(j, cube1, cube2);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    yield return StartCoroutine(ChangeColorCo(cube1, cube2, Color.green));
                }
                yield return StartCoroutine(ChangeColorCo(cube1, cube2, Color.white));
            }
            // ������ �� ȸ������ ǥ��
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
            // �ڽ��� ��ġ�� ��������
            var cube1Pos = cube1.transform.position;
            var cube2Pos = cube2.transform.position;

            // �ڽ� �迭, �ڽ� ��ġ�ٲ���
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
            // �����ϰ� ���� �Ҵ�
            // �迭�� �ִ� ���ڸ� �����ϰ� ����
            // 1) [�Ϸ�] �迭�� �ִ� ������ ���� ������ ������
            // 2) ���ڹ��� :  -�迭.ī��Ʈ ~ �迭.ī��Ʈ
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
        //������ �ִ��Ÿ� ������Ѵ�
        //�������ϰ� �ٽ��Ѵٸ� ?
        //������ �ִ� �ڽ��� ��������
        // ���� �ڷ�ƾ ����
        if (sortCoHandle != null)
            StopCoroutine(sortCoHandle);

        // �����ڽ��� �μž���
        cubes.ForEach(x => Destroy(x.gameObject));
        // �迭���� ���������Ƿ� �迭�� ��������
        cubes.Clear();

        // �ٽ� ���� ����
        sortCoHandle = StartCoroutine(SortCo());
    }
}
