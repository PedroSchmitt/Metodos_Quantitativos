using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transporte.Vogil
{
    public class CelulaDaMatriz
    {
        private Fracao.Fracao valor;

        public CelulaDaMatriz(Fracao.Fracao valor)
        {
            setValor(valor);
        }

        public void setValor(Fracao.Fracao valor)
        {
            if (valor.comparar(new Fracao.Fracao(0)) == 0)
                this.valor = null;
            this.valor = valor;
        }

        public Fracao.Fracao getValor()
        {
            return valor;
        }
    }
}
