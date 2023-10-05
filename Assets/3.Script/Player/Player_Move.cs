using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Rigidbody2D rigid; //������ �ٵ��̵��� ���� ����
    SpriteRenderer sprite; //�¿� ������ ���� ����
    Animator anim; // �ִϸ��̼� ������ ���� ����


    [SerializeField] private float MaxSpeed;
    [SerializeField] private float jump_Power;


    private int Jump_Count = 0;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //����
        if (Input.GetButtonDown("Jump")) // �ӵ� ���� ���� ���� �и��� ����
        {
            //normalized������ ���� �� ���� +, -�� �� �� �ִ�.
            rigid.AddForce(Vector2.up * jump_Power, ForceMode2D.Impulse);
            anim.SetBool("isJump", true); // ������ �ִϸ��̼� ����
        }


        if (Input.GetButtonUp("Horizontal")) // �ӵ� ���� ���� ���� �и��� ����
        {
             //normalized������ ���� �� ���� +, -�� �� �� �ִ�.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
        }

        //������ȯ
        if (Input.GetButtonDown("Horizontal"))
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;


        if(Mathf.Abs(rigid.velocity.x) < 0.5) // ��������
        {
            anim.SetBool("isRun", false); //�޸��� �ִϸ��̼� ����
        }
        else // ������
        {
            anim.SetBool("isRun", true);//�޸��� �ִϸ��̼� ����
        }


    }

    private void FixedUpdate() //�׸��� �̵��Ҷ� ����ϴ� ������Ʈ��
    {
        float move = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * move, ForceMode2D.Impulse);




        //������ �ӵ� ����
        if (rigid.velocity.x > MaxSpeed)//������ MaxSpeed
        {
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < MaxSpeed * (-1))//���� MaxSpeed
        {
            rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);
        }

        //���� Ȯ��
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

        //RaycastHit2D Ray�� ���� ������Ʈ
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1);

        if(rayHit.collider != null)
        {
            Debug.Log(rayHit.collider.name);
        }
    }

}
