using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;     // Referencia instancia estática del GameManager para acceder desde otros scripts

    private int puntos;         // Referencia variable puntos alcanzados
    private int comida;         // Referencia variable comida alcanzada
    private int cuerpo;         // Referencia variable cuerpo alcanzado
    private int dimension;      // Contador de dimensiones jugadas (escena)

    private float tiempoJuego = 0f;        // Referencia al tiempo actual de juego

    private bool ganaste;       // Referencia variable controla el estado del jeugo (ganaste)
    private bool gameOver;      // Referencia variable controla el gameOver del juego (si termina o no)
    private bool juegoPausado;  // Referencia variable controla si el juego está pausado o no


    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject); // Esto evitará que el objeto GameManager se destruya al cargar una nueva escena.
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        IniciaStart();
    }
    private void Update()
    {
        // Si el jeugo no termino y no está pausado => 
        if (!gameOver && !juegoPausado)
        {
            // el tiempo esté en marcha al inicio 
            Time.timeScale = 1f;

            // Actualiza el tiempo de juego si el juego no ha terminado y no está pausado
            tiempoJuego += Time.deltaTime;
            
            // Verifica el estado del jeugo
            VerificaEstadoJuego();
        }

    }

    // Gestiona el inicio de las variables
    private void IniciaStart()
    {
        tiempoJuego = 0;
        puntos = 0;
        comida = 0;
        cuerpo = 0;
        dimension = 0;
        ganaste = false;
        gameOver = false;
        juegoPausado = false;
    }
    
    // Gestiona el estado de los puntos y la escena actual
    private void VerificaEstadoJuego()
    {
        // Verifica los puntos actuales
        VerificaPuntos();

        // Verifica la escena actual
        VerificaEscenaActual();
    }

    // Verifica estado de puntos
    public void VerificaPuntos()
    {
        // Obtiene los puntos
        puntos = ObtienePuntos();

        // Si puntos < 0 => termina el juego (perdiste)
        if (puntos < 0)
        {
            // Cambia banderas
            ganaste = false;
            gameOver = true;
            
            // Cambia a la escena del menú final
            dimension = SceneManager.sceneCountInBuildSettings - 1;
            SceneManager.LoadScene(dimension);

            // Pausa el juego al final del juego
            PausarJuego(); 
        }
    }

    // Verifica escena actual
    private void VerificaEscenaActual()
    {
        // Obtiene el valor dela escena actual
        dimension = SceneManager.GetActiveScene().buildIndex; ;

        // Si la escena = a la última => el jeugo terminó (ganaste)
        if (dimension == SceneManager.sceneCountInBuildSettings - 1)
        {
            // Cambia banderas
            ganaste = true;
            gameOver = true;

            // Pausa el juego al final del juego
            PausarJuego(); 

        }
       
    }

    // Método para pausar el juego
    public void PausarJuego()
    {
        // Detiene el tiempo
        Time.timeScale = 0f;
        
        // Pausa el juego
        juegoPausado = true; 
    }

    // Gestiona el reinicio del jeugo
    public void ReiniciaJuego()
    {
        IniciaStart();
    }

    // Gestiona el inicio del menú incial
    public void IniciaMenu()
    {
        // Cargar la primera escena del juego (escena 0)
        SceneManager.LoadScene(0);
    }

    // Devuelve el tiempo de juego actual
    public float ObtieneTiempoDeJuego()
    {
        return tiempoJuego;
    }

    // Vevuelve el puntaje actual
    public int ObtienePuntos()
    {
        return puntos;
    }

    // Agregar puntos al puntaje total
    public void AgregaPuntos(int _puntos)
    {
        puntos += _puntos;

    }

    // Quitar puntos al puntaje total
    public void QuitaPuntos(int _puntos)
    {
        puntos -= _puntos;

    }

    // Incrementar el contador de dimensiones jugadas
    public void IncrementaEscenaActual()
    {

        dimension = SceneManager.GetActiveScene().buildIndex;

        dimension= (dimension + 1) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(dimension);

  

    }

    // Obtiene el número de dimensiones jugadas
    public int ObtieneEscenaActual()
    {
        return dimension;
    }

    // Obtiene el valor de la bandera ganaste
    public bool ObtieneEstadoGanaste()
    {
        return ganaste;
    }

    // Gestiona el cambio de la bandera ganaste
    public void CambiaEstadoGanaste(bool _ganaste)
    {
        ganaste = _ganaste;
    }

    // Obtiene el valor de la bandrea gameOver
    public bool ObtieneEstadoGameOver()
    {
        return gameOver;
    }

    // Gestiona el cambio de la bandera gameOver
    public void CambiaEstadoGameOver(bool _gameOver)
    {
        gameOver = _gameOver;
    }
    
    // Obtiene la comida consumida
    public int ObtieneComidaConsumida()
    {
        return comida;
    }

    // Gestiona el agregado de la comida
    public void AgregaComida(int _comida)
    {
        comida += _comida;
    }

    // Obtiene la cantidad de cuerpos de la serpiente
    public int ObtieneCuerpo()
    {
        return cuerpo;
    }

    // Obtiene la cantidad de cuerpos que tiene la serpiente
    public void EstadoCuerpoSerpiente(int _cuerpo)
    {
        cuerpo= _cuerpo;
    }

}
