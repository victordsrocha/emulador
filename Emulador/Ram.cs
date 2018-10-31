using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador {
    class Ram {

        public int[] memoria;
        //criei um vetor para receber o endereço do barramento de endereços
        //nao sei como a ram "reconhece e processa" a informação vinda do barramento de endereços
        public int[] vetorLeitorDeEndereco;
        public long enderecoLido;

        //booleano para armazenar sinal de controle, vai indicar quais operacoes poderao
        //ser feitas e sinalizar erro caso não sejam respeitadas
        public int ArmazenadorDeSinalDeControle; // será armzenado 0 ou 1
        public bool somenteLeitura;
        


        public Ram() {
            this.memoria = new int[Constantes.tamanhoRam];
            vetorLeitorDeEndereco = new int[Constantes.larguraBarramentoDeEndereco];
            somenteLeitura = true;
        }

        //'decoder' para ler informação que chega pelo barramento de endereços, armazena em enderecoLido
        public void leituraEndereco() {
            long somatorio = 0;
            for (int i = 0; i < vetorLeitorDeEndereco.Length; i++) {
                somatorio += (long)(Math.Pow(256, i) * vetorLeitorDeEndereco[vetorLeitorDeEndereco.Length - 1 - i]);
            }
            enderecoLido = somatorio;
        }

        //'decoder' para ler informação do barramento de controle
        public void leituraSinalDeControle() {
            if (ArmazenadorDeSinalDeControle == 0) {
                somenteLeitura = false;
            }
            if (ArmazenadorDeSinalDeControle == 1) {
                somenteLeitura = true;
            }
        }

        public void imprimeMemoriaRam() {
            Console.Write("[ ");
            string s = "";
            Console.BackgroundColor = ConsoleColor.DarkRed;
            for (int i = 0; i < Constantes.taxaDeTransferência-1; i++) {
                s += memoria[i] + " ";
            }
            s += memoria[Constantes.taxaDeTransferência - 1];
            Console.Write(s);
            s = " ";
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = Constantes.taxaDeTransferência; i < this.memoria.Length; i++) {
                s += memoria[i] + " ";
            }
            s += "]";
            Console.WriteLine(s);
        }
    }
}
