using System;
using System.Collections.Generic;

namespace Emulador {

    static class Constantes {
        public const int tamanhoPalavra = 32 / 8;
        public const int larguraBarramentoDeDados = 16 / 8;
        public const int larguraBarramentoDeEndereco = 8 / 8;
    }

    class Program {
        static void Main(string[] args) {

            var ES = new EntradaSaida();
            var listaInstrucoes = ES.executarParser();

            int j = 0;
            foreach (var item in listaInstrucoes) {
                for (int i = 0; i < listaInstrucoes[j].Count; i++) {
                    Console.WriteLine("Linha " + j + " ,Grupo " + i + ": " + listaInstrucoes[j][i]);
                }
                j++;
            }

            Encoder.codificaLiteraisEmByteArray(listaInstrucoes);
            Encoder.insereValorDeInstrucao(listaInstrucoes);
            Encoder.codificaEnderecos(listaInstrucoes);
            Encoder.codificaRegistradores(listaInstrucoes);
            

            Console.WriteLine("\n\n");
            j = 0;
            foreach (var item in listaInstrucoes) {
                for (int i = 0; i < listaInstrucoes[j].Count; i++) {
                    Console.WriteLine("Linha " + j + " ,Grupo " + i + ": " + listaInstrucoes[j][i]);
                }
                j++;
            }

            string nome = "victor";

            Console.WriteLine("\n\n");
            for (int i = 0; i < nome.Length; i++) {
                Console.WriteLine("Caractere de posição " + i + ": " + nome[i]);
            }

            nome = nome.Remove(0, 2);

            Console.WriteLine("\n\n");
            for (int i = 0; i < nome.Length; i++) {
                Console.WriteLine("Caractere de posição " + i + ": " + nome[i]);
            }

            Console.WriteLine("\n\nTeste: 0x00F4A = " + Encoder.hexaEndParaDecInt("0x00F4A"));


            


            Console.ReadLine();
        }
    }
}