using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Fracao
{
    public class Fracao
    {
        //n /d = numerador / denominador

        private BigInteger numerador;
        private BigInteger denominador;
        
        public Fracao()
        {
        }

        public Fracao(BigInteger numerador)
        {
            setNumerador(numerador);
            setDenominador(new BigInteger(1));
        }

        public Fracao(BigInteger numerador, BigInteger denominador)
        {
            setDenominador(denominador);
            setNumerador(numerador);
        }

        public Fracao(string fracao)
        {
            int p = fracao.IndexOf('/');

            string num = "";
            string den = "";
            if (p != -1)
            {
                num = fracao.Substring(0, p) + "";
                p++;
                den = fracao.Substring(p, fracao.Length-p) + "";
            }
            else
            {
                num = fracao.Substring(0, fracao.Length).ToString() + "";
                den = "1";
            }
            this.numerador = new BigInteger(int.Parse(num));
            this.denominador = new BigInteger(int.Parse(den));
        }

        #region Simplificação
        public void simplificar()
        {
            BigInteger mdc = mdcDosValores(getNumerador(), getDenominador());
            if (mdc != 0)
            {
                setNumerador(getNumerador() / mdc);
                setDenominador(getDenominador() / mdc);
                if (getDenominador() < 0)
                {
                    setDenominador(getDenominador() * -1);
                    setNumerador(getNumerador() * -1);
                }
            }
        }

        private BigInteger mdcDosValores(BigInteger bigInteger, BigInteger bigInteger_2)
        {
            if (bigInteger_2 == 0)
                return bigInteger;
            return mdcDosValores(bigInteger_2, (bigInteger % bigInteger_2));
        }
        #endregion

        #region Comparação e equals

        public bool equals(Fracao fracao, bool simplifacarAsFracoes)
        {
            #region Simplifiacando as frações
            if (simplifacarAsFracoes)
            {
                this.simplificar();
                fracao.simplificar();
            }
            #endregion

            if (this.getDenominador() != fracao.getDenominador())
                return false;
            if (this.getNumerador() != fracao.getNumerador())
                return false;
            return true;
        }
        
        public int comparar(Fracao fracao)
        {
            // this > fracao = +1
            // this < fracao = -1
            // this = fracao = 0

            /*
            5  9    ->   7*13 = 91   ->  5*13 = 65   ->   9*7 = 63   ----->>>>  65 é maior que 63, então 5 é maior que 9
            7  13                        7*13   91        13*7  91              91             91        7             13
             */
            Fracao fThis = new Fracao(this.getNumerador() * fracao.getDenominador(), this.getDenominador() * fracao.getDenominador());
            Fracao fFracao = new Fracao(fracao.getNumerador() * this.getDenominador(), fracao.getDenominador() * this.getDenominador());

            if (fThis.getNumerador() > fFracao.getNumerador())
                return +1;
            else if (fThis.getNumerador() < fFracao.getNumerador())
                return -1;
            else
                return 0;
        }
        
        #endregion

        #region Elevação ou potencia
        public Fracao elevarPotencia(BigInteger potencia)
        {
            //this.simplificar();
            Fracao resultado = this;
            if (potencia == BigInteger.Zero)
                resultado = new Fracao(1);
            else
            {
                for (BigInteger i = 1; i < potencia; i++)
                {
                    resultado = resultado.multiplicar(this);
                }
                resultado.simplificar();
            }
            return resultado;
        }
        #endregion

        #region Divisão e multiplicação
        public Fracao multiplicar(Fracao fracao)
        {
            //simplificar();
            //fracao.simplificar();
            Fracao retorno = new Fracao();
            retorno.setDenominador(this.getDenominador() * fracao.getDenominador());
            retorno.setNumerador(this.getNumerador() * fracao.getNumerador());
            retorno.simplificar();
            return retorno;
        }

        public Fracao dividir(Fracao fracao)
        {
            //simplificar();
            //fracao.simplificar();
            // 2/3 / 4/5 = 2/3 * 5/4 = 10/12
            Fracao f = new Fracao(fracao.getDenominador(), fracao.getNumerador());
            return this.multiplicar(f);
        }
        #endregion

        #region Adição e Subtração
        public Fracao subtrair(Fracao fracao)
        {
            //simplificar();
            //fracao.simplificar();
            Fracao resultado = new Fracao();
            resultado.setNumerador((this.getNumerador() * fracao.getDenominador()) - (this.getDenominador() * fracao.getNumerador()));
            resultado.setDenominador(this.getDenominador() * fracao.getDenominador());
            // 3/5 - 2/3 = (3*3 - 5*2) / 5*3 = (9 - 10) / 15 = -1/15
            resultado.simplificar();
            return resultado;
        }

        public Fracao adicionar(Fracao fracao)
        {
            //simplificar();
            //fracao.simplificar();
            // Adição
            Fracao resultado = new Fracao();
            resultado.setNumerador((this.getNumerador() * fracao.getDenominador()) + (this.getDenominador() * fracao.getNumerador()));
            resultado.setDenominador(this.getDenominador() * fracao.getDenominador());
            // 3/5 + 2/3 = (3*3 + 5*2) / 5*3 = 19/15
            resultado.simplificar();
            return resultado;
        }
        #endregion

        #region Set e Get
        public BigInteger getNumerador()
        {
            return this.numerador;
        }
        public BigInteger getDenominador()
        {
            return this.denominador;
        }

        public void setNumerador(BigInteger numerador)
        {
            this.numerador = numerador;
        }
        public void setDenominador(BigInteger denominador)
        {
            if (denominador == 0)
                throw new Exception("O denominador da fração não pode ser zero (0).");
            this.denominador = denominador;
        }
        #endregion

        public string toString()
        {
            return this.getNumerador() + "/" + this.getDenominador();
        }

        public string toStringDecimal()
        {
            try
            {
                decimal num = (decimal)getNumerador();
                decimal den = (decimal)getDenominador();
                return (num / den) + "";
            }
            catch (Exception e)
            {
                return "Erro ao converter para decimal";
            }
        }
    }
}
