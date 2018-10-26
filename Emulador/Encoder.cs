using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Emulador {
    static class Encoder {

        public static void insereValorDeInstrucao(List<List<string>> entradas) {
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
                if (Regex.IsMatch(fonte, @"(add)\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "1";
                }
                //instrução de código 2
                if (Regex.IsMatch(fonte, @"(add)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "2";
                }
                //instrução de código 3
                if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "3";
                }
                //instrução de código 4
                if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "4";
                }
                //instrução de código 5
                if (Regex.IsMatch(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "5";
                }
                //instrução de código 6
                if (Regex.IsMatch(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "6";
                }
                //instrução de código 7
                if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "7";
                }
                //instrução de código 8
                if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "8";
                }
                //instrução de código 9
                if (Regex.IsMatch(fonte, @"(inc)\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "9";
                }
                //instrução de código 10
                if (Regex.IsMatch(fonte, @"(inc)\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = "10";
                }
                //instrução de código 11
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "11";
                }
                //instrução de código 13
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "13";
                }
                //instrução de código 15
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "15";
                }
                //instrução de código 17
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = "17";
                }
                //instrução de código 18
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = "18";
                }
                //instrução de código 19
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = "19";
                }
                //instrução de código 20
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "20";
                }
                //instrução de código 22
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "22";
                }
                //instrução de código 24
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                    entradas[linha][1] = "24";
                }
                //instrução de código 26
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = "26";
                }
                //instrução de código 27
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = "27";
                }
                //instrução de código 28
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                    entradas[linha][1] = "28";
                }
                //instrução de código 12
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "12";
                }
                //instrução de código 14
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "14";
                }
                //instrução de código 16
                if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "16";
                }
                //instrução de código 21
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "21";
                }
                //instrução de código 23
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "23";
                }
                //instrução de código 25
                if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                    entradas[linha][1] = "25";
                }
            }
        }

        public static void codificaRegistradores(List<List<string>> entradas) {

            foreach (var instrucao in entradas) {

                for (int i = 1; i < instrucao.Count; i++) {

                    if (Regex.IsMatch(instrucao[i], @"([E-Z])")) {
                        Console.WriteLine("\n***** ERRO *****\n");
                        Console.WriteLine("Registrador " + instrucao[i] + " não existe");
                        Console.WriteLine("\n****************\n");

                        Console.ReadLine();
                        Environment.Exit(0);
                    }

                    if (Regex.IsMatch(instrucao[i], @"A")) {
                        instrucao[i] = "0";
                    }
                    if (Regex.IsMatch(instrucao[i], @"B")) {
                        instrucao[i] = "1";
                    }
                    if (Regex.IsMatch(instrucao[i], @"C")) {
                        instrucao[i] = "2";
                    }
                    if (Regex.IsMatch(instrucao[i], @"D")) {
                        instrucao[i] = "3";
                    }
                }
            }
        }

        public static int hexaEndParaDecInt(string hexa) {
            /* Recebe um string de endereço do tipo 0x00...0FF
             * Elimina os caracteres '0x' do inicio
             * Converte valor hexadecimal do endereço para decimal
             * retorna o decimal (inteiro)
             * 
             * Por enquanto é usado somente dentro de codificaEnderecos, posteriormente
             * poderia ser enviado para codificaEnderecos como função local
             */

            var dic = new Dictionary<char, int>();
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
            int valor = 0;
            for (int i = 0; i < hexa.Length; i++) {
                valor += (int)(Math.Pow(16, i)) * dic[hexa[hexa.Length - (i + 1)]];
            }
            return valor;
        }

        public static void codificaEnderecos(List<List<string>> entradas) {
            /* Altera todos os endereços do tipo 0x...FF em entradas para o inteiro decimal
             * correspondente. O valor decimal será armazenado como string em entradas
             */

            foreach (var instrucao in entradas) {

                for (int i = 1; i < instrucao.Count; i++) {

                    if (Regex.IsMatch(instrucao[i], @"(0x[A-F0-9]+)")) {

                        int end = hexaEndParaDecInt(instrucao[i]);
                        instrucao[i] = Convert.ToString(end);
                    }
                    
                }
            }

        }
    }
}
