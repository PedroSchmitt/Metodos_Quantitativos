using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Programacao.Linear
{
    public class Restricao
    {
        private List<Fracao.Fracao> coeficiente = new List<Fracao.Fracao>();
        private List<Variavel> variaveis = new List<Variavel>();
        private Operacao operacao;
        private List<Variavel> VariaveisDeFolga = new List<Variavel>();
        private List<Fracao.Fracao> valorDasVariaveisDeFolga = new List<Fracao.Fracao>();
        private Fracao.Fracao resultado;

        public void setVariaveisDeFolga(List<Variavel> var)
        {
            this.VariaveisDeFolga = var;
        }
        public List<Variavel> getVariaveisDeFolga()
        {
            return this.VariaveisDeFolga;
        }
        public List<Fracao.Fracao> getVariaveisDeFolga_Valor()
        {
            return this.valorDasVariaveisDeFolga;
        }
        public void setOperacao(Operacao ope)
        {
            this.operacao = ope;
        }
        public Operacao getOperacao()
        {
            return this.operacao;
        }
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

        public void adicionaVariavel(Variavel v, Fracao.Fracao valor)
        {
            VariaveisDeFolga.Add(v);
            valorDasVariaveisDeFolga.Add(valor);
        }

        public void setResultadoB(Fracao.Fracao fracao)
        {
            this.resultado = fracao;
        }
        public Fracao.Fracao getResultadoB()
        {
            return this.resultado;
        }


        public void dividirPeloPivot(Fracao.Fracao pivot)
        {
            for (int i = 0; i < coeficiente.Count; i++)
            {
                coeficiente[i] = coeficiente[i].dividir(pivot);
            }
            for (int i = 0; i < valorDasVariaveisDeFolga.Count; i++)
            {
                valorDasVariaveisDeFolga[i] = valorDasVariaveisDeFolga[i].dividir(pivot);
            }
            resultado = resultado.dividir(pivot);
        }

        public void aplicaFormulaPivot(Fracao.Fracao pivot, Restricao restricaoPivotiada)
        {
            //Valor antigo - (Valor equivalente * Pivot antigo)
            for (int i = 0; i < coeficiente.Count; i++)
            {
                coeficiente[i] = coeficiente[i].subtrair(restricaoPivotiada.getCoeficiente()[i].multiplicar(pivot));
            }
            for (int i = 0; i < valorDasVariaveisDeFolga.Count; i++)
            {
                valorDasVariaveisDeFolga[i] = valorDasVariaveisDeFolga[i].subtrair(restricaoPivotiada.getVariaveisDeFolga_Valor()[i].multiplicar(pivot));
            }
            resultado = resultado.subtrair(restricaoPivotiada.getResultadoB().multiplicar(pivot));
        }
    }
}
