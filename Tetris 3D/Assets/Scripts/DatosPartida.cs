using UnityEngine;

public class DatosPartida : MonoBehaviour
{  
    [SerializeField] private int filas;
    [SerializeField] private int columnas;
    [SerializeField] private int piezas;
    [SerializeField] private float velocidad;
    [SerializeField] private float profundidad;
    [SerializeField] private int estiloTexto;

    public static DatosPartida Instancia { get; private set; }

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //-----------------------------------------------------
    //----------------- GETTERS Y SETTERS -----------------
    //-----------------------------------------------------
    public int Filas
    {
        get { return filas; }
        set { filas = value; }
    }
        
    public int Columnas
    {
        get { return columnas; }
        set { columnas = value; }
    }

    public int Piezas
    {
        get { return piezas; }
        set { piezas = value; }
    }
    
    public float Velocidad
    {
        get { return velocidad; }
        set { velocidad = value; }
    }
    public float Profundidad
    {
        get { return profundidad; }
        set { profundidad = value; }
    }
    public int EstiloTexto
    {
        get { return estiloTexto; }
        set { estiloTexto = value; }
    }
}

