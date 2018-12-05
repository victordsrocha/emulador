using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emulador.barramentos;

namespace Emulador
{
    class CPU
    {
        public int instante = 0;
        public int[] A;
        public int[] B;
        public int[] C;
        public int[] D;
        public int[] Pi;
        public Queue<int> interrupcoes;

        public List<Label> listaDeLabels = new List<Label>();

        public CPU()
        {
            A = new int[Constantes.tamanhoPalavra];
            B = new int[Constantes.tamanhoPalavra];
            C = new int[Constantes.tamanhoPalavra];
            D = new int[Constantes.tamanhoPalavra];
            Pi = new int[Constantes.larguraBarramentoDeEndereco];
            //o ideal é que as interrupcoes sejam estruturas contendo endereço e tamanho
            //this.interrupcoes = new int[(int)(Constantes.taxaDeTransferência / Constantes.tamanhoPalavra)];
            interrupcoes = new Queue<int>();
        }

        public void imprimeRegistradores()
        {
            string s;
            s = "Registrador A: ";
            s += "[ ";
            for (int i = 0; i < Constantes.tamanhoPalavra; i++)
            {
                s += A[i] + " ";
            }
            s += "] " + "(" + Decoder.byteToLongLiteral(A) + ")\n";
            s += "Registrador B: ";
            s += "[ ";
            for (int i = 0; i < Constantes.tamanhoPalavra; i++)
            {
                s += B[i] + " ";
            }
            s += "] " + "(" + Decoder.byteToLongLiteral(B) + ")\n";
            s += "Registrador C: ";
            s += "[ ";
            for (int i = 0; i < Constantes.tamanhoPalavra; i++)
            {
                s += C[i] + " ";
            }
            s += "] " + "(" + Decoder.byteToLongLiteral(C) + ")\n";
            s += "Registrador D: ";
            s += "[ ";
            for (int i = 0; i < Constantes.tamanhoPalavra; i++)
            {
                s += D[i] + " ";
            }
            s += "] " + "(" + Decoder.byteToLongLiteral(D) + ")\n";
            s += "Registrador Pi: ";
            s += "[ ";
            for (int i = 0; i < Constantes.tamanhoPalavra; i++)
            {
                s += Pi[i] + " ";
            }
            s += "] " + "(" + Decoder.byteToLongLiteral(Pi) + ")\n";
            Console.WriteLine(s);
        }

        public void imprimeInterrupcoes()
        {
            Console.Write("Lista de interrupções no cpu: ");
            string s = "[ ";
            foreach (var item in interrupcoes)
            {
                s += item + " ";
            }
            s += "]";
            Console.WriteLine(s);
        }


        public void executaTodasInterrupcoes(EntradaSaida moduloES,
                                                BarramentoDeDados barramentoDeDados,
                                                BarramentoDeEnderecos barramentoDeEnderecos,
                                                BarramentoDeControle barramentoDeControle,
                                                Ram ram,
                                                Cache cacheReal)
        {

            int quantidadeInterrupcoes = interrupcoes.Count / (Constantes.larguraBarramentoDeDados
                + Constantes.larguraBarramentoDeEndereco);

            for (int i = 0; i < quantidadeInterrupcoes; i++)
            {
                executarProximaInterrupcao(moduloES,
                                           barramentoDeDados,
                                           barramentoDeEnderecos,
                                           barramentoDeControle,
                                           ram,
                                           cacheReal);
            }

        }



        public void executarProximaInterrupcao(EntradaSaida moduloES,
                                                BarramentoDeDados barramentoDeDados,
                                                BarramentoDeEnderecos barramentoDeEnderecos,
                                                BarramentoDeControle barramentoDeControle,
                                                Ram ram,
                                                Cache cacheMemoria)
        {

            int tp = Constantes.tamanhoPalavra;
            int lb = Constantes.larguraBarramentoDeEndereco;
            int offset = Constantes.tamanhoOffset;

            double porcentagem = Constantes.porcentagemCache / 100;

            //leitura do proxima endereço dado pela fila de interrupcões
            int[] endVetor = new int[Constantes.larguraBarramentoDeEndereco];
            for (int i = 0; i < Constantes.larguraBarramentoDeEndereco; i++)
            {
                endVetor[i] = interrupcoes.Dequeue();
            }
            long end = Decoder.byteToLong(endVetor);
            long endRam = end;
            //

            //leitura do tamanho da proxima instrução dado pela interrupção
            //tamanho em bytes
            int tamanho = 0;
            for (int i = 0; i < Constantes.larguraBarramentoDeDados; i++)
            {
                //o tamanho sempre estara na ultima casa do vetor que foi enviado
                //por isso basta capturar o ultimo e descartar os demais para a fila andar
                tamanho = interrupcoes.Dequeue();
            }

            //altera estado da ram para leitura
            barramentoDeControle.receive(1);
            barramentoDeControle.send(ram);

            //Armazena posição da instrução no registrador Pi
            Pi = endVetor;

            //altera 'ponteiro' da ram para endereço armazenado no registrador Pi
            barramentoDeEnderecos.receive(Pi);
            barramentoDeEnderecos.send(ram);

            //instrução do endereço é armazenada no cpu
            //"prototipo" de memoria cache(cacheVirtual)
            int[] nova_instrucao = new int[tamanho];
            int posicaoCacheVirtual = 0;
            int n = tamanho % Constantes.larguraBarramentoDeDados;
            int numeroBytesEnviados = 0;
            if (n == 0)
            {
                for (int i = 0; i < tamanho / Constantes.larguraBarramentoDeDados; i++)
                {
                    barramentoDeDados.receive(ram);
                    barramentoDeDados.send(nova_instrucao, posicaoCacheVirtual);
                    posicaoCacheVirtual += Constantes.larguraBarramentoDeDados;

                    //atualiza posicao do ponteiro na ram!
                    end += Constantes.larguraBarramentoDeDados;
                    var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                    barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                    barramentoDeEnderecos.send(ram);

                    //
                    numeroBytesEnviados += Constantes.larguraBarramentoDeDados;
                }
            }

            while (tamanho - numeroBytesEnviados != 0)
            {
                barramentoDeDados.receiveImpar(ram);
                barramentoDeDados.send(nova_instrucao, posicaoCacheVirtual);
                posicaoCacheVirtual += 1;

                //atualiza posicao do ponteiro na ram!
                end += 1;
                var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                barramentoDeEnderecos.send(ram);

                //
                numeroBytesEnviados++;
            }

            void atualizaRam(InformacaoMMU bloco)
            {
                long endR = bloco.posicaoRam;
                long posicaoCache = bloco.posicaoCache;
                //prepara barramento de controle
                barramentoDeControle.receive(0);
                barramentoDeControle.send(ram);
                //prepara barramento de endereços
                endR = (endR * tp) + offset;
                var endVetorR = Encoder.intToVetorByte(endR, lb);
                barramentoDeEnderecos.receive(endVetorR.vetor);
                barramentoDeEnderecos.send(ram);
                //clona os dados da cache para a ram usando barramento e clock
                for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++)
                {
                    barramentoDeDados.receive(cacheMemoria, posicaoCache);
                    barramentoDeDados.send(ram);
                    posicaoCache += Constantes.larguraBarramentoDeDados;

                    //atualiza posicao do ponteiro na ram!
                    endR += Constantes.larguraBarramentoDeDados;
                    var NovoEndVetor = Encoder.intToVetorByte(endR, Constantes.larguraBarramentoDeEndereco);
                    barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                    barramentoDeEnderecos.send(ram);
                }
            }

            InformacaoMMU retornaSerieDeBlocos(int tamanhoBlocoNovo)
            {
                var r1 = cacheMemoria.tabelaBlocos.OrderBy(p => p.posicaoCache).ToList();
                for (int i = 0; i < r1.Count-1; i++)
                {
                    if ((r1[i].tamanho + r1[i + 1].tamanho) >= tamanhoBlocoNovo)
                    {
                        r1[i].tamanho += r1[i + 1].tamanho;

                        atualizaRam(r1[i + 1]);
                        InformacaoMMU blocoParaExcluir = cacheMemoria.tabelaBlocos.Find(p => p.id_interno == r1[i + 1].id_interno);
                        cacheMemoria.tabelaBlocos.Remove(blocoParaExcluir);

                        return r1[i];
                    }
                }
                return null;
            }

            //armazena instrução na memoria cache (cache.vetor)

            //ALGORITMO DE SUBSTITUICAO

            //verificar se cache esta com preenchumento acima de 80%
            if (((double)cacheMemoria.preenchimento / cacheMemoria.vetor.Length) > porcentagem)
            {
                //encontrar blocos que tenham tamanho maior ou igual a instrução atual
                var r1 = cacheMemoria.tabelaBlocos.Where(p => p.tamanho >= nova_instrucao.Length).ToList(); //PONTODEPARADA

                //pegar alguem que tenha numeroAcessos minimo desempatado por instante de ultimo acesso minimo
                InformacaoMMU blocoParaExcluir;
                try
                {
                    var r2 = r1.OrderBy(p => p.numeroAcessos).ThenBy(p => p.instanteUltimoAcesso).ToList();
                    blocoParaExcluir = r2[0];
                }
                catch //escolhe dois blocos em sequencia caso nenhum bloco ocupado tenha tamanho suficiente para o bloco novo
                {
                    blocoParaExcluir = retornaSerieDeBlocos(nova_instrucao.Length);
                }

                ////gravar novas informações em vetor cache usando barramentos

                //identifica posicao da cache para gravar a partir do bloco que deve ser "excluido"
                long posicaoCache = blocoParaExcluir.posicaoCache;

                //prepara barramento de controle
                barramentoDeControle.receive(1);
                barramentoDeControle.send(ram);
                //prepara barramento de endereços
                barramentoDeEnderecos.receive(endVetor);
                barramentoDeEnderecos.send(ram);
                //clona os dados da ram para a cache usando barramento e clock
                for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++)
                {
                    barramentoDeDados.receive(ram);
                    barramentoDeDados.send(cacheMemoria.vetor, (int)(posicaoCache));
                    posicaoCache += Constantes.larguraBarramentoDeDados;

                    //atualiza posicao do ponteiro na ram!
                    end += Constantes.larguraBarramentoDeDados;
                    var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                    barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                    barramentoDeEnderecos.send(ram);
                }

                //cria novo bloco mmu com base nos dados carregados
                int tamanhoBloco = nova_instrucao.Length;
                long posicaoBlocoNaCache = blocoParaExcluir.posicaoCache;
                long posicaoBlocoNaRam = endRam;
                var novoBloco = new InformacaoMMU(tamanhoBloco, posicaoBlocoNaCache, posicaoBlocoNaRam, instante);
                cacheMemoria.tabelaBlocos.Add(novoBloco);

                //exclui bloco antigo
                cacheMemoria.tabelaBlocos.Remove(blocoParaExcluir);

            }
            else // somente insere no próximo espaço vago da cache
            {
                for (int i = 0; i < nova_instrucao.Length; i++)
                {
                    cacheMemoria.vetor[cacheMemoria.preenchimento + i] = nova_instrucao[i];
                }
                

                //cria novo bloco mmu com base nos dados carregados
                int tamanhoBloco = nova_instrucao.Length;
                long posicaoBlocoNaCache = cacheMemoria.preenchimento; //aponta para inicio do bloco na cache
                long posicaoBlocoNaRam = endRam; //endereço real do inicio da instrução na ram
                var novoBloco = new InformacaoMMU(tamanhoBloco, posicaoBlocoNaCache, posicaoBlocoNaRam, instante);
                cacheMemoria.tabelaBlocos.Add(novoBloco);
                cacheMemoria.preenchimento += nova_instrucao.Length;
            }


            



            //identifica código da instrução
            int codigo = nova_instrucao[Constantes.tamanhoPalavra - 1];

            //executa instrução
            executaInstrucao(codigo, nova_instrucao, barramentoDeDados, barramentoDeEnderecos,
                barramentoDeControle, ram, moduloES, cacheMemoria);
        }

        public void executaInstrucao(int codigo, int[] cacheVirtual, BarramentoDeDados barramentoDeDados,
            BarramentoDeEnderecos barramentoDeEnderecos, BarramentoDeControle barramentoDeControle,
            Ram ram, EntradaSaida ModuloES, Cache cacheReal)
        {
            double porcentagem = Constantes.porcentagemCache / 100;
            int tp = Constantes.tamanhoPalavra;
            int lb = Constantes.larguraBarramentoDeEndereco;
            int offset = Constantes.tamanhoOffset;

            long valorRegistrador(int codigoRegistrador)
            {
                if (codigoRegistrador == 0)
                {
                    return Decoder.byteToLongLiteral(A);
                }
                if (codigoRegistrador == 1)
                {
                    return Decoder.byteToLongLiteral(B);
                }
                if (codigoRegistrador == 2)
                {
                    return Decoder.byteToLongLiteral(C);
                }
                if (codigoRegistrador == 3)
                {
                    return Decoder.byteToLongLiteral(D);
                }
                return 0;
            }

            int identificaCodigoRegistrador(int qtdPalavras, int qtdEnds)
            {
                int pos = qtdPalavras * tp + qtdEnds * lb;
                return cacheVirtual[pos + tp - 1];
            }

            void armazenaNoRegistrador(int[] vetor, int codigoRegistrador)
            {
                if (codigoRegistrador == 0)
                {
                    A = vetor;
                }
                if (codigoRegistrador == 1)
                {
                    B = vetor;
                }
                if (codigoRegistrador == 2)
                {
                    C = vetor;
                }
                if (codigoRegistrador == 3)
                {
                    D = vetor;
                }
            }

            long identificaEndereco(int qtdPalavras, int qtdEnds)
            {
                int pos = qtdPalavras * tp + qtdEnds * lb;
                int[] vetorEnd = new int[lb];
                for (int i = 0; i < lb; i++)
                {
                    vetorEnd[i] = cacheVirtual[pos + i];
                }
                return Decoder.byteToLong(vetorEnd);
            }

            long leituraValorEndereco(long end)
            {
                long endParametro = end;
                long endRam = (end * tp) + offset;
                //verifica se endereco já está carregado na cache
                //se teste>0 entao temos um cache hit
                int posicaoBlocoCache = cacheReal.tabelaBlocos.FindIndex(p => p.posicaoRam == endRam);
                if (posicaoBlocoCache >= 0)//cache hit
                {
                    InformacaoMMU blocoCache = cacheReal.tabelaBlocos[posicaoBlocoCache];
                    long posicaoCache = blocoCache.posicaoCache;
                    int[] vetorValor = new int[tp];//vetorValor clona o vetor da cache
                    for (int i = 0; i < tp; i++)
                    {
                        vetorValor[tp - i - 1] = cacheReal.vetor[posicaoCache + blocoCache.tamanho - 1 - i];
                    }
                    blocoCache.numeroAcessos++;
                    blocoCache.instanteUltimoAcesso = instante;
                    return Decoder.byteToLongLiteral(vetorValor);
                }
                else//cache miss
                {
                    //alimentar o cache com essa informação!!!

                    //se cache esta com preenchumento acima de 80%
                    if (((double)cacheReal.preenchimento / cacheReal.vetor.Length) > porcentagem)
                    {
                        //encontrar blocos que tenham tamanho maior ou igual a uma palavra
                        var r1 = cacheReal.tabelaBlocos.Where(p => p.tamanho >= Constantes.tamanhoPalavra).ToList();
                        //pegar alguem que tenha numeroAcessos minimo desempatado por instante
                        var r2 = r1.OrderBy(p => p.numeroAcessos).ThenBy(p => p.instanteUltimoAcesso).ToList();
                        InformacaoMMU blocoParaExcluir = r2[0];
                        atualizaRam(blocoParaExcluir);//atualiza ram antes de excluir bloco
                        //gravar novas informações em vetor cache
                        //lembrar de usar barramentos para carregar novas informações na cache
                        long posicaoCache = blocoParaExcluir.posicaoCache;
                        //localiza end em ram
                        end = (end * tp) + offset;
                        var endVetor = Encoder.intToVetorByte(end, lb);
                        //prepara barramento de controle
                        barramentoDeControle.receive(1);
                        barramentoDeControle.send(ram);
                        //prepara barramento de endereços
                        barramentoDeEnderecos.receive(endVetor.vetor);
                        barramentoDeEnderecos.send(ram);
                        //clona os dados da ram para a cache usando barramento e clock
                        for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++)
                        {
                            barramentoDeDados.receive(ram);
                            barramentoDeDados.send(cacheReal.vetor, (int)(posicaoCache));
                            posicaoCache += Constantes.larguraBarramentoDeDados;
                            //atualiza posicao do ponteiro na ram!
                            end += Constantes.larguraBarramentoDeDados;
                            var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                            barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                            barramentoDeEnderecos.send(ram);
                        }
                        //cria novo bloco mmu com base nos dados carregados
                        int tamanho = tp;
                        long posicao = blocoParaExcluir.posicaoCache;
                        long posicaoRam = endRam;
                        var novoBloco = new InformacaoMMU(tamanho, posicao, posicaoRam, instante);
                        cacheReal.tabelaBlocos.Add(novoBloco);
                        novoBloco.numeroAcessos--;//como a função será chamada em recursão é preciso tirar um acesso aqui
                        //exclui bloco antigo
                        cacheReal.tabelaBlocos.Remove(blocoParaExcluir);
                        //executa função novamente, necessariamente teremos um cache hit
                        return leituraValorEndereco(endParametro);
                    }
                    else//caso o preenchimento seja menor que 80%
                    {
                        //gravar novas informações em vetor cache
                        //lembrar de usar barramentos para carregar novas informações na cache
                        long posicaoCache = cacheReal.preenchimento;
                        //localiza end em ram
                        end = (end * tp) + offset;
                        var endVetor = Encoder.intToVetorByte(end, lb);
                        //prepara barramento de controle
                        barramentoDeControle.receive(1);
                        barramentoDeControle.send(ram);
                        //prepara barramento de endereços
                        barramentoDeEnderecos.receive(endVetor.vetor);
                        barramentoDeEnderecos.send(ram);
                        //clona os dados da ram para a cache usando barramento e clock
                        for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++)
                        {
                            barramentoDeDados.receive(ram);
                            barramentoDeDados.send(cacheReal.vetor, (int)(posicaoCache));
                            posicaoCache += Constantes.larguraBarramentoDeDados;

                            //atualiza posicao do ponteiro na ram!
                            end += Constantes.larguraBarramentoDeDados;
                            var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                            barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                            barramentoDeEnderecos.send(ram);
                        }
                        //cria novo bloco com base nos dados carregados
                        int tamanho = tp;
                        long posicao = cacheReal.preenchimento;
                        long posicaoRam = endRam;
                        var novoBloco = new InformacaoMMU(tamanho, posicao, posicaoRam, instante);
                        cacheReal.tabelaBlocos.Add(novoBloco);
                        novoBloco.numeroAcessos--;//como a função será chamada em recursão é preciso tirar um acesso aqui
                        //atualiza informação de preenchimento
                        cacheReal.preenchimento += tamanho;
                        //executa função novamente, necessariamente teremos um cache hit
                        return leituraValorEndereco(endParametro);
                    }
                }


                //metodo antigo
                /*
                end = (end * tp) + offset;
                var endVetor = Encoder.intToVetorByte(end, lb);
                barramentoDeControle.receive(1);
                barramentoDeControle.send(ram);
                barramentoDeEnderecos.receive(endVetor.vetor);
                barramentoDeEnderecos.send(ram);
                int[] cacheVirtual2 = new int[tp];
                int posicaoCacheVirtual2 = 0;
                for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++) {
                    barramentoDeDados.receive(ram);
                    barramentoDeDados.send(cacheVirtual2, posicaoCacheVirtual2);
                    posicaoCacheVirtual2 += Constantes.larguraBarramentoDeDados;

                    //atualiza posicao do ponteiro na ram!
                    end += Constantes.larguraBarramentoDeDados;
                    var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                    barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                    barramentoDeEnderecos.send(ram);
                }
                return Decoder.byteToLongLiteral(cacheVirtual2);
                */
            }

            void atualizaRam(InformacaoMMU bloco)
            {
                long end = bloco.posicaoRam;
                long posicaoCache = bloco.posicaoCache;
                //prepara barramento de controle
                barramentoDeControle.receive(0);
                barramentoDeControle.send(ram);
                //prepara barramento de endereços
                end = (end * tp) + offset;
                var endVetor = Encoder.intToVetorByte(end, lb);
                barramentoDeEnderecos.receive(endVetor.vetor);
                barramentoDeEnderecos.send(ram);
                //clona os dados da cache para a ram usando barramento e clock
                for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++)
                {
                    barramentoDeDados.receive(cacheReal, posicaoCache);
                    barramentoDeDados.send(ram);
                    posicaoCache += Constantes.larguraBarramentoDeDados;

                    //atualiza posicao do ponteiro na ram!
                    end += Constantes.larguraBarramentoDeDados;
                    var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                    barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                    barramentoDeEnderecos.send(ram);
                }
            }

            void escritaValorEndereco(long valor, long end)
            {
                long endRam = (end * tp) + offset;
                //verifica se endereco já está carregado na cache
                //se teste>0 entao temos um cache hit
                int posicaoBlocoCache = cacheReal.tabelaBlocos.FindIndex(p => p.posicaoRam == endRam);
                if (posicaoBlocoCache >= 0)//cache hit
                {
                    InformacaoMMU blocoCache = cacheReal.tabelaBlocos[posicaoBlocoCache];
                    long posicaoCache = blocoCache.posicaoCache;
                    int[] vetorValor = Encoder.literalByte(valor);//vetor em bytes contendo literal a ser gravado
                    //atualiza valor na cache de acordo com a posicao dada pelo bloco
                    for (int i = 0; i < vetorValor.Length; i++)
                    {
                        cacheReal.vetor[posicaoCache + blocoCache.tamanho - 1 - i] = vetorValor[vetorValor.Length - 1 - i];
                    }
                    //verifica se quando numeroAtualizações receber incremento deverá atualizar a ram
                    if ((blocoCache.numeroAtualizacoes + 1) % Constantes.taxaDeAtualizacaoCacheParaRam == 0)
                    {
                        //prepara barramento de controle
                        barramentoDeControle.receive(0);
                        barramentoDeControle.send(ram);
                        //prepara barramento de endereços
                        end = (end * tp) + offset;
                        var endVetor = Encoder.intToVetorByte(end, lb);
                        barramentoDeEnderecos.receive(endVetor.vetor);
                        barramentoDeEnderecos.send(ram);
                        //clona os dados da cache para a ram usando barramento e clock
                        for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++)
                        {
                            barramentoDeDados.receive(cacheReal, posicaoCache);
                            barramentoDeDados.send(ram);
                            posicaoCache += Constantes.larguraBarramentoDeDados;

                            //atualiza posicao do ponteiro na ram!
                            end += Constantes.larguraBarramentoDeDados;
                            var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                            barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                            barramentoDeEnderecos.send(ram);
                        }
                    }
                    blocoCache.numeroAtualizacoes++;
                    blocoCache.instanteUltimoAcesso = instante;
                }
                else
                {
                    //cache miss
                    //alimentar o cache com essa informação!!!

                    //se cache esta com preenchumento acima de 80%
                    if (((double)cacheReal.preenchimento / cacheReal.vetor.Length) > porcentagem)
                    {
                        //encontrar blocos que tenham tamanho maior ou igual a uma palavra
                        var r1 = cacheReal.tabelaBlocos.Where(p => p.tamanho >= Constantes.tamanhoPalavra).ToList();
                        //pegar alguem que tenha numeroAcessos minimo desempatado por instante
                        var r2 = r1.OrderBy(p => p.numeroAcessos).ThenBy(p => p.instanteUltimoAcesso).ToList();
                        InformacaoMMU blocoParaExcluir = r2[0];
                        atualizaRam(blocoParaExcluir);//atualiza ram antes de excluir bloco
                        //gravar novas informações em vetor cache
                        long posicaoCache = blocoParaExcluir.posicaoCache;
                        int[] vetorValor = Encoder.literalByte(valor);//vetor em bytes contendo literal a ser gravado
                                                                      //atualiza valor na cache de acordo com a posicao dada pelo bloco
                        for (int i = 0; i < vetorValor.Length; i++)
                        {
                            cacheReal.vetor[posicaoCache + i] = vetorValor[i];
                        }

                        //cria novo bloco mmu com base nos dados carregados
                        int tamanho = tp;
                        long posicao = blocoParaExcluir.posicaoCache;
                        long posicaoRam = endRam;
                        var novoBloco = new InformacaoMMU(tamanho, posicao, posicaoRam, instante);
                        cacheReal.tabelaBlocos.Add(novoBloco);
                        novoBloco.numeroAtualizacoes++;//este valor já é criado com diferença entre o valor em cache e o valor em ram

                        //exclui bloco antigo
                        cacheReal.tabelaBlocos.Remove(blocoParaExcluir);

                    }
                    else
                    {
                        long posicaoCache = cacheReal.preenchimento;//posicaoCache aponta para primeiro byte do bloco na cache
                        //cria novo bloco com base nos dados carregados
                        int tamanho = tp;
                        long posicaoRam = endRam;
                        var novoBloco = new InformacaoMMU(tamanho, posicaoCache, posicaoRam, instante);
                        cacheReal.tabelaBlocos.Add(novoBloco);
                        novoBloco.numeroAtualizacoes++;//este valor já é criado com diferença entre o valor em cache e o valor em ram
                        //atualiza informação de preenchimento
                        cacheReal.preenchimento += tamanho;
                    }



                    //metodo antigo (sem cache)
                    /*
                    end = (end * tp) + offset;
                    var endVetor = Encoder.intToVetorByte(end, lb);
                    barramentoDeControle.receive(0);
                    barramentoDeControle.send(ram);
                    barramentoDeEnderecos.receive(endVetor.vetor);
                    barramentoDeEnderecos.send(ram);
                    var cacheVirtual2 = Encoder.literalByte(valor);
                    for (int i = 0; i < tp / Constantes.larguraBarramentoDeDados; i++)
                    {
                        barramentoDeDados.receive(cacheVirtual2);
                        barramentoDeDados.send(ram);
                        auxiliar.Auxiliar.popVetorLarguraDados(cacheVirtual2);

                        //atualiza posicao do ponteiro na ram!
                        end += Constantes.larguraBarramentoDeDados;
                        var NovoEndVetor = Encoder.intToVetorByte(end, Constantes.larguraBarramentoDeEndereco);
                        barramentoDeEnderecos.receive(NovoEndVetor.vetor);
                        barramentoDeEnderecos.send(ram);
                    }
                    */
                }
            }

            long valorLiteral(int qtdPalavras, int qtdEnds)
            {
                int pos = qtdPalavras * tp + qtdEnds * lb;
                int[] vetorLiteral = new int[tp];
                for (int i = 0; i < tp; i++)
                {
                    vetorLiteral[i] = cacheVirtual[pos + i];
                }
                return Decoder.byteToLongLiteral(vetorLiteral);
            }

            void enviaResultadoParaBuffer(int[] resultado)
            {
                for (int i = 0; i < resultado.Length / Constantes.larguraBarramentoDeDados; i++)
                {
                    barramentoDeDados.receive(resultado);
                    auxiliar.Auxiliar.popVetorLarguraDados(resultado);
                    auxiliar.Auxiliar.insereZerosNoInicio(ModuloES.buffer, Constantes.larguraBarramentoDeDados);
                    barramentoDeDados.send(ModuloES.buffer, 0);
                }
            }

            int[] vetorResultado = new int[Constantes.tamanhoPalavra];

            //add R i
            if (codigo == 1)
            {
                int codR = identificaCodigoRegistrador(1, 0);
                long valorR = valorRegistrador(codR);
                long valorL = valorLiteral(2, 0);
                long soma = valorR + valorL;
                int[] somaVetor = Encoder.literalByte(soma);
                armazenaNoRegistrador(somaVetor, codR);
                vetorResultado = Encoder.literalByte(soma);
            }
            //add R R
            if (codigo == 2)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                int codR2 = identificaCodigoRegistrador(2, 0);
                long valorR1 = valorRegistrador(codR1);
                long valorR2 = valorRegistrador(codR2);
                long soma = valorR1 + valorR2;
                int[] somaVetor = Encoder.literalByte(soma);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(soma);
            }
            //add e R
            if (codigo == 3)
            {
                long end1 = identificaEndereco(1, 0);
                long valorE1 = leituraValorEndereco(end1);
                int codR1 = identificaCodigoRegistrador(1, 1);
                long valorR1 = valorRegistrador(codR1);
                long soma = valorE1 + valorR1;
                escritaValorEndereco(soma, end1);
                vetorResultado = Encoder.literalByte(soma);
            }
            //add e i
            if (codigo == 4)
            {
                long end1 = identificaEndereco(1, 0);
                long valorE1 = leituraValorEndereco(end1);
                long valorL1 = valorLiteral(1, 1);
                long soma = valorE1 + valorL1;
                escritaValorEndereco(soma, end1);
                vetorResultado = Encoder.literalByte(soma);
            }
            //mov R i
            if (codigo == 5)
            {
                int codR = identificaCodigoRegistrador(1, 0);
                long valorL = valorLiteral(2, 0);
                long soma = valorL;
                int[] somaVetor = Encoder.literalByte(soma);
                armazenaNoRegistrador(somaVetor, codR);
                vetorResultado = Encoder.literalByte(soma);
            }
            //mov R R
            if (codigo == 6)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                int codR2 = identificaCodigoRegistrador(2, 0);
                long valorR2 = valorRegistrador(codR2);
                long soma = valorR2;
                int[] somaVetor = Encoder.literalByte(soma);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(soma);
            }
            //mov e R
            if (codigo == 7)
            {
                long end1 = identificaEndereco(1, 0);
                int codR1 = identificaCodigoRegistrador(1, 1);
                long valorR1 = valorRegistrador(codR1);
                long soma = valorR1;
                escritaValorEndereco(soma, end1);
                vetorResultado = Encoder.literalByte(soma);
            }
            //mov e i
            if (codigo == 8)
            {
                long end1 = identificaEndereco(1, 0);
                long valorL1 = valorLiteral(1, 1);
                long soma = valorL1;
                escritaValorEndereco(soma, end1);
                vetorResultado = Encoder.literalByte(soma);
            }
            //inc R
            if (codigo == 9)
            {
                int codR = identificaCodigoRegistrador(1, 0);
                long valorR = valorRegistrador(codR);
                long soma = valorR + 1;
                int[] somaVetor = Encoder.literalByte(soma);
                armazenaNoRegistrador(somaVetor, codR);
                vetorResultado = Encoder.literalByte(soma);
            }
            //inc e
            if (codigo == 10)
            {
                long end1 = identificaEndereco(1, 0);
                long valorE1 = leituraValorEndereco(end1);
                long soma = valorE1 + 1;
                escritaValorEndereco(soma, end1);
                vetorResultado = Encoder.literalByte(soma);
            }
            //imul R R R
            if (codigo == 11)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                int codR2 = identificaCodigoRegistrador(2, 0);
                int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR2 = valorRegistrador(codR2);
                long valorR3 = valorRegistrador(codR3);
                long mult = valorR2 * valorR3;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //imul R R i
            if (codigo == 12)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                int codR2 = identificaCodigoRegistrador(2, 0);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                long valorL1 = valorLiteral(3, 0);
                long mult = valorR2 * valorL1;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //R i R
            if (codigo == 13)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                int codR2 = identificaCodigoRegistrador(3, 0);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                long valorL1 = valorLiteral(2, 0);
                long mult = valorR2 * valorL1;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //R i i
            if (codigo == 14)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                //int codR2 = identificaCodigoRegistrador(2, 0);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                long valorL1 = valorLiteral(2, 0);
                long valorL2 = valorLiteral(3, 0);
                long mult = valorL2 * valorL1;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //R e R
            if (codigo == 15)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                //long valorL1 = valorLiteral(2, 0);
                //long valorL2 = valorLiteral(3, 0);
                long end1 = identificaEndereco(2, 0);
                long valorE1 = leituraValorEndereco(end1);
                long mult = valorE1 * valorR2;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //R e i
            if (codigo == 16)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                long valorL1 = valorLiteral(2, 1);
                //long valorL2 = valorLiteral(3, 0);
                long end1 = identificaEndereco(2, 0);
                long valorE1 = leituraValorEndereco(end1);
                long mult = valorE1 * valorL1;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //R e e
            if (codigo == 17)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                //long valorL1 = valorLiteral(2, 0);
                //long valorL2 = valorLiteral(3, 0);
                long end1 = identificaEndereco(2, 0);
                long valorE1 = leituraValorEndereco(end1);
                long end2 = identificaEndereco(2, 1);
                long valorE2 = leituraValorEndereco(end2);
                long mult = valorE1 * valorE2;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //R R e
            if (codigo == 18)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                int codR2 = identificaCodigoRegistrador(2, 0);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                //long valorL1 = valorLiteral(2, 0);
                //long valorL2 = valorLiteral(3, 0);
                long end1 = identificaEndereco(3, 0);
                long valorE1 = leituraValorEndereco(end1);
                //long end2 = identificaEndereco(2, 1);
                //long valorE2 = leituraValorEndereco(end2);
                long mult = valorR2 * valorE1;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //R i e
            if (codigo == 19)
            {
                int codR1 = identificaCodigoRegistrador(1, 0);
                //int codR2 = identificaCodigoRegistrador(2, 0);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR2 = valorRegistrador(codR2);
                //long valorR3 = valorRegistrador(codR3);
                long valorL1 = valorLiteral(2, 0);
                //long valorL2 = valorLiteral(3, 0);
                long end1 = identificaEndereco(3, 0);
                long valorE1 = leituraValorEndereco(end1);
                //long end2 = identificaEndereco(2, 1);
                //long valorE2 = leituraValorEndereco(end2);
                long mult = valorL1 * valorE1;
                int[] somaVetor = Encoder.literalByte(mult);
                armazenaNoRegistrador(somaVetor, codR1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e R R
            if (codigo == 20)
            {
                long end1 = identificaEndereco(1, 0);
                //long end2 = identificaEndereco(2, 1);
                //long valorE2 = leituraValorEndereco(end2);
                int codR1 = identificaCodigoRegistrador(1, 1);
                int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR1 = valorRegistrador(codR1);
                long valorR2 = valorRegistrador(codR2);
                //long valorL1 = valorLiteral(2, 0);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorR1 * valorR2;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e R i
            if (codigo == 21)
            {
                long end1 = identificaEndereco(1, 0);
                //long end2 = identificaEndereco(2, 1);
                //long valorE2 = leituraValorEndereco(end2);
                int codR1 = identificaCodigoRegistrador(1, 1);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                long valorL1 = valorLiteral(2, 1);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorR1 * valorL1;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e i R
            if (codigo == 22)
            {
                long end1 = identificaEndereco(1, 0);
                //long end2 = identificaEndereco(2, 1);
                //long valorE2 = leituraValorEndereco(end2);
                int codR1 = identificaCodigoRegistrador(2, 1);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                long valorL1 = valorLiteral(1, 1);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorR1 * valorL1;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e i i
            if (codigo == 23)
            {
                long end1 = identificaEndereco(1, 0);
                //long end2 = identificaEndereco(2, 1);
                //long valorE2 = leituraValorEndereco(end2);
                //int codR1 = identificaCodigoRegistrador(2, 1);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                long valorL1 = valorLiteral(1, 1);
                long valorL2 = valorLiteral(2, 1);

                long mult = valorL2 * valorL1;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e e R
            if (codigo == 24)
            {
                long end1 = identificaEndereco(1, 0);
                long end2 = identificaEndereco(1, 1);
                long valorE2 = leituraValorEndereco(end2);
                int codR1 = identificaCodigoRegistrador(1, 2);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                //long valorL1 = valorLiteral(1, 1);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorR1 * valorE2;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e e i
            if (codigo == 25)
            {
                long end1 = identificaEndereco(1, 0);
                long end2 = identificaEndereco(1, 1);
                long valorE2 = leituraValorEndereco(end2);
                //int codR1 = identificaCodigoRegistrador(1, 2);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                long valorL1 = valorLiteral(1, 2);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorL1 * valorE2;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e e e
            if (codigo == 26)
            {
                long end1 = identificaEndereco(1, 0);
                long end2 = identificaEndereco(1, 1);
                long end3 = identificaEndereco(1, 2);
                long valorE2 = leituraValorEndereco(end2);
                long valorE3 = leituraValorEndereco(end3);
                //int codR1 = identificaCodigoRegistrador(1, 2);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                //long valorL1 = valorLiteral(1, 2);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorE3 * valorE2;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e R e
            if (codigo == 27)
            {
                long end1 = identificaEndereco(1, 0);
                long end2 = identificaEndereco(2, 1);
                //long end3 = identificaEndereco(1, 2);
                long valorE2 = leituraValorEndereco(end2);
                //long valorE3 = leituraValorEndereco(end3);
                int codR1 = identificaCodigoRegistrador(1, 1);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                //long valorL1 = valorLiteral(1, 2);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorR1 * valorE2;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }
            //e i e
            if (codigo == 28)
            {
                long end1 = identificaEndereco(1, 0);
                long end2 = identificaEndereco(2, 1);
                //long end3 = identificaEndereco(1, 2);
                long valorE2 = leituraValorEndereco(end2);
                //long valorE3 = leituraValorEndereco(end3);
                //int codR1 = identificaCodigoRegistrador(1, 1);
                //int codR2 = identificaCodigoRegistrador(2, 1);
                //int codR3 = identificaCodigoRegistrador(3, 0);
                //long valorR1 = valorRegistrador(codR1);
                //long valorR2 = valorRegistrador(codR2);
                long valorL1 = valorLiteral(1, 1);
                //long valorL2 = valorLiteral(3, 0);

                long mult = valorL1 * valorE2;
                escritaValorEndereco(mult, end1);
                vetorResultado = Encoder.literalByte(mult);
            }

            //instrução label i
            if (codigo == 50)
            {
                int valorL1 = (int)valorLiteral(1, 0);
                var r1 = cacheReal.tabelaBlocos.OrderByDescending(p => p.id_interno).ToList();
                Label label = new Label(valorL1, r1[0]);
                listaDeLabels.Add(label);
            }

            //loop => A<100 : jmp 1 : brk = loop 1, A, 100 = [0,51,0,1,0,0,0,100]
            if (codigo == 51)
            {
                int codR1 = identificaCodigoRegistrador(2, 0);
                long valorR1 = valorRegistrador(codR1);
                int idLabel = (int)valorLiteral(1, 0);
                long valorLimite = valorLiteral(3, 0);

                //leitura da posicao CACHE da label
                var pos = this.listaDeLabels.FindIndex(p => p.id == idLabel);
                var labelCache = listaDeLabels[pos].bloco.posicaoCache;

                //leitura da posicao CACHE da instrucao loop
                long loopCache = cacheReal.tabelaBlocos.Find(p => p.instanteUltimoAcesso == instante).posicaoCache;

                //lista instrucoes dentro do loop
                var r1 = cacheReal.tabelaBlocos.Where(p => p.posicaoCache > labelCache && p.posicaoCache < loopCache).ToList();
                while (valorR1 < valorLimite)
                {
                    foreach (var item in r1)
                    {
                        if (item.tamanho > tp) // garante que não vai tentar executar um valor de posição de memoria que esteja na cache
                        {
                            this.Pi[1] = (int)item.posicaoRam; 
                            executaInstrucaoLoop(item);
                            item.numeroAcessos++;
                            item.instanteUltimoAcesso = instante;
                        }
                    }

                    valorR1 = valorRegistrador(codR1);
                }
            }


            //enviaResultadoParaBuffer(vetorResultado);
            this.instante++;


            int[] capturaInstrucao(InformacaoMMU bloco)
            {
                int[] instrucao = new int[bloco.tamanho];
                for (int i = 0; i < bloco.tamanho; i++)
                {
                    instrucao[i] = cacheReal.vetor[bloco.posicaoCache + i];
                }
                return instrucao;
            }

            void executaInstrucaoLoop(InformacaoMMU bloco)
            {
                int[] instrucao = capturaInstrucao(bloco);
                int cod = cacheReal.vetor[bloco.posicaoCache + tp - 1];
                executaInstrucao(cod, instrucao, barramentoDeDados, barramentoDeEnderecos, barramentoDeControle, ram, ModuloES, cacheReal);
            }


        }

    }
}
