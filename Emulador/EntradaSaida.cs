using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Emulador.barramentos;

namespace Emulador {
    class EntradaSaida {

        public int localAtualBuffer;
        public int localAtualRam;
        public int[] buffer;

        public EntradaSaida() {
            this.buffer = new int[Constantes.tamanhoBuffer];
            localAtualBuffer = 0;
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
            int contador = -1;
            int qtdColor = 0;
            bool maisInstrucoes = true;

            Console.WriteLine("Buffer " + Constantes.tamanhoBuffer + " bytes");

            Console.Write("[ ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            for (int i = 0; i < buffer.Length; i++) {

                contador++;
                if (contador == 0 && maisInstrucoes) {
                    int codigo = buffer[i + Constantes.tamanhoPalavra - 1];
                    qtdColor = tamanhoInstrucao(codigo) * Constantes.tamanhoPalavra;
                    mudaCorBackground();
                    if (qtdColor == 0) {
                        maisInstrucoes = false;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                if (contador == qtdColor - 1) {
                    contador = -1;
                }


                Console.Write(buffer[i] + " ");

            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("]");


            void mudaCorBackground() {
                if (Console.BackgroundColor == ConsoleColor.DarkGray) {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                else if (Console.BackgroundColor == ConsoleColor.DarkBlue) {
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                }
            }

        }

        public int tamanhoInstrucao(int codigo) {
            if (codigo >=1 && codigo < 9) {
                return 3;
            }
            if (codigo >= 9 && codigo < 11) {
                return 2;
            }
            if (codigo >= 11) {
                return 4;
            }
            return 0;
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

        public void carregaEnviaBarramentos(BarramentoDeDados barramentoDeDados,
                                            BarramentoDeEnderecos barramentoDeEnderecos,
                                            BarramentoDeControle barramentoDeControle,
                                            Ram ram) {

            enviaBarramentoDeControle();
            enviaBarramentoDeEndereco();
            enviaBarramentoDeDados();

            void enviaBarramentoDeDados() {
                //preenche barramento de dados
                barramentoDeDados.receive(buffer, localAtualBuffer);
                //atualiza local de memória utilizado do buffer
                localAtualBuffer += barramentoDeDados.largura;

                //envia conteúdo do barramento para a ram
                barramentoDeDados.send(ram.memoria, localAtualRam);
                //atualiza local de memória utilizado da ram
                localAtualRam += barramentoDeDados.largura;
            }

            void enviaBarramentoDeEndereco() {
                var endByte = Encoder.intToVetorByte(localAtualRam, Constantes.larguraBarramentoDeEndereco);
                barramentoDeEnderecos.receive(endByte.vetor);

                barramentoDeEnderecos.send(ram.vetorLeitorDeEndereco);
                ram.leituraEndereco();
            }

            void enviaBarramentoDeControle() {
                //Para o caso ES -> Ram o sinal de controle será sempre somente leitura (1)
                barramentoDeControle.receive(1);
                //Leva o sinal de controle e "armazena" na ram
                barramentoDeControle.send(ram.ArmazenadorDeSinalDeControle);
            }
        }


    }
}
