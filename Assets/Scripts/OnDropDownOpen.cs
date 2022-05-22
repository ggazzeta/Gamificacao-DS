using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnDropDownOpen : MonoBehaviour
{
    public TMP_Dropdown dropDown;
    // Start is called before the first frame update
    void Start()
    {
        dropDown = GetComponent<TMP_Dropdown>();
        if (UsuarioAtual.usuarioLogado.Funcao == 1)
        {
            dropDown.ClearOptions();
            dropDown.AddOptions(new List<string> { "Todo o time" });
            dropDown.AddOptions(UsuarioAtual.usuariosTimeGestor);
        }
    }

}
