using UnityEngine;
using System.Collections;

public class Barra : MonoBehaviour {


	float escalaBarra;
	bool terminou;
	bool comecou;
	public GameObject cameraCena;// para usar o outro script

    public AudioClip somAcaba;

	void Start () { 
		escalaBarra = this.transform.localScale.x; //recebe a escala em x da barra
	}

	void Update () {
		if(comecou){ // começa
			if(escalaBarra> 0.05f){ // do diminui se for maior que
				escalaBarra = (escalaBarra - 0.15f*Time.deltaTime); //diminuir
				this.transform.localScale = new Vector2(escalaBarra,1); // muda a scala
			}else{
				if(!terminou){
					terminou = true;
					cameraCena.SendMessage("FimDeJogo"); // para usar o outro script
					GetComponent<AudioSource>().PlayOneShot(somAcaba);
				}  
			}
		} 
	}

	void AumentaBarra () { 
		escalaBarra = escalaBarra+0.035f;
		if (escalaBarra>1.0f) {escalaBarra = 1.0f;} // para nao passar de 100
	}

	void ComecouJogo () { 
		comecou = true;
	}


}
