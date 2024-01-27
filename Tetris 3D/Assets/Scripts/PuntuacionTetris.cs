using UnityEngine;
using UnityEngine.UI;

public class PuntuacionTetris : MonoBehaviour
{
    private void OnEnable() { Tetris.puntuacionActualizada += ActualizarPuntuacion; }

    private void OnDisable() { Tetris.puntuacionActualizada -= ActualizarPuntuacion; }

    private void ActualizarPuntuacion(int nuevaPuntuacion)
    {
        GetComponent<Text>().text = $"{nuevaPuntuacion}";
    }
}
