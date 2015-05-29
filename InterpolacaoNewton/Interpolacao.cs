using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interpolacao.Newton
{
    public class Interpolacao
    {
        private Fracao.Fracao [] funcaoResultado;
        private Pontos pontos;
        private System.Windows.Forms.DataGridView tabelaResultado = null;
        private Fracao.Fracao[,] matrizCalculo;

        public Interpolacao(Pontos pontos)
        {
            this.pontos = pontos;
            funcaoResultado = new Fracao.Fracao[pontos.getPontosX().Length];
            matrizCalculo = new Fracao.Fracao[pontos.getPontosX().Length, pontos.getPontosX().Length];
        }
        
        public void iniciarCalculo()
        {
            if (tabelaResultado == null)
            {
                throw new Exception("Falta inserir uma tabela para armazenar o resultado.");
            }
            matrizCalculo[0, funcaoResultado.Length - 1] = new Fracao.Fracao(1);

            for (int i = 1; i < funcaoResultado.Length; i++)
            {
                for (int j = funcaoResultado.Length-1-i; j < funcaoResultado.Length; j++)
                {
                    if (j == funcaoResultado.Length-1)
                        matrizCalculo[i, j] = pontos.getPontosX()[i - 1].multiplicar(new Fracao.Fracao(-1)).multiplicar((matrizCalculo[i - 1, j] == null ? new Fracao.Fracao(0) : matrizCalculo[i - 1, j]));
                    else
                        matrizCalculo[i, j] = pontos.getPontosX()[i - 1].multiplicar(new Fracao.Fracao(-1)).multiplicar((matrizCalculo[i - 1, j] == null ? new Fracao.Fracao(0) : matrizCalculo[i - 1, j])).adicionar(matrizCalculo[i - 1, j + 1]);
                }
            }

            InterpolacaoNewton.DiferencaDividida di = new InterpolacaoNewton.DiferencaDividida(pontos);
            for (int i = 0; i < funcaoResultado.Length; i++)
            {
                for (int j = 0; j < funcaoResultado.Length; j++)
                {
                    if (matrizCalculo[i, j] != null)
                        matrizCalculo[i, j] = matrizCalculo[i, j].multiplicar(di.getDiferenca()[i]);
                }
            }

            int index = funcaoResultado.Length - 1;
            for (int i = 0; i < funcaoResultado.Length; i++)
            {
                Fracao.Fracao soma = new Fracao.Fracao(0);
                for (int j = 0; j < funcaoResultado.Length; j++)
                {
                    if (matrizCalculo[j, i] != null)
                        soma = soma.adicionar(matrizCalculo[j, i]);
                }
                funcaoResultado[index] = soma;
                index--;
            }
            montaResultadoNaTabela();

            #region Prova real
            int erro = 0;
            for (int i = 0; i < pontos.getPontosX().Length; i++)
            {
                Fracao.Funcao funcao = new Fracao.Funcao(funcaoResultado);
                if (!funcao.aplicarValor(pontos.getPontosX()[i]).equals(pontos.getPontosY()[i],true))
                {
                    erro++;
                }
            }
            MessageBox.Show("Prova real! Quantidade de erros: "+erro);
            #endregion
        }

        private void montaResultadoNaTabela()
        {
            tabelaResultado.Columns.Add("Tipo", "Tipo");

            tabelaResultado.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            for (int i = funcaoResultado.Length - 1; i >= 0; i--)
            {
                tabelaResultado.Columns.Add(i + "", "x^" + i);
            }

            object[] linha = new object[funcaoResultado.Length + 1];
            linha[0] = "Fração";
            int index = 1;
            for (int i = funcaoResultado.Length - 1; i >= 0; i--)
            {
                linha[index] = funcaoResultado[i].toString();
                index++;
            }
            tabelaResultado.Rows.Add(linha);

            linha = new object[funcaoResultado.Length + 1];
            linha[0] = "Decimal";
            index = 1;
            for (int i = funcaoResultado.Length - 1; i >= 0; i--)
            {
                linha[index] = funcaoResultado[i].toStringDecimal();
                index++;
            }
            tabelaResultado.Rows.Add(linha);

        }

        public void addTabelaResultado(System.Windows.Forms.DataGridView tabela)
        {
            tabelaResultado = tabela;
        }
    }
}
