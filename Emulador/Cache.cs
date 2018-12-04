using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emulador
{
    //toda comunicação de execução do cpu deverá acessar somente o cache
    //toda a lógica de acesso a ram durante a execução deverá ser simplesmente substituida por cache
    //passando por um conversor de interrupção
    class Cache
    {
        public int preenchimento = 0;//marcação de posição já preenchida para o preenchimento sem algoritmo
        public int[] vetor = new int[(int)(Constantes.tamanhoRam / 4)];


        public List<InformacaoMMU> tabelaBlocos = new List<InformacaoMMU>();
        
        

        public void imprimeCache()
        {
            string s = "Cache\n[ ";
            foreach (var item in vetor)
            {
                s += item + " ";
            }
            s += "]";
            Console.WriteLine(s);
        }
    }
}
