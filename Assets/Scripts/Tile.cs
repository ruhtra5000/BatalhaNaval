using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour {
    //Atributos padrões
    public bool temEmbarcacao { get; private set; }
    public bool foiAlvejado { get; private set; }

    //Atributos operacionais
    [SerializeField] Color corBase, corModificada, corErrado, corCerto;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject mouseEmCima;

    public void iniciar (bool mudarCor) {
        spriteRenderer.color = mudarCor ? corModificada : corBase;
        temEmbarcacao = false;
        foiAlvejado = false;
    }

    //Mostrar celulas onde o mouse está passando por
    void OnMouseEnter()
    {
        mouseEmCima.SetActive(true);
    }

    void OnMouseExit()
    {
        mouseEmCima.SetActive(false);
    }

    //Registrar clicks
    void OnMouseUp() {
        if(!foiAlvejado) {
            if (this.temEmbarcacao)
                spriteRenderer.color = corCerto;
            else {
                spriteRenderer.color = corErrado;
                FindAnyObjectByType<JogoAdmin>().setClickTrue();
            }
            foiAlvejado = true;
        }
    }

    public void marcarTileComBarco() {
        this.temEmbarcacao = true;
    }
}
