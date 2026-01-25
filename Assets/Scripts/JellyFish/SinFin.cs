using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class JellyTentacle : MonoBehaviour
{
    [Header("Tentacle")]
    public int points = 35;
    public float length = 4f;
    public Vector2 baseDir = Vector2.down;   // 기본 늘어지는 방향

    [Header("Motion (각도 휘어짐)")]
    public float bendAmplitudeDeg = 35f;     // 휘는 최대 각도(도)
    public float waveSpeed = 2.5f;           // 움직임 속도
    public float waveFrequency = 0.55f;      // 촉수 길이 방향으로 파가 몇 번 도는지(낮을수록 길게 휨)
    public AnimationCurve bendByT = AnimationCurve.EaseInOut(0, 0, 1, 1);
    // t=0(뿌리) 0, t=1(끝) 1 => 끝으로 갈수록 더 휘게

    [Header("Extra (불규칙 맛)")]
    public float secondaryWave = 0.35f;      // 2번째 파 섞기
    public float noise = 0.15f;              // 약간 랜덤감

    LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = points;
        lr.useWorldSpace = false;
        lr.numCapVertices = 12;
        lr.numCornerVertices = 6;
    }

    void Update()
    {
        Vector2 dir0 = baseDir.normalized;
        float segLen = length / (points - 1);

        Vector2 pos = Vector2.zero;
        lr.SetPosition(0, new Vector3(pos.x, pos.y, 0));

        float tTime = Time.time;

        for (int i = 1; i < points; i++)
        {
            float t = i / (float)(points - 1); // 0..1 (끝으로 갈수록 1)
            float w = bendByT.Evaluate(t);

            // “각도”를 시간/길이방향으로 변화시키기 (이게 촉수 핵심)
            float phase = (t * Mathf.PI * 2f * waveFrequency) + (tTime * waveSpeed);

            float s1 = Mathf.Sin(phase);
            float s2 = Mathf.Sin(phase * 1.7f + 1.3f) * secondaryWave;

            float n = (Mathf.PerlinNoise(t * 3.1f, tTime * 0.9f) - 0.5f) * 2f * noise;

            float angleDeg = (s1 + s2 + n) * bendAmplitudeDeg * w;

            Vector2 segDir = Rotate(dir0, angleDeg).normalized;

            pos += segDir * segLen;
            lr.SetPosition(i, new Vector3(pos.x, pos.y, 0));
        }
    }

    static Vector2 Rotate(Vector2 v, float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad);
        float s = Mathf.Sin(rad);
        return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
    }
}
