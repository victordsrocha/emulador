using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador {
    static class Decoder {

        public static long byteToLong(int[] vetor) {
            long somatorio = 0;
            for (int i = 0; i < vetor.Length; i++) {
                somatorio += (long)(Math.Pow(256, i) * vetor[vetor.Length - 1 - i]);
            }
            return somatorio;
        }

        public static long byteToLongLiteral(int[] vetor) {
            long somatorio = 0;
            if (vetor[0] < 128) { //indica que o numero é positivo
                for (int i = 0; i < vetor.Length; i++) {
                    somatorio += (long)(Math.Pow(256, i) * vetor[vetor.Length - 1 - i]);
                }
                return somatorio;
            }
            else {
                //os dois comandos abaixos correspondem a uma multiplicacao por -1 na notacao em bytes
                vetor[0] -= 128;
                vetor[vetor.Length - 1] += 1;
                //em seguida é so seguir como se o numero fosse positivo mas retornar um valor negativo *(-1)
                for (int i = 0; i < vetor.Length; i++) {
                    somatorio += (long)(Math.Pow(256, i) * vetor[vetor.Length - 1 - i]);
                }
                return -somatorio;
            }
        }
    }
}
