using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecionamentoCovid_Template
{
    class Pessoa
    {
		public string Nome { get; set; }
		public DateTime DataNascimento { get; set; }
		public int Idade { get; set; }
		public string Sexo { get; set; }
		public string Cpf { get; set; }
		public int CarteiraVacin { get; set; }
		public string Telefone { get; set; }
		public int TempoDeSintomas { get; set; }
		public bool Comorbidade { get; set; }
		public int ResultadoExame { get; set; }
		public Pessoa Proximo { get; set; }
		public DateTime DataEntrada { get; set; }
		public string PrioridadeInternacao { get; set; }

		public Pessoa() { }
		
		public override string ToString()
		{
			string resultadoExame;
			if (ResultadoExame == 1)
			{
				resultadoExame = "Positivo Sintomatico";
			}
			else
			if (ResultadoExame == 2)
			{
				resultadoExame = "Positivo Assintomatico";
			}
			else resultadoExame = "Negativo";
			// E se ainda não fez exame?



			return ">>>>>>> Dados do Paciente <<<<<<\nNome: " + Nome +
				"\nData de Nascimento: " + DataNascimento + "\nIdade: " + Idade + "\nSexo: " + Sexo +
				"\nCPF: " + Cpf + "\nCarteira de Vacinação: " + CarteiraVacin +
				"\nComorbidade: " + Comorbidade + "\nTempo de Sintomas: " + TempoDeSintomas +
				"\nResultado do Exame: "+ResultadoExame + " = " + resultadoExame;
		}

		

	}
}
