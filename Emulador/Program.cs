using System;
using System.Collections.Generic;

namespace Emulador {

    static class Constantes {
        public const int tamanhoPalavra = 16 / 8; //Tamanho da palavra em bits [16, 32 ou 64]
        public const int larguraBarramentoDeDados = 8 / 8;
        public const int larguraBarramentoDeEndereco = 8 / 8; //Largura do barramento em bits [8, 16 ou 32]
        public const int tamanhoBuffer = 128; //Tamanho do buffer de entrada/saída em bytes [64, 128 ou 256]
        //Tamanho da RAM em bytes [128, 256 ou 512]
    }

    class Program {
        static void Main(string[] args) {

            var moduloES = new EntradaSaida();

            moduloES.preencheBuffer();

            moduloES.imprimeBuffer();

            


            Console.ReadLine();
        }
    }
}

/* O buffer finalmente está preenchido!!! Corretamente, ao que parece!
 * 
 * Agora é hora de criar os barramentos e a ram e começar a pensar em como será a transferencia
 * ***Lembrar que os barramentos devem usar herança!
 * 
 * o moduloES deve ser capaz de reconhecer a instrução para organizar o uso dos diferentes barramentos
 * assim como saber o tamanho da instruções
 * 
 * é preciso pensar a respeito se será necessário implementar algo parecido com o que foi chamado de
 * interrupção da cpu
 * 
 * pensar em como será a questão da taxa de transferência! Se minha taxa de transferencia por de 100
 * bytes por segundo, então para cada vez que eu apertar enter (simulando um segundo) 100 bytes devem
 * sair do buffer e ir para a ram (lembrar que não será preciso ser exatamente 100 bytes pois o professor
 * falou que não seria necessário quebrar instruções, portanto deve se enviar o maximo de instruções 
 * completas que cabem em 100 bytes)
 * 
 * Pergunta: para a instrução (add 0x0F, 5) temos [4,15,5], o 15 (valor de endereço) não será usado de
 * imediato, pois faz parte de uma instrução a ser armazenada na memoria principal, portanto esse 15
 * é na verdade para ser enviado no barramento de dados. O barramento de endereço só precisa saber o
 * endereço que esta sendo usado agora, por exemplo, para qual endereço vou enviar esse [4,15,5] neste
 * exato momento? Então, pelo que entendi, o barramento de endereço não é para "trasnferir 
 * dados de endereços", é somente para indicar o endereço atual!
 *
 * Pergunta 2: Se o valor de endereço vai ser usado como um dado qualquer, talvez ele tambem seja guardado
 * na ram usando o tamanho de uma palavra, visto que tudo na ram usa palavra como base. O interessante
 * disso é que tornaria o programa muito mais simples, especialmente a cpu, pois não haveria toda a
 * preocupação em cuidar da leitura da instrução de acordo com o tamanho de bytes de uma palavra ou
 * endereço. Somente na hora de carregar o barramento de endereço é que o endereço passaria a de fato
 * ter somente a quantidade de bytes do barramento de endereço. Para isso é preciso criar um verificador
 * previo para garantir que não será armazenado nenhum valor de endereço maior do que o meu intervalo
 * de endereçamento!!!!!!!!! Primeira tarefa para amanhã será tornar os dados de endereço do tamanho de
 * uma palavra, vai facilitar muito o restante do trabalho.
 */