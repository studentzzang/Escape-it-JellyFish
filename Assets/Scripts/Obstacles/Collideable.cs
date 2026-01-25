/// <summary>
/// 모든 충돌가능 물체에 부착
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collideable : MonoBehaviour
{
    [Header("Corridor 1당 확률과 가변되어갈때 최대확률")]
    public float _defaultProb = 30f;
    public float _maxProb = 60;

    // TODO: 다른 Obs_ 스크립트의 특정함수 퍼블릭받아오기

    private void OnCollisionEnter2D(Collision2D collision)
    {
        React();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        React();
    }
    void React()
    {
        // TODO: 다른 Obs_ 스크립트의 특정함수 퍼블릭받아오기 호출하기
    }
}
