using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecionamentoCovid_Template
{
    class FilaSenha 
    {
        public SenhaEmEspera Head { set; get; }
        public SenhaEmEspera Tail { set; get; }

		public bool Vazio()
        {
            if ((Head == null) && (Tail == null))
                return true;
            return false;
        }

        public void Push(SenhaEmEspera aux)
        {
            if (Vazio())
            {
                Head = aux;
                Tail = aux;
            }
            else
            {
                Tail.Proximo = aux;
                Tail = aux;
            }
            Console.WriteLine("Elemento Inserido com sucesso!!!");

        }

        public void Pop()
        {
            if (Vazio())
            {
                Console.WriteLine("Impossível Remover! Fila Vazia!");
            }
            else
            {
                Head = Head.Proximo;
                Console.WriteLine("Elemento Removido com Sucesso!");
                if (Head == null)
                {
                    Tail = null;
                    Console.WriteLine("Fila de Senha agora está vazia!");
                }
            }
        }

        public void Print()
        {
            if (Vazio())
            {
                Console.WriteLine("Impossível Imprimir! Fila de Senha Vazia!");
            }
            else
            {
                SenhaEmEspera aux = Head;
                Console.WriteLine("\n>>>Impressão da fila<<<\n");
                do
                {
                    Console.WriteLine(aux.ToString());
                    aux = aux.Proximo;
                } while (aux != null);
            }
        }

       

    }
}
