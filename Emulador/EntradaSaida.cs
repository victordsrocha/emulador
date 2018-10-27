using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Emulador {
    class EntradaSaida {

        public int[] buffer;

        public EntradaSaida() {
            this.buffer = new int[Constantes.tamanhoBuffer];
        }

        public void preencheBuffer() {

            var entradas = this.executarParser();

            Encoder.codificaLiteraisEmByteArray(entradas);
            Encoder.insereValorDeInstrucao(entradas);
            Encoder.codificaEnderecos(entradas);
            Encoder.codificaRegistradores(entradas);

            int contador = 0;
            foreach (var linha in entradas) {
                for (int i = 1; i < linha.Count; i++) {
                    auxiliar.VetorByte vetorByte = linha[i];
                    for (int j = 0; j < vetorByte.vetor.Length; j++) {
                        try {
                            this.buffer[contador] = vetorByte.vetor[j];
                            contador++;
                        }
                        catch {
                            Console.WriteLine("\n***** ERRO *****\n");
                            Console.WriteLine("Buffer não é grande o bastante para a lista de instruções");
                            Console.WriteLine("\n****************\n");

                            Console.ReadLine();
                            Environment.Exit(0);
                            
                        }
                    }
                }
            }
        }

        public void imprimeBuffer() {

            string s = "[";
            for (int i = 0; i < buffer.Length; i++) {
                if (i != buffer.Length - 1) {
                    s += +buffer[i] + ",";
                }
                else {
                    s += +buffer[i];
                }
            }
            s += "]";
            Console.WriteLine(s);

        }

        public List<List<dynamic>> executarParser() {
            // listaEntradas é lista contendo linhas do arquivo
            var listaEntradas = lerArquivoDeEntrada();
            // é criada uma lista para armazenar as listas de grupos para cada instrução do arquivo
            var listaDeListaDeGrupoDeInstrucaoParseada = new List<List<dynamic>>();
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
