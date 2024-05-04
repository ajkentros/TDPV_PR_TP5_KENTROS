using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlFinal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tituloFinal;           // Referencia variable al texto del t�tulo
    [SerializeField] private TextMeshProUGUI tiempoFinal;           // Referencia variable al texto del tiempo final
    [SerializeField] private TextMeshProUGUI puntosFinal;           // Referencia variable al texto de los puntos finales alcanzados
    [SerializeField] private TextMeshProUGUI cantidadCuerpos;       // Referencia variable al texto de cantidad de cuerpos alcanzados
    [SerializeField] private TextMeshProUGUI cantidadFrutas;        // Referencia variable al texto de cantidad de frutas comidas
    [SerializeField] private TextMeshProUGUI numeroDimension;       // Referencia variable al texto de numero de dimensi�n alcanzada


    private float tiempo;          // Referencia variable tiempoJuego
    private bool ganaste;
    private bool gameOver;

    // Update is called once per frame
    void Update()
    {
        // Define la variable que verifica si el juego est� corriendo o en pausa
        ganaste = GameManager.gameManager.ObtieneEstadoGanaste();

        // Define la variable que verifica si el juego termin�
        gameOver = GameManager.gameManager.ObtieneEstadoGameOver();
        
        // Actualiza el men� final
        ActualizaUIMenuFinal();
        

    }

    // Actuliza el canvas del men� final
    private void ActualizaUIMenuFinal()
    {
        // Toma el tiempo de juego del Game Manager
        tiempo = GameManager.gameManager.ObtieneTiempoDeJuego();

        // Sino si el juego est� corriendo y gameOver = false => el jugador gan�
        if (ganaste == true && gameOver == true)
        {
            // Cambia el t�tulo de men�
            tituloFinal.text = "GANASTE";

        }
        
        if(ganaste == false && gameOver == true)
        {
            // Pausa el jeugo
            GameManager.gameManager.PausarJuego();

            // Cambia el t�tulo de men�
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

        // Actualiza el texto de la dimensi�n �ltima alcanzada
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

   
    // Gestiona la accci�n del bot�n Men�
    public void BotonMenu()
    {
        // Setea gameOver = true para iniciar variables del juego
        GameManager.gameManager.IniciaMenu();

    }
}
