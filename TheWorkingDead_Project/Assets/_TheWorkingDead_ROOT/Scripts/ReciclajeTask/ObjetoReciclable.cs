using System.Collections.Generic;
using UnityEngine;

public class ObjetoReciclable : MonoBehaviour
{
    [Header("Tipos correctos")]
    public List<TipoReciclaje> tiposCorrectos = new List<TipoReciclaje>();

    // Devuelve true si contiene este tipo
    public bool EsTipoCorrecto(TipoReciclaje tipo)
    {
        return tiposCorrectos.Contains(tipo);
    }
}
