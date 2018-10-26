using System;

namespace Emulador {
    class Program {
        static void Main(string[] args) {

            var ES = new EntradaSaida();
            ES.lerArquivoDeEntrada();
            Console.ReadLine();
        }
    }
}
