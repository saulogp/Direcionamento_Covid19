using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecionamentoCovid_Template
{
    class SenhaEmEspera
    {
        public int Senha { set; get; }
        public SenhaEmEspera Proximo { set; get; }

        public override string ToString()
        {
            return "Senha é:"+Senha;
        }
    }
}
