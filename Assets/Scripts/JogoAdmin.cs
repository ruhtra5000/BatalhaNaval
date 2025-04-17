using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JogoAdmin : MonoBehaviour {
    //Atributos operacionais
    [SerializeField] private GradeAdmin gradeAdmin;
    [SerializeField] private BarcosAdmin barcosAdmin;
    [SerializeField] private Bot bot;
    private EstadoJogo estadoJogo;
    public static int vencedor;
    private bool click = false;

    void Start() {
        mudarEstado(EstadoJogo.GERAR_GRADE);
    }

    public void mudarEstado(EstadoJogo estado) {
        this.estadoJogo = estado;

        switch (estadoJogo) {
            case EstadoJogo.GERAR_GRADE: {
                iniciarGrade();
                break;
            }
            case EstadoJogo.POSICIONAR_NAVIOS_J1: {
                posicionarEmbarcacoesJ1();
                break;
            }
            case EstadoJogo.POSICIONAR_NAVIOS_J2: {
                if (BotoesMenuInicial.modo == "1J")
                    posicionarEmbarcacoesJ2Bot();
                else
                    posicionarEmbarcacoesJ2();
                break;
            }
            case EstadoJogo.TURNO_J1: {
                turnoJ1();
                break;
            }
            case EstadoJogo.TURNO_J2: {
                turnoJ2();
                break;
            }
            case EstadoJogo.FIM_DE_JOGO: {
                fimDeJogo();
                break;
            }
        }
    }

    private void iniciarGrade() {
        this.gradeAdmin.init();

        mudarEstado(EstadoJogo.POSICIONAR_NAVIOS_J1);
    }

    private async void posicionarEmbarcacoesJ1() {
        this.gradeAdmin.modificarGrade(1, false);
        this.gradeAdmin.modificarGrade(2, false);

        await this.barcosAdmin.initJogador1(16, 7);

        this.gradeAdmin.adicionarBarcos(1);

        this.barcosAdmin.removerColisaoBarcos(1);

        mudarEstado(EstadoJogo.POSICIONAR_NAVIOS_J2);
    }

    private async void posicionarEmbarcacoesJ2() {
        await this.barcosAdmin.initJogador2(3, 7);

        this.gradeAdmin.adicionarBarcos(2);

        this.barcosAdmin.removerColisaoBarcos(2);

        mudarEstado(EstadoJogo.TURNO_J1);
    }

    private async void posicionarEmbarcacoesJ2Bot() {
        await bot.PosicionarBarcos();

        this.gradeAdmin.adicionarBarcos(2);

        this.barcosAdmin.removerColisaoBarcos(2);

        mudarEstado(EstadoJogo.TURNO_J1);
    }

    private async void turnoJ1() {
        this.click = false;
        this.gradeAdmin.modificarGrade(1, false);
        this.gradeAdmin.modificarGrade(2, true);

        //Esperar pelo clique do jogador 1 no campo do jogador 2
        await celulaClicada();

        if (this.gradeAdmin.checarVitoria(1)) {
            vencedor = 1;
            mudarEstado(EstadoJogo.FIM_DE_JOGO);
        }

        mudarEstado(EstadoJogo.TURNO_J2);
    }

    private void turnoJ2() {
        if (BotoesMenuInicial.modo == "1J") 
            turnoJ2Maquina();
        
        else if (BotoesMenuInicial.modo == "2J") 
            turnoJ2Humano();
    }

    //Turno 2 operado por um humano
    private async void turnoJ2Humano() {
        this.click = false;
        this.gradeAdmin.modificarGrade(1, true);
        this.gradeAdmin.modificarGrade(2, false);

        //Esperar pelo clique do jogador 2 no campo do jogador 1
        await celulaClicada();

        if (this.gradeAdmin.checarVitoria(2)) {
            vencedor = 2;
            mudarEstado(EstadoJogo.FIM_DE_JOGO);
        }

        mudarEstado(EstadoJogo.TURNO_J1);
    }

    //Turno 2 operado pela máquina
    private async void turnoJ2Maquina() {
        this.gradeAdmin.modificarGrade(1, false);
        this.gradeAdmin.modificarGrade(2, false);

        if(BotoesMenuInicial.dificuldade == "fácil"){
            await bot.AtacarFacil(this.gradeAdmin);
        }
        else if(BotoesMenuInicial.dificuldade == "difícil"){
            await bot.Atacar(this.gradeAdmin);
        }

        if (this.gradeAdmin.checarVitoria(2)) {
            vencedor = 2;
            mudarEstado(EstadoJogo.FIM_DE_JOGO);
        }
        else {
            mudarEstado(EstadoJogo.TURNO_J1);
        }
    }

    private void fimDeJogo() {
        SceneManager.LoadSceneAsync("TelaVitoria");
    }

    //Métodos auxiliares para capturar o clique do jogador em alguma celula
    private async Task celulaClicada() {
        while (!click) {
            await Task.Yield();
        }
    }

    public void setClickTrue() {
        this.click = true;
    }
}

