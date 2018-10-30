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

        public void send(Ram ram) {
            for (int i = 0; i < largura; i++) {
                ram.vetorLeitorDeEndereco[i] = fila.Dequeue();
                ram.leituraEndereco();
            }
        }

        public void send(CPU cpu) {
            for (int i = 0; i < largura; i++) {
                cpu.interrupcoes.Enqueue(this.fila.Dequeue());
            }
        }
    }
}
