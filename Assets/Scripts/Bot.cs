using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Bot : MonoBehaviour {
    [SerializeField] private BarcosAdmin barcosAdmin;
    [SerializeField] private int largura = 10;
    [SerializeField] private int altura = 10;
    private Queue<Vector2Int> alvosPendentes = new();
    private HashSet<Vector2Int> alvosJaAtacados = new();

    public async Task PosicionarBarcos() {
        List<(Vector2 tamanho, System.Func<int, int, Barco> instanciador)> tiposBarcos = new()
        {
            (new Vector2(1, 1), barcosAdmin.instanciarSubmarino),
            (new Vector2(3, 1), barcosAdmin.instanciarEncouracado),
            (new Vector2(3, 1), barcosAdmin.instanciarEncouracado),
            (new Vector2(4, 1), barcosAdmin.instanciarNavioDeGuerra),
            (new Vector2(6, 1), barcosAdmin.instanciarPortaAvioes)
        };

        List<Barco> barcosBot = new();

        foreach (var (tamanhoOriginal, instanciador) in tiposBarcos) {
            bool posicionado = false;
            Vector2 tamanho;
            Quaternion rotacao;

            while (!posicionado) {
                bool horizontal = Random.value > 0.5f;
                tamanho = horizontal ? tamanhoOriginal : new Vector2(tamanhoOriginal.y, tamanhoOriginal.x);
                rotacao = ObterRotacao(horizontal);

                int maxX = largura - (int)tamanho.x;
                int maxY = altura - (int)tamanho.y;

                int x = Random.Range(0, maxX + 1);
                int y = Random.Range(0, maxY + 1);

                Vector3 posicaoCentralizada = new Vector3(x + 11 + tamanho.x / 2f - 0.5f, y + tamanho.y / 2f - 0.5f, 0);
                Barco barcoTemp = instanciador(x + 11, y);
                barcoTemp.transform.position = posicaoCentralizada;
                barcoTemp.transform.rotation = rotacao;

                await Task.Yield();

                Bounds bounds = barcoTemp.GetComponent<Collider2D>().bounds;

                Collider2D[] colisores = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0);
                colisores = colisores.Where(c => c.gameObject != barcoTemp.gameObject).ToArray();

                if (colisores.Length > 0) {
                    GameObject.Destroy(barcoTemp.gameObject);
                    await Task.Yield();
                    continue;
                }

                float limiteEsquerdo = 11f;
                float limiteDireito = 11f + largura;
                float limiteInferior = 0f;
                float limiteSuperior = altura;

                bool dentroDosLimites =
                    bounds.min.x >= limiteEsquerdo &&
                    bounds.max.x <= limiteDireito &&
                    bounds.min.y >= limiteInferior &&
                    bounds.max.y <= limiteSuperior;

                if (!dentroDosLimites)
                {
                    GameObject.Destroy(barcoTemp.gameObject);
                    await Task.Yield();
                    continue;
                }

                barcosBot.Add(barcoTemp);
                posicionado = true;

                await Task.Yield();
            }
        }

        barcosAdmin.setBarcosJogador2Bot(barcosBot.ToArray());
    }

    private Quaternion ObterRotacao(bool horizontal) {
        return horizontal ? new Quaternion(0f, 0f, 0f, 1f) : new Quaternion(0f, 0f, 0.7071068f, 0.7071068f);
    }

    private void EsconderVisualDoBarco(Barco barco) {
        foreach (var renderer in barco.GetComponentsInChildren<Renderer>()) {
            renderer.enabled = false;
        }
    }

    public async Task Atacar(GradeAdmin gradeAdmin) {
        await Task.Delay(1000);

        Dictionary<Vector2, Tile> gradeJ1 = gradeAdmin.GetGrade(1);
        Vector2Int alvo;

        if (alvosPendentes.Count == 0)
        {
            do
            {
                int x = Random.Range(0, 10);
                int y = Random.Range(0, 10);
                alvo = new Vector2Int(x, y);
            }
            while (alvosJaAtacados.Contains(alvo) || gradeJ1[new Vector2(alvo.x, alvo.y)].foiAlvejado);
        }
        else
        {
            alvo = alvosPendentes.Dequeue();
            while ((alvosJaAtacados.Contains(alvo) || gradeJ1[new Vector2(alvo.x, alvo.y)].foiAlvejado) && alvosPendentes.Count > 0)
                alvo = alvosPendentes.Dequeue();
        }

        alvosJaAtacados.Add(alvo);
        Tile tile = gradeJ1[new Vector2(alvo.x, alvo.y)];

        if (tile.temEmbarcacao)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.green;
            tile.GetType().GetProperty("foiAlvejado").SetValue(tile, true, null);
            tile.tocarSomAcerto();

            List<Vector2Int> direcoesPossiveis = DirecoesAdjacentes();
            foreach (var direcao in direcoesPossiveis)
            {
                Vector2Int vizinho = alvo + direcao;
                if (vizinho.x >= 0 && vizinho.x < 10 && vizinho.y >= 0 && vizinho.y < 10)
                {
                    Tile tileVizinho = gradeJ1[new Vector2(vizinho.x, vizinho.y)];
                    if (tileVizinho.temEmbarcacao && !alvosJaAtacados.Contains(vizinho))
                    {
                        alvosPendentes.Enqueue(vizinho);

                        Vector2Int oposta = new Vector2Int(-direcao.x, -direcao.y);
                        Vector2Int vizinhoOposto = alvo + oposta;
                        if (vizinhoOposto.x >= 0 && vizinhoOposto.x < 10 && vizinhoOposto.y >= 0 && vizinhoOposto.y < 10)
                        {
                            Tile tileOposto = gradeJ1[new Vector2(vizinhoOposto.x, vizinhoOposto.y)];
                            if (tileOposto.temEmbarcacao && !alvosJaAtacados.Contains(vizinhoOposto))
                            {
                                alvosPendentes.Enqueue(vizinhoOposto);
                            }
                        }
                    }
                }
            }

            await Task.Delay(500);
            await Atacar(gradeAdmin);
        }
        else {
            tile.GetComponent<SpriteRenderer>().color = Color.red;
            tile.GetType().GetProperty("foiAlvejado").SetValue(tile, true, null);
            tile.tocarSomErro();
            await Task.Delay(500);
        }
    }

    private List<Vector2Int> DirecoesAdjacentes() {
        return new List<Vector2Int> {
            new Vector2Int(1, 0),   // Direita
            new Vector2Int(-1, 0),  // Esquerda
            new Vector2Int(0, 1),   // Cima
            new Vector2Int(0, -1)   // Baixo
        };
    }
}

