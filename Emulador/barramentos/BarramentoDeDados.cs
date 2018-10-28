using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador.barramentos {
    class BarramentoDeDados : Barramento {
        public BarramentoDeDados(int largura) : base(largura) {
        }

        public void receive(int[] vetorMemoria, int localMemoria) {
            for (int i = 0; i < largura; i++) {
                fila.Enqueue(vetorMemoria[localMemoria + i]);
            }
        }

        public void send(int[] vetorMemoria, int localMemoria) {
            for (int i = 0; i < largura; i++) {
                vetorMemoria[localMemoria + i] = fila.Dequeue();
            }
        }

    }
}
