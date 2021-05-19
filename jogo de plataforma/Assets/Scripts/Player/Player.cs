using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isGround;
    private bool doubleJump;
    private bool isAttack;
    private bool isDead;
    private float recoveryCount;
    private PlayerVidaHud vida;
    private PlayerAudio playerAudio;

    private Rigidbody2D rig;
    
    [Header("Variáveis")]
    [SerializeField] private float velocidade;
    [SerializeField] private float forcaPulo;
    [SerializeField] private float radius;
    [SerializeField] private float timeStopRecovery;

    [Header("Componentes da Unity")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform pontoAttack;
    [SerializeField] private LayerMask EnemyLayer;

    private static Player playerInstance;
    private void Awake()
    {
        if(playerInstance == null)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);
        }else if(playerInstance != this){
            Destroy(playerInstance.gameObject);
            playerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        rig = GetComponent<Rigidbody2D>();
        playerAudio = GetComponent<PlayerAudio>();
        vida = GetComponent<PlayerVidaHud>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            jump();
            attack();
        }
    }



    private void FixedUpdate()
    {
        move();
    }

    public void move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * velocidade, rig.velocity.y);

        if(movement > 0)
        {
            if (isGround && !isAttack)
            {
                anim.SetInteger("transition", 2);
            }
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if(movement < 0)
        {
            if (isGround && !isAttack)
            {
                anim.SetInteger("transition", 2);
            }
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if(movement == 0 && isGround && !isAttack)
        { 
            anim.SetInteger("transition", 0);
        }
    }

    public void jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
                playerAudio.playerAudioEfeito(playerAudio.jumpSound);
                rig.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                anim.SetInteger("transition", 1);
                isGround = false;
                doubleJump = true;
            }else if (doubleJump)
            {
                playerAudio.playerAudioEfeito(playerAudio.jumpSound);
                rig.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                anim.SetInteger("transition", 1); 
                doubleJump = false;
            }
            
        }
    }

    public void attack()
    {

        if (Input.GetButtonDown("Fire1") && !isAttack)
        {
            playerAudio.playerAudioEfeito(playerAudio.hitSound);
            isAttack = true;
            Collider2D hit = Physics2D.OverlapCircle(pontoAttack.position, radius, EnemyLayer);
            anim.SetInteger("transition", 3);

            if (hit != null)
            {
                if (hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().onHit();
                }else if (hit.GetComponent<Goblin>())
                {
                    hit.GetComponent<Goblin>().onHit();
                }
            }
            StartCoroutine("onAttack");
        }
        
    }

    IEnumerator onAttack()
    {
        yield return new WaitForSeconds(0.333f);
        isAttack = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pontoAttack.position, radius); 
    }


    public void onHit()
    {
        recoveryCount += Time.deltaTime;

        if (recoveryCount >= timeStopRecovery)
        {
            vida.vida--;

            if (vida.vida <= 0)
            {
                anim.SetTrigger("death");
                isDead = true;
                velocidade = 0f;
                Destroy(gameObject, 0.667f);
                GameController.instance.gameOver();
            }
            else
            {
                anim.SetTrigger("hit");
            }
            recoveryCount = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
            Debug.Log(collision.gameObject.name);
        {
            isGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            onHit();
        }

        if(collision.gameObject.layer == 11)
        {
            PlayerPos.instance.checkPoint();
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            playerAudio.playerAudioEfeito(playerAudio.coinSound);
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("hit");
            Destroy(collision.gameObject, 0.500f);
            GameController.instance.getCoin();
        }
    }
}
