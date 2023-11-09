using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.UI;

public class Tetris : MonoBehaviour{
    public static Tetris instance;
    public GameObject[][] tablero;    public Text textoPuntuacion;    [Range(4, 20)]    public int anchoTablero = 12;    [Range(10, 22)]    public int altoTablero = 22;    [Range(0.05f, 1f)]    public float velocidad = 0.5f;    [Range(0f, 1f)]    public float profundidadPieza;    [Range(1, 5)]    public int piezaDistintas;    private int puntuacion;    private void Awake()    {        tablero = new GameObject[altoTablero][];        if (instance == null)        {            instance = this;        }        else        {            Destroy(gameObject);        }    }    private void Start()    {        puntuacion = 0;

        //generamos bordes del tablero
        GameObject paredIzquierda = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject paredDerecha = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject paredInferior = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //pared inferior
        paredInferior.transform.position = new Vector3(anchoTablero / 2 - (anchoTablero % 2 == 0 ? 0.5f : 0), -1f, 0f);
        paredInferior.transform.localScale = new Vector3(anchoTablero + 2, 1f, 1f);

        //pared izquierda
        paredIzquierda.transform.position = new Vector3(-1, altoTablero / 2 - 1, 0f);
        paredIzquierda.transform.localScale = new Vector3(1f, altoTablero - 1, 1f);

        //pared derecha
        paredDerecha.transform.position = new Vector3(anchoTablero, altoTablero / 2 - 1, 0f);
        paredDerecha.transform.localScale = new Vector3(1f, altoTablero - 1, 1f);

        //movemos la cámara dependiendo del ancho del tablero
        if (anchoTablero < 12) Camera.main.transform.Translate(Vector3.left * (12 - anchoTablero) / 2);
        if (anchoTablero > 12) Camera.main.transform.Translate(Vector3.right * (anchoTablero - 12) / 2);

        //iniciamos matriz
        for (int i = 0; i < tablero.Length; i++)        {            tablero[i] = new GameObject[anchoTablero];        }
        GenerarPieza();    }    public void ActualizarPuntuacion()
    {
        puntuacion += Mathf.RoundToInt(100f - velocidad * 100);
        textoPuntuacion.text = $"Puntos: {puntuacion}";
        if (velocidad > 0.04) velocidad -= 0.01f;
    }    public void GenerarPieza()    {        new GameObject("pieza").AddComponent<PiezaTetris>();    }    public bool ComprobarInferioresLibres(PosicionTetris cubo1, PosicionTetris cubo2, PosicionTetris cubo3, PosicionTetris cubo4)    {
        //comprobamos indices, para no salirnos de las dimensiones del tablero
        if (!IndiceValido(cubo1.fila - 1, cubo1.columna)) return false;        if (!IndiceValido(cubo2.fila - 1, cubo2.columna)) return false;        if (!IndiceValido(cubo3.fila - 1, cubo3.columna)) return false;        if (!IndiceValido(cubo4.fila - 1, cubo4.columna)) return false;

        //si la fila actual es la cero, no hay libres
        if (cubo1.fila == 0 || cubo2.fila == 0 || cubo3.fila == 0 || cubo4.fila == 0) return false;

        //si las posiciones inferiores de cada uno de los 4 cubos est?n libres --> true
        if (!tablero[cubo1.fila - 1][cubo1.columna]
&& !tablero[cubo2.fila - 1][cubo2.columna]
&& !tablero[cubo3.fila - 1][cubo3.columna]
&& !tablero[cubo4.fila - 1][cubo4.columna]) return true;        return false;    }    public bool ComprobarIzquierdaLibres(PosicionTetris cubo1, PosicionTetris cubo2, PosicionTetris cubo3, PosicionTetris cubo4)    {
        //comprobamos indices, para no salirnos de las dimensiones del tablero
        if (!IndiceValido(cubo1.fila, cubo1.columna - 1)) return false;        if (!IndiceValido(cubo2.fila, cubo2.columna - 1)) return false;        if (!IndiceValido(cubo3.fila, cubo3.columna - 1)) return false;        if (!IndiceValido(cubo4.fila, cubo4.columna - 1)) return false;

        //si la columna actual es la cero, no hay libres
        if (cubo1.columna == 0 || cubo2.columna == 0 || cubo3.columna == 0 || cubo4.columna == 0) return false;

        //si las posiciones de la izquierda de cada uno de los 4 cubos est?n libres --> true
        if (!tablero[cubo1.fila][cubo1.columna - 1]
&& !tablero[cubo2.fila][cubo2.columna - 1]
&& !tablero[cubo3.fila][cubo3.columna - 1]
&& !tablero[cubo4.fila][cubo4.columna - 1]) return true;        return false;    }    public bool ComprobarDerechaLibres(PosicionTetris cubo1, PosicionTetris cubo2, PosicionTetris cubo3, PosicionTetris cubo4)    {
        //comprobamos indices, para no salirnos de las dimensiones del tablero
        if (!IndiceValido(cubo1.fila, cubo1.columna + 1)) return false;        if (!IndiceValido(cubo2.fila, cubo2.columna + 1)) return false;        if (!IndiceValido(cubo3.fila, cubo3.columna + 1)) return false;        if (!IndiceValido(cubo4.fila, cubo4.columna + 1)) return false;

        //si la columna actual es la ?ltima, no hay libres
        if (cubo1.columna == anchoTablero || cubo2.columna == anchoTablero || cubo3.columna == anchoTablero || cubo4.columna == anchoTablero) return false;

        //si las posiciones de la derecha de cada uno de los 4 cubos est?n libres --> true
        if (!tablero[cubo1.fila][cubo1.columna + 1]
&& !tablero[cubo2.fila][cubo2.columna + 1]
&& !tablero[cubo3.fila][cubo3.columna + 1]
&& !tablero[cubo4.fila][cubo4.columna + 1]) return true;        return false;    }    public bool IndiceValido(int fila, int columna)    {        if (fila < 0 || fila > altoTablero - 1) return false;        if (columna < 0 || columna > anchoTablero - 1) return false;        return true;    }


}