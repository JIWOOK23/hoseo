using UnityEngine;

public class InsectInfo : MonoBehaviour
{
    public InsectGrade grade = InsectGrade.Grade1;
}
public enum InsectGrade
{
    Grade1 = 1,
    Grade2 = 2,
    Grade3 = 3,
    Grade4 = 4,
    Bomb = -10 // 폭탄벌레는 음수로 처리
}
