using TMPro;
using UnityEngine;

public class TextosTetris : MonoBehaviour
{
    public static TextosTetris Instance { get; private set; }

    public TextMeshProUGUI[] textosUI;
    public Material[] materiales;

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

    private void Start()
    {
        foreach (TextMeshProUGUI texto in textosUI)
        {
            texto.material = materiales[0];
        }
    }

    public void ActualizarEstilosTextos(int estilo)
    {
        foreach (TextMeshProUGUI texto in textosUI)
        {
            texto.fontMaterial = materiales[estilo];
        }
    }
}
