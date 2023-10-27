using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public GameObject pieza;     
    public int anchoTablero = 12;
    public int altoTablero = 22;
    public bool[][] tablero;
    public float velocidad = 1f;

    public static Tetris instance;

    private void Awake()
    {
        tablero = new bool[altoTablero][];

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < tablero.Length; i++)
        {
            tablero[i] = new bool[anchoTablero];
        }
        GenerarPieza();
    }

    public void GenerarPieza()
    {
        Instantiate(pieza, new Vector3(0, 0, 0), Quaternion.identity);
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

        //si las posiciones inferiores de cada uno de los 4 cubos est�n libres --> true
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

        //si las posiciones de la izquierda de cada uno de los 4 cubos est�n libres --> true
        if (!tablero[cubo1.fila][cubo1.columna-1]
            && !tablero[cubo2.fila][cubo2.columna-1]
            && !tablero[cubo3.fila][cubo3.columna-1]
            && !tablero[cubo4.fila][cubo4.columna-1]) return true;

        return false;
    }

    public bool ComprobarDerechaLibres(PosicionTetris cubo1, PosicionTetris cubo2, PosicionTetris cubo3, PosicionTetris cubo4)
    {
        //comprobamos indices, para no salirnos de las dimensiones del tablero
        if (!IndiceValido(cubo1.fila, cubo1.columna + 1)) return false;
        if (!IndiceValido(cubo2.fila, cubo2.columna + 1)) return false;
        if (!IndiceValido(cubo3.fila, cubo3.columna + 1)) return false;
        if (!IndiceValido(cubo4.fila, cubo4.columna + 1)) return false;

        //si la columna actual es la �ltima, no hay libres
        if (cubo1.columna == anchoTablero || cubo2.columna == anchoTablero || cubo3.columna == anchoTablero || cubo4.columna == anchoTablero) return false;

        //si las posiciones de la derecha de cada uno de los 4 cubos est�n libres --> true
        if (!tablero[cubo1.fila][cubo1.columna+1]
            && !tablero[cubo2.fila][cubo2.columna+1]
            && !tablero[cubo3.fila][cubo3.columna+1]
            && !tablero[cubo4.fila][cubo4.columna+1]) return true;

        return false;
    }

    public bool IndiceValido(int fila, int columna)
    {
        if(fila < 0 || fila > altoTablero - 1) return false;
        if (columna < 0 || columna > anchoTablero - 1) return false;
        return true;
    }

    
}
