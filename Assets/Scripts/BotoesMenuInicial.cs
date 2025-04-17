using UnityEngine;
using UnityEngine.SceneManagement;

public class BotoesMenuInicial : MonoBehaviour {
    //Variavel que armazena qual das opções foi escolhida
    public static string modo;

    //Armazena a dificuldade do bot
    public static string dificuldade;

    //Realizam a mudança de tela
    public void BotaoJogar() {
        SceneManager.LoadSceneAsync("SelecaoModoJogo");
    }   

    public void BotaoBatalha1() {
        modo = "1J";
        SceneManager.LoadSceneAsync("SelecaoDificuldade");
    }

    public void BotaoBatalha2() {
        modo = "2J";
        SceneManager.LoadSceneAsync("BatalhaNaval");
    }

    public void BotaoFacil() {
        dificuldade = "facil";
        SceneManager.LoadSceneAsync("BatalhaNaval");
    }

    public void BotaoDificil() {
        dificuldade = "dificil";
        SceneManager.LoadSceneAsync("BatalhaNaval");
    }
}
