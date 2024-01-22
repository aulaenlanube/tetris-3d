using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class ListaPuntuacionesTetris
{
    public List<int> listaPuntuaciones;

    public ListaPuntuacionesTetris()
    {
        listaPuntuaciones = new List<int>();
    }
}

public class PuntuacionesTetris
{
    private static PuntuacionesTetris instancia = null;
    private ListaPuntuacionesTetris puntuaciones;    
    private string rutaFicheroPuntuacionesBinario = Application.persistentDataPath + "PuntuacionesTetris.dat";

    public static PuntuacionesTetris Instancia
    {
        get
        {
            if (instancia == null)
            {
                instancia = new PuntuacionesTetris();
            }
            return instancia;
        }
    }

    private PuntuacionesTetris()
    {
        puntuaciones = new ListaPuntuacionesTetris();
        CargarPuntuacionesBinario();
    }

    public int[] ObtenerMejoresPuntuaciones(int cantidad)
    {  
        List<int> listaPuntuaciones = puntuaciones.listaPuntuaciones
            .OrderByDescending(x => x)
            .Take(cantidad)
            .ToList();

        int[] puntuacionesOrdenadas = new int[cantidad];
        for (int i = 0; i < cantidad; i++)
        {
            if (i < listaPuntuaciones.Count)  puntuacionesOrdenadas[i] = listaPuntuaciones[i];            
            else  puntuacionesOrdenadas[i] = 0;            
        }

        return puntuacionesOrdenadas;
    }


    public void AgregarPuntuacion(int puntuacion)
    {
        puntuaciones.listaPuntuaciones.Add(puntuacion);
        GuardarPuntuacionesBinario();
    }


    public void GuardarPuntuacionesBinario()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(rutaFicheroPuntuacionesBinario);
        formatter.Serialize(file, puntuaciones);
        file.Close();
    }

    public void CargarPuntuacionesBinario()
    {
        if (File.Exists(rutaFicheroPuntuacionesBinario))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(rutaFicheroPuntuacionesBinario, FileMode.Open);
            puntuaciones = (ListaPuntuacionesTetris)formatter.Deserialize(file);
            file.Close();
            return;
        }
        puntuaciones = new ListaPuntuacionesTetris();
    }


}
