using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador.auxiliar {
    static class Auxiliar {

        public static int popVetor(int[] vetor) {
            int pop = vetor[0];
            for (int i = 0; i < vetor.Length; i++) {
                vetor[i] = vetor[i + 1];
            }
            vetor[vetor.Length - 1] = 0;
            return pop;
        }

    }
}
