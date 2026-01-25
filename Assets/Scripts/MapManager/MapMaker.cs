using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public Transform corridorPrefab;

    public float corridorWidth = 5f;
    public float minLen = 3f;
    public float maxLen = 10f;

    public int count = 40;
    public int maxSameTurnStreak = 3;

    [Header("Retry / Collision")]
    public int maxTriesPerSegment = 50;
    public LayerMask obstacleMask;     // 통로 레이어만 넣어라 (Default 넣지마)
    public float overlapPadding = 0.05f;

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

        // ===== 0) 첫 세그먼트는 "무조건" 오른쪽 직선 하나 생성 =====
        {
            float len0 = Mathf.Clamp(minLen, 0.1f, maxLen);
            Vector2 center0 = start + dir * (len0 * 0.5f);
            float angle0 = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            PlaceSegment(center0, angle0, len0);

            // 첫 세그먼트 끝점
            Vector2 end0 = start + dir * len0;

            // 첫 세그 이후, 원래 로직대로 다음 코너 준비를 위해 회전을 "한 번" 결정해야 하는데,
            // 네 요구는 "첫 직선은 꼭 있어야 함"이라서
            // i=1부터는 아래 tries 루프에서 정상적으로 회전 포함해서 뽑는다.
            start = end0;
        }

        // ===== 1) 나머지 세그먼트는 "겹치면 다시 뽑기" =====
        for (int i = 1; i < count; i++)
        {
            bool placed = false;

            // 지금 세그 시도 전에 상태를 저장해두고, 실패하면 되돌린다
            Vector2 savedStart = start;
            Vector2 savedDir = dir;
            int savedLastTurn = lastTurn;
            int savedStreak = streak;

            for (int tries = 0; tries < maxTriesPerSegment; tries++)
            {
                // 실패했을 때 상태 원복
                start = savedStart;
                dir = savedDir;
                lastTurn = savedLastTurn;
                streak = savedStreak;

                float len = Random.Range(minLen, maxLen);

                // ===== 현재 세그먼트 =====
                Vector2 center = start + dir * (len * 0.5f);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                // ---- 여기서 "생성 전에" 겹침 검사 ----
                if (!CanPlace(center, angle, len, corridorWidth))
                {
                    continue; // 겹치면 다시 뽑기
                }

                // 통과하면 생성
                PlaceSegment(center, angle, len);

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

                // ===== 코너 바깥쪽 보정 (네 원본 그대로) =====
                Vector2 leftPrev = Left(dir);
                Vector2 corner = end - turn * leftPrev * (corridorWidth * 0.5f);

                Vector2 nextDir = Rotate90(dir, turn);
                Vector2 leftNext = Left(nextDir);

                start = corner - turn * leftNext * (corridorWidth * 0.5f);
                dir = nextDir;

                placed = true;
                break;
            }

            if (!placed)
            {
                Debug.LogError($"Failed to place segment {i} (tries={maxTriesPerSegment}). Stop.");
                return;
            }
        }
    }

    void PlaceSegment(Vector2 center, float angle, float len)
    {
        Transform seg = Instantiate(corridorPrefab, transform);
        seg.localPosition = center;
        seg.localRotation = Quaternion.Euler(0, 0, angle);
        seg.localScale = new Vector3(len, corridorWidth, 1);
    }

    bool CanPlace(Vector2 center, float angle, float len, float width)
    {
        // overlap box는 세그의 "길이 x 폭"을 angle로 회전시켜 검사하면 됨
        float l = Mathf.Max(0.01f, len - overlapPadding);
        float w = Mathf.Max(0.01f, width - overlapPadding);

        // IMPORTANT: Physics2D.OverlapBox의 angle은 degree.
        Collider2D hit = Physics2D.OverlapBox(center, new Vector2(l, w), angle, obstacleMask);
        if (hit == null) return true;

        // 뭐가 막는지 찍어두면 레이어/바닥 콜라이더 문제 바로 잡힘
        Debug.LogWarning($"BLOCKED center={center} size=({l},{w}) angle={angle} by {hit.name} layer={LayerMask.LayerToName(hit.gameObject.layer)}");
        return false;
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

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // 필요하면 여기서 start/dir 시각화 가능
    }
#endif
}
