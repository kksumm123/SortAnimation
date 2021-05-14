using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BubbleSortNewManager : MonoBehaviour
{
    public BubbleSortNewItem cube;
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public List<int> cubeNumList;
    public List<BubbleSortNewItem> cubes;
    IEnumerator Start()
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
                cube1.ChangeColor(Color.yellow);
                cube2.ChangeColor(Color.yellow);
                yield return new WaitForSeconds(0.5f);
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
                    cube1.ChangeColor(Color.green);
                    cube2.ChangeColor(Color.green);
                    yield return new WaitForSeconds(0.5f);
                }
                cube1.ChangeColor(Color.white);
                cube2.ChangeColor(Color.white);
                yield return new WaitForSeconds(0.5f);
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
    }

}
