using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButtom : MonoBehaviour
{
    private Animator anim;

    [Header("Componentes da Unity")]
    [SerializeField] private Animator barreiraAnim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    public void onPressed()
    {
        anim.SetBool("isPressed", true);
        barreiraAnim.SetBool("isPressed", true);
    }

    public void onExit()
    {
        anim.SetBool("isPressed", false);
        barreiraAnim.SetBool("isPressed", false);
    }

    // retorna enquanto um objeto está em colisão com outro
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            onPressed();
        }   
    }

    // retorna quando o objeto sai de colisão com o outro
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            onExit();
        }
    }
}
