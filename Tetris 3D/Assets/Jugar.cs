using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jugar : MonoBehaviour
{
    private bool opcionesVisibles;

    private void Start()
    {
        opcionesVisibles = false;
    }
    public void InciarPartida(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void MostrarOcultarOpciones()
    {
        if (!opcionesVisibles)
        {
            StartCoroutine(Mover(GetComponent<RectTransform>().position + Vector3.up * 500f, 1f));
            opcionesVisibles = true;
        }
        else
        {
            StartCoroutine(Mover(GetComponent<RectTransform>().position + Vector3.down * 500f, 1f));
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
            GetComponent<RectTransform>().position = Vector3.Lerp(puntoInicial, objetivo, tiempoPasado / duracion);            
            yield return null;
        }
    }
}
