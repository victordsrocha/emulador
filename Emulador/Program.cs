using System;

namespace Emulador {
    class Program {
        static void Main(string[] args) {

            var ES = new EntradaSaida();
            var teste = ES.executarParser();
            Console.WriteLine("Grupo 2 da linha 6: " + teste[6][2]);
            Console.ReadLine();
        }
    }
}
