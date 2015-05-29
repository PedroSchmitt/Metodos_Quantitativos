using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Newton
{
    public class ErroRelativo
    {
        private Fracao.Fracao valorAtual;
        private Fracao.Fracao valorAnterior;
        private Fracao.Fracao valorDoErroFracao;

        public ErroRelativo(decimal erro)
        {
            string e = erro + "";
            int posicao = e.IndexOf(',');
            if (posicao != -1)
                posicao = (e.Length - 1) - posicao;
            string zeros = "1";
            for (int i = 0; i < posicao; i++)
                zeros = zeros + "0";
            valorDoErroFracao = new Fracao.Fracao(new BigInteger(int.Parse(e.Replace(",", ""))), new BigInteger(int.Parse(zeros)));
            valorDoErroFracao.simplificar();
        }

        public ErroRelativo(Fracao.Fracao valorAtual, Fracao.Fracao valorAnterior)
        {
            this.valorAtual = valorAtual;
            this.valorAnterior = valorAnterior;
            valorDoErroFracao = this.valorAtual.subtrair(valorAnterior).dividir(valorAtual);
            if (valorDoErroFracao.getNumerador() < 0)
                valorDoErroFracao.setNumerador(valorDoErroFracao.getNumerador() * -1);
        }

        public string toString()
        {
            try
            {
                decimal num = (decimal)valorDoErroFracao.getNumerador();
                decimal den = (decimal)valorDoErroFracao.getDenominador();
                decimal valorDoErro = (decimal)num / den;
                return valorDoErroFracao.toString() + " = " + valorDoErro;

            }
            catch (Exception r)
            {
                return valorDoErroFracao.toString() + " = Em decimal valor não suportado";
            }
        }

        #region Sets e Gets
        public Fracao.Fracao getErro()
        {
            return valorDoErroFracao;
        }
        #endregion
    }
}
    