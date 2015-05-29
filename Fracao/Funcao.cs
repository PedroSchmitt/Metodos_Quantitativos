using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fracao;
using System.Windows.Forms;
using System.Numerics;

namespace Fracao
{
    public class Funcao
    {
        // Vetor de fracoes
        private Fracao [] funcao;

        public Funcao(Fracao[] funcao)
        {
            this.funcao = funcao;
        }

        public Fracao aplicarValor(Fracao a)
        {
            // Substituir o 'a' pelo 'x' da função
            Fracao resultado = funcao[0];
            for (int i = 1; i < funcao.Count(); i++)
            {
                resultado = resultado.adicionar(funcao[i].multiplicar(a.elevarPotencia(i)));
            }
            return resultado;
        }

        public Fracao[] valorPositivoNegatico()
        {
            Fracao[] posNega = new Fracao[2];

            return posNega;
        }

        public Fracao[] toFuncao()
        {
            return this.funcao;
        }

        public string toString()
        {
            string stri = "";
            Fracao f;
            for (int i = funcao.Count()-1; i >= 0 ; i--)
            {
                f = funcao[i];
                if (f != null)
                {
                    if (f.comparar(new Fracao(new BigInteger(0))) != 0)
                        if (f.comparar(new Fracao(new BigInteger(0))) == -1)
                        {
                            stri = stri + "-" + f.toString() + (i > 0 ? "x"+( i > 1? "^" + i:"" ): "");
                        }
                        else
                        {
                            stri = stri + "+" + f.toString() + (i > 0 ? "x" + (i > 1 ? "^" + i : "") : "");
                        }
                }
            }
            return stri+"=0";
        }

        public Funcao derivar()
        {
            Fracao[] funcaoDerivada = new Fracao[this.funcao.Count() - 1];
            Fracao zero = new Fracao(0);
            for (int i = 1; i < this.funcao.Count(); i++ )
            {
                if (this.funcao[i] != zero)
                {
                    funcaoDerivada[i - 1] = this.funcao[i].multiplicar(new Fracao(i));
                }
            }
            return new Funcao(funcaoDerivada);
        }

        public void calcularZerosDaFuncao(int a, int b, DataGridView tabelaDeZerosDaFuncao)
        {
            int ordem = 1; 
            for (int i = a; i < b; i++)
            {
                int i1 = this.aplicarValor(new Fracao(new BigInteger(i))).comparar(new Fracao(new BigInteger(0)));
                int i2 = this.aplicarValor(new Fracao(new BigInteger(i + 1))).comparar(new Fracao(new BigInteger(0)));
                if (i1 != i2)
                {
                    object[] o = { ordem,i,i+1};
                    tabelaDeZerosDaFuncao.Rows.Add(o);
                    ordem++;
                }
                
            }
        }
    }
}
