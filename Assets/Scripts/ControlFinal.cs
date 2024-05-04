using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlFinal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tituloFinal;           // Referencia variable al texto del título
    [SerializeField] private TextMeshProUGUI tiempoFinal;           // Referencia variable al texto del tiempo final
    [SerializeField] private TextMeshProUGUI puntosFinal;           // Referencia variable al texto de los puntos finales alcanzados
    [SerializeField] private TextMeshProUGUI cantidadCuerpos;       // Referencia variable al texto de cantidad de cuerpos alcanzados
    [SerializeField] private TextMeshProUGUI cantidadFrutas;        // Referencia variable al texto de cantidad de frutas comidas
    [SerializeField] private TextMeshProUGUI numeroDimension;       // Referencia variable al texto de numero de dimensión alcanzada


    private float tiempo;          // Referencia variable tiempoJuego
    private bool ganaste;
    private bool gameOver;

    // Update is called once per frame
    void Update()
    {
        // Define la variable que verifica si el juego está corriendo o en pausa
        ganaste = GameManager.gameManager.ObtieneEstadoGanaste();

        // Define la variable que verifica si el juego terminó
        gameOver = GameManager.gameManager.ObtieneEstadoGameOver();
        
        // Actualiza el menú final
        ActualizaUIMenuFinal();
        

    }

    // Actuliza el canvas del menú final
    private void ActualizaUIMenuFinal()
    {
        // Toma el tiempo de juego del Game Manager
        tiempo = GameManager.gameManager.ObtieneTiempoDeJuego();

        // Sino si el juego está corriendo y gameOver = false => el jugador ganó
        if (ganaste == true && gameOver == true)
        {
            // Cambia el título de menú
            tituloFinal.text = "GANASTE";

        }
        
        if(ganaste == false && gameOver == true)
        {
            // Pausa el jeugo
            GameManager.gameManager.PausarJuego();

            // Cambia el título de menú
            tituloFinal.text = "PERDISTE";
        }

        // Actualiza el TextMeshPro con el tiempo formateado
        tiempoFinal.text = FormatoTiempo(tiempo);

        // Actualiza el texto de los puntos alcanzados
        puntosFinal.text = GameManager.gameManager.ObtienePuntos().ToString();

        // Actualiza el texto de la cantidad de comida consumida
        cantidadFrutas.text = GameManager.gameManager.ObtieneComidaConsumida().ToString();

        // Actualiza el texto de la cantidad de cuerpos que tiene la serpiente
        cantidadCuerpos.text = GameManager.gameManager.ObtieneCuerpo().ToString();

        // Actualiza el texto de la dimensión última alcanzada
        numeroDimension.text = (GameManager.gameManager.ObtieneEscenaActual() - 1).ToString();
    }
    
    // Gestiona el formato del tiempo
    private string FormatoTiempo(float tiempo)
    {
        // Formatea el tiempo en minutos y segundos
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);
        int milisegundos = Mathf.FloorToInt((tiempo * 100) % 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, milisegundos); ;
    }

   
    // Gestiona la accción del botón Menú
    public void BotonMenu()
    {
        // Setea gameOver = true para iniciar variables del juego
        GameManager.gameManager.IniciaMenu();

    }
}
