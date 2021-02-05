using UnityEngine;
using System.Collections;

public class Bloco : MonoBehaviour {

	public int direcaoBloco;

	void Start(){
	}  
	void TomaPancadaDireita(){
		GetComponent<Rigidbody2D>().velocity = new Vector2(-10,2); // recebe velocidade a esquerda
		GetComponent<Rigidbody2D>().isKinematic = false; // desliga
		GetComponent<Rigidbody2D>().AddTorque(100f); // gira
		Invoke("ApagaBloco", 2f);
	}

	void TomaPancadaEsquerda(){
		GetComponent<Rigidbody2D>().velocity = new Vector2(10,2);// recebe velocidade a direita
        GetComponent<Rigidbody2D>().AddTorque(-100f);// gira
        GetComponent<Rigidbody2D>().isKinematic = false;// desliga
        Invoke("ApagaBloco", 2f);
	}
	void ApagaBloco(){ 
		Destroy(this.gameObject); //Apaga o barril
	}

}
