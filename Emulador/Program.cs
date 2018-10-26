using System;
using System.Collections.Generic;

namespace Emulador {
    class Program {
        static void Main(string[] args) {

            var ES = new EntradaSaida();
            var listaInstrucoes = ES.executarParser();

            int j = 0;
            foreach (var item in listaInstrucoes) {
                for (int i = 0; i < listaInstrucoes[j].Count; i++) {
                    Console.WriteLine("Linha "+j +" ,Grupo " + i + ": " + listaInstrucoes[j][i]);
                }
                j++;
            }
            

            Encoder.insereValorDeInstrucao(listaInstrucoes);
            Encoder.codificaRegistradores(listaInstrucoes);
            Encoder.codificaEnderecos(listaInstrucoes);

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
