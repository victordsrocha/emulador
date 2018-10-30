using System;
using System.Collections.Generic;

namespace Emulador {

    static class Constantes {
        public const int tamanhoPalavra = 32 / 8; //Tamanho da palavra em bits [16, 32 ou 64]
        public const int larguraBarramentoDeDados = 16 / 8;
        public const int larguraBarramentoDeEndereco = 8 / 8; //Largura do barramento em bits [8, 16 ou 32]
        public const int tamanhoBuffer = 128; //Tamanho do buffer de entrada/saída em bytes [64, 128 ou 256]
        public const int tamanhoRam = 256; //Tamanho da RAM em bytes [128, 256 ou 512]
        public const int taxaDeTransferência = 50;//bytes por segundo
        public const int tamanhoOffset = taxaDeTransferência;//tamanho do offset em bytes
    }

    class Program {
        static void Main(string[] args) {

            var moduloES = new EntradaSaida();
            var ram = new Ram();
            var cpu = new CPU();
            var barramentoDeControle = new barramentos.BarramentoDeControle(1);
            var barramentoDeDados = new barramentos.BarramentoDeDados(Constantes.larguraBarramentoDeDados);
            var barramentodeEnderecos = new barramentos.BarramentoDeEnderecos(Constantes.larguraBarramentoDeEndereco);

            moduloES.preencheBuffer();
            moduloES.imprimeBuffer();

            Console.WriteLine("\n\n");
            ram.imprimeMemoriaRam();

            Console.WriteLine("\n\n");
            Console.WriteLine("Quantidade de larguras de barramento de dados que cabem em uma rajada de levando " +
                "somente instruções inteiras e em ordem: "
                + Constantes.taxaDeTransferência +" bytes: " + moduloES.quantidadeBytesParaRajada());

            Console.WriteLine("\n\nquantidade de instruções da rajada = " + moduloES.quantidadeInstrucoesParaRajada());
            
            Console.WriteLine("\n\n");
            Console.WriteLine("\nApós Rajada:\n");

            moduloES.rajada(barramentoDeDados, barramentodeEnderecos, barramentoDeControle, ram, cpu);

            moduloES.imprimeBuffer();
            Console.WriteLine("\n\n");
            ram.imprimeMemoriaRam();

            Console.WriteLine("\n\n");
            cpu.imprimeRegistradores();

            Console.WriteLine("\nLista de interrupcoes\n");
            cpu.imprimeInterrupcoes();

            
            Console.WriteLine("\n\nExecução da rajada:\n\n");
            cpu.executaTodasInterrupcoes(moduloES, barramentoDeDados, barramentodeEnderecos,
                barramentoDeControle, ram);


            Console.WriteLine("\n\n");
            cpu.imprimeRegistradores();

            Console.WriteLine("\n\n");
            moduloES.imprimeBuffer();

            Console.WriteLine("\n\n");
            ram.imprimeMemoriaRam();
            
            Console.ReadLine();

        }
    }
}