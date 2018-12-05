using System;
using System.Collections.Generic;
using System.Text;

namespace Emulador
{
    class Label
    {
        public int id { get; set; }
        public InformacaoMMU bloco { get; set; }

        public Label(int id, InformacaoMMU bloco)
        {
            this.id = id;
            this.bloco = bloco;
        }



        //o tamanho do loop não pode ser maior que o tamanho da cache?
    }
}
