using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVidaHud : MonoBehaviour
{
    public int vida;
    [SerializeField] private int qtdCoracao;

    [SerializeField] Image[] countCoracao;

    [SerializeField] Sprite coracaoVivo;
    [SerializeField] Sprite coracaoMorto;

    private void Start()
    {
        vida = qtdCoracao;
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < countCoracao.Length; i++)
        {

            if(i < vida)
            {
                countCoracao[i].sprite = coracaoVivo;
            }
            else
            {
                countCoracao[i].sprite = coracaoMorto;
            }

            if(i < qtdCoracao)
            {
                countCoracao[i].enabled = true;
            }
            else
            {
                countCoracao[i].enabled = false;
            }
        }
    }
}
