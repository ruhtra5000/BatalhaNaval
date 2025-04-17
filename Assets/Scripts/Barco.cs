using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Barco : MonoBehaviour {
    //Atributos operacionais
    private Vector3 diferenca = Vector3.zero;
    [SerializeField] UnityEngine.Camera CameraPrincipal;
    private Quaternion rotacao = new Quaternion(0, 0, 0, 0); //Armazena a rotação atual do barco

    public void OnMouseDown(){
        CameraPrincipal = UnityEngine.Camera.main;
        diferenca = CameraPrincipal.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    void OnMouseDrag() {
        transform.position = CameraPrincipal.ScreenToWorldPoint(Input.mousePosition) - diferenca;
        //Caso o botão direito seja clicado, enquanto 
        //movendo um barco, uma rotação em 90° será aplicada
        if(Input.GetMouseButtonDown(1)) {
            if(rotacao.z == 0) {
                rotacao.z = 0.7071068f;
                rotacao.w = 0.7071068f;
            }
            else {
                rotacao.z = 0f;
                rotacao.w = 0f;
            }
            
            transform.rotation = rotacao;
        }
    }

    public void esconderVisualizacao() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    public void removerColisao() {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
    }
}
