using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DTOs
{
    public class CURSO
    {
        public int Codigo { get; set; }

        public string NomeCurso { get; set; }

        public string DescricaoCurso { get; set; }

        public string AreaCurso { get; set; }

        public int Minutos { get; set; }

        public DateTime DataTermino { get; set; }

        public int Usuario { get; set; }
    }
}
