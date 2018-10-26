using System;

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

            Console.WriteLine("\n\n");
            j = 0;
            foreach (var item in listaInstrucoes) {
                for (int i = 0; i < listaInstrucoes[j].Count; i++) {
                    Console.WriteLine("Linha " + j + " ,Grupo " + i + ": " + listaInstrucoes[j][i]);
                }
                j++;
            }

            Console.ReadLine();
        }
    }
}
