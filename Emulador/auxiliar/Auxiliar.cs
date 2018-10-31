using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador.auxiliar {
    static class Auxiliar {

        public static void popVetorLarguraDados(int[] vetor) {
            //Faz a "fila" do buffer andar sempre que um byte é enviado para o barramento de dados
            for (int i = 0; i < Constantes.larguraBarramentoDeDados; i++) {
                for (int j = 0; j < vetor.Length - 1; j++) {
                    vetor[j] = vetor[j + 1];
                }
                vetor[vetor.Length - 1] = 0;
            }
        }

        public static void insereZerosNoInicio(int[] vetor, int qtdZeros) {
            for (int i = 0; i < vetor.Length - qtdZeros; i++) {
                vetor[vetor.Length - 1 - i] = vetor[vetor.Length - 1 - qtdZeros - i];
            }
            for (int i = 0; i < qtdZeros; i++) {
                vetor[i] = 0;
            }
        }

        public static void popIndividial(int[] vetor) {
            //Faz a "fila" do buffer andar sempre que um byte é enviado para o barramento de dados
            for (int i = 0; i < 1; i++) {
                for (int j = 0; j < vetor.Length - 1; j++) {
                    vetor[j] = vetor[j + 1];
                }
                vetor[vetor.Length - 1] = 0;
            }
        }

        public static int tamanhoInstrucao(int codigo) {
            //retorna quantidade de bytes ocupada pela instrução na memória
            int tp = Constantes.tamanhoPalavra;
            int lb = Constantes.larguraBarramentoDeEndereco;

            if (codigo == 1) {
                return 3 * tp;
            }
            if (codigo == 2) {
                return 3 * tp;
            }
            if (codigo == 3) {
                return 2 * tp + lb;
            }
            if (codigo == 4) {
                return 2 * tp + lb;
            }
            if (codigo == 5) {
                return 3 * tp;
            }
            if (codigo == 6) {
                return 3 * tp;
            }
            if (codigo == 7) {
                return 2 * tp + lb;
            }
            if (codigo == 8) {
                return 2 * tp + lb;
            }
            if (codigo == 9) {
                return 2 * tp;
            }
            if (codigo == 10) {
                return tp + lb;
            }
            if (codigo == 11) {
                return 4 * tp;
            }
            if (codigo == 12) {
                return 4 * tp;
            }
            if (codigo == 13) {
                return 4 * tp;
            }
            if (codigo == 14) {
                return 4 * tp;
            }
            if (codigo == 15) {
                return 3 * tp + lb;
            }
            if (codigo == 16) {
                return 3 * tp + lb;
            }
            if (codigo == 17) {
                return 2 * tp + 2 * lb;
            }
            if (codigo == 18) {
                return 3 * tp + lb;
            }
            if (codigo == 19) {
                return 3 * tp + lb;
            }
            if (codigo == 20) {
                return 3 * tp + lb;
            }
            if (codigo == 21) {
                return 3 * tp + lb;
            }
            if (codigo == 22) {
                return 3 * tp + lb;
            }
            if (codigo == 23) {
                return 3 * tp + lb;
            }
            if (codigo == 24) {
                return 2 * tp + 2 * lb;
            }
            if (codigo == 25) {
                return 2 * tp + 2 * lb;
            }
            if (codigo == 26) {
                return tp + 3 * lb;
            }
            if (codigo == 27) {
                return 2 * tp + 2 * lb;
            }
            if (codigo == 28) {
                return 2 * tp + 2 * lb;
            }
            return 0;
        }

        public static void imprimeBarramentos(barramentos.BarramentoDeDados barramentoDeDados,
            barramentos.BarramentoDeEnderecos barramentodeEnderecos,
            barramentos.BarramentoDeControle barramentoDeControle) {
            Console.WriteLine("Barramento de controle:");
            barramentoDeControle.imprimeFila();
            Console.WriteLine("Barramento de endereço:");
            barramentodeEnderecos.imprimeFila();
            Console.WriteLine("Barramento de dados:");
            barramentoDeDados.imprimeFila();
        }

    }
}
