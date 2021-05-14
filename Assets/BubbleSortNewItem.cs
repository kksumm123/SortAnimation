using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleSortNewItem : MonoBehaviour
{
    int number;
    public void SetNumber(int number)
    {
        this.number = number;
        GetComponentInChildren<TextMeshPro>().text = number.ToString();
    }
    public void ChangeColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
}
