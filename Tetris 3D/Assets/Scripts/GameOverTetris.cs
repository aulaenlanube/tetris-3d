using UnityEngine;
using UnityEngine.UI;

public class GameOverTetris : MonoBehaviour
{
    private void OnEnable() { Tetris.gameOverTetris += FinalizarPartida; }
    private void OnDisable() { Tetris.gameOverTetris -= FinalizarPartida; }
    private void FinalizarPartida(int nuevaPuntuacion)
    {
        GetComponent<Text>().enabled = true;
        GetComponent<Text>().text = $"GAME OVER\n{nuevaPuntuacion} puntos";
    }
}
