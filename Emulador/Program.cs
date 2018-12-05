using System;
using System.Timers;
using System.Collections.Generic;

namespace Emulador {

    static class Constantes {
        public const int tamanhoPalavra = 16 / 8; //Tamanho da palavra em bits [16, 32 ou 64]
        public const int larguraBarramentoDeDados = 16 / 8;
        public const int larguraBarramentoDeEndereco = 16 / 8; //Largura do barramento em bits [8, 16 ou 32]
        public const int tamanhoBuffer = 256; //Tamanho do buffer de entrada/saída em bytes [64, 128 ou 256]
        public const int tamanhoRam = 256; //Tamanho da RAM em bytes [128, 256 ou 512]
        public const int tamanhoOffset = taxaDeTransferência;//tamanho do offset em bytes

        public const int ciclosPorSegundo = 1;//somente para ajuste do timer
        public const int transferênciasPorCiclo = 10;//Transferências de barramentos em cada ciclo


        public const int taxaDeTransferência = larguraBarramentoDeDados * transferênciasPorCiclo;


        public const int taxaDeAtualizacaoCacheParaRam = 4;
        public const double porcentagemCache = 80;


        //public const bool impressaoDeBarramentos = false;

    }

    class Program {

        private static System.Timers.Timer aTimer;
        public static List<long> resultados = new List<long>();

        static void Main(string[] args) {

            //instancia de todos os componentes
            var moduloES = new EntradaSaida();
            var ram = new Ram();
            var cpu = new CPU();
            var barramentoDeControle = new barramentos.BarramentoDeControle(1);
            var barramentoDeDados = new barramentos.BarramentoDeDados(Constantes.larguraBarramentoDeDados);
            var barramentodeEnderecos = new barramentos.BarramentoDeEnderecos(Constantes.larguraBarramentoDeEndereco);
            var cache = new Cache();

            //carrega buffer com dados de entrada
            moduloES.preencheBuffer();

            Console.WriteLine("Digite 0 para usar o emulador no modo destaque de rajadas");
            //Console.WriteLine("Digite 1 para usar no modo barramento");
            Console.WriteLine("Digite 1 para usar no modo timer");
            Console.WriteLine("Digite 2 para usar no modo destaque de memoria cache");
            int c = int.Parse(Console.ReadLine());
            if (c == 0) {
                emuladorDestaqueRajadas();
            }
            if (c == 1) {
                emuladorTimer();
            }
            if (c == 2)
            {
                emuladorDestaqueCache();
            }

            void emuladorDestaqueCache()
            {
                Console.Clear();
                mostrarDadosComCache();
                Console.Write("\nPressiona enter para enviar próxima rajada");
                Console.ReadLine();
                while (!testeBufferVazio())
                {
                    enviarRajada();
                    Console.Clear();
                    mostrarDadosComCache();
                    Console.Write("\nPressiona enter para executar interrupções");
                    Console.ReadLine();
                    executarRajada();
                    Console.Clear();
                    mostrarDadosComCache();
                    Console.Write("\nPressiona enter para enviar próxima rajada");
                    Console.ReadLine();
                }
            }

            void emuladorDestaqueRajadas() {
                Console.Clear();
                mostrarDados();
                Console.Write("\nPressiona enter para enviar próxima rajada");
                Console.ReadLine();
                while (!testeBufferVazio()) {
                    enviarRajada();
                    Console.Clear();
                    mostrarDados();
                    Console.Write("\nPressiona enter para executar interrupções");
                    Console.ReadLine();
                    executarRajada();
                    Console.Clear();
                    mostrarDados();
                    Console.Write("\nPressiona enter para enviar próxima rajada");
                    Console.ReadLine();
                }
            }

            void emuladorTimer() {
                Console.Clear();
                mostrarDadosComCache();
                SetTimer();
                Console.Read();
                aTimer.Stop();
                aTimer.Dispose();

                //Console.Clear();
                //mostrarDados();
            }

            void mostrarDados() {
                //mostra buffer na tela
                moduloES.imprimeBuffer();

                //mostram memoria principal na tela
                Console.WriteLine("\n");
                ram.imprimeMemoriaRam();

                Console.WriteLine();
                //imprime registradores na tela
                cpu.imprimeRegistradores();

                //mostra lista atual de interrupções
                cpu.imprimeInterrupcoes();

                imprimirResultados();
            }

            void mostrarDadosComCache()
            {
                //mostra buffer na tela
                moduloES.imprimeBuffer();

                //mostram memoria principal na tela
                Console.WriteLine("\n");
                ram.imprimeMemoriaRam();

                //mostra cache
                Console.WriteLine("\n");
                cache.imprimeCache();

                Console.WriteLine();
                //imprime registradores na tela
                cpu.imprimeRegistradores();

                //mostra lista atual de interrupções
                cpu.imprimeInterrupcoes();

                imprimirResultados();
            }

            void enviarRajada() {
                moduloES.rajada(barramentoDeDados, barramentodeEnderecos, barramentoDeControle, ram, cpu);
            }

            void executarRajada() {
                cpu.executaTodasInterrupcoes(moduloES, barramentoDeDados, barramentodeEnderecos,
                barramentoDeControle, ram, cache);
                //moduloES.ResultadoBuffer();
            }

            bool testeBufferVazio() {
                //retorna true se o buffer esta vazio
                //nao tem como usar findIndex em vetor
                for (int i = 0; i < moduloES.buffer.Length; i++) {
                    if (moduloES.buffer[i] != 0) {
                        return false;
                    }
                }
                return true;
            }

            void pulso() {
                Console.Clear();
                enviarRajada();
                executarRajada();
                mostrarDadosComCache();
            }

            void OnTimedEvent(Object source, ElapsedEventArgs e) {
                //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",e.SignalTime);
                
                if (!testeBufferVazio()) {
                    pulso();
                }
                
            }

            void SetTimer() {
                // Create a timer with a two second interval.
                aTimer = new System.Timers.Timer(1000/Constantes.ciclosPorSegundo);
                // Hook up the Elapsed event for the timer. 
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }

            void imprimirResultados() {
                Console.Write("output: ");
                string s = "";
                for (int i = 0; i < resultados.Count; i++) {
                    s += "[" + resultados[resultados.Count - 1 - i] + "]";
                }
                resultados.Clear();
                Console.WriteLine(s);
            }

            Console.ReadLine();

        }





    }
}