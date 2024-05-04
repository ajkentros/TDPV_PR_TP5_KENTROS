using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPuntoTransicion : MonoBehaviour
{

    [SerializeField] private Collider2D areaJuego;          // Referencia al Collider2D que define el �rea de juego
    [SerializeField] private GameObject puntoTransicion;
    [SerializeField] private float tiempoPuntoTransicion; // Tiempo para que el punto de transici�n se active
    
    
    private ControladorSerpiente serpiente;                 // Referencia al script ControladorSerpiente
    private ControladorComida comida;                       // Referencia al script ControladorComida
        
    private float tiempoParaActivar;            // Tiempo para que el punto de transici�n se active
    private float tiempoTransicion;             // Tiempo que dura la transici�n antes de desactivarse
    private float tiempoSiguienteTransicion;    // Tiempo de espera para la siguiente transici�n
    private float tiempoEscena;                 // Tiempo transcurrido desde que comenz� la escena
    
    private bool puntoTransicionActivo = false; // Indica si el punto de transici�n est� activo


    private void Start()
    {
        // Inicializar el punto de transici�n desactivado
        puntoTransicion.SetActive(false);

        // Instancia a serpiente como una variable del tipo ControladorSerpiente
        serpiente = FindObjectOfType<ControladorSerpiente>();

        // Instancia a serpiente como una variable del tipo ControladorComida
        comida = FindObjectOfType<ControladorComida>();

        GeneraTiempos();
    }

    private void Update()
    {
        // Actualizar el tiempo transcurrido desde que comenz� la escena
        tiempoEscena += Time.deltaTime;

        // Verificar si el punto de transici�n debe activarse
        if (!puntoTransicionActivo && tiempoEscena >= tiempoParaActivar && tiempoEscena <= tiempoParaActivar + tiempoTransicion)
        {
            //Debug.Log("tiempoEscena = " + tiempoEscena + " tiempoParaActivar = " + tiempoParaActivar);

            // Posiciona el punto de transici�n dentro del �rea de juego
            PosicionPuntoTransicion();

            // Activa el punto de transici�n
            puntoTransicion.SetActive(true);
            puntoTransicionActivo = true;

            // Programar la desactivaci�n del punto de transici�n despu�s de tiempoTransicion segundos
            Invoke(nameof(DesactivarPuntoTransicion), tiempoTransicion);
        }

        // Verifica si se debe activar un nuevo punto de transici�n
        if (!puntoTransicionActivo && tiempoEscena >= tiempoSiguienteTransicion + tiempoParaActivar + tiempoTransicion)
        {
            //Debug.Log("tiempoEscena = " + tiempoEscena + " re inicio ");
            // Reinicia el tiempo de la escena
            tiempoEscena = 0f;
            GeneraTiempos();
        }

    }

    // GEstiona la desactivaci�n del punto de transici�n
    private void DesactivarPuntoTransicion()
    {
        // Desactivar el punto de transici�n
        puntoTransicion.SetActive(false);
        puntoTransicionActivo = false;
    }
    
    // Gestiona la colisi�n con la serpiente con el Punto de Transici�n
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si colisiona con la serpinete => Desactiva el punto de transici�n si colisiona con la serpiente
        if (other.CompareTag("Serpiente"))
        {
            DesactivarPuntoTransicion();
        }
    }

    // Gestiona la posici�n del punto de transicion
    public void PosicionPuntoTransicion()
    {
        // Define las dimensiones del �rea de juego en una caja denominada 
        Bounds limites = areaJuego.bounds;

        // Elige una posici�n aleatoria dentro de los l�mites y redondea los valores para asegurar que se alineen con la cuadr�cula
        int x = Mathf.RoundToInt(Random.Range(limites.min.x, limites.max.x));
        int y = Mathf.RoundToInt(Random.Range(limites.min.y, limites.max.y));

        // Mientras el espacio calculado para la comida este ocupado (true) porque no est� la serpiente y la comida y los obst�culos => busca un nuevo lugar
        while (serpiente.EspacioOcupadoSerpiente(x, y) && comida.EspacioOcupadoComida(x, y) && HayObstaculoEnCoordenada(x, y))
        {
            x++;

            // Si la posici�n x de la comida este dentro de los l�mites m�ximos del �rea de juego => la posici�n x = l�mite m�nimo del �rea de juego
            if (x > limites.max.x)
            {
                x = Mathf.RoundToInt(limites.min.x);

                y++;

                // Si la posici�n y de la comida este dentro de los l�mites m�ximos del �rea de juego => la posici�n y = l�mite m�nimo del �rea de juego
                if (y > limites.max.y)
                {
                    y = Mathf.RoundToInt(limites.min.y);
                }
            }
        }

        // Posiciona la comida en la nueva direcci�n x, y
        transform.position = new Vector2(x, y);
    }

    // Gestiona la generaci�n de tiempos para activar el PT
    private void GeneraTiempos()
    {
        // Define el tiempo para activar el PT en funci�n del tiempo definido en la dimensi�n
        tiempoParaActivar = Random.Range(tiempoPuntoTransicion * 0.5f, tiempoPuntoTransicion * 1f);

        // Define el tiempo que est� activo el PT en funci�n del tiempo definido en la dimensi�n
        tiempoTransicion = Random.Range(tiempoPuntoTransicion / 2 * 0.8f, tiempoPuntoTransicion / 2f * 1f);

        // Define el tiempo que se espera para el siguiente PT en funci�n del tiempo definido en la dimensi�n
        tiempoSiguienteTransicion = Random.Range(tiempoPuntoTransicion * 0.5f, tiempoPuntoTransicion * 1f);

        //Debug.Log(" tiempoParaActivar = " + tiempoParaActivar + " tiempoTransicion = " + tiempoTransicion + " tiempoSiguienteTransicion = "+ tiempoSiguienteTransicion);
    }

    // Gestiona si hay obst�culos en la coordenadas donde se genera el PT
    public bool HayObstaculoEnCoordenada(int x, int y)
    {
        // Obtiene todos los colliders en la posici�n x, y
        Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(x, y));

        // Sobre los colliders encontrados
        foreach (Collider2D collider in colliders)
        {
            // Verifica si el collider corresponde a un obst�culo
            if (collider.CompareTag("Obstaculo"))
            {
                // Si hay un obst�culo en la posici�n x, y => verdadero
                return true;
            }
        }

        // Si no se encontraron obst�culos en la posici�n x, y => falso
        return false;
    }


}
