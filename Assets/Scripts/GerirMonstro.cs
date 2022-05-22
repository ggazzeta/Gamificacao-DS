using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GerirMonstro : MonoBehaviour
{

    public int Level;
    public int XPAtual;
    public float XPRequirido;
    public SQLCon _sqlConnection;
    public Animator animatorMonstro;
    public TextMeshProUGUI textLevel;
    public Image XPBar;
    public TextMeshProUGUI textoPorcentagem;

    [Header("Formula")]
    [Range(1f, 300f)]
    public float multiplicadorAdicao = 300;
    [Range(2f, 4f)]
    public float multiplicadorPotencia = 2;
    [Range(7f, 14f)]
    public float multiplicadorDivisao = 7;

    // Start is called before the first frame update
    void Start()
    {
        XPRequirido = CalcularXPRequirido();
        animatorMonstro = gameObject.GetComponentInChildren<Animator>();
        //Level = UsuarioAtual.usuarioLogado.Nivel;
        //textLevel.text = Level.ToString();
        //XPAtual = UsuarioAtual.usuarioLogado.XP;
        //XPRequirido = CalcularXPRequirido();


        //if (XPAtual > XPRequirido)
        //{
        //    GanharLevel();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        AtualizaInformacoesMonstro();
        var floatProximoNivel = XPAtual / XPRequirido;
        var porcentagemProximoNivel = (floatProximoNivel * 100).ToString("N0");
        XPBar.fillAmount = floatProximoNivel;
        textoPorcentagem.text = porcentagemProximoNivel + "%";

    }

    public void AtualizaInformacoesMonstro()
    {
        Level = UsuarioAtual.usuarioMonstro.Nivel;
        textLevel.text = Level.ToString();
        XPAtual = UsuarioAtual.usuarioMonstro.XP;
        XPRequirido = CalcularXPRequirido();


        if (XPAtual > XPRequirido)
        {
            GanharLevel();
        }
    }

    public async Task GanharXP(int xpGanho)
    {
        XPAtual += xpGanho;
        UsuarioAtual.usuarioMonstro.XP = XPAtual;

        if (XPAtual > XPRequirido)
        {
            GanharLevel();
        }

        await _sqlConnection.GravarXP(XPAtual, UsuarioAtual.usuarioMonstro);
    }

    public void GanharLevel()
    {
        Level++;
        UsuarioAtual.usuarioMonstro.Nivel = Level;
        textLevel.text = Level.ToString();
        XPAtual = Mathf.RoundToInt(XPAtual - XPRequirido);
        UsuarioAtual.usuarioMonstro.XP = XPAtual;
        XPRequirido = CalcularXPRequirido();
        _sqlConnection.GravarNivel(Level, UsuarioAtual.usuarioMonstro);
    }

    private int CalcularXPRequirido()
    {
        int resolverXPRequirido = 0;
        for (int cicloLevels = 0; cicloLevels <= Level; cicloLevels++)
        {
            resolverXPRequirido += (int)Mathf.Floor(cicloLevels + multiplicadorAdicao * Mathf.Pow(multiplicadorPotencia, cicloLevels / multiplicadorDivisao));
        }

        return resolverXPRequirido / 4;
    }

    public void MonstroMordeu()
    {
        animatorMonstro.SetTrigger("clicouMonstro");
    }

}
