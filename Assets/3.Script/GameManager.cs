using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint; //총 점수
    public int stagePoint; // 스테이지 점수
    public int stageIndex; // 스테이지
    public int HP;

    public void NextStage()
    {
        //스테이지 레벨 상승
        stageIndex++;

        //총 점수에 스테이지 점수 더 함
        totalPoint += stagePoint;
        //스테이지 점수 초기화
        stagePoint = 0;
    }

}
