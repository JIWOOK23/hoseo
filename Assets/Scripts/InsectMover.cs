using UnityEngine;

public class InsectMover : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    private float moveTimer = 0f;
    private float moveDuration;

    private bool isCaught = false;

    private float minDistance = 0.5f;
    private float maxDistance = 3f;

    private float directionChangeInterval = 1f; // 등급에 따라 줄어듦

    void Start()
    {
        var info = GetComponent<InsectInfo>();

        switch (info.grade)
        {
            case InsectGrade.Grade1:
                moveSpeed = 1f;
                directionChangeInterval = 2f;
                break;
            case InsectGrade.Grade2:
                moveSpeed = 1.5f;
                directionChangeInterval = 1.5f;
                break;
            case InsectGrade.Grade3:
                moveSpeed = 2f;
                directionChangeInterval = 1f;
                break;
            case InsectGrade.Grade4:
                moveSpeed = 2.5f;
                directionChangeInterval = 0.6f; // 빠르게 휙휙 바꿈
                break;
            case InsectGrade.Bomb:
                moveSpeed = 1.5f;
                directionChangeInterval = 1.5f;
                break;
        }

        PickNewDirection();
    }

    void Update()
    {
        if (isCaught) return;

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 회전 (방향 바라보게)
        if (moveDirection != Vector2.zero)
            transform.up  = moveDirection; // 또는 transform.right = moveDirection;right 

        moveTimer += Time.deltaTime;

        if (moveTimer >= directionChangeInterval)
        {
            PickNewDirection();
        }
    }


    void PickNewDirection()
    {
        // x/y 거리값 랜덤 설정 (튀는 움직임 유도)
        float x = Random.Range(-2f, 2f);
        float y = Random.Range(-4f, 4f);

        moveDirection = new Vector2(x, y).normalized;
        moveTimer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCaught) return;

        if (collision.collider.CompareTag("Wall"))
        {
            // 방향을 새로 골라버리기 (등급 높을수록 더 튀게)
            PickNewDirection();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tongue"))
        {
            isCaught = true;
            moveDirection = Vector2.zero;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null) rb.velocity = Vector2.zero;
        }
    }
}
