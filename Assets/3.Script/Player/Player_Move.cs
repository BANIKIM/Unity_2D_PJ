using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Rigidbody2D rigid; //리지드 바디이동을 위해 선언
    SpriteRenderer sprite; //좌우 반전을 위해 선언
    Animator anim; // 애니메이션 변경을 위해 선언


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
        //점프
        if (Input.GetButtonDown("Jump")) // 속도 제어 멈춰 설때 밀리는 정도
        {
            //normalized단위를 구할 때 쓴다 +, -를 알 수 있다.
            rigid.AddForce(Vector2.up * jump_Power, ForceMode2D.Impulse);
            anim.SetBool("isJump", true); // 점프로 애니메이션 변경
        }


        if (Input.GetButtonUp("Horizontal")) // 속도 제어 멈춰 설때 밀리는 정도
        {
             //normalized단위를 구할 때 쓴다 +, -를 알 수 있다.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
        }

        //방향전환
        if (Input.GetButtonDown("Horizontal"))
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;


        if(Mathf.Abs(rigid.velocity.x) < 0.5) // 서있으면
        {
            anim.SetBool("isRun", false); //달리기 애니메이션 변경
        }
        else // 걸으면
        {
            anim.SetBool("isRun", true);//달리기 애니메이션 변경
        }


    }

    private void FixedUpdate() //그림을 이동할때 사용하는 업데이트문
    {
        float move = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * move, ForceMode2D.Impulse);




        //움직임 속도 제어
        if (rigid.velocity.x > MaxSpeed)//오른쪽 MaxSpeed
        {
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < MaxSpeed * (-1))//왼쪽 MaxSpeed
        {
            rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);
        }

        //점프 확인
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));

        //RaycastHit2D Ray에 닿은 오브젝트
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1);

        if(rayHit.collider != null)
        {
            Debug.Log(rayHit.collider.name);
        }
    }

}
