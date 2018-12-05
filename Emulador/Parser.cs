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



        public static List<dynamic> separaGrupos(string fonte)
        {
            /*
             * Recebe um string e verifica sintaxa executando verificaSintaxe()
             * verificaSintaxe() transforma string de entrada em tipo Match
             * separaGrupos retorna uma lista na qual a posição x contem o valor
             * (string) do grupo x (do Match)
             */

            if (!Regex.IsMatch(fonte, @"([A-Z])<(\d+)\s+:\s+jmp\s+(\d+)\s+:\s+brk"))
            {

                Match instrucao = verificaSintaxe(fonte);

                var grupos = new List<dynamic>();
                for (int i = 0; i < instrucao.Groups.Count; i++)
                {
                    grupos.Add(instrucao.Groups[i].Value);
                }

                return grupos;
            }
            else
            {
                Match instrucao = verificaSintaxe(fonte);

                var grupos = new List<dynamic>();

                grupos.Add(fonte);
                grupos.Add("loop");
                grupos.Add(instrucao.Groups[3].Value);
                grupos.Add(instrucao.Groups[1].Value);
                grupos.Add(instrucao.Groups[2].Value);

                return grupos;

            }
        }

        

        public static Match verificaSintaxe(string fonte) {
            /* Recebe uma string e retorna um objeto do tipo Match caso a string seja uma instrução bem formada
             * Para acessar os grupos de captura:
             * string contida no grupo x vai estar em Match.Groups[x].Value
             */

            //instrucão de código 50
            if (Regex.IsMatch(fonte, @"(lbl)\s+(\d+)"))
            {
                return Regex.Match(fonte, @"(lbl)\s+(\d+)");
            }
            //instrucão de código 51
            if (Regex.IsMatch(fonte, @"([A-Z])<(\d+)\s+:\s+jmp\s+(\d+)\s+:\s+brk"))
            {
                return Regex.Match(fonte, @"([A-Z])<(\d+)\s+:\s+jmp\s+(\d+)\s+:\s+brk");
            }


            //instrução de código 1
            if (Regex.IsMatch(fonte, @"(add)\s+([A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(add)\s+([A-Z]),\s+(\d+|-\d+)");
            }
            //instrução de código 2
            if (Regex.IsMatch(fonte, @"(add)\s+([A-Z]),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(add)\s+([A-Z]),\s+([A-Z])");
            }
            //instrução de código 3
            if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(add)\s+(0x[A-F0-9]+),\s+([A-Z])");
            }
            //instrução de código 4
            if (Regex.IsMatch(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(add)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)");
            }
            //instrução de código 5
            if (Regex.IsMatch(fonte, @"(mov)\s+([A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(mov)\s+([A-Z]),\s+(\d+|-\d+)");
            }
            //instrução de código 6
            if (Regex.IsMatch(fonte, @"(mov)\s+([A-Z]),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(mov)\s+([A-Z]),\s+([A-Z])");
            }
            //instrução de código 7
            if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+([A-Z])");
            }
            //instrução de código 8
            if (Regex.IsMatch(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(mov)\s+(0x[A-F0-9]+),\s+(\d+|-\d+)");
            }
            //instrução de código 9
            if (Regex.IsMatch(fonte, @"(inc)\s+([A-Z])")) {
                return Regex.Match(fonte, @"(inc)\s+([A-Z])");
            }
            //instrução de código 10
            if (Regex.IsMatch(fonte, @"(inc)\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(inc)\s+(0x[A-F0-9]+)");
            }
            //instrução de código 11
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+([A-Z]),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+([A-Z]),\s+([A-Z])");
            }
            //instrução de código 13
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+(\d+|-\d+),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+(\d+|-\d+),\s+([A-Z])");
            }
            //instrução de código 15
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+(0x[A-F0-9]+),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+(0x[A-F0-9]+),\s+([A-Z])");
            }
            //instrução de código 17
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 18
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+([A-Z]),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+([A-Z]),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 19
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 20
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+([A-Z]),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+([A-Z]),\s+([A-Z])");
            }
            //instrução de código 22
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+([A-Z])");
            }
            //instrução de código 24
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+([A-Z])")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+([A-Z])");
            }
            //instrução de código 26
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 27
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+([A-Z]),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+([A-Z]),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 28
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+(\d+|-\d+),\s+(0x[A-F0-9]+)");
            }
            //instrução de código 12
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+([A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+([A-Z]),\s+(\d+|-\d+)");
            }
            //instrução de código 14
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+(\d+|-\d+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+(\d+|-\d+),\s+(\d+|-\d+)");
            }
            //instrução de código 16
            if (Regex.IsMatch(fonte, @"(imul)\s+([A-Z]),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+([A-Z]),\s+(0x[A-F0-9]+),\s+(\d+|-\d+)");
            }
            //instrução de código 21
            if (Regex.IsMatch(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+([A-Z]),\s+(\d+|-\d+)")) {
                return Regex.Match(fonte, @"(imul)\s+(0x[A-F0-9]+),\s+([A-Z]),\s+(\d+|-\d+)");
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
