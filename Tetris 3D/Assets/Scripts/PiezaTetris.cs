using System.Collections;
using UnityEngine;

public enum TipoPieza
{
    PiezaO,
    PiezaT,
    PiezaS,
    PiezaL,
    PiezaI
}

public struct PosicionTetris
{
    public int fila;
    public int columna;
    public float profundidad;

    public PosicionTetris(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        this.profundidad = Random.Range(-Tetris.Instancia.ProfundidadPieza / 2, Tetris.Instancia.ProfundidadPieza / 2);
    }

    public Vector3 Posicionar()
    {
        return new Vector3(columna, fila, profundidad);
    }
}

public class PiezaTetris : MonoBehaviour
{    
    private Tetris tetris;
    private TipoPieza tipoPieza;
    private GameObject cubo1;
    private GameObject cubo2;
    private GameObject cubo3;
    private GameObject cubo4;
    private PosicionTetris posCubo1;
    private PosicionTetris posCubo2;
    private PosicionTetris posCubo3;
    private PosicionTetris posCubo4;
    private PosicionTetris posCentro;
    private int minFila, maxColumna, minColumna;
    private bool bloqueada;

    private void Start()
    {
        //creamos el Tetris
        tetris = Tetris.Instancia;

        //generamos un tipo de pieza
        tipoPieza = Random.Range(0, tetris.PiezaDistintas) switch
        {
            0 => TipoPieza.PiezaI,
            1 => TipoPieza.PiezaO,
            2 => TipoPieza.PiezaL,
            3 => TipoPieza.PiezaS,
            4 => TipoPieza.PiezaT,
            _ => TipoPieza.PiezaI
        };

        //se puede mover
        bloqueada = false;

        //posicionamos la pieza
        PosicionarPieza();

        //iniciamos el movimiento de bajada
        StartCoroutine(BajarPieza());
    }

    void PosicionarPieza()
    {
        //posicionamos los 4 cubos según el tipo de pieza
        switch (tipoPieza)
        {
            case TipoPieza.PiezaO:

                posCubo1 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2 - 1);
                posCubo2 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2);
                posCubo3 = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2 - 1);
                posCubo4 = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                posCentro = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2 - 1);
                PosicionarCubos();
                break;

            case TipoPieza.PiezaT:

                posCubo1 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2 - 1);
                posCubo2 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2);
                posCubo3 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2 + 1);
                posCubo4 = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                posCentro = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                PosicionarCubos();
                break;

            case TipoPieza.PiezaS:

                posCubo1 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2 + 1);
                posCubo2 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2);
                posCubo3 = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                posCubo4 = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2 - 1);
                posCentro = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                PosicionarCubos();
                break;

            case TipoPieza.PiezaL:

                posCubo1 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2);
                posCubo2 = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                posCubo3 = new PosicionTetris(tetris.AltoTablero - 3, tetris.AnchoTablero / 2);
                posCubo4 = new PosicionTetris(tetris.AltoTablero - 3, tetris.AnchoTablero / 2 - 1);
                posCentro = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2 - 1);
                PosicionarCubos();
                break;

            case TipoPieza.PiezaI:

                posCubo1 = new PosicionTetris(tetris.AltoTablero - 1, tetris.AnchoTablero / 2);
                posCubo2 = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                posCubo3 = new PosicionTetris(tetris.AltoTablero - 3, tetris.AnchoTablero / 2);
                posCubo4 = new PosicionTetris(tetris.AltoTablero - 4, tetris.AnchoTablero / 2);
                posCentro = new PosicionTetris(tetris.AltoTablero - 2, tetris.AnchoTablero / 2);
                PosicionarCubos();
                break;
        }
    }

    void PosicionarCubos()
    {
        //creamos los 4 cubos de la pieza
        cubo1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo4 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //coloreamos los cubos
        EstablecerColorCubos();

        //añadimos tags
        cubo1.tag = "cubo";
        cubo2.tag = "cubo";
        cubo3.tag = "cubo";
        cubo4.tag = "cubo";

        //movemos los cubos
        MoverCubos();

        //establecemos máximos y mínimos
        EstablecerMaximosMinimos();
    }

    void EstablecerMaximosMinimos()
    {
        //establecemos máximos y mínimos
        maxColumna = Mathf.Max(posCubo1.columna, posCubo2.columna, posCubo3.columna, posCubo4.columna);
        minColumna = Mathf.Min(posCubo1.columna, posCubo2.columna, posCubo3.columna, posCubo4.columna);        
        minFila = Mathf.Min(posCubo1.fila, posCubo2.fila, posCubo3.fila, posCubo4.fila);
    }

    void Update()
    {
        if (!Tetris.Instancia.JuegoPausado())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Rotar();
                tetris.ReproducirSonidoMoverGirarPieza();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoverIzquierda();
                tetris.ReproducirSonidoMoverGirarPieza();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoverDerecha();
                tetris.ReproducirSonidoMoverGirarPieza();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoverArriba();
                tetris.ReproducirSonidoMoverGirarPieza();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoverAbajo();
                tetris.ReproducirSonidoMoverGirarPieza();
            }
        }
    }

    IEnumerator BajarPieza()
    {
        //si no hay posiciones libres bajo, la partida termina
        if (!tetris.ComprobarInferioresLibres(posCubo1, posCubo2, posCubo3, posCubo4))
        {
            Tetris.Instancia.FinalizarPartida();
            tetris.ReproducirSonidoGameOver();
            Destroy(gameObject);
        }
        //si hay posicines libres, la partida sigue
        else
        {
            float velocidadActual = tetris.Velocidad;
            while (true)
            {
                if (bloqueada)
                {
                    tetris.Velocidad = velocidadActual; //volvemos a poner la velocidad a como estaba al salir la pieza, debemos utilizar corrutinas para que se actualice la velocidad
                    yield break;
                }

                yield return new WaitForSeconds(tetris.Velocidad);
                MoverAbajo();
            }
        }
    }

    void MoverArriba()
    {
        tetris.Velocidad = 0; // utilizado para resolver el bug de pieza doble
    }

    void MoverAbajo()
    {
        if (minFila != 0 && tetris.ComprobarInferioresLibres(posCubo1, posCubo2, posCubo3, posCubo4))
        {
            posCubo1.fila--;
            posCubo2.fila--;
            posCubo3.fila--;
            posCubo4.fila--;
            posCentro.fila--;
            minFila--;            
            MoverCubos();
        }
        else FijarPieza();
    }

    void FijarPieza()
    {

        //ponemos a true las posiciones para ocuparlas
        tetris.Tablero[posCubo1.fila][posCubo1.columna] = cubo1;
        tetris.Tablero[posCubo2.fila][posCubo2.columna] = cubo2;
        tetris.Tablero[posCubo3.fila][posCubo3.columna] = cubo3;
        tetris.Tablero[posCubo4.fila][posCubo4.columna] = cubo4;

        //deshabilitamos componente
        Destroy(gameObject.GetComponent<PiezaTetris>());

        //bloqueamos la pieza
        bloqueada = true;

        //comprobamos si hay filas que eliminar
        EliminarFilasCompletadas();

        //generamos nueva pieza
        tetris.GenerarPieza();

        //destruimos la pieza, los 4 cubos son independientes y se quedan en el tablero
        Destroy(gameObject);
    }

    void MoverIzquierda()
    {
        if (minColumna > 0 && tetris.ComprobarIzquierdaLibres(posCubo1, posCubo2, posCubo3, posCubo4))
        {
            posCubo1.columna--;
            posCubo2.columna--;
            posCubo3.columna--;
            posCubo4.columna--;
            posCentro.columna--;
            minColumna--;
            maxColumna--;
            MoverCubos();
        }
    }

    void MoverDerecha()
    {
        if (maxColumna < tetris.AnchoTablero - 1 && tetris.ComprobarDerechaLibres(posCubo1, posCubo2, posCubo3, posCubo4))
        {
            posCubo1.columna++;
            posCubo2.columna++;
            posCubo3.columna++;
            posCubo4.columna++;
            posCentro.columna++;
            minColumna++;
            maxColumna++;
            MoverCubos();
        }
    }

    private void MoverCubos()
    {
        cubo1.transform.position = posCubo1.Posicionar();
        cubo2.transform.position = posCubo2.Posicionar();
        cubo3.transform.position = posCubo3.Posicionar();
        cubo4.transform.position = posCubo4.Posicionar();
    }


    public void Rotar()
    {
        //verificamos rotación y no rotamos el cuadrado
        if (RotacionValida() && tipoPieza != TipoPieza.PiezaO)
        {
            //rotamos los 4cubos
            RotarCubo(ref posCubo1);
            RotarCubo(ref posCubo2);
            RotarCubo(ref posCubo3);
            RotarCubo(ref posCubo4);

            //actualizamos máximos y mínimos
            EstablecerMaximosMinimos();

            //movemos los cubos
            MoverCubos();
        }
    }

    private void RotarCubo(ref PosicionTetris p)
    {
        int deltaX = p.columna - posCentro.columna;
        int deltaY = p.fila - posCentro.fila;
        p.columna = posCentro.columna + deltaY;
        p.fila = posCentro.fila - deltaX;
    }

    private bool RotacionValida()
    {
        if (!PosicionLibreAlRotar(posCubo1)) return false;
        if (!PosicionLibreAlRotar(posCubo2)) return false;
        if (!PosicionLibreAlRotar(posCubo3)) return false;
        if (!PosicionLibreAlRotar(posCubo4)) return false;
        return true;
    }

    private bool PosicionLibreAlRotar(PosicionTetris p)
    {
        int deltaX = p.columna - posCentro.columna;
        int deltaY = p.fila - posCentro.fila;
        int indiceFila = posCentro.fila - deltaX;
        int indiceColumna = posCentro.columna + deltaY;

        //comprobamos que los índices sean validos
        if (!tetris.IndiceValido(indiceFila, indiceColumna)) return false;

        //si la posición está a true, está ocupada
        if (tetris.Tablero[indiceFila][indiceColumna] != null)
            return false;

        //posición válida y libre
        return true;
    }


    private void EliminarFilasCompletadas()
    {
        for (int fila = 0; fila < tetris.AltoTablero; fila++)
        {
            int elementosFila = 0;
            for (int columna = 0; columna < tetris.AnchoTablero; columna++)
            {
                if (tetris.Tablero[fila][columna] != null) elementosFila++;

                //si es la última posición de la fila y están todas ocupadas
                if (columna == tetris.AnchoTablero - 1 && elementosFila == tetris.AnchoTablero)
                {
                    //eliminamos cubos y bajamos la parte superior del tablero respecto a la fila que debemos eliminar
                    BajarFila(fila);
                    tetris.ActualizarPuntuacion();
                    tetris.ReproducirSonidoEliminarLinea();
                    return;
                }
            }
        }
    }

    private void BajarFila(int fila)
    {
        //ajustamos tablero a partir de la fila actual
        for (int i = fila; i < tetris.Tablero.Length; i++)
        {
            for (int j = 0; j < tetris.AnchoTablero; j++)
            {
                //fila que debemos destruir
                if (i == fila) Destroy(tetris.Tablero[i][j]);

                //si no es la última fila, bajamos la de arriba
                if (i < tetris.Tablero.Length - 1) tetris.Tablero[i][j] = tetris.Tablero[i + 1][j];
                if (tetris.Tablero[i][j] != null) tetris.Tablero[i][j].transform.position += Vector3.down;
            }
        }

        //eliminamos los que están por debajo de 0
        GameObject[] cubos = GameObject.FindGameObjectsWithTag("cubo");
        foreach (GameObject cubo in cubos)
            if (cubo.transform.position.y < 0) Destroy(cubo);

        //comprobamos de nuevo después de modificar
        EliminarFilasCompletadas();
    }

    private void EstablecerColorCubos()
    {
        switch (tipoPieza)
        {
            case TipoPieza.PiezaL:
                EstablecerColorMaterial(Color.blue);
                break;
            case TipoPieza.PiezaS:
                EstablecerColorMaterial(Color.red);
                break;
            case TipoPieza.PiezaI:
                EstablecerColorMaterial(Color.cyan);
                break;
            case TipoPieza.PiezaT:
                EstablecerColorMaterial(Color.green);
                break;
            case TipoPieza.PiezaO:
                EstablecerColorMaterial(Color.yellow);
                break;
        }
    }

    private void EstablecerColorMaterial(Color c)
    {
        cubo1.GetComponent<Renderer>().material = tetris.MaterialPiezas;
        cubo2.GetComponent<Renderer>().material = tetris.MaterialPiezas;
        cubo3.GetComponent<Renderer>().material = tetris.MaterialPiezas;
        cubo4.GetComponent<Renderer>().material = tetris.MaterialPiezas;

        cubo1.GetComponent<Renderer>().material.color = c;
        cubo2.GetComponent<Renderer>().material.color = c;
        cubo3.GetComponent<Renderer>().material.color = c;
        cubo4.GetComponent<Renderer>().material.color = c;
    }
}
