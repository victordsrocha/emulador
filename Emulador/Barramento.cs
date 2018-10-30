using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador {
    abstract class Barramento {

        public Queue<int> fila;
        public int largura;

        public Barramento(int largura) {
            this.fila = new Queue<int>();
            this.largura = largura;
        }

        
        public void imprimeFila() {
            Console.Write("Barramento de dados\n[ ");
            foreach (var item in fila) {
                Console.Write(item + " ");
            }
            Console.WriteLine("]");
        }

    }
}
