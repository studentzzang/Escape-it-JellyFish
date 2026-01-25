using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    
 
    [Header("Position")]
    public float smoothTime = 0f;

    [Header("Rotation")]
    public float rotationLerp = 5f;
    public float _defaultTargetZ = -90;

    Vector3 velocity = Vector3.zero;
    Vector3 _targetPos;

    void FixedUpdate()
    {
        FollowTarget();
        //FollowTargetRotation();
    }
    void FollowTarget()
    {
        /*
        transform.position = Vector3.SmoothDamp(
            transform.position,
            target.position,
            ref velocity,
            smoothTime
        );
        */
        transform.position = target.position;
    }
    void FollowTargetRotation()
    {
        float targetZ = target.eulerAngles.z;

        // default 대비 얼마나 기울었는지 ( -180~180 으로 안정적으로 나옴 )
        float delta = Mathf.DeltaAngle(_defaultTargetZ, targetZ);

        // 카메라는 그 delta만큼만 기울이거나, 원하는 기준에 더해도 됨
        Quaternion desired = Quaternion.Euler(0f, 0f, delta);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desired,
            Time.deltaTime * rotationLerp
        );
    }
}
