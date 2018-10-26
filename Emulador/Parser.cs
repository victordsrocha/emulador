using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Emulador {
    static class Parser {
        /*
            Recebe uma instrução
            Exemplo:

            mov A, 2
            mov B, 3
            add A, B
            mov 0x0001, A
            inc 0x0001
            imul C, 0x0001, 4
            mov 0x0002, C

            1. Reconhece se há erros.
            2. Separa a instrução em strings menores correspondentes as partes da instrução

            Identificar o código da instrução, código de registradores, valores de endereço ou
            transformar inteiros em bytes deve ser feito somente do encoder!
        */

        public static Match executarParser(string fonte) {
           /* Recebe uma string e retorna um objeto do tipo Match caso a string seja uma instrução bem formada
            * Para acessar os grupos de captura:
            * string contida no grupo x vai estar em Match.Groups[x].Value
            */
            
            //instrução de código 1
            if (Regex.IsMatch(fonte, @"(add)\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(add)\s+(Pi|[A-Z]),\s+(\d+|-\d+)");
            }
            //instrução de código 2
            if (Regex.IsMatch(fonte, @"(add)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(add)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])");
            }
            //instrução de código 3
            if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])");
            }
            //instrução de código 4
            if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)");
            }
            //instrução de código 5
            if (Regex.IsMatch(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(\d+|-\d+)");
            }
            //instrução de código 6
            if (Regex.IsMatch(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(mov)\s+(Pi|[A-Z]),\s+(Pi|[A-Z])");
            }
            //instrução de código 7
            if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])");
            }
            //instrução de código 8
            if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)");
            }
            //instrução de código 9
            if (Regex.IsMatch(fonte, @"(inc)\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(inc)\s+(Pi|[A-Z])");
            }
            //instrução de código 10
            if (Regex.IsMatch(fonte, @"(inc)\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(inc)\s+(0x[A-F0-9]+)");
            }
            //instrução de código 11
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])");
            }
            //instrução de código 13
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(Pi|[A-Z])");
            }
            //instrução de código 15
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])");
            }
            //instrução de código 17
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 18
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 19
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 20
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(Pi|[A-Z])");
            }
            //instrução de código 22
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(Pi|[A-Z])");
            }
            //instrução de código 24
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(Pi|[A-Z])");
            }
            //instrução de código 26
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 27
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 28
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 12
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(Pi|[A-Z]),\s+(\d+|-\d+)");
            }
            //instrução de código 14
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(\d+|-\d+),\s+(\d+|-\d+)");
            }
            //instrução de código 16
            if (Regex.IsMatch(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+(Pi|[A-Z]),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)");
            }
            //instrução de código 21
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(Pi|[A-Z]),\s+(\d+|-\d+)");
            }
            //instrução de código 23
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(\d+|-\d+)");
            }
            //instrução de código 25
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)");
            }

            Console.WriteLine("\n***** ERRO *****\n");
            Console.WriteLine("Erro de sintaxe");
            Console.WriteLine("\n****************\n");
            Console.ReadLine();
            Environment.Exit(0);

            return null;
        }




    }
}
