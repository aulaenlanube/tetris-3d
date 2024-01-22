using UnityEngine;
using UnityEngine.UI;

public class GameOverTetris : MonoBehaviour
{
    private void OnEnable() { Tetris.gameOverTetris += FinalizarPartida; }

    private void OnDisable() { Tetris.gameOverTetris -= FinalizarPartida; }

    private void FinalizarPartida(int nuevaPuntuacion)
    {               
        //mostramos game over y puntuacion
        GetComponent<Text>().text = $"GAME OVER\n{nuevaPuntuacion} puntos";         

        //guardamos puntuacion
        PuntuacionesTetris.Instancia.AgregarPuntuacion(nuevaPuntuacion);
    }
}
