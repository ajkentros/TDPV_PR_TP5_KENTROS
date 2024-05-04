using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ControladorUI : MonoBehaviour
{
    
    [Header("PanelJuego")]
    [SerializeField] private TextMeshProUGUI tiempoCronometro;      // Referencia al componente Text del cronómetro
    [SerializeField] private TextMeshProUGUI dimension;             // Referencia al componente Text de la dimension del juego
    [SerializeField] private TextMeshProUGUI puntos;                // Referencia al componente Text de los puntos


    private void Start()
    {
        // Actualiza el panel del juego
        ActualizaPanelJuego();
    }

    private void Update()
    {
        // Actualiza el panel del juego
        ActualizaPanelJuego();

    }

      
    // Gestiona el formarto de tiempo para mostrar en minutos y segundos
    private string FormateaTiempo(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);

        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    private void ActualizaPanelJuego()
    {
        // Obtiene el tiempo de juego actual del GameManager y actualiza el texto del cronómetro
        float tiempoActual = GameManager.gameManager.ObtieneTiempoDeJuego();

        // Actualiza el TextMeshPro con el tiempo formateado 
        tiempoCronometro.text = FormateaTiempo(tiempoActual);

        // Actualiza el texto de los puntos alcanzados
        puntos.text = GameManager.gameManager.ObtienePuntos().ToString();

        // Actualiza el número de dimensiones jugadas en el panel UI
        dimension.text = (GameManager.gameManager.ObtieneEscenaActual()).ToString();

    }

}


