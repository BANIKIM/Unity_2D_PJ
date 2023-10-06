using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint; //�� ����
    public int stagePoint; // �������� ����
    public int stageIndex; // ��������
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
        //�������� ���� ���
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UI_Starg.text = "STAGE " + (stageIndex+1);
        }
        else//��������
        {
            //�÷��̾� ��Ʈ���� ���� 
            Time.timeScale = 0; // �ð��� ����
            Debug.Log("����Ŭ����");
            
            Text btnText = RestartBtn.GetComponentInChildren<Text>();
            btnText.text = "���� Ŭ����!";
            RestartBtn.SetActive(true);
        }



        //�� ������ �������� ���� �� ��
        totalPoint += stagePoint;
        //�������� ���� �ʱ�ȭ
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
            //ü�� ǥ��
            UI_Hp[0].color = new Color(1, 0, 0, 0.2f);
            //�÷��̾� ���� ����Ʈ
            player.OnDie();
            //UI ����
            Debug.Log("����");
            //���ν��۹�ư
            RestartBtn.SetActive(true);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            //�÷��̾� �ٽ� ���� �ø���
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
        Time.timeScale = 1;//�ð� ���� ����
        SceneManager.LoadScene(0);
    }


}
