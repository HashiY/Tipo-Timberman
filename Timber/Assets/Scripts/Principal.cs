using UnityEngine;
using System.Collections;
using System.Collections.Generic; // para usar lista de gameobj
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Principal : MonoBehaviour {
	 
	public GameObject jogador;
	public GameObject felpudoIdle;
	public GameObject felpudoBate;

	public GameObject blocoEsq; //inimigo
	public GameObject blocoDir; //inimigo
	public GameObject blocoCentro; //barril
	public GameObject barra;

	public Text scoreText;
	public Text startText;
	public Text highScoreText;

	public GameObject objectCanvasScore;
	public GameObject objectCanvasHighscore;

	private int score, highscore;
	private string sceneName;

	public AudioClip somBate;
	public AudioClip somPerde;

	bool acabou;
    bool comecou; 
	bool ladoJogador; //lado
	
	private float escalaHorizontalJogador;

	private List<GameObject> listaBlocos;

	void Start () {
		felpudoBate.SetActive(false); // desaparece
		escalaHorizontalJogador = jogador.transform.localScale.x; //escala
		listaBlocos = new List<GameObject>();  //inicializar a lista
		CriaBlocosNaCena();

		scoreText.text = "0";
		objectCanvasScore.transform.position = new Vector2(Screen.width / 6 - 46, Screen.height - 50);
		objectCanvasHighscore.transform.position = new Vector2(Screen.width / 6 - 46, Screen.height - 100);

		//Colocando o highscore para ser salvo
		sceneName = SceneManager.GetActiveScene().name;
		if (PlayerPrefs.HasKey(sceneName + "score"))
		{
			highscore = PlayerPrefs.GetInt(sceneName + "score");
			highScoreText.text = highscore.ToString();
		}

		startText.enabled = true;
		startText.transform.position = new Vector2(Screen.width/2,Screen.height/2+200);
		startText.text = "Toque para iniciar!";
		startText.fontSize = 45;
	}

	void Update () {
		if(!acabou){
			if(Input.GetButtonDown("Fire1")){  //mause 

				startText.enabled = false;

				if (!comecou){
					comecou=true;
					barra.SendMessage("ComecouJogo"); // para usar a barra
				}

				if(Input.mousePosition.x > Screen.width/2) //clicou na direita
				{
					bateDireita(); 
				}else{ // clicou na esquerda
					bateEsquerda();
				}
				Invoke("VoltaAnimacao", 0.25f);

				felpudoBate.SetActive(true);  // ativa
				felpudoIdle.SetActive(false); //desativa

				listaBlocos.RemoveAt(0); //remove o q esta no 0
				ReposicionaBlocos(); 
				confereJogada();
			}
		}
	}

	void bateDireita(){ 
		ladoJogador = false;
		listaBlocos[0].SendMessage("TomaPancadaDireita"); //naposiçao 0, usa a funçao q vai remover
		jogador.transform.position = new Vector2(1.1f, jogador.transform.position.y);//reposiciona
        jogador.transform.localScale = new Vector2(-escalaHorizontalJogador,jogador.transform.localScale.y); //escala
    }

	void bateEsquerda(){ 
		ladoJogador = true;
		listaBlocos[0].SendMessage("TomaPancadaEsquerda"); 
		jogador.transform.position = new Vector2(-1.1f, jogador.transform.position.y);//reposiciona
		jogador.transform.localScale = new Vector2(escalaHorizontalJogador,jogador.transform.localScale.y);//escala
	}

	void VoltaAnimacao(){ 
		felpudoBate.SetActive(false); //desativa
		felpudoIdle.SetActive(true); //ativa
	}
 
	void CriaBlocosNaCena(){
		for(int i=0; i<=7; i++) // criar sete obstaculos na cena
		{
			//			CriaNovoBarril(new Vector2(0,-2.162f+(i*0.835f)));
			GameObject novoObj = CriaNovoBarril(new Vector2(0,-2.162f+(i*0.835f))); // local para criar os setes barrius
			listaBlocos.Add(novoObj); // coloca na lista
		} 
	}

	void ReposicionaBlocos(){
		GameObject novoBarril = CriaNovoBarril(new Vector2(0,-2.162f+(8*0.835f))); //cria novo na ultima posiçao
		listaBlocos.Add(novoBarril); 

		for(int i=0; i<=7; i++) // percorrer todo item para
		{   // para réposicionar 
			listaBlocos[i].transform.position = new Vector2(listaBlocos[i].transform.position.x,listaBlocos[i].transform.position.y-0.835f);
		} //propria posiçao do x e diminuir o valor da altura no y
	}

	GameObject CriaNovoBarril(Vector2 posicao){ // onde vai criar um novo barril
		GameObject novoBarril; 
		if((Random.value > 0.5f) || listaBlocos.Count<2 ){ // sortear o barril eo inimigo, se tiver ate 2 obj sao neutros
			novoBarril= Instantiate(blocoCentro); // barril normal
		}else{

			if(Random.value > 0.5f){ // ve denovo 
				novoBarril= Instantiate(blocoDir);// inimigo
			}else{
				novoBarril= Instantiate(blocoEsq); // inimigo
			}
		}  
		novoBarril.transform.position = posicao;  //posiciona no parametro
		return novoBarril; // retorna o gameobj criado
	} 

	void confereJogada(){ 
		if(listaBlocos[0].gameObject.CompareTag("Inimigo")){ // se a lista na posiçao 0 for inimigo

            //verifica o lado do inimigo e do jogador
			if((listaBlocos[0].name=="inimigoEsq(Clone)" && ladoJogador) || (listaBlocos[0].name=="inimigoDir(Clone)" && !ladoJogador))
			{ 
				FimDeJogo();

			}else{
				barra.SendMessage("AumentaBarra");
				MarcaPonto(); 
			} 
            //se colocar barra.SendMessage("AumentaBarra");  nesee else de baixo fica mais facil o jogo
		}else{MarcaPonto();}
	}
	void RecarregaCena(){
		Application.LoadLevel("Timber1");
	}


	void MarcaPonto(){ // nada de novo
		score++;
		scoreText.text = score.ToString();

		if (score > highscore)
		{
			highscore = score;
			highScoreText.text = highscore.ToString();
			PlayerPrefs.SetInt(sceneName + "score", highscore);
		}

		GetComponent<AudioSource>().PlayOneShot(somBate); //som
	}

	void FimDeJogo(){
		acabou = true; 

		felpudoBate.GetComponent<SpriteRenderer>().color = new Color(1f,0.25f,0.25f,1.0f); //vermelho
		felpudoIdle.GetComponent<SpriteRenderer>().color = new Color(1f,0.25f,0.25f,1.0f);//vermelho
        jogador.GetComponent<Rigidbody2D>().isKinematic = false; 
		jogador.GetComponent<Rigidbody2D>().AddTorque(100f);

		if(ladoJogador){
			jogador.GetComponent<Rigidbody2D>().velocity = new Vector2(-5,3);
		}else{jogador.GetComponent<Rigidbody2D>().velocity = new Vector2(5,3);}

        GetComponent<AudioSource>().PlayOneShot(somPerde); // som

        Invoke("RecarregaCena", 1);

	}
}
