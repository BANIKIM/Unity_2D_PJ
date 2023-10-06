using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint; //총 점수
    public int stagePoint; // 스테이지 점수
    public int stageIndex; // 스테이지
    public int HP;
    public Player_Move player;
    public GameObject[] Stages;

    public Image[] UI_Hp;
    public Text UI_Point;
    public Text UI_Starg;
    public GameObject RestartBtn;

    private void Update()
    {
        UI_Point.text = (totalPoint + stagePoint).ToString();
    }


    public void NextStage()
    {
        //스테이지 레벨 상승
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UI_Starg.text = "STAGE " + (stageIndex+1);
        }
        else//게임종료
        {
            //플레이어 컨트롤을 막음 
            Time.timeScale = 0; // 시간을 멈춤
            Debug.Log("게임클리어");
            
            Text btnText = RestartBtn.GetComponentInChildren<Text>();
            btnText.text = "게임 클리어!";
            RestartBtn.SetActive(true);
        }



        //총 점수에 스테이지 점수 더 함
        totalPoint += stagePoint;
        //스테이지 점수 초기화
        stagePoint = 0;
    }

    public void HpDown()
    {
        if(HP > 1)
        {
            HP--;
            UI_Hp[HP].color = new Color(1, 0, 0, 0.2f);
        }
        else
        {
            //체력 표시
            UI_Hp[0].color = new Color(1, 0, 0, 0.2f);
            //플레이어 죽음 이펙트
            player.OnDie();
            //UI 리셋
            Debug.Log("죽음");
            //새로시작버튼
            RestartBtn.SetActive(true);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            //플레이어 다시 위로 올리기
            if(HP>1)
            {
                PlayerReposition();
            }
            HpDown();
        }
    }


    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 1);
        player.VelocityZero();
    }


    public void Restart()
    {
        Time.timeScale = 1;//시간 멈춤 복구
        SceneManager.LoadScene(0);
    }


}
