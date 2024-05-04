using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPuntoTransicion : MonoBehaviour
{

    [SerializeField] private Collider2D areaJuego;          // Referencia al Collider2D que define el área de juego
    [SerializeField] private GameObject puntoTransicion;
    [SerializeField] private float tiempoPuntoTransicion; // Tiempo para que el punto de transición se active
    
    
    private ControladorSerpiente serpiente;                 // Referencia al script ControladorSerpiente
    private ControladorComida comida;                       // Referencia al script ControladorComida
        
    private float tiempoParaActivar;            // Tiempo para que el punto de transición se active
    private float tiempoTransicion;             // Tiempo que dura la transición antes de desactivarse
    private float tiempoSiguienteTransicion;    // Tiempo de espera para la siguiente transición
    private float tiempoEscena;                 // Tiempo transcurrido desde que comenzó la escena
    
    private bool puntoTransicionActivo = false; // Indica si el punto de transición está activo


    private void Start()
    {
        // Inicializar el punto de transición desactivado
        puntoTransicion.SetActive(false);

        // Instancia a serpiente como una variable del tipo ControladorSerpiente
        serpiente = FindObjectOfType<ControladorSerpiente>();

        // Instancia a serpiente como una variable del tipo ControladorComida
        comida = FindObjectOfType<ControladorComida>();

        GeneraTiempos();
    }

    private void Update()
    {
        // Actualizar el tiempo transcurrido desde que comenzó la escena
        tiempoEscena += Time.deltaTime;

        // Verificar si el punto de transición debe activarse
        if (!puntoTransicionActivo && tiempoEscena >= tiempoParaActivar && tiempoEscena <= tiempoParaActivar + tiempoTransicion)
        {
            //Debug.Log("tiempoEscena = " + tiempoEscena + " tiempoParaActivar = " + tiempoParaActivar);

            // Posiciona el punto de transición dentro del área de juego
            PosicionPuntoTransicion();

            // Activa el punto de transición
            puntoTransicion.SetActive(true);
            puntoTransicionActivo = true;

            // Programar la desactivación del punto de transición después de tiempoTransicion segundos
            Invoke(nameof(DesactivarPuntoTransicion), tiempoTransicion);
        }

        // Verifica si se debe activar un nuevo punto de transición
        if (!puntoTransicionActivo && tiempoEscena >= tiempoSiguienteTransicion + tiempoParaActivar + tiempoTransicion)
        {
            //Debug.Log("tiempoEscena = " + tiempoEscena + " re inicio ");
            // Reinicia el tiempo de la escena
            tiempoEscena = 0f;
            GeneraTiempos();
        }

    }

    // GEstiona la desactivación del punto de transición
    private void DesactivarPuntoTransicion()
    {
        // Desactivar el punto de transición
        puntoTransicion.SetActive(false);
        puntoTransicionActivo = false;
    }
    
    // Gestiona la colisión con la serpiente con el Punto de Transición
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si colisiona con la serpinete => Desactiva el punto de transición si colisiona con la serpiente
        if (other.CompareTag("Serpiente"))
        {
            DesactivarPuntoTransicion();
        }
    }

    // Gestiona la posición del punto de transicion
    public void PosicionPuntoTransicion()
    {
        // Define las dimensiones del área de juego en una caja denominada 
        Bounds limites = areaJuego.bounds;

        // Elige una posición aleatoria dentro de los límites y redondea los valores para asegurar que se alineen con la cuadrícula
        int x = Mathf.RoundToInt(Random.Range(limites.min.x, limites.max.x));
        int y = Mathf.RoundToInt(Random.Range(limites.min.y, limites.max.y));

        // Mientras el espacio calculado para la comida este ocupado (true) porque no está la serpiente y la comida y los obstáculos => busca un nuevo lugar
        while (serpiente.EspacioOcupadoSerpiente(x, y) && comida.EspacioOcupadoComida(x, y) && HayObstaculoEnCoordenada(x, y))
        {
            x++;

            // Si la posición x de la comida este dentro de los límites máximos del área de juego => la posición x = límite mínimo del área de juego
            if (x > limites.max.x)
            {
                x = Mathf.RoundToInt(limites.min.x);

                y++;

                // Si la posición y de la comida este dentro de los límites máximos del área de juego => la posición y = límite mínimo del área de juego
                if (y > limites.max.y)
                {
                    y = Mathf.RoundToInt(limites.min.y);
                }
            }
        }

        // Posiciona la comida en la nueva dirección x, y
        transform.position = new Vector2(x, y);
    }

    // Gestiona la generación de tiempos para activar el PT
    private void GeneraTiempos()
    {
        // Define el tiempo para activar el PT en función del tiempo definido en la dimensión
        tiempoParaActivar = Random.Range(tiempoPuntoTransicion * 0.5f, tiempoPuntoTransicion * 1f);

        // Define el tiempo que está activo el PT en función del tiempo definido en la dimensión
        tiempoTransicion = Random.Range(tiempoPuntoTransicion / 2 * 0.8f, tiempoPuntoTransicion / 2f * 1f);

        // Define el tiempo que se espera para el siguiente PT en función del tiempo definido en la dimensión
        tiempoSiguienteTransicion = Random.Range(tiempoPuntoTransicion * 0.5f, tiempoPuntoTransicion * 1f);

        //Debug.Log(" tiempoParaActivar = " + tiempoParaActivar + " tiempoTransicion = " + tiempoTransicion + " tiempoSiguienteTransicion = "+ tiempoSiguienteTransicion);
    }

    // Gestiona si hay obstáculos en la coordenadas donde se genera el PT
    public bool HayObstaculoEnCoordenada(int x, int y)
    {
        // Obtiene todos los colliders en la posición x, y
        Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(x, y));

        // Sobre los colliders encontrados
        foreach (Collider2D collider in colliders)
        {
            // Verifica si el collider corresponde a un obstáculo
            if (collider.CompareTag("Obstaculo"))
            {
                // Si hay un obstáculo en la posición x, y => verdadero
                return true;
            }
        }

        // Si no se encontraron obstáculos en la posición x, y => falso
        return false;
    }


}
