using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer sprite;
    CapsuleCollider2D capsulcollider;


    public int nextMove;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        capsulcollider = GetComponent<CapsuleCollider2D>();
        //Invoke() - ���� �ð��� ���� ��, ������ �Լ��� �����ϴ� �Լ�
        Invoke("Think",5);//("�޼����̸�", x�� �� ����)
    }


    private void FixedUpdate()
    {
        //�����̴� �ڵ�
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);


        //����üũ (������������ �ƴ���)
        Vector2 forntVec = new Vector2(rigid.position.x + nextMove*0.4f, rigid.position.y);
        Debug.DrawRay(forntVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(forntVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }

    }

    //����Լ� - �ڽ��� ������ ȣ���ϴ� �Լ�
    void Think()//���� ������ ���� ���� �����ϴ� �޼���
    {
        nextMove = Random.Range(-1, 2);

       

        anim.SetInteger("RunSpeed", nextMove);
        if(nextMove !=0)
        sprite.flipX = nextMove == 1;


        //���
        float nextThinkTime = Random.Range(2f, 5f); // �ð�����
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;//���Ͽ� ������ �ٲ۴�.
        sprite.flipX = nextMove == 1;
        CancelInvoke(); //CancelInvoke - ���� �۵� ���� ��� Invoke�Լ��� ���ߴ� �Լ�
        Invoke("Think", 5);
    }

   public void OnDamaged()
    {
        //�ǰ� ���
        sprite.color = new Color(1, 1, 1, 0.4f);
        //�ڷ� ������ ���
        sprite.flipY = true;
        //�ݶ��̴� ��Ȱ��ȭ - �浹�̳� ��� ���� ���� 
        capsulcollider.enabled = false;
        //�׾��� �� ���� ��¦ ��
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //5�� �ڿ� ��ü�� ��Ȱ��ȭ
        Invoke("DeActive", 5);

    }



    void DeActive()
    {
        gameObject.SetActive(false);
    }

}
