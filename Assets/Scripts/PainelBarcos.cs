using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PainelBarcos : MonoBehaviour {
    //Atributos operacionais
    [SerializeField] BotaoConfirmar botaoConfirmar;

    public async Task init(int x, int y) {
        botaoConfirmar = Instantiate(botaoConfirmar, new Vector3(x, y), Quaternion.identity);
        while (botaoConfirmar.menuAberto) {
            await Task.Yield(); //Aguarando o botão de confirmação ser pressionado
        }

        Destroy(this.botaoConfirmar.gameObject);
    }

}
