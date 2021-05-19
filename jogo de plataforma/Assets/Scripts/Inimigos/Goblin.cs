using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    [Header("Variáveis")]
    [SerializeField] private float velocidade;
    [SerializeField] private float visaoMaxima;
    [SerializeField] private bool isRight;
    [SerializeField] private float stopDistance;
    [SerializeField] private int health;

    [Header("Unity Componentes")]
    [SerializeField] private Transform point;
    [SerializeField] private Transform behind;

    private Rigidbody2D rig;
    private Animator anim;
    private bool isFront;
    private Vector2 direction;
    private bool isDead;


    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mudarPosicaoGoblin();   
    }

    private void FixedUpdate()
    {
        vendoPLayer();
    }

    public void mudarPosicaoGoblin()
    {
        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
            move(this.velocidade);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
            move(-this.velocidade);
        }
    }

    


    public void move(float velocidade)
    {
        if (isFront && !isDead)
        {
            anim.SetInteger("transition", 1);
            rig.velocity = new Vector2(velocidade, rig.velocity.y);
        }
    }
    public void vendoPLayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, visaoMaxima);
        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, visaoMaxima);

        if (hit.collider != null && !isDead)
        {
            if (hit.transform.CompareTag("Player")){
                isFront = true;
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance <= stopDistance)
                {
                    isFront = false;
                    rig.velocity = Vector2.zero;

                    anim.SetInteger("transition", 2);
                    hit.transform.GetComponent<Player>().onHit();
                }
            }
        }
        
        if(behindHit.collider != null)
        {
            if (behindHit.transform.CompareTag("Player"))
            {
                isRight = !isRight;
                isFront = true;
            }
        }
    }


    public void onHit()
    {
        health--;

        if (health <= 0)
        {
            velocidade = 0;
            isDead = true;
            anim.SetTrigger("death");
            Destroy(gameObject, 0.500f);
        }
        else
        {
            anim.SetTrigger("hit");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(point.position, direction * visaoMaxima);
        Gizmos.DrawRay(behind.position, -direction * visaoMaxima);
    }
}
