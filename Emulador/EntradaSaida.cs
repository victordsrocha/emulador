using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Emulador {
    class EntradaSaida {

        public List<List<string>> executarParser() {
            // listaEntradas é lista contendo linhas do arquivo
            var listaEntradas = lerArquivoDeEntrada();
            // é criada uma lista para armazenar as listas de grupos para cada instrução do arquivo
            var listaDeListaDeGrupoDeInstrucaoParseada = new List<List<string>>();
            // as linhas do arquivo são verificadas e separadas em grupos pelos métodos do parser
            foreach (var instrucao in listaEntradas) {
                listaDeListaDeGrupoDeInstrucaoParseada.Add(Parser.separaGrupos(instrucao));
            }
            //retorna a lista de listas de grupos de cada instrução
            return listaDeListaDeGrupoDeInstrucaoParseada;
            //notar que grupo x da linha y será encontrado em lista[y-1][x]

            //Função local para criar a lista com strings do arquivo
            List<string> lerArquivoDeEntrada() {
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
}
