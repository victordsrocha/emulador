using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Emulador {
    class EntradaSaida {


        public List<string> lerArquivoDeEntrada() {
            //Lê o arquivo de entrada de instruções linha por linha e devolve uma lista
            //na qual a posição x contém (string) a linha x do arquivo
            string linha;
            var entradas = new List<string>();

            var arquivo = new StreamReader(
                @"C:\Users\Victor\source\repos\Emulador\Emulador\arquivo de entrada\arquivo.txt");

            while ((linha = arquivo.ReadLine()) != null) {
                entradas.Add(linha);
            }

            arquivo.Close();
            return entradas;
        }


    }
}
