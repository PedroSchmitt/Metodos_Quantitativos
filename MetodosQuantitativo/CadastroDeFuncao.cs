using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MetodosQuantitativo
{
    public partial class CadastroDeFuncao : Form
    {
        private TelaPrincipal telaPrincipal;
        private Fracao.Funcao funcao;
        private TipoFuncao tipo;

        public CadastroDeFuncao()
        {
            InitializeComponent();
        }

        
        public CadastroDeFuncao(TelaPrincipal telaPrincipal, Fracao.Funcao funcao, TipoFuncao tipo)
        {
            InitializeComponent();
            this.tipo = tipo;
            this.telaPrincipal = telaPrincipal;
            this.funcao = funcao;

            if (funcao != null)
            {
                foreach (Fracao.Fracao f in funcao.toFuncao())
                {
                    tabelaFuncao.Rows.Add(f.toString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int linha = 0;
                Fracao.Fracao[] fun = new Fracao.Fracao[tabelaFuncao.Rows.Count - 1];
                try
                {

                    for (int i = 0; i < tabelaFuncao.Rows.Count - 1; i++)
                    {
                        linha = i;
                        if (tabelaFuncao[0, i].Value == null)
                            fun[i] = new Fracao.Fracao(0);
                        else
                            fun[i] = new Fracao.Fracao(tabelaFuncao[0, i].Value.ToString());
                    }
                }
                catch (Exception ee)
                {
                    throw new Exception("Erro ao montar função. Erro na fração da linha " + linha + ". Por favor corrigir. Erro: " + ee.Message);
                }
                telaPrincipal.atualizarFuncao(new Fracao.Funcao(fun), tipo);
                this.Close();
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabelaFuncao.Rows.Clear();
        }
    }
}
