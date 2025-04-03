using UnityEngine;
using UnityEngine.SceneManagement;

public class BotoesMenuInicial : MonoBehaviour {
    //Variavel que armazena qual das opções foi escolhida
    public static string modo;

    //Realizam a mudança de tela
    public void BotaoJogar() {
        SceneManager.LoadSceneAsync("SelecaoModoJogo");
    }   

    public void BotaoBatalha1() {
        modo = "1J";
        SceneManager.LoadSceneAsync("BatalhaNaval");
    }

    public void BotaoBatalha2() {
        modo = "2J";
        SceneManager.LoadSceneAsync("BatalhaNaval");
    }
}
