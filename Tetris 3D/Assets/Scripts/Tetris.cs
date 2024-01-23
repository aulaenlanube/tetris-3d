using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Tetris : MonoBehaviour
{  
    public GameObject[][] tablero;

    [Range(4, 20)]
    public int anchoTablero = 12;

    [Range(10, 22)]
    public int altoTablero = 22;

    [Range(0.05f, 1f)]
    public float velocidad = 0.5f;

    [Range(0f, 1f)]
    public float profundidadPieza;

    [Range(1, 5)]
    public int piezaDistintas;

    public Material materialPiezas;
    public Material materialParedes;
      
    //eventos puntuación y gameOver
    public delegate void puntuacionTetris(int n);    
    public static event puntuacionTetris puntuacionActualizada;
    public static event puntuacionTetris gameOverTetris;

    //evento pausa
    public delegate void pausaJuego(bool b);
    public static event pausaJuego juegoPausado;

    //evento textos
    public delegate void modificarEstiloTexto(int n);
    public static event modificarEstiloTexto estiloTextoActualizado;


    //singleton para acceder al Tetris
    public static Tetris instance;
    
    private int puntuacion;    
   
    [SerializeField] private AudioClip efectoSonidoMoverGirar;
    [SerializeField] private AudioClip efectoSonidoEliminarLinea;
    [SerializeField] private AudioClip efectoSonidoGameOver;

    private bool pause;

    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject panelGameOver;

    private int estiloTextos;


    private void Awake()
    {           
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LeerDatos(); //leemos los datos desde el singleton DatosPartida

        GenerarParedes();
        GenerarTablero();
        GenerarPieza();

        pause = false;
        puntuacion = 0;
        estiloTextos = 0;
        puntuacionActualizada?.Invoke(puntuacion);
        estiloTextoActualizado?.Invoke(estiloTextos);
    }

    public void LeerDatos()
    {
        //si tenemos datos configurados en la escena de inicio, los cargamos
        if(DatosPartida.Instance)
        {
            anchoTablero = DatosPartida.Instance.columnas;
            altoTablero = DatosPartida.Instance.filas;
            piezaDistintas = DatosPartida.Instance.piezas;
            velocidad = DatosPartida.Instance.velocidad;
            profundidadPieza = DatosPartida.Instance.profundidad;
            estiloTextos = DatosPartida.Instance.estiloTexto;
        }              
    }

    private void GenerarParedes()
    {
        //generamos paredes del tablero
        GameObject paredIzquierda = GameObject.CreatePrimitive(PrimitiveType.Cube);
        paredIzquierda.name = "ParedIzquierda";
        GameObject paredDerecha = GameObject.CreatePrimitive(PrimitiveType.Cube);
        paredDerecha.name = "ParedDerecha";
        GameObject paredInferior = GameObject.CreatePrimitive(PrimitiveType.Cube);
        paredInferior.name = "ParedInferior";

        //pared inferior
        paredInferior.transform.position = new Vector3(anchoTablero / 2 - (anchoTablero % 2 == 0 ? 0.5f : 0), -1f, 0f);
        paredInferior.transform.localScale = new Vector3(anchoTablero + 2, 1f, 1.5f);

        //pared izquierda
        paredIzquierda.transform.position = new Vector3(-1, altoTablero / 2 - 1, 0f);
        paredIzquierda.transform.localScale = new Vector3(1f, altoTablero - 1, 1.5f);

        //pared derecha
        paredDerecha.transform.position = new Vector3(anchoTablero, altoTablero / 2 - 1, 0f);
        paredDerecha.transform.localScale = new Vector3(1f, altoTablero - 1, 1.5f);

        //movemos la cámara dependiendo del ancho del tablero
        if (anchoTablero < 12) Camera.main.transform.Translate(Vector3.left * (12 - anchoTablero) / 2);
        if (anchoTablero > 12) Camera.main.transform.Translate(Vector3.right * (anchoTablero - 12) / 2);

        //establecemos los materiales de las paredes del tablero
        paredIzquierda.GetComponent<Renderer>().material = materialParedes;
        paredDerecha.GetComponent<Renderer>().material = materialParedes;
        paredInferior.GetComponent<Renderer>().material = materialParedes;
    }

    public void GenerarTablero()
    {
        tablero = new GameObject[altoTablero][];
        for (int i = 0; i < tablero.Length; i++)
        {
            tablero[i] = new GameObject[anchoTablero];
        }
    }

    public void GenerarPieza()
    {
       new GameObject("pieza").AddComponent<PiezaTetris>();
    }

    public void ActualizarPuntuacion()
    {
        //actualizamos puntuación y publicamos evento
        puntuacion += Mathf.RoundToInt(100f - velocidad * 100);
        puntuacionActualizada?.Invoke(puntuacion);        

        //modificación de la velocidad, plantear solución logaritmíca que tienda a 0.05
        if (velocidad > 0.1) velocidad -= 0.01f; 
        else if (velocidad > 0.05) velocidad -= 0.005f;
        
    }

    public bool ComprobarInferioresLibres(PosicionTetris cubo1, PosicionTetris cubo2, PosicionTetris cubo3, PosicionTetris cubo4)
    {
        //comprobamos indices, para no salirnos de las dimensiones del tablero
        if (!IndiceValido(cubo1.fila - 1, cubo1.columna)) return false;
        if (!IndiceValido(cubo2.fila - 1, cubo2.columna)) return false;
        if (!IndiceValido(cubo3.fila - 1, cubo3.columna)) return false;
        if (!IndiceValido(cubo4.fila - 1, cubo4.columna)) return false;

        //si la fila actual es la cero, no hay libres
        if (cubo1.fila == 0 || cubo2.fila == 0 || cubo3.fila == 0 || cubo4.fila == 0) return false;

        //si las posiciones inferiores de cada uno de los 4 cubos est?n libres --> true
        if (!tablero[cubo1.fila - 1][cubo1.columna]
            && !tablero[cubo2.fila - 1][cubo2.columna]
            && !tablero[cubo3.fila - 1][cubo3.columna]
            && !tablero[cubo4.fila - 1][cubo4.columna]) return true;

        return false;
    }

    public bool ComprobarIzquierdaLibres(PosicionTetris cubo1, PosicionTetris cubo2, PosicionTetris cubo3, PosicionTetris cubo4)
    {
        //comprobamos indices, para no salirnos de las dimensiones del tablero
        if (!IndiceValido(cubo1.fila, cubo1.columna - 1)) return false;
        if (!IndiceValido(cubo2.fila, cubo2.columna - 1)) return false;
        if (!IndiceValido(cubo3.fila, cubo3.columna - 1)) return false;
        if (!IndiceValido(cubo4.fila, cubo4.columna - 1)) return false;

        //si la columna actual es la cero, no hay libres
        if (cubo1.columna == 0 || cubo2.columna == 0 || cubo3.columna == 0 || cubo4.columna == 0) return false;

        //si las posiciones de la izquierda de cada uno de los 4 cubos est?n libres --> true
        if (!tablero[cubo1.fila][cubo1.columna - 1]
            && !tablero[cubo2.fila][cubo2.columna - 1]
            && !tablero[cubo3.fila][cubo3.columna - 1]
            && !tablero[cubo4.fila][cubo4.columna - 1]) return true;

        return false;
    }

    public bool ComprobarDerechaLibres(PosicionTetris cubo1, PosicionTetris cubo2, PosicionTetris cubo3, PosicionTetris cubo4)
    {
        //comprobamos indices, para no salirnos de las dimensiones del tablero
        if (!IndiceValido(cubo1.fila, cubo1.columna + 1)) return false;
        if (!IndiceValido(cubo2.fila, cubo2.columna + 1)) return false;
        if (!IndiceValido(cubo3.fila, cubo3.columna + 1)) return false;
        if (!IndiceValido(cubo4.fila, cubo4.columna + 1)) return false;

        //si la columna actual es la ?ltima, no hay libres
        if (cubo1.columna == anchoTablero || cubo2.columna == anchoTablero || cubo3.columna == anchoTablero || cubo4.columna == anchoTablero) return false;

        //si las posiciones de la derecha de cada uno de los 4 cubos est?n libres --> true
        if (!tablero[cubo1.fila][cubo1.columna + 1]
            && !tablero[cubo2.fila][cubo2.columna + 1]
            && !tablero[cubo3.fila][cubo3.columna + 1]
            && !tablero[cubo4.fila][cubo4.columna + 1]) return true;

        return false;
    }

    public bool IndiceValido(int fila, int columna)
    {
        if (fila < 0 || fila > altoTablero - 1) return false;
        if (columna < 0 || columna > anchoTablero - 1) return false;
        return true;
    }

    public void FinalizarPartida()
    {
        MostrarPanelGameOver();
        gameOverTetris?.Invoke(puntuacion);
    }

    //efectos de sonido
    public void ReproducirSonidoGameOver()
    {        
        GetComponent<AudioSource>().PlayOneShot(efectoSonidoGameOver);
    }
    public void ReproducirSonidoEliminarLinea()
    {
        GetComponent<AudioSource>().PlayOneShot(efectoSonidoEliminarLinea);
    }
    public void ReproducirSonidoMoverGirarPieza()
    {
        GetComponent<AudioSource>().PlayOneShot(efectoSonidoMoverGirar);
    }
    
    public void PausarReanudarJuego()
    {
        if(pause)
        {
            pause = false;
            juegoPausado?.Invoke(pause); //publicamos evento de pausa
            Time.timeScale = 1f; // reanuda el tiempo del juego
            panelPausa.SetActive(false);
            botonPausa.SetActive(true);
            
        }
        else
        { 
            pause = true;
            juegoPausado?.Invoke(pause); //publicamos evento de pausa
            Time.timeScale = 0f; // detiene el tiempo del juego
            panelPausa.SetActive(true);
            botonPausa.SetActive(false);
        }       
    }

    public void IniciarReiniciarPartida(string nombreEscena)
    {
        Time.timeScale = 1f; // reanuda el tiempo del juego
        SceneManager.LoadScene(nombreEscena);
    }

    public void MostrarPanelGameOver()
    {        
        botonPausa.SetActive(false);
        panelGameOver.SetActive(true);
    }

    public bool JuegoPausado()
    {
        return pause;
    }
}
