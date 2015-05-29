using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Programacao.Linear
{
    public class VariavelDentro
    {
        private Variavel variavel;
        private Fracao.Fracao valor;

        public void setVariavel(Variavel var)
        {
            this.variavel = var;
        }
        public void setValor(Fracao.Fracao valor)
        {
            this.valor= valor;
        }
        public Variavel getVariavel()
        {
            return this.variavel;
        }
        public Fracao.Fracao getValor()
        {
            return this.valor;
        }
    }
}
