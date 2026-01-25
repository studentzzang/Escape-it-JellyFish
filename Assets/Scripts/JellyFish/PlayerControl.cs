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

    [Header("회전 관련")]
    public float maxTurnSpeed = 240f;   // deg/sec (최대 회전 속도)
    public float deadRadius = 0.9f;     // 이 거리 이내면 회전 고정
    private float lastTargetZ;

    private Vector3 _mouseWorld;
    private Vector2 _deltaMouse;
    float _dist;

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
        GetMousePosDelta();
        Debug.Log(_dist); 
        
    }
    private void FixedUpdate()
    {
        GoFoward();
        RotateToMouse();
    }

    void GoFoward()
    {
        Vector2 dir = transform.up; // 로컬 Y 앞
        rb.MovePosition(rb.position + dir * speed * Time.fixedDeltaTime);
        _dist = Vector2.Distance(transform.position, _mouseWorld);
    }
    void GetMousePosDelta()
    {
        _mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _deltaMouse = (Vector2)(_mouseWorld - transform.position);
    }

    void RotateToMouse()
    {
        

        // 1) 너무 가까우면 회전 목표를 고정(튐 방지)
        if (_deltaMouse.sqrMagnitude < deadRadius * deadRadius)
        {
            // 원하는 경우: 그냥 return 해서 현재 회전 유지
            // return;

            // 또는: 마지막 목표를 유지하면서 부드럽게만 수렴
            float curZ0 = transform.localEulerAngles.z;
            float nextZ0 = Mathf.SmoothDampAngle(curZ0, lastTargetZ, ref angVel, smoothTime);
            transform.localRotation = Quaternion.Euler(0f, 0f, nextZ0);
            return;
        }

        float targetZ = Mathf.Atan2(_deltaMouse.y, _deltaMouse.x) * Mathf.Rad2Deg + baseZ;
        lastTargetZ = targetZ;

        float curZ = transform.localEulerAngles.z;

        // SmoothDamp로 “원래 다음 각도” 계산
        float dampZ = Mathf.SmoothDampAngle(curZ, targetZ, ref angVel, smoothTime);

        // 2) 프레임당 최대 회전량 제한 (deg/sec)
        float maxStep = maxTurnSpeed * Time.deltaTime;
        float nextZ = Mathf.MoveTowardsAngle(curZ, dampZ, maxStep);

        transform.localRotation = Quaternion.Euler(0f, 0f, nextZ);
    }
}
