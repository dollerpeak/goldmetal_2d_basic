using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int nextMove;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        RandomMove();
    }

    void FixedUpdate()
    {
        // 이동 //////////////////////////////////////
        // - 좌우
        rigidbody2D.linearVelocity = new Vector2(nextMove, rigidbody2D.linearVelocityY);
        if (rigidbody2D.linearVelocityX == 0)
        {
            animator.SetBool("ANI_RUN", false);
        }
        else
        {
            animator.SetBool("ANI_RUN", true);
        }

        Vector2 nextDir = new Vector2(rigidbody2D.position.x + (nextMove * 0.5f), rigidbody2D.position.y);
        Debug.DrawRay(nextDir, Vector3.down, Color.green);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(nextDir, Vector2.down, 1, LayerMask.GetMask("Floor"));
        if(raycastHit2D.collider == null)
        {
            //Debug.Log("가지 못하는 영역");
            InitRandomMove();
        }

        // 방향에 맞는 이미지 적용 //////////////////////////////////////
        if (rigidbody2D.linearVelocityX > 0)
        {
            // 오른쪽
            spriteRenderer.flipX = true;
        }
        else if (rigidbody2D.linearVelocityX < 0)
        {
            // 왼쪽
            spriteRenderer.flipX = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomMove()
    {
        nextMove = Random.Range(-1, 2);
        //Debug.Log("nextMove = " + nextMove);

        float invokeTimeSec = Random.Range(1.0f, 5.0f);
        //Debug.Log("invokeTimeSec = " + invokeTimeSec);

        Invoke("RandomMove", invokeTimeSec);
    }

    void InitRandomMove()
    {
        nextMove *= -1;
        CancelInvoke("RandomMove");

        float invokeTimeSec = Random.Range(1.0f, 5.0f);
        //Debug.Log("invokeTimeSec = " + invokeTimeSec);
        Invoke("RandomMove", invokeTimeSec);
    }

    public void OnDamage()
    {
        //투명
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //뒤집기
        spriteRenderer.flipY = true;
        //이후 충돌하지 않게
        capsuleCollider2D.enabled = false;
        //효과
        rigidbody2D.AddForce(Vector2.up * 4, ForceMode2D.Impulse);

        //사라지기
        Invoke("OffActive", 5);
    }

    void OffActive()
    {
        gameObject.SetActive(false);
    }
}
