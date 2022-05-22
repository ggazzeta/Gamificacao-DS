using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DTOs
{
    public class TAREFASPROPOSTA
    {
        public int Codigo { get; set; }

        public string NomeTarefa { get; set; }

        public string DescricaoTarefa { get; set; }

        public int Aceita { get; set; }

        public string UsuarioTarefa { get; set; }

        public int? Time { get; set; }
    }
}
