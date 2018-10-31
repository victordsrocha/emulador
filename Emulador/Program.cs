using System;
using System.Timers;
using System.Collections.Generic;

namespace Emulador {

    static class Constantes {
        public const int tamanhoPalavra = 16 / 8; //Tamanho da palavra em bits [16, 32 ou 64]
        public const int larguraBarramentoDeDados = 16 / 8;
        public const int larguraBarramentoDeEndereco = 16 / 8; //Largura do barramento em bits [8, 16 ou 32]
        public const int tamanhoBuffer = 128; //Tamanho do buffer de entrada/saída em bytes [64, 128 ou 256]
        public const int tamanhoRam = 256; //Tamanho da RAM em bytes [128, 256 ou 512]
        public const int tamanhoOffset = taxaDeTransferência;//tamanho do offset em bytes

        public const int taxaDeTransferência = 20;//bytes por segundo

    }

    class Program {

        private static System.Timers.Timer aTimer;

        static void Main(string[] args) {

            //instancia de todos os componentes
            var moduloES = new EntradaSaida();
            var ram = new Ram();
            var cpu = new CPU();
            var barramentoDeControle = new barramentos.BarramentoDeControle(1);
            var barramentoDeDados = new barramentos.BarramentoDeDados(Constantes.larguraBarramentoDeDados);
            var barramentodeEnderecos = new barramentos.BarramentoDeEnderecos(Constantes.larguraBarramentoDeEndereco);

            //carrega buffer com dados de entrada
            moduloES.preencheBuffer();

            Console.WriteLine("Digite 0 para usar o emulador no modo destaque de rajadas");
            Console.WriteLine("Digite 1 para usar no modo timer");
            int a = int.Parse(Console.ReadLine());
            if (a == 0) {
                emuladorDestaqueRajadas();
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
            }

            void enviarRajada() {
                Console.WriteLine("\n\nEnvio de rajada de buffer para ram e de interrupções de moduloES para cpu:");
                moduloES.rajada(barramentoDeDados, barramentodeEnderecos, barramentoDeControle, ram, cpu);
            }

            void executarRajada() {
                cpu.executaTodasInterrupcoes(moduloES, barramentoDeDados, barramentodeEnderecos,
                barramentoDeControle, ram);
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


            




            /*
            SetTimer();

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();

            Console.WriteLine("Terminating the application...");
            */

            Console.ReadLine();

        }

        private static void SetTimer() {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e) {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                              e.SignalTime);
        }

    }
}