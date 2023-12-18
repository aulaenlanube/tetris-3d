using UnityEngine;

public class MelodiaTetris : MonoBehaviour
{  
    private void OnEnable()
    {
        Tetris.juegoPausado += ActivarDesactivarMelodia;
    }

    private void OnDisable()
    {
        Tetris.juegoPausado -= ActivarDesactivarMelodia;
    }

    public void ActivarDesactivarMelodia(bool pausa) 
    {
        if (pausa) GetComponent<AudioSource>().Pause();
        else GetComponent<AudioSource>().Play();
    }
}
