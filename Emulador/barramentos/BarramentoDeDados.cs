using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador.barramentos {
    class BarramentoDeDados : Barramento {
        public BarramentoDeDados(int largura) : base(largura) {
        }

        public void receive(int[] vetorMemoria) {
            for (int i = 0; i < largura; i++) {
                fila.Enqueue(vetorMemoria[i]);
            }
        }

        public void receive(EntradaSaida ModuloES) {
            for (int i = 0; i < largura; i++) {
                fila.Enqueue(ModuloES.buffer[i]);
            }
        }

        public void receiveImpar(EntradaSaida ModuloES) {
            fila.Enqueue(ModuloES.buffer[0]);
        }

        public void receive(Ram ram) {
            long posicao = ram.enderecoLido;
            for (int i = 0; i < largura; i++) {
                fila.Enqueue(ram.memoria[i + posicao]);
            }
        }

        public void receiveImpar(Ram ram) {
            long posicao = ram.enderecoLido;
                fila.Enqueue(ram.memoria[posicao]);
        }

        public void send(int[] vetorMemoria, int localMemoria) {
            int count = fila.Count;
            for (int i = 0; i < count; i++) {
                vetorMemoria[i + localMemoria] = fila.Dequeue();
            }
        }

        public void send(Ram ram) {
            long posicaoMemoria = ram.enderecoLido;
            if (!ram.somenteLeitura) {
                int count = this.fila.Count;
                for (int i = 0; i < count; i++) {
                    ram.memoria[ram.enderecoLido + i] = this.fila.Dequeue();
                }
            }
        }

        



        public void send(CPU cpu) {
            for (int i = 0; i < largura; i++) {
                cpu.interrupcoes.Enqueue(this.fila.Dequeue());
            }
        }

    }
}
