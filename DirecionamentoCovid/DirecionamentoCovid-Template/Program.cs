using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DirecionamentoCovid_Template
{
	class Program
	{
		static void Main(string[] args)
		{
			FilaSenha FSenhaPreferencial = new FilaSenha();
			FilaSenha FSenhaComum = new FilaSenha();
			CovidPositivoSintomatico FSintomatico = new CovidPositivoSintomatico();
			LeitosInternacao LInternacao = new LeitosInternacao { Leitos = new Pessoa[10] };
			Fila Negativos = new Fila();
			Fila Assintomaticos = new Fila();
			int senhaFilaEspera = 1, count = 0;

			int op;

			do
			{
				Console.Clear();
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("<<<HOSPITAL DE CAMPANHA DR. F.PAPINI>>>\n");
				Console.WriteLine("     <<<Diretor Dr. F. Pestana>>>\n");
				Console.ResetColor();
				Console.WriteLine("+-----------------------------------+");

				Console.BackgroundColor = ConsoleColor.DarkGreen;
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("|           >>> MENU <<<            |\n"
					+ "|1 - Retirar Senha                  |\n"
					+ "|2 - Realizar Triagem               |\n"
					+ "|3 - Pacientes Aguardando Internação|\n"
					+ "|4 - Internar paciente              |\n"
					+ "|5 - Dar Alta                       |\n"
					+ "|6 - Abrir Arquivo                  |\n"
					+ "|7 - Sair                           |\n");
				Console.ResetColor();
				Console.WriteLine("+-----------------------------------+");
				Console.Write("\nInforme a opcao desejada: ");

				if (int.TryParse(Console.ReadLine(), out op))

					switch (op)
					{
						case 1:
							RetirarSenha(FSenhaComum, FSenhaPreferencial, ref senhaFilaEspera);
							break;
						case 2:
							RealizarTriagem(FSintomatico, FSenhaComum, FSenhaPreferencial, ref count, Negativos, Assintomaticos);

							Console.ReadKey();
							break;
						case 3:
							FSintomatico.Print();
							Console.ReadKey();
							break;
						case 4:
							TransferirParaInternacao(FSintomatico, LInternacao);
							break;
						case 5:
							RealizarAlta(LInternacao);
							break;
						case 6:
							Console.WriteLine("Arquivos:\n1- Assintomatico\n2- Sintomatico\n3- Negativo\n4- Leitos\n0- Sair\n>>>");
							int op2 = -1;
							do
							{
								try
								{
									op2 = int.Parse(Console.ReadLine());

								}
								catch (Exception)
								{
									Console.WriteLine("Digite um numero de acordo com o MENU de arquivos");

								}
								if (op2 == 1)
								{
									op2 = ImprimeCSV("Arquivo Assintomatico", "\nAssintomaticos");
								}
								else if (op2 == 2)
								{
									op2 = ImprimeCSV("Arquivo Sintomatico", "\nSintomatico");
								}
								else if (op2 == 3)
								{
									op2 = ImprimeCSV("Arquivo Negativos", "\nNegativo");
								}
								else if (op2 == 4)
								{
									op2 = ImprimeCSV("Arquivo Leitos", "\nLeitos");
								}
							}
							while (op2 != 0);
							break;
						case 7:
							break;
						default:
							Console.WriteLine("Opção Invalida!!!");
							Console.ReadKey();
							break;
					}
			} while (op != 7);


		}

		static public void RetirarSenha(FilaSenha fc, FilaSenha fp, ref int s)
		{
			Console.WriteLine("RETIRE SUA SENHA");
			Console.WriteLine("1- Atendimento Preferêncial(60 anos ou mais)");
			Console.WriteLine("2- Atendimento Comum");
			int tipoAtendimento;
			bool controle;
			do
			{
				controle = false;
				if (int.TryParse(Console.ReadLine(), out tipoAtendimento))
				{
					controle = true;
					if (tipoAtendimento < 1 || tipoAtendimento > 2) Console.Write("\nInforme um numero válido do menu!!!\n");
				}
				else Console.Write("Você não digitou um numero, informe um numero válido do menu!!!\n");

			}
			while (controle == false || (tipoAtendimento < 1 || tipoAtendimento > 2));

			SenhaEmEspera Senha = new SenhaEmEspera { Senha = s };
			if (tipoAtendimento == 1)
			{
				fp.Push(Senha);
			}
			else
			{
				fc.Push(Senha);
			}
			Console.WriteLine("Sua senha é: " + s);
			Console.ReadKey();
			s++;
		}

		static public void RealizarTriagem(CovidPositivoSintomatico fSintomatico, FilaSenha fc, FilaSenha fp, ref int count, Fila Negativo, Fila Assintomatico)
		{

			int senhaAtual = ChamarSenha(fc, fp, ref count);
			if (senhaAtual != -1)
			{
				Pessoa pessoa = new Pessoa();
				Console.WriteLine("Chamar senha: " + senhaAtual);
				pessoa = Triagem.Cadastramento();
				pessoa.ResultadoExame = Triagem.Exame();

				if (pessoa.ResultadoExame == 2 || pessoa.ResultadoExame == 3)
				{
					Console.WriteLine("Arquivado");

					if (pessoa.ResultadoExame == 2)
					{
						Assintomatico.Push(pessoa);
						WriteFileCSV(Assintomatico, null, "Assintomaticos", "Arquivo Assintomatico");
					}
					else
					{
						Negativo.Push(pessoa);
						WriteFileCSV(Negativo, null, "Negativos", "Arquivo Negativos");
					}
				}

				else
				{
					Console.WriteLine("Arquivado");
					//fSintomatico.Push(pessoa);
					//WriteFileCSV(fSintomatico, null, "Cabeçalho", "Nome Arquivo");
					pessoa = ClassificarPrioridadePaciente(pessoa);
					TransferirParaFilaTratamento(fSintomatico, pessoa);
					WriteFileCSV(fSintomatico, null, "Sintomaticos", "Arquivo Sintomatico");
				}

				Console.WriteLine(pessoa.ToString());

				Console.ReadKey();



			}
			else
				Console.WriteLine("As filas de espera estam vazias!");
		}

		static public int ChamarSenha(FilaSenha fc, FilaSenha fp, ref int count)
		{
			int senhaAtual = 0;

			if (count < 2) //tratamento fila preferencial
			{
				if (!fp.Vazio())
				{
					senhaAtual = fp.Head.Senha;
					fp.Pop();
					count++;
					if (fc.Vazio()) count = 0;

					return senhaAtual;
				}
				else
					count = 2;
			}

			if (count == 2) //tratamento da fila comum
			{
				if (!fc.Vazio())
				{
					senhaAtual = fc.Head.Senha;
					fc.Pop();
					count = 0;

					return senhaAtual;
				}
				else
					count = 0;
			}

			return -1; //Se as duas filas estam vazias
		}

		static public Pessoa ClassificarPrioridadePaciente(Pessoa pessoa)
		{
			if (pessoa.TempoDeSintomas <= 9 && pessoa.TempoDeSintomas >= 0)
			{
				pessoa.PrioridadeInternacao = "Alta";
				Console.WriteLine("PRIORIDADE ALTA");
			}

			else
			{
				pessoa.PrioridadeInternacao = "Baixa";
				Console.WriteLine("PRIORIDADE BAIXA");
			}

			return pessoa;
		}

		static public void TransferirParaFilaTratamento(CovidPositivoSintomatico FSintomatico, Pessoa pessoa)
		{
			if (pessoa.Idade >= 60 && pessoa.Comorbidade && pessoa.PrioridadeInternacao == "Alta")
			{
				FSintomatico.Push(pessoa);
				Console.WriteLine("TRANSFERIDA RISCO");
			}
			else
			{

				Console.WriteLine("TRANSFERIDA SEM RISCO");
			}


		}

		static public void TransferirParaInternacao(CovidPositivoSintomatico FSintomatico, LeitosInternacao LInternacao)
		{
			int numeroLeito;
			bool taVazio = false;


			if (FSintomatico.Head != null)
			{
				Console.WriteLine("\n>>>>Proximo da fila de internação<<<<\n");
				Console.WriteLine(FSintomatico.Head);
				Console.Write("\nDeseja internar o paciente?");
				string resposta = Console.ReadLine();
				if (resposta.ToLower() == "s")
				{

					Console.Write("Leitos vazios: ");

					for (int i = 0; i < LInternacao.Leitos.Length; i++)
					{
						if (LInternacao.Leitos[i] == null)
						{
							taVazio = true;
							Console.Write("(" + i + ")" + "\t");
						}
					}

					if (taVazio)
					{
						Console.Write("\nInforme qual o leito o paciente será internado: ");

						numeroLeito = int.Parse(Console.ReadLine());

						LInternacao.Leitos[numeroLeito] = FSintomatico.Head;
						FSintomatico.Pop();

						int numEspaco = 0;
						for (int i = 0; i < LInternacao.Leitos.Length; i++)
						{
							if (LInternacao.Leitos[i] != null) numEspaco++;
						}
						Pessoa[] vet = new Pessoa[numEspaco];
						int j = 0;
						for (int i = 0; i < LInternacao.Leitos.Length; i++)
						{
							if (LInternacao.Leitos[i] != null)
							{
								vet[j] = LInternacao.Leitos[i];
								j++;
							}
						}
						WriteFileCSV(null, vet, "Leitos", "Arquivo Leitos");

						Console.WriteLine("\n>>>>>FILA DE INTERNAÇÃO");
						FSintomatico.Print();
					}
					else
						Console.WriteLine("Todos o leitos estão ocupados!!!!");
				}
			}
			else
				Console.WriteLine("Não tem pacientes esperando internação!!!");

			Console.ReadKey();
		}

		static public void RealizarAlta(LeitosInternacao LInternacao)
		{
			bool taVazio = true;

			for (int i = 0; i < LInternacao.Leitos.Length; i++)
			{
				if (LInternacao.Leitos[i] != null)
				{
					Console.WriteLine("Paciente - Nome: " + LInternacao.Leitos[i].Nome + " CPF: " + LInternacao.Leitos[i].Cpf);
					Console.Write("\nDeseja dar alta ao paciente? S/N: ");
					string resposta = Console.ReadLine();
					if (resposta.ToLower() == "s")
					{
						//gravar paciente no arquivo
						Console.WriteLine("Liberou paciente");
						LInternacao.Leitos[i] = null;
					}
					taVazio = false;
				}
			}

			if (taVazio)
				Console.WriteLine("Não tem pacientes nos leitos!!!");


			Console.ReadKey();
		}
		static int ImprimeCSV(string file_name, string header)
		{
			string linha = "";
			string[] linhaseparada = null;
			string file_path = "C:\\Users\\55169\\Desktop\\DirecionamentoCovid\\arquivoscovid\\" + file_name + ".csv";
			StreamReader reader = null;
			try
			{
				reader = new StreamReader(file_path, Encoding.UTF8, true);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("Arquivo não encontrado!!!\n");
				return 0;
			}
			Console.WriteLine(header);

			while (true)
			{
				linha = reader.ReadLine();
				if (linha == null) break;
				linhaseparada = linha.Split(',');
				int i = 0;
				while (true)
				{
					try
					{
						Console.WriteLine(linhaseparada[i] + "\t" + linhaseparada[1]);
						break;
					}
					catch (IndexOutOfRangeException)
					{
						break;
					}
					i++;
				}

			}
			Console.WriteLine("Pressione Enter para continuar!");
			Console.ReadKey();
			return 0;
		}

		static void WriteFileCSV(Fila f, Pessoa[] vet, string header, string file_name)
		{
			//caso for inserir um vetor e não uma fila usa null como parâmetro ou ao contrario!!!
			//WriteFileCSV(null, vetor, "Cabeçalho", "Nome Arquivo");
			//WriteFileCSV(fila, null, "Cabeçalho", "Nome Arquivo");
			try
			{
				string file_path = "C:\\Users\\55169\\Desktop\\DirecionamentoCovid\\arquivoscovid\\" + file_name + ".csv";

				StreamWriter arq = File.CreateText(file_path); //criando arquivo


				arq.WriteLine(header);
				arq.WriteLine("Nome,Idade");
				if (f != null) arq.WriteLine(f.Escreve());
				if (vet != null) arq.WriteLine(EscreveVet(vet));
				arq.Close();
			}
			catch (System.IO.DirectoryNotFoundException ex)
			{
				Console.WriteLine("ERRO!!!" + ex.Message);

			}
			catch (System.IO.IOException ex)
			{
				Console.WriteLine("ERRO!!!" + ex.Message);

			}
		}

		static string EscreveVet(Pessoa[] vet)
		{
			string text = "";
			foreach (Pessoa p in vet)
			{
				text += p.Nome + "," + p.Idade + ",\n";
			}
			return text;
		}








	}
}
