using System.Collections.Generic;
using UnityEngine;

public class FrogTongue : MonoBehaviour
{
    public Transform tongueOrigin;
    public Transform tongueTip;
    public float tongueSpeed = 10f;
    public float maxDistance = 5f;
    public LineRenderer lineRenderer;
    public int segmentCount = 20; // 혀 라인 포인트 개수
    public float curveAmplitude = 0.2f; // 구불구불 정도
    public ParticleSystem eatEffect;
    private float shootStartTime;
    public float curveDelayTime = 0.1f; // 커브 시작까지 대기 시간

    private bool isShooting = false;
    private bool isRetracting = false;
    private Vector3 targetPosition;
    private List<Transform> caughtTarget = new List<Transform>();

    public bool isEating = false;

    void Start()
    {
        lineRenderer.positionCount = segmentCount;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isShooting && !isRetracting)
        {
            isEating = true;
            isShooting = true;
            shootStartTime = Time.time; // 현재 시간 저장!
            targetPosition = tongueOrigin.position + transform.right * maxDistance;
            lineRenderer.enabled = true;
        }


        if (isShooting)
        {
            tongueTip.position = Vector3.MoveTowards(tongueTip.position, targetPosition, tongueSpeed * Time.deltaTime);
            if (Vector3.Distance(tongueTip.position, targetPosition) < 0.1f)
            {
                isShooting = false;
                isRetracting = true;
            }
        }
        else if (isRetracting)
        {
            tongueTip.position =
                Vector3.MoveTowards(tongueTip.position, tongueOrigin.position, tongueSpeed * Time.deltaTime);
            if (Vector3.Distance(tongueTip.position, tongueOrigin.position) < 0.1f)
            {


                if (caughtTarget != null && caughtTarget.Count > 0)
                {
                    for (int i = 0; i < caughtTarget.Count; i++)
                    {
                        // 점수 처리
                        InsectInfo info = caughtTarget[i].GetComponent<InsectInfo>();
                        if (info != null)
                        {
                            UIManager.Instance.AddScore((int)info.grade);
                        }

                        caughtTarget[i].parent = null;
                        Destroy(caughtTarget[i].gameObject);
                        eatEffect.Play();
                    }
                    caughtTarget.Clear();
                }
                isRetracting = false;
                isEating = false;
                lineRenderer.enabled = false;
            }
        }

        if (lineRenderer.enabled)
        {
            UpdateCurvedLine();
        }
    }

    void UpdateCurvedLine()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector3 point = Vector3.Lerp(tongueOrigin.position, tongueTip.position, t);

            // 조건: 0.1초 지난 뒤부터 커브 적용
            bool shouldUseCurve = (Time.time - shootStartTime) >= curveDelayTime;

            if (shouldUseCurve)
            {
                Vector3 direction = tongueTip.position - tongueOrigin.position;
                Vector3 normal = Vector3.Cross(direction.normalized, Vector3.forward);
                float wave = Mathf.Sin(t * Mathf.PI * 2 + Time.time * 10f) * curveAmplitude;
                point += normal * wave;
            }

            lineRenderer.SetPosition(i, point);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isShooting && other.CompareTag("Insect"))
        {
            caughtTarget.Add(other.transform);
            var transform1 = other.transform;
            transform1.parent = tongueTip;
            transform1.localPosition = Vector3.zero;
            isShooting = false;
            isRetracting = true;
        }

        if (isRetracting && other.CompareTag("Insect"))
        {
            caughtTarget.Add(other.transform);
            var transform1 = other.transform;
            transform1.parent = tongueTip;
            transform1.localPosition = Vector3.zero;
        }
    }
}