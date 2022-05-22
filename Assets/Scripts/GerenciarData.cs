using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GerenciarData : MonoBehaviour
{

    public GameObject datePicker;
    private DatePickerControl data;
    public TextMeshProUGUI textoData;

    // Start is called before the first frame update
    void Start()
    {
        datePicker.SetActive(false);
        data = datePicker.GetComponentInChildren<DatePickerControl>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AbrirDatePicker()
    {
        datePicker.SetActive(true);
    }

    public void FechaDatePicker()
    {
        textoData.text = data.fecha.Date.ToString("dd/MM/yyyy");
        datePicker.SetActive(false);
    }
}
