using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;

namespace Bissecao
{
    public class CalculoDoErro
    {
        private Fracao.Fracao a = null;
        private Fracao.Fracao b = null;
        private Fracao.Fracao media = null;
        private Fracao.Fracao resultadoA = null;
        private Fracao.Fracao resultadoB = null;
        private Fracao.Fracao resultadoMedia = null;
        private ErroRelativo erro = null;

        public CalculoDoErro(Fracao.Fracao a, Fracao.Fracao b)
        {
            this.a = a;
            this.b = b;
            this.media = a.adicionar(b).dividir(new Fracao.Fracao(new BigInteger(2)));
        }
        public CalculoDoErro(Fracao.Fracao a, Fracao.Fracao resultadoA, Fracao.Fracao b, Fracao.Fracao resultadoB)
        {
            this.a = a;
            this.b = b;
            this.resultadoA = resultadoA;
            this.resultadoB = resultadoB;
            this.media = a.adicionar(b).dividir(new Fracao.Fracao(new BigInteger(2)));
        }

        public CalculoDoErro calcularResultados(Fracao.Funcao funcao)
        {
            this.resultadoA = funcao.aplicarValor(a);// resultadoA.simplificar();
            this.resultadoB = funcao.aplicarValor(b);// resultadoB.simplificar();
            this.resultadoMedia = funcao.aplicarValor(media);// resultadoMedia.simplificar();

            #region Verifica se não deu valor zero
            if (resultadoMedia.equals(new Fracao.Fracao(new BigInteger(0)), true))
            {
                return null;
            }
            else if (resultadoA.equals(new Fracao.Fracao(new BigInteger(0)), true))
            {
                return null;
            }
            else if (resultadoB.equals(new Fracao.Fracao(new BigInteger(0)), true))
            {
                return null;
            }
            #endregion

            CalculoDoErro proximoCalculo;
            if (resultadoMedia.comparar(new Fracao.Fracao(new BigInteger(0))) == resultadoA.comparar(new Fracao.Fracao(new BigInteger(0))))
            {
                // media é menor
                proximoCalculo = new CalculoDoErro(this.media, resultadoMedia, b, resultadoB);
            }
            else
            {
                // media é maior ou igual
                proximoCalculo = new CalculoDoErro(a, resultadoA, this.media, resultadoMedia);
            }
            return proximoCalculo;
        }

        public void calcularErro(CalculoDoErro calculoAnterior)
        {
            this.erro = new ErroRelativo(getMedia(), calculoAnterior.getMedia());
        }

        public ErroRelativo getErro()
        {
            if (this.media == null)
                throw new Exception("O calcular erro ainda não foi executado! Erro ainda está nulo.");
            return this.erro;
        }
        public Fracao.Fracao getMedia()
        {
            if (this.media == null)
                throw new Exception("O calcular resultados ainda não foi executado! Média ainda está nula.");
            return this.media;
        }

        public object[] toColum(int count)
        {
            object[] o = { count, this.a.toString(), this.media.toString(), this.b.toString(),
                          this.resultadoA.toString(), this.resultadoMedia.toString(), this.resultadoB.toString(),  (erro == null?"":this.erro.toString())};
            
            return o;
        }

        public object[] toColumResultado()
        {
            object[] o = { "", "Resultado:", this.media.toStringDecimal(), "", "", "", "",  ""};
            return o;
        }
    }
}
