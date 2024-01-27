using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jugar : MonoBehaviour
{
    //partida por defecto
    [SerializeField][Range(10,22)] private int filas = 22;
    [SerializeField][Range(4, 20)] private int columnas = 12;
    [SerializeField][Range(10, 22)] private int piezas = 5;
    [SerializeField][Range(0.05f, 1f)] private float velocidad = 1f;
    [SerializeField][Range(0f, 1f)] private float profundidad = 1f;
    [SerializeField][Range(0, 4)] private int estiloTexto = 0;

    //engranaje
    [SerializeField] private RectTransform engranaje;

    //dificultad
    [SerializeField] private Slider sliderDificultad;
    [SerializeField] private TextMeshProUGUI textoDificultad;

    //columnas
    [SerializeField] private Slider sliderColumnas;
    [SerializeField] private TextMeshProUGUI textoColumnas;

    //filas
    [SerializeField] private Slider sliderFilas;
    [SerializeField] private TextMeshProUGUI textoFilas;

    //piezas
    [SerializeField] private Slider sliderPiezas;
    [SerializeField] private TextMeshProUGUI textoPiezas;

    //profundidad
    [SerializeField] private Slider sliderProfundidad;
    [SerializeField] private TextMeshProUGUI textoProfundidad;

    //estilo texto
    [SerializeField] private Slider sliderEstiloTexto;
    [SerializeField] private TextMeshProUGUI textoEstiloTexto;

    private bool opcionesVisibles;

    private void Start()
    {
        //cargamos datos por defecto
        DatosPartida.Instancia.Filas = filas;
        DatosPartida.Instancia.Columnas = columnas;
        DatosPartida.Instancia.Piezas = piezas;
        DatosPartida.Instancia.Velocidad = velocidad;
        DatosPartida.Instancia.Profundidad = profundidad;
        DatosPartida.Instancia.EstiloTexto = estiloTexto;

        //agregamos los listeners de los sliders
        sliderDificultad?.onValueChanged.AddListener(DificultadModificada);
        sliderColumnas?.onValueChanged.AddListener(ColumnasModificadas);
        sliderFilas?.onValueChanged.AddListener(FilasModificadas);
        sliderPiezas?.onValueChanged.AddListener(PiezasModificada);
        sliderProfundidad?.onValueChanged.AddListener(ProfundidadModificada);
        sliderEstiloTexto?.onValueChanged.AddListener(EstiloModificado);

        //booleano para mostrar/ocultar opciones
        opcionesVisibles = false;
    }


    public void InciarPartida(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void MostrarOcultarOpciones()
    {
        //obtenemos el 70% de la pantalla para desplazar el RectTransform
        float alturaRectTransform = transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta.y * 0.7f;
        
        if (!opcionesVisibles)
        {
            StartCoroutine(Mover(GetComponent<RectTransform>().position + Vector3.up * alturaRectTransform, 1f));
            opcionesVisibles = true;
        }
        else
        {
            StartCoroutine(Mover(GetComponent<RectTransform>().position + Vector3.down * alturaRectTransform, 1f));
            opcionesVisibles = false;
        }
    }

    IEnumerator Mover(Vector3 objetivo, float duracion)
    {
        Vector3 puntoInicial = GetComponent<RectTransform>().position;
        float tiempoPasado = 0;

        while (tiempoPasado < duracion)
        {
            tiempoPasado += Time.deltaTime;

            //movimiento del RectTransform principal de la UI
            Vector3 posActual = GetComponent<RectTransform>().position;
            float posY = Mathf.SmoothStep(puntoInicial.y, objetivo.y, tiempoPasado / duracion);
            GetComponent<RectTransform>().position = new Vector3(posActual.x, posY, posActual.z);
            
            //rotación del engranaje
            engranaje.Rotate(new Vector3(0, 0, 5));

            yield return null;
        }
    }

    private void DificultadModificada(float valorSlider)
    {      
        //ajustamos el texto
        string dificultad = Mathf.RoundToInt(valorSlider) switch
        {   
            1 => "Min",
            2 => "Medio",
            3 => "Hard",
            4 => "Max",
            _ => "Error"
        };
        textoDificultad.text = dificultad;

        //ajustamos la velocidad
        float velocidad = Mathf.RoundToInt(valorSlider) switch
        {
            1 => 1f,
            2 => 0.5f,
            3 => 0.2f,
            4 => 0.05f,
            _ => 1f
        };

        DatosPartida.Instancia.Velocidad = velocidad;
    }

    private void PiezasModificada(float valorSlider)
    { 
        int piezas = Mathf.RoundToInt(valorSlider);
        DatosPartida.Instancia.Piezas = piezas;
        textoPiezas.text = piezas.ToString();
    }

    private void FilasModificadas(float valorSlider)
    {
        int filas = Mathf.RoundToInt(valorSlider);
        DatosPartida.Instancia.Filas = filas;
        textoFilas.text = filas.ToString();
    }

    private void ColumnasModificadas(float valorSlider)
    {
        int columnas = Mathf.RoundToInt(valorSlider);
        DatosPartida.Instancia.Columnas = columnas;
        textoColumnas.text = columnas.ToString();
    }

    private void ProfundidadModificada(float valorSlider)
    {
        //ajustamos la profundidad de la pieza
        float profundidad = Mathf.RoundToInt(valorSlider) switch
        {
            1 => 0f,
            2 => 0.25f,
            3 => 0.5f,
            4 => 0.75f,
            5 => 1f,
            _ => 0f
        };

        DatosPartida.Instancia.Profundidad = profundidad;
        textoProfundidad.text = Mathf.RoundToInt(valorSlider).ToString();
    }


    private void EstiloModificado(float valorSlider)
    {
        int estiloTexto = Mathf.RoundToInt(valorSlider);
        DatosPartida.Instancia.EstiloTexto = estiloTexto;
        textoEstiloTexto.text = estiloTexto.ToString();
        TextosTetris.Instance.ActualizarEstilosTextos(estiloTexto);
    }

}
