using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newton
{
    public class CalculoDoErro
    {
        private Fracao.Fracao x = null;
        private Fracao.Fracao x1 = null;
        private Fracao.Fracao resultadoFuncao = null;
        private Fracao.Fracao resultadoFuncaoDerivada = null;
        private ErroRelativo erro = null;

        public CalculoDoErro(Fracao.Fracao x)
        {
            this.x = x;
        }

        public CalculoDoErro calcularResultados(Fracao.Funcao funcao, Fracao.Funcao funcaoDerivada)
        {
            this.resultadoFuncao = funcao.aplicarValor(x);
            this.resultadoFuncaoDerivada = funcaoDerivada.aplicarValor(x);
            
            CalculoDoErro proximoCalculo = null;
            x1 = x.subtrair(resultadoFuncao.dividir(resultadoFuncaoDerivada));
            proximoCalculo = new CalculoDoErro(x1);
            return proximoCalculo;
        }

        public void calcularErro()
        {
            this.erro = new ErroRelativo(x1, x);
        }

        public ErroRelativo getErro()
        {
            if (this.erro == null)
                throw new Exception("O calcular erro ainda não foi executado!");
            return this.erro;
        }
        
        public object[] toColum(int count)
        {
            object[] o = { count, this.x.toString(), this.resultadoFuncao.toString(), this.resultadoFuncaoDerivada.toString(),
                          this.x1.toString(), (erro == null?"":this.erro.toString())};

            return o;
        }

        public object[] toColumResultado()
        {
            object[] o = { "","","", "Resultado:", this.x1.toStringDecimal(), this.erro.toString() };
            return o;
        }
    }
}
