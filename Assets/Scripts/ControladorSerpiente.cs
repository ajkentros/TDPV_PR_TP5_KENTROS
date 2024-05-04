using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Asegura que el componente BoxCollider2D esté presente en el GameObject
[RequireComponent(typeof(BoxCollider2D))]

public class ControladorSerpiente : MonoBehaviour
{
    [SerializeField] private Collider2D areaJuego;                              // Referencia al Collider2D que define el área de juego
    [SerializeField] private Transform cuerpoSerpiente;                         // Referencia al Transform del cuerpo de la serpiente
    [SerializeField] private float velocidadSerpiente = 20f;                    // Referencia a la velocidad de la serpiente
    [SerializeField] private float multiplicadorVelocidadSerpiente = 1f;        // Referencia al multiplicador de la velocidad de la serpiente
    [SerializeField] private int cantidadCuerposIniciales = 2;                  // Referencia a la cantidad de cuerpos que tiene la serpiente al inciar el juego
    [SerializeField] private bool atraviesaParedes = false;                     // Referencia a la posibilidad de atravesar las paredes (true = si puede atravesar)


    private Vector2Int direccion;                // Referencia a la dirección del movimiento de la serpiente
    private Vector2Int entradaTeclado;          // Referencia a la dirección de entrada del teclado
   
    private float actualizacionSerpiente;       // referencia a la etapa que actualiza a la serpiente

    private readonly List<Transform> cuerpos = new();    // referencia a la lista que contiene los cuerpos que tiene la serpiente


    private void Start()
    {
        // Inicia las variables
        IniciaStart();
        
    }

    private void Update()
    {
        // Si la direeción de la serpiente en X es disntinta de 0 => gira hacia arriba o hacia abajo mientras se mueve en el eje x
        if (direccion.x != 0f)
        {
            // Si clic en W o Arriba => vector arriba
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            {
                entradaTeclado = Vector2Int.up;

            } else
            {
                // Si clic en S o Abajo => vector abajo
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    entradaTeclado = Vector2Int.down;
                }
            }
            
        }
        
        else
        // Si la direeción de la serpiente en X es disntinta de 0 => gira hacia la izquierda o hacia la derecha mientras se mueve en el eje y
        if (direccion.y != 0f)
        {
            // Si clic en D o Derecha => vector derecha
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                entradaTeclado = Vector2Int.right;

            }
            else
            {
                // Si clic en A o Izquierda => vector izquierda
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    entradaTeclado = Vector2Int.left;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Si el tiempo < que la actualización de la serpiente => retorna sin hacer nada
        if (Time.time < actualizacionSerpiente) 
        {
            return;
        }

        // Si la entrada del teclado es distinta al vector (0,0) => la direccion de la serpiente = a la entrada del teclado
        if (entradaTeclado != Vector2Int.zero) {
            
            direccion = entradaTeclado;
        }

        // Establece la posición de cada segmento para que sea la misma que la siguiente. 
        // recorre la lista em orden inverso para que la posición se establezca en la anterior posición
        for (int i = cuerpos.Count - 1; i > 0; i--) 
        {
            // Define la posición del cuerpo i en la posición del cuerpo i-1
            cuerpos[i].position = cuerpos[i - 1].position;
        }

        // Mueve la serpiente en la dirección hacia la que mira y redondea los valores para asegurar que se alineen con la cuadrícula
        int x = Mathf.RoundToInt(transform.position.x) + direccion.x;
        int y = Mathf.RoundToInt(transform.position.y) + direccion.y;
        
        // Establece la posición de la serpiente en las coordenadas calculadas antes
        transform.position = new Vector2(x, y);

        // Defiene el tiempo de actualización de la serpiente en función de la velocidad y el multiplicador de la velocidad
        actualizacionSerpiente = Time.time + (1f / (velocidadSerpiente * multiplicadorVelocidadSerpiente));
    }

    // Gestion el crecimiento de la serpiente
    public void CreceSerpiente()
    {
        // Defiine una variable del tipo Transform: serpiente para instanciar el prefab cuerpoSerpiente)
        Transform segment = Instantiate(cuerpoSerpiente);

        // Pisiciona el nuevo cuerpo de la serpiente en el último elemento en la secuencia
        segment.position = cuerpos[^1].position;

        // Adiciona a la lista el nuevo cuerpo de la serpiente
        cuerpos.Add(segment);


    }

    // Gestiona el inicio de las variables
    public void IniciaStart()
    {
        // Define las dimensiones del área de juego en una caja denominada 
        Bounds limites = areaJuego.bounds;

        // Mientras esté dentro del área de juego y no haya obstáculos => genera una serpiente
        int x, y;
        do
        {
            // Elige una posición aleatoria dentro de los límites y redondea los valores para asegurar que se alineen con la cuadrícula
            x = Mathf.RoundToInt(Random.Range(limites.min.x, limites.max.x));
            y = Mathf.RoundToInt(Random.Range(limites.min.y, limites.max.y));

        } while (HayObstaculoEnCoordenada(x, y));

        // Define la posición inicial de la serpiente en (0,0)
        transform.position = new(x, y, 0);


        // Define la dirección de inicio del movimiento de la serpiente de manera aleatoria
        int direccionInicial = Random.Range(0, 4);         // 0 = derecha, 1= izquierda, 2 = arriba, 3 = abajo

        switch (direccionInicial)
        {
            case 0: direccion = Vector2Int.right; break;
            case 1: direccion = Vector2Int.left; break;
            case 2: direccion = Vector2Int.up; break;
            case 3: direccion = Vector2Int.down; break;
        }



        // Empieza en 1 para no destruir la cabeza
        for (int i = 1; i < cuerpos.Count; i++) {
            Destroy(cuerpos[i].gameObject);
        }

        // Borra la lista cuerpos pero agrega la cabeza
        cuerpos.Clear();
        cuerpos.Add(transform);

        // Recorre la lista desde -1 (la cabeza ocupa la última posición y está en la lista)
        for (int i = 0; i < cantidadCuerposIniciales - 1; i++) 
        {
            // Lama el método CreceSerpiente
            CreceSerpiente();
            
        }

        GameManager.gameManager.EstadoCuerpoSerpiente(LongitudSerpiente());
    }

    // Devuelve si la posición está ocupada
    public bool EspacioOcupadoSerpiente(int x, int y)
    {
        // Recorre toda la lista con los cuerpos de la serpiente
        foreach (Transform segment in cuerpos)
        {
            // Si la posición del cuerpo en x = a la posición de movimiento Y la posición del cuerpo en y = a la posición de movimiento => true
            if (Mathf.RoundToInt(segment.position.x) == x && Mathf.RoundToInt(segment.position.y) == y) 
            {
                return true;
            }
        }

        // Si el espacio no está ocupado por la serpiente => flase
        return false;
    }

    //
    public bool HayObstaculoEnCoordenada(int x, int y)
    {
        // Obtener todos los colliders en la posición x, y
        Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(x, y));
        // Si colisiona con Obstáculo => IniciaStart
        if (!direccion.Equals(Vector2Int.zero))
        {
            // Iterar sobre los colliders encontrados
            foreach (Collider2D collider in colliders)
            {
                // Verificar si el collider corresponde a un obstáculo
                if (collider.CompareTag("Obstaculo"))
                {
                    // Si hay un obstáculo en la posición x, y, devolver verdadero
                    return true;
                }
            }
        }
        // Si no se encontraron obstáculos en la posición x, y, devolver falso
        return false;
    }

    // Gestiona la colisión de la serpiente
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si colisiona con la Comida => la serpiente crece
        if (other.gameObject.CompareTag("Comida"))
        {
            
            // Llama al método para crecer la serpiente
            CreceSerpiente();

            // Suma puntos en GameManager
            int puntos = GameManager.gameManager.ObtieneEscenaActual();
            GameManager.gameManager.AgregaPuntos(puntos);

            // Actualiza el contador de comida consumiada por la serpiente
            GameManager.gameManager.AgregaComida(1);

        }
        else
        // Si colisiona con Obstáculo => InciaStart
        if (other.gameObject.CompareTag("Obstaculo")) // Solo restar puntos si la serpiente ya ha comenzado a moverse
        {

            
            // Suma puntos en GameManager
            int puntos = 10;
            GameManager.gameManager.QuitaPuntos(puntos);

            // Lama al método para iniciar el juego
            IniciaStart();
            
        }
        else
        // Si colisiona con Pared => InciaStart
        if (other.gameObject.CompareTag("Pared"))
        {
            // Si atraviesaParedes = true (está habilitad atravesar paredes => verifica que objeto atravesar
            // Sino atraviesaParedes = false Inicia Start
            if (atraviesaParedes) 
            {
                // Llama al método para atravesar la pared
                Atravesar(other.transform);

            } else 
            {
                // Llama al método para iniciar el juego
                IniciaStart();
            }
        }
        else
        // Si colisiona con la serpiente (cabeza) y el punto de transición está activo
        if (other.gameObject.CompareTag("PuntoDeTransicion"))
        {
            //Debug.Log("Colisión de la serpiente con el punto de transición");

            // Desactiva el Rigidbody de la serpiente para evitar que se mueva
            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.simulated = false;
            }
            
            // Suma puntos en GameManager
            int puntos = GameManager.gameManager.ObtieneEscenaActual() * 10;
            GameManager.gameManager.AgregaPuntos(puntos);

            // Cambia la dimensión activa en el GameManager
            GameManager.gameManager.IncrementaEscenaActual();

            
        }

        GameManager.gameManager.EstadoCuerpoSerpiente(cuerpos.Count);
    }

    // Gestiona atravesar obstáculos
    private void Atravesar(Transform pared)
    {
        
        // Define un vector posicion Serpiente con la posición actual de la serpiente
        Vector3 posicionSerpiente = transform.position;

        // Si la dirección en x es distinta de 0 => calcula la nueva posición de la serpiente en x teniendo en cuenta la pared
        if (direccion.x != 0f)
        {
            posicionSerpiente.x = Mathf.RoundToInt(-pared.position.x + direccion.x);

        }
        else
        // Si la dirección en y es distinta de 0 => calcula la nueva posición de la serpiente en y teniendo en cuenta la pared
        if (direccion.y != 0f)
        {
            posicionSerpiente.y = Mathf.RoundToInt(-pared.position.y + direccion.y);
        }

        // Posiciona a la serpiente en la nueva posición.
        transform.position = posicionSerpiente;
        
        
    }

    // Devuelve la longitud de la serpiente
    public int LongitudSerpiente()
    {
        return cuerpos.Count;
    }
    
}
