using UnityEngine;

public class BotaoConfirmar : MonoBehaviour {
    //Atributos padrões
    public bool menuAberto = true;

    void OnMouseUp() {
        menuAberto = false;
    }
}
