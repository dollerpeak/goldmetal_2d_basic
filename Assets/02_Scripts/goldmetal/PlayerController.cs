using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    string sceneName;
    //[Header("palyer maxSpeed")]
    float maxSpeed = 5.0f;
    float jumpForce = 11f;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // 이동 //////////////////////////////////////
        // - 좌우
        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody2D.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);
        if (rigidbody2D.linearVelocityX == 0)
        {
            animator.SetBool("ANI_RUN", false);
        }
        else
        {
            animator.SetBool("ANI_RUN", true);
        }
        //Debug.Log("ANI_RUN = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_RUN"));

        // - 정지
        if (Input.GetButtonUp("Horizontal") == true)
        {
            //Debug.Log("button up");
            rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocityY);
        }
        // - 점프
        if ((Input.GetKeyDown(KeyCode.UpArrow) == true || Input.GetKeyDown(KeyCode.Space) == true)
            && animator.GetBool("ANI_JUMP") == false)
        {
            //Debug.Log("arrow up");
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("ANI_JUMP", true);
            animator.SetBool("ANI_RUN", false);
        }
        //Debug.Log("ANI_JUMP = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_JUMP"));

        // 착지 체크
        // - raycast가 점프를 시작할때 벌써 floor를 인지하므로 y force 값이 없을때만 체크
        //Debug.Log("rigidbody2D.linearVelocityY = " + rigidbody2D.linearVelocityY);        
        if (rigidbody2D.linearVelocityY <= 0)
        {
            Debug.DrawRay(rigidbody2D.position, Vector3.down, Color.green);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(rigidbody2D.position, Vector2.down, 1, LayerMask.GetMask("Floor"));
            // layer=floor랑 충돌했을때
            if (raycastHit2D.collider != null)
            {
                //Debug.Log("raycastHit2D.distance = " + raycastHit2D.distance);
                //if (raycastHit2D.distance <= 0.5f)
                if (animator.GetBool("ANI_JUMP") == true)
                {
                    //Debug.Log("raycastHit2D = " + raycastHit2D.collider.name);
                    animator.SetBool("ANI_JUMP", false);
                    animator.SetBool("ANI_RUN", false);
                }
            }
        }
        //Debug.Log("ANI_RUN = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_RUN"));
        //Debug.Log("ANI_JUMP = " + animator.GetCurrentAnimatorStateInfo(0).IsName("ANI_JUMP"));

        // 속도제한
        if (rigidbody2D.linearVelocityX >= maxSpeed || rigidbody2D.linearVelocityX <= -maxSpeed)
        {
            if (rigidbody2D.linearVelocityX > 0)
            {
                // 오른쪽
                rigidbody2D.linearVelocity = new Vector2(maxSpeed, rigidbody2D.linearVelocityY);
            }
            else if (rigidbody2D.linearVelocityX < 0)
            {
                // 왼쪽
                rigidbody2D.linearVelocity = new Vector2(-maxSpeed, rigidbody2D.linearVelocityY);
            }
        }

        // 애니메이션 //////////////////////////////////////
        // 이동방향
        //if (rigidbody2D.linearVelocityX == 0)
        //{
        //    animator.SetBool("ANI_RUN", false);
        //}
        //else
        //{
        //    animator.SetBool("ANI_RUN", true);
        //}

        // 방향에 맞는 이미지 적용 //////////////////////////////////////
        if (rigidbody2D.linearVelocityX > 0)
        {
            // 오른쪽
            spriteRenderer.flipX = false;
        }
        else if (rigidbody2D.linearVelocityX < 0)
        {
            // 왼쪽
            spriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ReStart();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Block")
        {
            Debug.Log("player.rigid.x = " + rigidbody2D.linearVelocityX);
            if (rigidbody2D.linearVelocityY < 0 
                && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision);
            }
            else
            {
                OnDamage(collision);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int score;
        if(collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);

            if(collision.gameObject.name.Contains("bronze") == true)
            {
                score = 10;
            }
            else if (collision.gameObject.name.Contains("bronze") == true)
            {
                score = 20;
            }
            else
            {
                score = 30;
            }
            gameManager.stagePoint += score;
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // next stage
            //gameManager.NextStage("filed_1");
            Debug.Log("게임종료");
        }
    }

    void ReStart()
    {
        if(transform.position.y <= -10f)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void OnDamage(Collision2D collision)
    {
        // damage 상태
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 충격이동
        //Debug.Log("player.x = " + transform.position.x);
        //Debug.Log("target.x = " + nTargetPosition.x);
        //int dir = (nTargetPosition.x - transform.position.x) > 0 ? 1 : -1;
        //Debug.Log("player.rigid.x = " + rigidbody2D.linearVelocityX);
        int dir = rigidbody2D.linearVelocityX >= 0 ? -1 : 1;
        rigidbody2D.AddForce(new Vector2(dir, 1) * 4, ForceMode2D.Impulse);

        // anim
        animator.SetTrigger("ANI_DAMAGE");

        Invoke("OffDamage", 3);

        // 플레이어 처리
        gameManager.playerHp -= 1;
        if(gameManager.playerHp <= 0)
        {
            CancelInvoke("OffDamage");
            ReStart();
        }
    }

    void OffDamage()
    {
        // damage 해제 상태
        gameObject.layer = LayerMask.NameToLayer("Player");
        spriteRenderer.color = new Color(1, 1, 1);
    }

    void OnAttack(Collision2D collision)
    {
        // palyer 효과
        rigidbody2D.AddForce(Vector2.up * 4, ForceMode2D.Impulse);

        //enemy 처리
        EnemyController enemyController = collision.transform.GetComponent<EnemyController>();
        enemyController.OnDamage();

        gameManager.stagePoint += 50;
    }

}
