using System;
using System.Collections.Generic;

namespace Emulador {

    static class Constantes {
        public const int tamanhoPalavra = 32 / 8; //Tamanho da palavra em bits [16, 32 ou 64]
        public const int larguraBarramentoDeDados = 16 / 8;
        public const int larguraBarramentoDeEndereco = 8 / 8; //Largura do barramento em bits [8, 16 ou 32]
        public const int tamanhoBuffer = 256; //Tamanho do buffer de entrada/saída em bytes [64, 128 ou 256]
        //Tamanho da RAM em bytes [128, 256 ou 512]
    }

    class Program {
        static void Main(string[] args) {

            var moduloES = new EntradaSaida();

            moduloES.preencheBuffer();

            moduloES.imprimeBuffer();

            


            Console.ReadLine();
        }
    }
}