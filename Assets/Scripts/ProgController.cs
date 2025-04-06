using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgController : MonoBehaviour
{
    public float rotationSpeed = 180f; // 회전 속도 (도/초)

    [SerializeField] private FrogTongue _tongue;

    private bool isEating => _tongue.isEating;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isEating)
            return;
        // A키와 D키 입력 감지
        if (Input.GetKey(KeyCode.A))
        {
            // A키: 시계 반대 방향 회전 (왼쪽)
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // D키: 시계 방향 회전 (오른쪽)
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
}
