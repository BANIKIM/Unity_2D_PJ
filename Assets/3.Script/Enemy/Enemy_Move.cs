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
        //Invoke() - 일정 시간이 지난 뒤, 지정된 함수를 실행하는 함수
        Invoke("Think",5);//("메서드이름", x초 뒤 실행)
    }


    private void FixedUpdate()
    {
        //움직이는 코드
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);


        //지형체크 (낭떨어지인지 아닌지)
        Vector2 forntVec = new Vector2(rigid.position.x + nextMove*0.4f, rigid.position.y);
        Debug.DrawRay(forntVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(forntVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }

    }

    //재귀함수 - 자신을 스스로 호출하는 함수
    void Think()//왼쪽 오른쪽 어디로 갈지 결정하는 메서드
    {
        nextMove = Random.Range(-1, 2);

       

        anim.SetInteger("RunSpeed", nextMove);
        if(nextMove !=0)
        sprite.flipX = nextMove == 1;


        //재귀
        float nextThinkTime = Random.Range(2f, 5f); // 시간랜덤
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;//곱하여 방향을 바꾼다.
        sprite.flipX = nextMove == 1;
        CancelInvoke(); //CancelInvoke - 현재 작동 중인 모든 Invoke함수를 멈추는 함수
        Invoke("Think", 5);
    }

   public void OnDamaged()
    {
        //피격 모션
        sprite.color = new Color(1, 1, 1, 0.4f);
        //뒤로 뒤집는 모션
        sprite.flipY = true;
        //콜라이더 비활성화 - 충돌이나 모든 것을 무시 
        capsulcollider.enabled = false;
        //죽었을 때 위로 살짝 뜸
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //5초 뒤에 시체가 비활성화
        Invoke("DeActive", 5);

    }



    void DeActive()
    {
        gameObject.SetActive(false);
    }

}
