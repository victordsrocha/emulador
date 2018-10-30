using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Emulador.barramentos;

namespace Emulador {
    class EntradaSaida {

        public int localAtualRam;
        public int localAtualCPU;
        public int[] buffer;

        public EntradaSaida() {
            this.buffer = new int[Constantes.tamanhoBuffer];
            localAtualRam = 0;
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
                    qtdColor = tamanhoInstrucao(codigo);
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

        public void rajada(BarramentoDeDados barramentoDeDados,
                                            BarramentoDeEnderecos barramentoDeEnderecos,
                                            BarramentoDeControle barramentoDeControle,
                                            Ram ram,
                                            CPU cpu) {


            int numInstrucoesRajada = quantidadeInstrucoesParaRajada();
            for (int i = 0; i < numInstrucoesRajada; i++) {
                enviaEnderecoDeInterrupcao();
                enviaTamanhoDeInterrupcao();
                //numero de repeticoes que o barramento precisa para enviar a proxima instrução
                if ((tamanhoInstrucao(buffer[Constantes.tamanhoPalavra - 1]) % Constantes.larguraBarramentoDeDados) == 0) {
                    int numEnvios = (tamanhoInstrucao(buffer[Constantes.tamanhoPalavra - 1])) / Constantes.larguraBarramentoDeDados;
                    for (int j = 0; j < numEnvios; j++) {
                        enviaBarramentoDeControle();
                        enviaBarramentoDeEndereco();
                        enviaBarramentoDeDados();
                        auxiliar.Auxiliar.popVetorLarguraDados(buffer);//fila do buffer anda uma largura de barramento de dados
                    }
                }
                else {
                    int tamanho = tamanhoInstrucao(buffer[Constantes.tamanhoPalavra - 1]);
                    int numEnviados = 0;
                    int numEnvios = (tamanhoInstrucao(buffer[Constantes.tamanhoPalavra - 1])) / Constantes.larguraBarramentoDeDados;
                    for (int j = 0; j < numEnvios; j++) {
                        enviaBarramentoDeControle();
                        enviaBarramentoDeEndereco();
                        enviaBarramentoDeDados();
                        auxiliar.Auxiliar.popVetorLarguraDados(buffer);//fila do buffer anda uma largura de barramento de dados
                        numEnviados += Constantes.larguraBarramentoDeDados;
                    }
                    
                    while (tamanho - numEnviados != 0) {
                        enviaBarramentoDeControle();
                        enviaBarramentoDeEndereco();
                        barramentoDeDados.receiveImpar(this);
                        barramentoDeDados.send(ram);
                        localAtualRam += 1;
                        auxiliar.Auxiliar.popIndividial(buffer);
                        numEnviados++;
                    }
                }
                
            }

            void enviaBarramentoDeDados() {
                //preenche barramento de dados
                barramentoDeDados.receive(this);

                //envia conteúdo do barramento para a ram
                barramentoDeDados.send(ram);
                //atualiza local de memória utilizado da ram
                localAtualRam += barramentoDeDados.largura;
            }

            void enviaBarramentoDeEndereco() {
                var endByte = Encoder.intToVetorByte(localAtualRam, Constantes.larguraBarramentoDeEndereco);
                barramentoDeEnderecos.receive(endByte.vetor);

                barramentoDeEnderecos.send(ram);
                
            }

            void enviaBarramentoDeControle() {
                //Para o caso ES -> Ram o sinal de controle será sempre somente escrita (0)
                barramentoDeControle.receive(0);
                //Leva o sinal de controle e "armazena" na ram
                barramentoDeControle.send(ram);
            }

            void enviaEnderecoDeInterrupcao() {
                var vetorLocalRam = Encoder.intToVetorByte(localAtualRam, Constantes.larguraBarramentoDeEndereco);
                barramentoDeEnderecos.receive(vetorLocalRam.vetor);
                barramentoDeEnderecos.send(cpu);
            }

            void enviaTamanhoDeInterrupcao() {
                int tamanho = tamanhoInstrucao(buffer[Constantes.tamanhoPalavra - 1]);
                var tamanhoInstrucaoVetorByte = Encoder.intToVetorByte(tamanho, Constantes.larguraBarramentoDeDados);
                barramentoDeDados.receive(tamanhoInstrucaoVetorByte.vetor);
                barramentoDeDados.send(cpu);
            }
        }

        public int quantidadeInstrucoesParaRajada() {
            //retorna o numero de instruções da rajada
            int qtdBytes = 0;

            int qtdInst = 0;

            while (qtdBytes <= quantidadeBytesParaRajada() * Constantes.larguraBarramentoDeDados) {
                qtdBytes += tamanhoInstrucao(buffer[Constantes.tamanhoPalavra - 1 + qtdBytes]);
                if (qtdBytes <= quantidadeBytesParaRajada() * Constantes.larguraBarramentoDeDados) {
                    qtdInst++;
                }
                else if(tamanhoInstrucao(buffer[Constantes.tamanhoPalavra - 1 + qtdBytes]) == 0) {
                    return qtdInst;
                }
                else {
                    return qtdInst;
                }
            }
            return 0;

        }

        public int quantidadeBytesParaRajada() {
            //analisa buffer e retorna quantidade de bytes que cabem na 'largura de banda' sem quebrar instruções!
            //após a rajada será preciso fazer o "pop" de todos os dados já usados do buffer
            //a função para isso já está pronta em auxiliar.auxiliar!

            //atualizado para retornar o numero de barramentos que deverão ser preenchidos

            int posicao = 0;
            int tamanhoAcumuladoEmBytes = 0;
            while (true) {
                posicao = (Constantes.tamanhoPalavra - 1) + tamanhoAcumuladoEmBytes;
                if (tamanhoAcumuladoEmBytes + tamanhoInstrucao(buffer[posicao]) <= Constantes.taxaDeTransferência) {
                    tamanhoAcumuladoEmBytes += tamanhoInstrucao(buffer[posicao]);
                }
                else if (tamanhoInstrucao(buffer[posicao])==0) {
                    return (int)tamanhoAcumuladoEmBytes / Constantes.larguraBarramentoDeDados;
                }
                else {
                    return (int)tamanhoAcumuladoEmBytes / Constantes.larguraBarramentoDeDados;
                }
            }
        }



    }
}
