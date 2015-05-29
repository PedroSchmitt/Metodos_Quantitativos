using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interpolacao.Lagrange
{
    public class Interpolacao
    {
        private Coeficiente [] funcaoResultado;
        private Pontos pontos;
        private Fracao.Fracao[] bDenominador;
        private System.Windows.Forms.DataGridView tabelaResultado = null;
        
        public Interpolacao(Pontos pontos)
        {
            this.pontos = pontos;
            funcaoResultado = new Coeficiente[pontos.getPontosX().Length];
            bDenominador = new Fracao.Fracao[pontos.getPontosX().Length];
        }

        public void iniciarCalculo()
        {
            if (tabelaResultado == null)
            {
                throw new Exception("Falta inserir uma tabela para armazenar o resultado.");
            }
            calcularOsBDenominador();
            bool mais = true;
            for (int i = funcaoResultado.Length-1; i >= 0; i--)
            {
                funcaoResultado[i] = new Coeficiente(pontos, mais, funcaoResultado.Length-i-1, bDenominador);
                mais = !mais;
            }

            montaResultadoNaTabela();

            #region Prova real
            int erro = 0;
            Fracao.Fracao [] f = new Fracao.Fracao[funcaoResultado.Length];
            for(int j = 0; j < funcaoResultado.Length; j++){
                f[j] = funcaoResultado[j].getResultado();
            }
            
            for (int i = 0; i < pontos.getPontosX().Length; i++)
            {
                Fracao.Funcao funcao = new Fracao.Funcao(f);
                if (!funcao.aplicarValor(pontos.getPontosX()[i]).equals(pontos.getPontosY()[i], true))
                {
                    erro++;
                }
            }
            MessageBox.Show("Prova real! Quantidade de erros: " + erro);
            #endregion
        }

        private void montaResultadoNaTabela()
        {
            tabelaResultado.Columns.Add("Tipo", "Tipo");

            tabelaResultado.Columns[0].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            for (int i = funcaoResultado.Length - 1; i >= 0; i--)
            {
                tabelaResultado.Columns.Add(i+"","x^"+i);
            }

            object[] linha = new object[funcaoResultado.Length + 1];
            linha[0] = "Fração";
            int index = 1;
            for (int i = funcaoResultado.Length - 1; i >= 0; i--)
            {
                linha[index] = funcaoResultado[i].getResultado().toString();
                index++;
            }
            tabelaResultado.Rows.Add(linha);

            linha = new object[funcaoResultado.Length + 1];
            linha[0] = "Decimal";
            index = 1;
            for (int i = funcaoResultado.Length - 1; i >= 0; i--)
            {
                linha[index] = funcaoResultado[i].getResultado().toStringDecimal();
                index++;
            }
            tabelaResultado.Rows.Add(linha);
            
        }

        public void addTabelaResultado(System.Windows.Forms.DataGridView tabela)
        {
            tabelaResultado = tabela;
        }

        private void calcularOsBDenominador()
        {
            for (int i = 0; i < bDenominador.Length; i++)
            {
                Fracao.Fracao r = new Fracao.Fracao(0);
                bool mais = true;
                for (int j = 0; j < bDenominador.Length; j++)
                {
                    Permutacao permutacao = new Permutacao(pontos.getPontosX(),i,j);
                    if (mais)
                        r = r.adicionar(permutacao.getPermutacao().multiplicar(pontos.getPontosX()[i].elevarPotencia(bDenominador.Length-1-j)));
                    else
                        r = r.subtrair(permutacao.getPermutacao().multiplicar(pontos.getPontosX()[i].elevarPotencia(bDenominador.Length-1-j)));
                    
                    mais = !mais;
                }
                bDenominador[i] = r;
            }
        }
    }
}
