using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class BarcosAdmin : MonoBehaviour {   
    //Armazena os barcos de cada jogador
    private Barco [] barcos1 = new Barco[15];
    private Barco [] barcos2 = new Barco[15];

    //Atributos operacionais
    [SerializeField] Barco embarcacao;
    [SerializeField] PainelBarcos painelBarcos;
    public SpriteRenderer spriteRenderer;
    private bool flag;

    //Sprites
    [SerializeField] Sprite submarino;
    [SerializeField] Sprite encouracado;
    [SerializeField] Sprite navioGuerra;
    [SerializeField] Sprite portaAvioes;
    
    //Iniciar o menu dos barcos de cada jogador
    public async Task initJogador1(int x, int y){
        flag = false;

        barcos1 = new Barco[5];
        barcos1[0] = instanciarSubmarino(x, y);
        barcos1[1] = instanciarEncouracado(x, y-1);
        barcos1[2] = instanciarEncouracado(x, y-2);
        barcos1[3] = instanciarNavioDeGuerra(x, y-3);
        barcos1[4] = instanciarPortaAvioes(x, y-4);

        criarMenuJogador1();

        while(!flag) {
            await Task.Yield();
        }
    }

    public async Task initJogador2(int x, int y){
        flag = false;

        barcos2 = new Barco[5];
        barcos2[0] = instanciarSubmarino(x, y);
        barcos2[1] = instanciarEncouracado(x, y-1);
        barcos2[2] = instanciarEncouracado(x, y-2);
        barcos2[3] = instanciarNavioDeGuerra(x, y-3);
        barcos2[4] = instanciarPortaAvioes(x, y-4);

        criarMenuJogador2();

        while(!flag) {
            await Task.Yield();
        }
    }

    //Criar embarcações
    public Barco instanciarSubmarino(int x, int y) {
        spriteRenderer.sprite = submarino;
        embarcacao.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        return Instantiate(embarcacao, new Vector3(x, y), Quaternion.identity);
    }

    public Barco instanciarEncouracado(int x, int y) {
        spriteRenderer.sprite = encouracado;
        embarcacao.GetComponent<BoxCollider2D>().size = new Vector2(3, 1);
        return Instantiate(embarcacao, new Vector3(x, y), Quaternion.identity);
    }

    public Barco instanciarNavioDeGuerra(int x, int y) {
        spriteRenderer.sprite = navioGuerra;
        embarcacao.GetComponent<BoxCollider2D>().size = new Vector2(4, 1);
        return Instantiate(embarcacao, new Vector3(x, y), Quaternion.identity);
    }

    public Barco instanciarPortaAvioes(int x, int y) {
        spriteRenderer.sprite = portaAvioes;
        embarcacao.GetComponent<BoxCollider2D>().size = new Vector2(6, 1);
        return Instantiate(embarcacao, new Vector3(x, y), Quaternion.identity);
    }

    //Instanciar menus
    public async void criarMenuJogador1() {
        PainelBarcos painel = Instantiate(painelBarcos, new Vector3(17, 6, 0), Quaternion.identity);
        await painel.init(16, 0); //Aguarda o painel ser fechado
        Destroy(painel.gameObject);

        esconderBarcos(1);

        flag = true;
    }

    private async void criarMenuJogador2() {
        PainelBarcos painel = Instantiate(painelBarcos, new Vector3(3, 6, 0), Quaternion.identity);
        await painel.init(3, 0); //Aguarda o painel ser fechado
        Destroy(painel.gameObject);

        esconderBarcos(2);

        flag = true;
    }

    public void setBarcosJogador2Bot(Barco[] barcos) {
        this.barcos2 = barcos;
    }
    
    //Esconder os barcos de cada jogador
    private void esconderBarcos(int jogador) {
        Barco [] barcos = null;
        if(jogador == 1) 
            barcos = this.barcos1;
        else if(jogador == 2) 
            barcos = this.barcos2;

        foreach (Barco barco in barcos){
            SpriteRenderer sr = barco.GetComponent<SpriteRenderer>();
            sr.enabled = false;
        }
    }

    //Remover a colisão dos barcos de cada jogador
    public void removerColisaoBarcos(int jogador) {
        Barco [] barcos = null;
        if(jogador == 1) 
            barcos = this.barcos1;
        else if(jogador == 2) 
            barcos = this.barcos2;

        foreach (Barco barco in barcos){
            BoxCollider2D collider = barco.GetComponent<BoxCollider2D>();
            collider.enabled = false;
        }
    }
}
