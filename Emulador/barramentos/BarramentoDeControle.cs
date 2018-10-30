using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador.barramentos {
    class BarramentoDeControle : Barramento {
        public BarramentoDeControle(int largura) : base(largura) {
        }

        public void receive(int sinalDeControle) {
            this.fila.Enqueue(sinalDeControle);
        }

        public void send (Ram ram) {
            ram.ArmazenadorDeSinalDeControle = fila.Dequeue();
            ram.leituraSinalDeControle();
        }

    }
}
