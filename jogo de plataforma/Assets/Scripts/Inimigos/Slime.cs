using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;
    private bool isDead;

    [Header("Variáveis")]
    [SerializeField] private int vida; 
    [SerializeField] private float velocidade;
    [SerializeField] private float radius;

    [Header("Componentes da Unity")]
    [SerializeField] private Transform point;
    [SerializeField] private LayerMask layer;

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
        
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            rig.velocity = new Vector2(velocidade, rig.velocity.y);
            colisaoParede();
        }
    }

    void colisaoParede()
    {
        Collider2D colisao = Physics2D.OverlapCircle(point.position, radius, layer);

        if(colisao != null)
        {
            Debug.Log("Encostou na parrede");
            velocidade = -velocidade;

            if(transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector2(0, 180);
            }else if(transform.eulerAngles.y == 180)
            {
                transform.eulerAngles = new Vector2(0, 0);
            }

            
        }
    }

    public void onHit()
    {
        vida--;

        if(vida <= 0)
        {
            anim.SetTrigger("death");
            isDead = true;
            velocidade = 0;
            Destroy(gameObject, 0.500f);
        }
        else
        {
            anim.SetTrigger("hit");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }
}
 