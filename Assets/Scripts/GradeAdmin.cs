using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GradeAdmin : MonoBehaviour {
    //Atributos padrões
    private Dictionary<Vector2, Tile> celulas1;
    private Dictionary<Vector2, Tile> celulas2;

    //Atributos operacionais
    [SerializeField] private int largura, altura;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform camera;

    public void init() {
        celulas1 = new Dictionary<Vector2, Tile>();
        celulas2 = new Dictionary<Vector2, Tile>();

        gerarGrade(0, ref celulas1);   
        gerarGrade(11, ref celulas2);   
        reposicionarCamera();
    }

    public void gerarGrade(int deslocamento, ref Dictionary<Vector2, Tile> celulas) {
        for (int i = 0; i < largura; i++) {
            for (int j = 0; j < altura; j++) {
                Tile tile = Instantiate(tilePrefab, new Vector3(i + deslocamento, j), Quaternion.identity);
                tile.name = $"{deslocamento}({i}, {j})";

                bool mudarCor = ((i % 2 == 0 && j %2 != 0) || (i % 2 != 0 && j %2 == 0));

                tile.iniciar(mudarCor);
                
                celulas.Add(new Vector2(i, j), tile);
            }
        }
    }

    public void reposicionarCamera() {
        camera.transform.SetPositionAndRotation(new Vector3((float) largura, (float) altura/2 - 0.5f, -12), Quaternion.identity);
    }

    //jogador -> indica qual grade será modificada
    //estadoGrade = true -> click habilitado
    //estadoGrade = false -> click desabilitado
    public void modificarGrade(int jogador, bool estadoGrade) {
        Dictionary<Vector2, Tile> celulas = null;
        if(jogador == 1)
            celulas = this.celulas1;
        else if (jogador == 2)
            celulas = this.celulas2;

        Tile tile;
        Vector2 pos = new Vector2();

        for (int i = 0; i < largura; i++) {
            for (int j = 0; j < altura; j++){
                pos.Set(i, j);
                celulas.TryGetValue(pos, out tile);
                tile.GetComponent<BoxCollider2D>().enabled = estadoGrade;
            }
        }
    }

    //Marca as celulas que contém embarcações
    public void adicionarBarcos(int jogador) {
        Dictionary<Vector2, Tile> celulas = null;
        if(jogador == 1)
            celulas = this.celulas1;
        else if (jogador == 2)
            celulas = this.celulas2;

        Tile tile;
        Vector2 pos = new Vector2();
        BoxCollider2D collider;
        
        for (int i = 0; i < largura; i++) {
            for (int j = 0; j < altura; j++){
                pos.Set(i, j);
                celulas.TryGetValue(pos, out tile);
                collider = tile.GetComponent<BoxCollider2D>();
                
                Collider2D[] colliders = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max);

                if(colliders.Any()) {
                    tile.marcarTileComBarco();
                }
            }
        }
    }

    //Checa a vitória do jogador 1 ou 2
    public bool checarVitoria(int jogador) {
        Dictionary<Vector2, Tile> celulas = null;
        if(jogador == 1)
            celulas = this.celulas2;
        else if (jogador == 2)
            celulas = this.celulas1;
        
        Tile tile;
        Vector2 pos = new Vector2();

        for (int i = 0; i < largura; i++) {
            for (int j = 0; j < altura; j++){
                pos.Set(i, j);
                celulas.TryGetValue(pos, out tile);

                //Caso haja alguma celula com embarcação, que ainda não foi alvejada
                if(tile.temEmbarcacao && !tile.foiAlvejado)
                    return false;
            }
        }
        return true;
    }
}
