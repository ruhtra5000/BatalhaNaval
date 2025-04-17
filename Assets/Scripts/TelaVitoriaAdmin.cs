using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaVitoriaAdmin : MonoBehaviour {
    
    void Start(){
        GameObject.Find("Conteudo")
        .GetComponent<TMPro.TextMeshProUGUI>()
        .text = "Jogador " + JogoAdmin.vencedor + " venceu!";
    }

    public void BotaoVoltarMenu() {
        SceneManager.LoadSceneAsync("MenuPrincipal");
    }
}
