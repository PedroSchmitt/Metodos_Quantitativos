using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Programacao.Linear
{
    public class Operacao
    {
        public enum operacao {
            menor = 0,
            maior = 1,
            igual = 2
        }

        public operacao oper;

        public Operacao(int op)
        {
            switch (op)
            {
                case 0:
                    this.oper = operacao.menor;
                    break;
                case 1:
                    this.oper = operacao.maior;
                    break;
                case 2:
                    this.oper = operacao.igual;
                    break;
            }
        }
    }
}
