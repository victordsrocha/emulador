using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Emulador {
    static class Encoder {

        public static void insereValorDeInstrucao(List<List<dynamic>> entradas) {
            /* Esta função modifica a lista contendo instruções!
             * 
             * O grupo um  (contendo nome da instrução :mov, add, inc ou imul) será substuido por um 
             * número correspondente ao código da instrução.
             * 
             * Esta função é redundante visto que o código da instrução poderia facilmente ser
             * atribuído no parser. Contudo esta é uma tarefa do encoder e deve estar nesta classe.
             */

            int linha;
            string fonte;

            for (int i = 0; i < entradas.Count; i++) {
                linha = i;
                fonte = entradas[linha][0];
                insereCodigoEmGrupoUm();
            }

            void insereCodigoEmGrupoUm() {
                //instrução de código 1
                if (Regex.IsMatch(fonte, @"(add)\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(1,Constantes.tamanhoPalavra);
                }
                //instrução de código 2
                if (Regex.IsMatch(fonte, @"(add)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(2, Constantes.tamanhoPalavra);
                }
                //instrução de código 3
                if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(3, Constantes.tamanhoPalavra);
                }
                //instrução de código 4
                if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(4, Constantes.tamanhoPalavra);
                }
                //instrução de código 5
                if (Regex.IsMatch(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(5, Constantes.tamanhoPalavra);
                }
                //instrução de código 6
                if (Regex.IsMatch(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(6, Constantes.tamanhoPalavra);
                }
                //instrução de código 7
                if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(7, Constantes.tamanhoPalavra);
                }
                //instrução de código 8
                if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(8, Constantes.tamanhoPalavra);
                }
                //instrução de código 9
                if (Regex.IsMatch(fonte, @"(inc)\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(9, Constantes.tamanhoPalavra);
                }
                //instrução de código 10
                if (Regex.IsMatch(fonte, @"(inc)\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = intToVetorByte(10, Constantes.tamanhoPalavra);
                }
                //instrução de código 11
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(11, Constantes.tamanhoPalavra);
                }
                //instrução de código 13
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(13, Constantes.tamanhoPalavra);
                }
                //instrução de código 15
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(15, Constantes.tamanhoPalavra);
                }
                //instrução de código 17
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = intToVetorByte(17, Constantes.tamanhoPalavra);
                }
                //instrução de código 18
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = intToVetorByte(18, Constantes.tamanhoPalavra);
                }
                //instrução de código 19
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = intToVetorByte(19, Constantes.tamanhoPalavra);
                }
                //instrução de código 20
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(20, Constantes.tamanhoPalavra);
                }
                //instrução de código 22
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(22, Constantes.tamanhoPalavra);
                }
                //instrução de código 24
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = intToVetorByte(24, Constantes.tamanhoPalavra);
                }
                //instrução de código 26
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = intToVetorByte(26, Constantes.tamanhoPalavra);
                }
                //instrução de código 27
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = intToVetorByte(27, Constantes.tamanhoPalavra);
                }
                //instrução de código 28
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = intToVetorByte(28, Constantes.tamanhoPalavra);
                }
                //instrução de código 12
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(12, Constantes.tamanhoPalavra);
                }
                //instrução de código 14
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(14, Constantes.tamanhoPalavra);
                }
                //instrução de código 16
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(16, Constantes.tamanhoPalavra);
                }
                //instrução de código 21
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(21, Constantes.tamanhoPalavra);
                }
                //instrução de código 23
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(23, Constantes.tamanhoPalavra);
                }
                //instrução de código 25
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = intToVetorByte(25, Constantes.tamanhoPalavra);
                }
            }
        }

        public static void codificaRegistradores(List<List<dynamic>> entradas) {

            foreach (var instrucao in entradas) {

                for (int i = 1; i < instrucao.Count; i++) {
                    try {
                        string textoItemInstrucao = (string)instrucao[i];
                        if (Regex.IsMatch(textoItemInstrucao, @"([E-Z])")) {
                            Console.WriteLine("\n***** ERRO *****\n");
                            Console.WriteLine("Registrador " + instrucao[i] + " não existe");
                            Console.WriteLine("\n****************\n");

                            Console.ReadLine();
                            Environment.Exit(0);
                        }

                        if (Regex.IsMatch(textoItemInstrucao, @"A")) {
                            instrucao[i] = intToVetorByte(0,Constantes.tamanhoPalavra);
                        }
                        if (Regex.IsMatch(textoItemInstrucao, @"B")) {
                            instrucao[i] = intToVetorByte(1, Constantes.tamanhoPalavra);
                        }
                        if (Regex.IsMatch(textoItemInstrucao, @"C")) {
                            instrucao[i] = intToVetorByte(2, Constantes.tamanhoPalavra);
                        }
                        if (Regex.IsMatch(textoItemInstrucao, @"D")) {
                            instrucao[i] = intToVetorByte(3, Constantes.tamanhoPalavra);
                        }
                    }
                    catch {
                    }
                }
            }
        }

        public static long hexaEndParaDecInt(string hexa) {
            /* Recebe um string de endereço do tipo 0x00...0FF
             * Elimina os caracteres '0x' do inicio
             * Converte valor hexadecimal do endereço para decimal
             * retorna o decimal (inteiro)
             * 
             * Por enquanto é usado somente dentro de codificaEnderecos, posteriormente
             * poderia ser enviado para codificaEnderecos como função local
             */

            var dic = new Dictionary<char, long>();
            dic.Add('0', 0);
            dic.Add('1', 1);
            dic.Add('2', 2);
            dic.Add('3', 3);
            dic.Add('4', 4);
            dic.Add('5', 5);
            dic.Add('6', 6);
            dic.Add('7', 7);
            dic.Add('8', 8);
            dic.Add('9', 9);
            dic.Add('A', 10);
            dic.Add('B', 11);
            dic.Add('C', 12);
            dic.Add('D', 13);
            dic.Add('E', 14);
            dic.Add('F', 15);
            dic.Add('a', 10);
            dic.Add('b', 11);
            dic.Add('c', 12);
            dic.Add('d', 13);
            dic.Add('e', 14);
            dic.Add('f', 15);

            //remove dois primeiros caracteres do endereço
            hexa = hexa.Remove(0, 2);

            //converte para inteiro decimal
            long valor = 0;
            for (int i = 0; i < hexa.Length; i++) {
                valor += (long)(Math.Pow(16, i)) * dic[hexa[hexa.Length - (i + 1)]];
            }
            return valor;
        }

        public static void codificaEnderecos(List<List<dynamic>> entradas) {
            /* Altera todos os endereços do tipo 0x...FF em entradas para o inteiro decimal
             * correspondente. O valor decimal será armazenado como string em entradas
             * 
             * Os dados de endereços serão armazenados ocupando 'palavras', ainda que a
             * largura do barramento de endereços seja menor que a palavra
             */

            foreach (var instrucao in entradas) {

                for (int i = 1; i < instrucao.Count; i++) {
                    try {
                        string textoItemInstrucao = (string)instrucao[i];
                        if (Regex.IsMatch(textoItemInstrucao, @"(0x[A-F0-9]+)")) {

                            long end = hexaEndParaDecInt(instrucao[i]);
                            instrucao[i] = intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                        }
                    }
                    catch {
                    }
                }
            }

        }

        public static void codificaLiteraisEmByteArray(List<List<dynamic>> entradas) {

            foreach (var instrucao in entradas) {
                for (int i = 2; i < instrucao.Count; i++) {

                    try {
                        long inteiro = long.Parse(instrucao[i]);
                        var vetorByte = new auxiliar.VetorByte(Constantes.tamanhoPalavra);
                        vetorByte.vetor = literalByte(inteiro);
                        instrucao[i] = vetorByte;
                    }
                    catch {
                    }
                }
            }

            int[] literalByte(long literal) {
                /* Função interna de codificaLiteraisEmByteArray
                 * Recebe um literal positivo ou negativo
                 * retorna um array correspondente (com tamanho indicado pelo tamanho da palavra)
                 * cada posição do array corresponde à uma base de 2^8 dado que um byte tem 8 bits
                 * ex: array = [4,2,5] -> literal correspondente = 4 * (2^8)^2 + 2 * (2^8)^1 + 5 * (2^8)^0
                 * os numeros negativos ocupam a metade superior desse array
                 * ex: [128,0,0] -> literal correspondente = -1
                 * como [128,255,255] é exatamente metade e [0,0,0] já contém o valor zero
                 * então [128,0,0] contém -1
                 * [128,0,1] contém -2
                 * ...assim por diante
                 */
                int[] vetorByte = new int[Constantes.tamanhoPalavra];


                //Teste overflow!
                long maiorLiteral = (long)Math.Pow(2, (Constantes.tamanhoPalavra * 8) - 1) - 1;
                if(literal>maiorLiteral || literal < ((maiorLiteral * -1) - 1)) {
                    Console.WriteLine("\n***** ERRO *****\n");
                    Console.WriteLine("literal fora do intervalo");
                    Console.WriteLine("Literal: " + literal);
                    Console.WriteLine("Intervalo permitido para tamanho de palavra = " +
                        Constantes.tamanhoPalavra * 8 + " bits:");
                    Console.WriteLine(maiorLiteral * (-1) - 1 + " até " + maiorLiteral);
                    Console.WriteLine("\n****************\n");

                    Console.ReadLine();
                    Environment.Exit(0);
                }

                if (literal >= 0) {
                    for (int i = Constantes.tamanhoPalavra - 1; i >= 0; i--) {
                        vetorByte[i] = (int)(literal % 256);
                        literal = literal / 256;
                    }
                    return vetorByte;
                }
                else {
                    //maior literal possível levando em conta que metade do intervalo é negativo
                    Console.WriteLine("literal capturado: " + literal);
                    Console.WriteLine("maiorLiteral = " + maiorLiteral);
                    literal *= -1;
                    literal += maiorLiteral;

                    for (int i = Constantes.tamanhoPalavra - 1; i >= 0; i--) {
                        vetorByte[i] = (int)(literal % 256);
                        literal = literal / 256;
                    }
                    return vetorByte;
                }

            }

        }

        public static auxiliar.VetorByte intToVetorByte(int inteiro, int tamanho) {

            var vetorByte = new auxiliar.VetorByte(tamanho);
            for (int i = vetorByte.vetor.Length - 1; i >= 0; i--) {
                vetorByte.vetor[i] = (int)(inteiro % 256);
                inteiro = inteiro / 256;
            }
            return vetorByte;
        }

        public static auxiliar.VetorByte intToVetorByte(long inteiro64, int tamanho) {

            var vetorByte = new auxiliar.VetorByte(tamanho);
            for (int i = vetorByte.vetor.Length - 1; i >= 0; i--) {
                vetorByte.vetor[i] = (int)(inteiro64 % 256);
                inteiro64 = inteiro64 / 256;
            }
            return vetorByte;
        }

        public static int[] literalByte(long literal) {
            /* Função interna de codificaLiteraisEmByteArray
             * Recebe um literal positivo ou negativo
             * retorna um array correspondente (com tamanho indicado pelo tamanho da palavra)
             * cada posição do array corresponde à uma base de 2^8 dado que um byte tem 8 bits
             * ex: array = [4,2,5] -> literal correspondente = 4 * (2^8)^2 + 2 * (2^8)^1 + 5 * (2^8)^0
             * os numeros negativos ocupam a metade superior desse array
             * ex: [128,0,0] -> literal correspondente = -1
             * como [128,255,255] é exatamente metade e [0,0,0] já contém o valor zero
             * então [128,0,0] contém -1
             * [128,0,1] contém -2
             * ...assim por diante
             */
            int[] vetorByte = new int[Constantes.tamanhoPalavra];


            //Teste overflow!
            long maiorLiteral = (long)Math.Pow(2, (Constantes.tamanhoPalavra * 8) - 1) - 1;
            if (literal > maiorLiteral || literal < ((maiorLiteral * -1) - 1)) {
                Console.WriteLine("\n***** ERRO *****\n");
                Console.WriteLine("literal fora do intervalo");
                Console.WriteLine("Literal: " + literal);
                Console.WriteLine("Intervalo permitido para tamanho de palavra = " +
                    Constantes.tamanhoPalavra * 8 + " bits:");
                Console.WriteLine(maiorLiteral * (-1) - 1 + " até " + maiorLiteral);
                Console.WriteLine("\n****************\n");

                Console.ReadLine();
                Environment.Exit(0);
            }

            if (literal >= 0) {
                for (int i = Constantes.tamanhoPalavra - 1; i >= 0; i--) {
                    vetorByte[i] = (int)(literal % 256);
                    literal = literal / 256;
                }
                return vetorByte;
            }
            else {
                //maior literal possível levando em conta que metade do intervalo é negativo
                Console.WriteLine("literal capturado: " + literal);
                Console.WriteLine("maiorLiteral = " + maiorLiteral);
                literal *= -1;
                literal += maiorLiteral;

                for (int i = Constantes.tamanhoPalavra - 1; i >= 0; i--) {
                    vetorByte[i] = (int)(literal % 256);
                    literal = literal / 256;
                }
                return vetorByte;
            }

        }
    }
}
