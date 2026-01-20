using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public Transform corridorPrefab;

    public float corridorWidth = 5f;
    public float minLen = 3f;
    public float maxLen = 10f;

    public int count = 40;
    public int maxSameTurnStreak = 3;

    Vector2 start;   // 다음 세그먼트의 센터라인 시작점
    Vector2 dir;     // 현재 진행방향 (right/up/left/down)

    int lastTurn;
    int streak;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        start = Vector2.zero;
        dir = Vector2.right;

        lastTurn = 0;
        streak = 0;

        for (int i = 0; i < count; i++)
        {
            float len = Random.Range(minLen, maxLen);

            // ===== 현재 세그먼트 =====
            Vector2 center = start + dir * (len * 0.5f);

            Transform seg = Instantiate(corridorPrefab, transform);
            seg.localPosition = center;
            seg.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            seg.localScale = new Vector3(len, corridorWidth, 1);

            Vector2 end = start + dir * len;

            // ===== 회전 방향 결정 (+1 / -1) =====
            int turn = Random.value < 0.5f ? -1 : 1;

            if (turn == lastTurn)
            {
                streak++;
                if (streak > maxSameTurnStreak)
                {
                    turn = -turn;
                    streak = 1;
                }
            }
            else
            {
                streak = 1;
            }

            lastTurn = turn;

            // ===== 코너 바깥쪽 보정 =====
            Vector2 leftPrev = Left(dir);
            Vector2 corner = end - turn * leftPrev * (corridorWidth * 0.5f);

            Vector2 nextDir = Rotate90(dir, turn);
            Vector2 leftNext = Left(nextDir);

            start = corner - turn * leftNext * (corridorWidth * 0.5f);
            dir = nextDir;
        }
    }

    static Vector2 Left(Vector2 v)
    {
        return new Vector2(-v.y, v.x);
    }

    static Vector2 Rotate90(Vector2 dir, int turn)
    {
        // +1 = CCW, -1 = CW
        Vector2 l = Left(dir);
        return turn > 0 ? l : -l;
    }
}
