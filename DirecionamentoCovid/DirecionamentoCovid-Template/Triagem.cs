using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecionamentoCovid_Template
{
	class Triagem
	{
		public Pessoa DadosPessoas { get; set; }

		static public Pessoa Cadastramento()
		{
			bool teste = false;
			DateTime dataNascimento;
			Pessoa pessoa = new Pessoa();
			Console.Write("**Cadastre o paciente:**\n");
			Console.Write("Nome do paciente: ");
			pessoa.Nome = TratamentoString();	
			do
			{
				
				Console.Write("Data de nascimento do paciente(dd/mm/aaaa): ");
				if (DateTime.TryParse(Console.ReadLine(), out dataNascimento))
                {
					teste = true;
					pessoa.DataNascimento = dataNascimento;
                }
				pessoa.Idade = ConverteIdade(pessoa.DataNascimento);

				

			}
			while (pessoa.Idade < 0 || teste == false);

			Console.Write("Digite Sexo M (Masculino) ou F (Feminino):  ");
			do
			{
				pessoa.Sexo = TratamentoString();
				if (pessoa.Sexo.ToLower() != "m" && pessoa.Sexo.ToLower() != "f") Console.WriteLine("Digite M- para Masculino ou F - Feminino");
			}
			while (pessoa.Sexo.ToLower() != "m" && pessoa.Sexo.ToLower() != "f");
						
			Console.Write("CPF: ");
			pessoa.Cpf = TratamentoString();
			Console.Write("Carteira de vacinação: ");
			pessoa.CarteiraVacin = TratamentoInt();
			Console.Write("Telefone: ");
			pessoa.Telefone = TratamentoString();
			Console.Write("Tempo de sintomas: ");
			pessoa.TempoDeSintomas = TratamentoInt();
			Console.Write("Tem comorbidade?(s/n): ");
			string comorbidade;
			do
			{
				comorbidade = TratamentoString();
				if (comorbidade.ToLower() == "s")
					pessoa.Comorbidade = true;
				else if (comorbidade.ToLower() == "n")
				{
					pessoa.Comorbidade = false;
				}
                if (comorbidade.ToLower() != "s" && comorbidade.ToLower() != "n") Console.WriteLine("Digite S- para sim ou N - não"); 
			}
			while (comorbidade.ToLower() != "s" && comorbidade.ToLower() != "n");
			return pessoa;

		}

       static public int Exame()
        {
            int resultado;
            Console.WriteLine("Coletando amostras para exame!!");
            Console.WriteLine("Processando exame... aguarde!");
            Console.WriteLine("Menu para resultados de exame: ");
            do
            {
                Console.WriteLine("1 - Positivo Sintomatico ");
                Console.WriteLine("2 - Positivo Assintomatico");
                Console.WriteLine("3 - Negativo");
				resultado = TratamentoInt(); 
                if ((resultado < 1) || (resultado > 3)) Console.WriteLine("Digite de 1 a 3 para inserir possiveis resultados do exame: ");
			} while ((resultado < 1) || (resultado > 3));

            if (resultado == 1)
                return 1;
            else if (resultado == 2)
                return 2;
            else
                return 3;

        }

		static public int ConverteIdade(DateTime DataNascimento)
		{
			int dataNascimento = int.Parse(DataNascimento.ToString("yyyy"));
			int dataAtual = int.Parse(DateTime.Today.ToString("yyyy"));
			int idade = dataAtual - dataNascimento;
			return idade;
		}

		static public string TratamentoString()
		{
			string Stratamento;
			do
			{
				Stratamento = Console.ReadLine();
				if (Stratamento == "") Console.WriteLine("O campo não pode ser vazio!!! Digite novamente");
			}
			while (Stratamento == "");

			return Stratamento;
		}

		static public int TratamentoInt()
		{
			bool teste = false;
			int Itratamento;
			do
			{
				if (int.TryParse(Console.ReadLine(), out Itratamento))
				{
					teste = true;
				}
				else Console.WriteLine("O campo só pode receber numeros!!! Digite novamente");
			}
			while (teste == false);

			return Itratamento;
		}
	}
}
