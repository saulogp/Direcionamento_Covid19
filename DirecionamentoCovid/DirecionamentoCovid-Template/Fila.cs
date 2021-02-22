using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecionamentoCovid_Template
{
     class Fila
    {
        public Pessoa Head { get; set; }
        public Pessoa Tail { get; set; }
        public Pessoa Pessoa { get; set; }

        private bool Vazio()
        {
            if ((Head == null) && (Tail == null))
                return true;
            return false;
        }

        public void Push(Pessoa aux)
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
                    Console.WriteLine("Fila agora está vazia!");
                }
            }
        }

        public void Print()
        {
            if (Vazio())
            {
                Console.WriteLine("Impossível Imprimir! Fila Vazia!");
            }
            else
            {
                Pessoa aux = Head;
                Console.WriteLine("\n>>>Impressão da fila<<<\n");
                do
                {
                    Console.WriteLine(aux.ToString());
                    aux = aux.Proximo;
                } while (aux != null);
            }
        }

        public string Escreve()
        {
            string text = "";
            if (Vazio())
            {
                Console.WriteLine("Impossível Imprimir! Fila Vazia!");
            }
            else
            {
                Pessoa aux = Head;
                do
                {
                    text += aux.Nome + "," + aux.Idade + ",\n";
                    aux = aux.Proximo;
                } while (aux != null);
            }
            return text;
        }
   
    
    
    
    
    }
}
