using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint; //�� ����
    public int stagePoint; // �������� ����
    public int stageIndex; // ��������
    public int HP;

    public void NextStage()
    {
        //�������� ���� ���
        stageIndex++;

        //�� ������ �������� ���� �� ��
        totalPoint += stagePoint;
        //�������� ���� �ʱ�ȭ
        stagePoint = 0;
    }

}
