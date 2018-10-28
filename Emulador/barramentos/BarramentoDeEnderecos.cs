using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador.barramentos {
    class BarramentoDeEnderecos : Barramento {
        public BarramentoDeEnderecos(int largura) : base(largura) {
        }

        public void receive(int[] vetor) {
            for (int i = 0; i < largura; i++) {
                fila.Enqueue(vetor[i]);
            }
        }

        public void send(int[] vetor) {
            for (int i = 0; i < largura; i++) {
                vetor[i] = fila.Dequeue();
            }
        }
    }
}
