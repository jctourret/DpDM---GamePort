using UnityEngine;
using System.Collections.Generic;

public class GameManager1 : MonoBehaviour
{
	//public static Player[] Jugadoers;
	
	public static GameManager1 Instancia;
	
	public float TiempoDeJuego = 60;
	
	public enum EstadoJuego{Calibrando, Jugando, Finalizado}
	public EstadoJuego EstAct = EstadoJuego.Calibrando;
	
	public PlayerInfo PlayerInfo1 = null;
	
	public Player Player1;
	
	//mueve los esqueletos para usar siempre los mismos
	public Transform Esqueleto1;
	//public Vector3[] PosEsqsCalib;
	public Vector3[] PosEsqsCarrera;
	
	bool PosSeteada = false;
	
	bool ConteoRedresivo = true;
	public Rect ConteoPosEsc;
	public float ConteoParaInicion = 3;
	public GUISkin GS_ConteoInicio;
	
	public Rect TiempoGUI = new Rect();
	public GUISkin GS_TiempoGUI;
	Rect R = new Rect();
	
	public float TiempEspMuestraPts = 3;
	
	//posiciones de los camiones dependientes del lado que les toco en la pantalla
	//la pos 0 es para la izquierda y la 1 para la derecha
	public Vector3[]PosCamionesCarrera = new Vector3[2];
	//posiciones de los camiones para el tutorial
	public Vector3 PosCamion1Tuto = Vector3.zero;
	
	//listas de GO que activa y desactiva por sub-escena
	//escena de calibracion
	public GameObject[] ObjsCalibracion1;
	//escena de tutorial
	public GameObject[] ObjsTuto1;
	//la pista de carreras
	public GameObject[] ObjsCarrera;
	//de las descargas se encarga el controlador de descargas
	
	//para saber que el los ultimos 5 o 10 segs se cambie de tamaño la font del tiempo
	//bool SeteadoNuevaFontSize = false;
	//int TamOrigFont = 75;
	//int TamNuevoFont = 75;
	
	/*
	//para el testing
	public float DistanciaRecorrida = 0;
	public float TiempoTranscurrido = 0;
	*/
	
	IList<int> users;
	
	//--------------------------------------------------------//
	
	void Awake()
	{
		GameManager1.Instancia = this;
	}
	
	void Start()
	{
		IniciarCalibracion();
		
		//para testing
		//PosCamionesCarrera[0].x+=100;
		//PosCamionesCarrera[1].x+=100;
	}
	
	void Update()
	{
		//REINICIAR
		if(Input.GetKey(KeyCode.Mouse1) &&
		   Input.GetKey(KeyCode.Keypad0))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		
		//CIERRA LA APLICACION
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		
		switch (EstAct)
		{
		case EstadoJuego.Calibrando:
			
			//SKIP EL TUTORIAL
			if(Input.GetKey(KeyCode.Mouse0) &&
			   Input.GetKey(KeyCode.Keypad0))
			{
				if(PlayerInfo1 != null)
				{
					FinCalibracion(0);
					
					FinTutorial(0);
				}
			}

                if (PlayerInfo1.PJ == null) {
                    PlayerInfo1 = new PlayerInfo(0, Player1);
                    PlayerInfo1.LadoAct = Visualizacion.Lado.Izq;
                    SetPosicion(PlayerInfo1);
                }

			//cuando los 2 pj terminaron los tutoriales empiesa la carrera
			if(PlayerInfo1.PJ != null)
			{
				if(PlayerInfo1.FinTuto2)
				{
					EmpezarCarrera();
				}
			}
			
			break;
			
			
		case EstadoJuego.Jugando:
			
			//SKIP LA CARRERA
			if(Input.GetKey(KeyCode.Mouse1) && 
			   Input.GetKey(KeyCode.Keypad0))
			{
				TiempoDeJuego = 0;
			}
			
			if(TiempoDeJuego <= 0)
			{
				FinalizarCarrera();
			}
			
			/*
			//para testing
			TiempoTranscurrido += T.GetDT();
			DistanciaRecorrida += (Player1.transform.position - PosCamionesCarrera[0]).magnitude;
			*/
			
			if(ConteoRedresivo)
			{
				//se asegura de que los vehiculos se queden inmobiles
				//Player1.rigidbody.velocity = Vector3.zero;
				//Player2.rigidbody.velocity = Vector3.zero;
				
				ConteoParaInicion -= T.GetDT();
				if(ConteoParaInicion < 0)
				{
					EmpezarCarrera();
					ConteoRedresivo = false;
				}
			}
			else
			{
				//baja el tiempo del juego
				TiempoDeJuego -= T.GetDT();
				if(TiempoDeJuego <= 0)
				{
					//termina el juego
				}
				/*
				//otro tamaño
				if(!SeteadoNuevaFontSize && TiempoDeJuego <= 5)
				{
					SeteadoNuevaFontSize = true;
					GS_TiempoGUI.box.fontSize = TamNuevoFont;
					GS_TiempoGUI.box.normal.textColor = Color.red;
				}
				*/
			}
			
			break;
			
			
		case EstadoJuego.Finalizado:
			
			//nada de trakeo con kinect, solo se muestra el puntaje
			//tambien se puede hacer alguna animacion, es el tiempo previo a la muestra de pts
			
			TiempEspMuestraPts -= Time.deltaTime;
			if(TiempEspMuestraPts <= 0)
				Application.LoadLevel(Application.loadedLevel +1);				
			
			break;		
		}
	}
	
	void OnGUI()
	{
		switch (EstAct)
		{
		case EstadoJuego.Jugando:
			if(ConteoRedresivo)
			{
				GUI.skin = GS_ConteoInicio;
				
				R.x = ConteoPosEsc.x * Screen.width/100;
				R.y = ConteoPosEsc.y * Screen.height/100;
				R.width = ConteoPosEsc.width * Screen.width/100;
				R.height = ConteoPosEsc.height * Screen.height/100;
				
				if(ConteoParaInicion > 1)
				{
					GUI.Box(R, ConteoParaInicion.ToString("0"));
				}
				else
				{
					GUI.Box(R, "GO");
				}
			}
			
			GUI.skin = GS_TiempoGUI;
			R.x = TiempoGUI.x * Screen.width/100;
			R.y = TiempoGUI.y * Screen.height/100;
			R.width = TiempoGUI.width * Screen.width/100;
			R.height = TiempoGUI.height * Screen.height/100;
			GUI.Box(R,TiempoDeJuego.ToString("00"));
			break;
		}
		
		GUI.skin = null;
	}
	
	//----------------------------------------------------------//
	
	public void IniciarCalibracion()
	{
		for(int i = 0; i < ObjsCalibracion1.Length; i++)
		{
			ObjsCalibracion1[i].SetActiveRecursively(true);
		}
		
		for(int i = 0; i < ObjsTuto1.Length; i++)
		{
			ObjsTuto1[i].SetActiveRecursively(false);
		}
		
		for(int i = 0; i < ObjsCarrera.Length; i++)
		{
			ObjsCarrera[i].SetActiveRecursively(false);
		}	
		Player1.CambiarACalibracion();
	}
		
	/*
	public void CambiarADescarga(Player pj)
	{
		//en la escena de la pista, activa la camara y las demas propiedades 
		//de la escena de descarga
	}
	
	public void CambiarAPista(Player pj)//de descarga ala pista de vuelta
	{
		//lo mismo pero al revez
	}
	*/	
	
	void CambiarATutorial()
	{
		PlayerInfo1.FinCalibrado = true;
			
		for(int i = 0; i < ObjsTuto1.Length; i++)
		{
			ObjsTuto1[i].SetActiveRecursively(true);
		}
		
		for(int i = 0; i < ObjsCalibracion1.Length; i++)
		{
			ObjsCalibracion1[i].SetActiveRecursively(false);
		}
		Player1.GetComponent<Frenado>().Frenar();
		Player1.CambiarATutorial();
		Player1.gameObject.transform.position = PosCamion1Tuto;//posiciona el camion
		Player1.transform.forward = Vector3.forward;			
	}
	
	void EmpezarCarrera()
	{
		Player1.GetComponent<Frenado>().RestaurarVel();
		Player1.GetComponent<ControlDireccion>().Habilitado = true;
	}
	
	void FinalizarCarrera()
	{		
		EstAct = GameManager1.EstadoJuego.Finalizado;
		
		TiempoDeJuego = 0;
		
		
			//lado que gano
			DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
			
			//puntajes
			DatosPartida.PtsGanador = Player1.Dinero;
		
			//lado que gano
		
		
		Player1.GetComponent<Frenado>().Frenar();	
		Player1.ContrDesc.FinDelJuego();
	}
	
	//se encarga de posicionar la camara derecha para el jugador que esta a la derecha y viseversa
	void SetPosicion(PlayerInfo pjInf)
	{	
		pjInf.PJ.GetComponent<Visualizacion>().SetLado(pjInf.LadoAct);
		//en este momento, solo la primera vez, deberia setear la otra camara asi no se superponen
		pjInf.PJ.ContrCalib.IniciarTesteo();
		PosSeteada = true;
		Player1.GetComponent<Visualizacion>().SetLado(Visualizacion.Lado.Der);
	}
	
	void CambiarACarrera()
	{
		//Debug.Log("CambiarACarrera()");
		
		Esqueleto1.transform.position = PosEsqsCarrera[0];
		
		for(int i = 0; i < ObjsCarrera.Length; i++)
		{
			ObjsCarrera[i].SetActiveRecursively(true);
		}
		
		/*
		for(int i = 0; i < ObjsTuto1.Length; i++)
		{
			ObjsTuto1[i].SetActiveRecursively(false);
			ObjsTuto2[i].SetActiveRecursively(false);
		}
		*/
		
		
		//desactivacion de la calibracion
		PlayerInfo1.FinCalibrado = true;
			
		for(int i = 0; i < ObjsTuto1.Length; i++)
		{
			ObjsTuto1[i].SetActiveRecursively(true);
		}
		
		for(int i = 0; i < ObjsCalibracion1.Length; i++)
		{
			ObjsCalibracion1[i].SetActiveRecursively(false);
		}
		
		//posiciona los camiones dependiendo de que lado de la pantalla esten
		if(PlayerInfo1.LadoAct == Visualizacion.Lado.Izq)
		{
			Player1.gameObject.transform.position = PosCamionesCarrera[0];
		}
		else
		{
			Player1.gameObject.transform.position = PosCamionesCarrera[1];
		}
		
		Player1.transform.forward = Vector3 .forward;
		Player1.GetComponent<Frenado>().Frenar();
		Player1.CambiarAConduccion();
		
		//los deja andando
		Player1.GetComponent<Frenado>().RestaurarVel();
		//cancela la direccion
		Player1.GetComponent<ControlDireccion>().Habilitado = false;
		//les de direccion
		Player1.transform.forward = Vector3.forward;
		
		EstAct = GameManager1.EstadoJuego.Jugando;
	}
	
	public void FinTutorial(int playerID)
	{
		if(playerID == 0)
		{
			PlayerInfo1.FinTuto2 = true;
			
		}
		if(PlayerInfo1.FinTuto2)
		{
			CambiarACarrera();
		}
	}
	
	public void FinCalibracion(int playerID)
	{
		if(playerID == 0)
		{
			PlayerInfo1.FinTuto1 = true;
			
		}
		
		if(PlayerInfo1.PJ != null)
			if(PlayerInfo1.FinTuto1)
				CambiarACarrera();//CambiarATutorial();
	}
	
	
	
	
	[System.Serializable]
	public class PlayerInfo
	{
		public PlayerInfo(int tipoDeInput, Player pj)
		{
            TipoDeInput = tipoDeInput;
			PJ = pj;
		}
		
		public bool FinCalibrado = false;
		public bool FinTuto1 = false;
		public bool FinTuto2 = false;
		
		public Visualizacion.Lado LadoAct;

        public int TipoDeInput = -1;
		
		public Player PJ;
	}
	
}
