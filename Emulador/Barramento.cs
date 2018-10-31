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
            string s = "[ ";
            foreach (var item in fila) {
                s += item + " ";
            }
            s += "]";
            Console.WriteLine(s);
        }

    }
}
