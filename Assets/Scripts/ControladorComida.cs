using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class ControladorComida : MonoBehaviour
{
    
    [SerializeField] private Collider2D areaJuego;              // Referencia al Collider2D que define el área de juego
    private ControladorSerpiente serpiente;                     // Referencia al script Controlador Serpiente

    public delegate void ComidaGeneradaEventHandler();              // Delega el evento de generación de nueva comida
    public event ComidaGeneradaEventHandler EventoComidaGenerada;   // Evento cuando se genera una nueva comida

    

    private void Awake()
    {
        // Instancia a serpiente como una variable del tipo ControladorSerpiente
        serpiente = FindObjectOfType<ControladorSerpiente>();

    }

    private void Start()
    {
        // Inicia las variables
        CrearComida();
        

    }

    // Gestiona pa losición de la comida
    public void CrearComida()
    {
        
        // Define las dimensiones del área de juego en una caja denominada 
        Bounds limites = areaJuego.bounds;

        // Elige una posición aleatoria dentro de los límites y redondea los valores para asegurar que se alineen con la cuadrícula
        int x = Mathf.RoundToInt(Random.Range(limites.min.x, limites.max.x));
        int y = Mathf.RoundToInt(Random.Range(limites.min.y, limites.max.y));

        // Mientras el espacio calculado para la comida este ocupado (true) porque no está la serpiente o porque no hay obstáculos => busca un nuevo lugar
        while (serpiente.EspacioOcupadoSerpiente(x, y) && HayObstaculoEnCoordenada(x, y))
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

        // Establece la posición de la comida
        transform.position = new Vector2(x, y);


        // Activa el evento NuevaComidaGenerada
        ComidaGenerada();

    }

    // Gestiona la colisión de la comida con otro objeto
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Llama al método para generar y posicionar la comida dentro del áreaJuego
        CrearComida();
    }

    // Gestiona la activación del evento NuevaComidaGenerada
    private void ComidaGenerada()
    {
        // Activa el evento
        EventoComidaGenerada?.Invoke();
    }

    // Devuelve si la posición está ocupada
    public bool EspacioOcupadoComida(int x, int y)
    {
        // Si la posición del cuerpo en x = a la posición de movimiento Y la posición del cuerpo en y = a la posición de movimiento => true
        if (Mathf.RoundToInt(transform.position.x) == x && Mathf.RoundToInt(transform.position.y) == y)
        {
            return true;
        }
       

        // Si el espacio no está ocupado por la serpiente => flase
        return false;
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
