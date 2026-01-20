using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("앞스피트")]

    public float speed = 5f;
    public float maxSpeed = 10f;
    public float minSpeed = 3f;

    // 천적 스피드가 디폴트스피트 +- 0.5정도

    public float smoothTime = 0.18f;
    float baseZ;
    float angVel;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 시작 시 로컬 회전값 저장
        baseZ = transform.localEulerAngles.z;

   
    }

    void Update()
    {
        RotateToMouse();
    }
    private void FixedUpdate()
    {
        GoFoward();
    }

    void GoFoward()
    {
        Vector2 dir = transform.up; // 로컬 Y 앞
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
    }

    void RotateToMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 d = mouseWorld - transform.position;

        float targetZ = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg + baseZ;

        float curZ = transform.localEulerAngles.z;
        float nextZ = Mathf.SmoothDampAngle(curZ, targetZ, ref angVel, smoothTime);

        transform.localRotation = Quaternion.Euler(0f, 0f, nextZ);
    }
}
