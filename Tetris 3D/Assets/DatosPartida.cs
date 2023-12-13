using UnityEngine;

public class DatosPartida : MonoBehaviour
{
    public static DatosPartida Instance { get; private set; }

    public int filas;
    public int columnas;
    public int piezas;
    public float velocidad;
    public float profundidad;    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
