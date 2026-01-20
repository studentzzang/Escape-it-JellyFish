/// <summary>
/// 지느러미 그룹부모에 부착
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinAutoColor : MonoBehaviour
{
    Color color;
    void Start()
    {
        GetColor();
        SetColor();
    }

    void GetColor()
    {
        SpriteRenderer headSprite = GetComponentInParent<SpriteRenderer>();
        color = headSprite.color;

    }

    void SetColor()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        LineRenderer lr;
        for (int i=1; i<transform.childCount; i++) {
            lr = children[i].GetComponent<LineRenderer>();

            lr.sharedMaterial.SetColor("_Color", color);
        }
    }
}
