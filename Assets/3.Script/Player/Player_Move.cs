using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    Rigidbody2D rigid; //������ �ٵ��̵��� ���� ����
    SpriteRenderer sprite; //�¿� ������ ���� ����
    Animator anim; // �ִϸ��̼� ������ ���� ����
    CapsuleCollider2D capsulcollider;//�׾����� ��Ȱ��ȭ�� ���ؼ�
    AudioSource audioSource;// ����� �ҽ� ��������


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
        //����
        if (Input.GetButtonDown("Jump")&& !anim.GetBool("isJump")) // �ӵ� ���� ���� ���� �и��� ���� &&!anim.GetBool("isJump")���� ���°� �ƴ� ��
        {
            //normalized������ ���� �� ���� +, -�� �� �� �ִ�.
            rigid.AddForce(Vector2.up * jump_Power, ForceMode2D.Impulse);
            anim.SetBool("isJump", true); // ������ �ִϸ��̼� ����

            PlaySound("JUMP");
        }


        if (Input.GetButtonUp("Horizontal")) // �ӵ� ���� ���� ���� �и��� ����
        {
             //normalized������ ���� �� ���� +, -�� �� �� �ִ�.
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
        }

        //������ȯ
        if (Input.GetButton("Horizontal"))//����ũ�� �� ���� ���� GetButtondown -> GetButton�� ����
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
        if(rigid.velocity.y < 0) //�÷��̾ ���� ������ 
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            //RaycastHit2D Ray�� ���� ������Ʈ
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            //GetMask("") ���̾� �̸��� �ش��ϴ� �������� �����ϴ� �Լ�
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.8f) // distance - Ray�� ����� ���� �Ÿ�
                    anim.SetBool("isJump", false);
            }
        }

       
    }
    //�浹 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //������ �������� ���� �������� �ش�
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            {
                //�ƴ϶�� �������� �Դ´�
                OnDamged(collision.transform.position);
            }
        }
    }

    //���� ������Ʈ�� ����� �� �۵��ϴ� �ڵ� - ���� �Ա�, ��¼�
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("item"))
        {
            //���� ���� ȹ��
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
            
            //���� ������Ʈ ����
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("�ǴϽ�");
            PlaySound("FINISH");
            gamemanager.NextStage();
        }
    }


    //�����Լ�
    void OnAttack(Transform enemy)
    {
        // ���� ȹ��
        gamemanager.stagePoint += 100;
        PlaySound("ATTACK");
        //���� ����� �� �پ� ������
        rigid.AddForce(Vector2.up * 6, ForceMode2D.Impulse);
        // �� ����
        Enemy_Move enmey_move = enemy.GetComponent<Enemy_Move>();
        enmey_move.OnDamaged();
    }



    //�����ð�
    void OnDamged(Vector2 targetPos)
    {
        PlaySound("DAMAGED");
        //ü�¿� ���ظ� ����
        gamemanager.HpDown();
        //���̾� ����
        gameObject.layer = 9; //���̾ ���� 9������ �����Ѵ�
        //�ǰ� ����Ʈ 
        sprite.color = new Color(1, 1, 1, 0.4f);
        //�ǰ� �и�
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7,ForceMode2D.Impulse);

        //�ǰݾִϸ��̼�
        anim.SetTrigger("doDamaged");


        Invoke("OffDamaged", 1.5f);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        sprite.color = new Color(1, 1, 1, 1);


    }

    //����
    public void OnDie()
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
    }


    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

}
