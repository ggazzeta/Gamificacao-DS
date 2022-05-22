using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    public int Level;
    public int XPAtual;
    public float XPRequirido;
    public SQLCon _sqlConnection;

    [Header("UI")]
    public TextMeshProUGUI txtLevel;
    public Image XPBar;
    public TextMeshProUGUI textoPorcentagem;

    [Header("Formula")]
    [Range(1f,300f)]
    public float multiplicadorAdicao = 300;
    [Range(2f, 4f)]
    public float multiplicadorPotencia = 2;
    [Range(7f, 14f)]
    public float multiplicadorDivisao = 7;


    // Start is called before the first frame update
    void Start()
    {
        Level = UsuarioAtual.usuarioLogado.Nivel;
        txtLevel.text = Level.ToString();
        XPAtual = UsuarioAtual.usuarioLogado.XP;
        XPRequirido = CalcularXPRequirido();


        if (XPAtual > XPRequirido)
        {
            GanharLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var floatProximoNivel = XPAtual / XPRequirido;
        var porcentagemProximoNivel = (floatProximoNivel * 100).ToString("N0");
        XPBar.fillAmount = floatProximoNivel;
        textoPorcentagem.text = porcentagemProximoNivel + "%";

    }

    public async Task GanharXPTime(int xpGanho, int timeXP)
    {
        XPAtual += xpGanho;
        UsuarioAtual.usuarioLogado.XP = XPAtual;

        if (XPAtual > XPRequirido)
        {
            GanharLevel();
        }

        await _sqlConnection.GravarXPTime(xpGanho, timeXP);
    }

    public async Task GanharXP(int xpGanho)
    {
        XPAtual += xpGanho;
        UsuarioAtual.usuarioLogado.XP = XPAtual;

        if (XPAtual > XPRequirido)
        {
            GanharLevel();
        }

       await _sqlConnection.GravarXP(XPAtual);
    }

    public void GanharLevel()
    {
        Level++;
        UsuarioAtual.usuarioLogado.Nivel = Level;
        txtLevel.text = Level.ToString();
        XPAtual = Mathf.RoundToInt(XPAtual - XPRequirido);
        UsuarioAtual.usuarioLogado.XP = XPAtual;
        XPRequirido = CalcularXPRequirido();
        _sqlConnection.GravarNivel(Level);
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
}
