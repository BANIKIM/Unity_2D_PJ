using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Rigidbody2D rigid; //리지드 바디이동을 위해 선언
    SpriteRenderer sprite; //좌우 반전을 위해 선언
    Animator anim; // 애니메이션 변경을 위해 선언
    CapsuleCollider2D capsulcollider;//죽었을때 비활성화를 외해서
    AudioSource audioSource;// 오디오 소스 가져오기


    public GameManager gamemanager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;



    [SerializeField] private float MaxSpeed;
    [SerializeField] private float jump_Power;

    


    //private int Jump_Count = 0;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsulcollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();

    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }


    private void Update()
    {
        //점프
        if (Input.GetButtonDown("Jump")&& !anim.GetBool("isJump")) // 속도 제어 멈춰 설때 밀리는 정도 &&!anim.GetBool("isJump")점프 상태가 아닐 때
        {
            //normalized단위를 구할 때 쓴다 +, -를 알 수 있다.
            rigid.AddForce(Vector2.up * jump_Power, ForceMode2D.Impulse);
            anim.SetBool("isJump", true); // 점프로 애니메이션 변경

            PlaySound("JUMP");
        }


        if (Input.GetButtonUp("Horizontal")) // 속도 제어 멈춰 설때 밀리는 정도
        {
             //normalized단위를 구할 때 쓴다 +, -를 알 수 있다.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
        }

        //방향전환
        if (Input.GetButton("Horizontal"))//문워크를 할 때가 있음 GetButtondown -> GetButton로 수정
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
        if(rigid.velocity.y < 0) //플레이어가 떨어 질때만 
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            //RaycastHit2D Ray에 닿은 오브젝트
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            //GetMask("") 레이어 이름에 해당하는 정수값을 리턴하는 함수
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.8f) // distance - Ray에 닿았을 때의 거리
                    anim.SetBool("isJump", false);
            }
        }

       
    }
    //충돌 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //위에서 접촉했을 때는 데미지를 준다
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            {
                //아니라면 데미지를 입는다
                OnDamged(collision.transform.position);
            }
        }
    }

    //게임 오브젝트가 닿았을 때 작동하는 코드 - 동전 먹기, 결승선
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            //동전 점수 획득
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");
            PlaySound("ITEM");
            if (isBronze)
            {
                gamemanager.stagePoint += 50;
            }
            else if (isSilver)
            {
                gamemanager.stagePoint += 100;
            }
            else if (isGold)
            {
                gamemanager.stagePoint += 300;
            }
            
            //닿은 오브젝트 삭제
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("피니쉬");
            PlaySound("FINISH");
            gamemanager.NextStage();
        }
    }


    //공격함수
    void OnAttack(Transform enemy)
    {
        // 점수 획득
        gamemanager.stagePoint += 100;
        PlaySound("ATTACK");
        //적을 밟았을 때 뛰어 오르기
        rigid.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        // 적 죽음
        Enemy_Move enmey_move = enemy.GetComponent<Enemy_Move>();
        enmey_move.OnDamaged();
    }



    //무적시간
    void OnDamged(Vector2 targetPos)
    {
        PlaySound("DAMAGED");
        //체력에 피해를 입음
        gamemanager.HpDown();
        //레이어 변경
        gameObject.layer = 9; //레이어를 변경 9번으로 변경한다
        //피격 이펙트 
        sprite.color = new Color(1, 1, 1, 0.4f);
        //피격 밀림
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7,ForceMode2D.Impulse);

        //피격애니메이션
        anim.SetTrigger("doDamaged");


        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        sprite.color = new Color(1, 1, 1, 1);


    }

    //죽음
    public void OnDie()
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
    }


    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

}
