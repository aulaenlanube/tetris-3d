using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MejoresPuntuacionesTetris : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI puntuacion1;
    [SerializeField] private TextMeshProUGUI puntuacion2;
    [SerializeField] private TextMeshProUGUI puntuacion3;

    // Start is called before the first frame update
    void Start()
    {
        int[] puntuaciones = PuntuacionesTetris.Instancia.ObtenerMejoresPuntuaciones(3);
        puntuacion1.text = puntuaciones[0].ToString();
        puntuacion2.text = puntuaciones[1].ToString(); 
        puntuacion3.text = puntuaciones[2].ToString(); 
    }

    
}
