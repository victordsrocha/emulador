using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador
{
    class InformacaoMMU
    {
        public int id_interno { get; set; }
        public int tamanho { get; set; }
        public int numeroAtualizacoes { get; set; }
        public int numeroAcessos { get; set; }
        public long posicaoCache { get; set; }
        public long posicaoRam { get; set; }
        public int instanteUltimoAcesso { get; set; }

        static int id = 1;

        public InformacaoMMU(int tamanho, long posicaoCache, long posicaoRam, int instanteUltimoAcesso)
        {
            addId();
            this.tamanho = tamanho;
            this.numeroAtualizacoes = 0;
            this.numeroAcessos = 1;
            this.posicaoCache = posicaoCache;
            this.posicaoRam = posicaoRam;
            this.instanteUltimoAcesso = instanteUltimoAcesso;
        }

        void addId()
        {
            this.id_interno = id;
            id++;
        }
    }
}
