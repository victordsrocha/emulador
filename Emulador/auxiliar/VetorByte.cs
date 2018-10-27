using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador.auxiliar {
    class VetorByte {

        //usa essa classe para implementar soma de vetores
        //conversoes tem que estar no decoder ou encoder

        public int[] vetor;

        public VetorByte(int tamanho) {
            vetor = new int[tamanho];
        }


        public override string ToString() {
            string s = "[";
            for (int i = 0; i < vetor.Length; i++) {
                if (i != vetor.Length - 1) {
                    s += +vetor[i] + ",";
                }
                else {
                    s += +vetor[i];
                }
            }
            s += "]";
            return s;
        }
    }
}
