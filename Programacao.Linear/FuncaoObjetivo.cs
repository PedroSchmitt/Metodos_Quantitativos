using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fracao;

namespace Programacao.Linear
{
    public class FuncaoObjetivo
    {
        public List<Fracao.Fracao> coeficiente = new List<Fracao.Fracao>();
        private List<Variavel> variaveis = new List<Variavel>();

        public void setCoeficiente(List<Fracao.Fracao> coeficientes)
        {
            this.coeficiente = coeficientes;
        }
        public void setVariaveis(List<Variavel> variaveis)
        {
            this.variaveis = variaveis;
        }
        public List<Fracao.Fracao> getCoeficiente()
        {
            return coeficiente;
        }
        public List<Variavel> getVariaveis()
        {
            return this.variaveis;
        }

        public Fracao.Fracao maiorValorCoeficiente()
        {
            Fracao.Fracao maior = new Fracao.Fracao(0);
            foreach (Fracao.Fracao valor in coeficiente)
            {
                if (valor.comparar(maior) == 1)
                {
                    maior = valor;
                }
            }
            return maior;
        }

        public void adicionarVariavel(Variavel v, Fracao.Fracao valor)
        {
            coeficiente.Add(valor);
            variaveis.Add(v);
        }
    }
}
