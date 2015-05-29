using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fracao;
using System.Numerics;
using Bissecao;
using Newton;
using Interpolacao.Lagrange;
using Interpolacao;
using Transporte.Vogil;
using Programacao.Linear;

namespace MetodosQuantitativo
{
    public partial class TelaPrincipal : Form
    {
        private Funcao funcao = null;
        private Funcao funcaoNewton = null;
        private Funcao funcaoNewtonDerivada = null;
        private Funcao funcaoZero = null;
        private Vogil metodoVogil = null;
        private int passoAtual = 1;
        private int qtdPassos = 1;

        #region Prog. Linear
        private List<Variavel> variaveis = new List<Variavel>();
        #endregion

        public TelaPrincipal()
        {
            String pasta = System.Environment.CurrentDirectory.ToString();
            //NumerosPrimos.Importar();
            InitializeComponent();

        }
        
        private void bt_CalcularErroRelativo_Click(object sender, EventArgs e)
        {
            tabelaErroRelativo.Rows.Clear();

            try
            {
                #region Verificando dados
                bool aconteceuErro = false;
                
                decimal erroRelativo;
                try
                {
                    erroRelativo = decimal.Parse(tb_ErroRelativo.Text);
                }
                catch (Exception ee)
                {
                    aconteceuErro = true;
                    string x = ee.Message;
                    throw new Exception("Erro no valor do erro relativo!");
                }
                int a,b;
                try
                {
                    a = int.Parse(valorA.Text);
                    b = int.Parse(valorB.Text);
                }
                catch (Exception ee)
                {
                    aconteceuErro = true;
                    throw new Exception("Os valores do intervalo devem ser numeros inteiros!");
                }
                if (!aconteceuErro)
                    if (a > b)
                        throw new Exception("O valor de A não pode ser maior que o valor de B!");
                
                if (funcao == null)
                {
                    aconteceuErro = true;
                    throw new Exception("Por favor, cadastrar alguma função!");
                }
                #endregion

                if (!aconteceuErro)
                {
                    MetodoBissecao metodo = new MetodoBissecao(funcao, new Bissecao.ErroRelativo(erroRelativo));
                    metodo.addTabelaResultado(tabelaErroRelativo);
                    metodo.iniciarMetodoBissecao(a,b);
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try{
                CadastroDeFuncao funcao = new CadastroDeFuncao(this, this.funcao, TipoFuncao.FUNCAOBISSECAO);
                funcao.Show();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        public void atualizarFuncao(Funcao funcao, TipoFuncao tipo)
        {
            switch (tipo)
            {
                case TipoFuncao.FUNCAOBISSECAO:
                    this.funcao = funcao;
                    this.tb_Funcao.Text = funcao.toString();
                break;
                case TipoFuncao.NEWTON:
                    this.funcaoNewton = funcao;
                    this.tb_funcaoNewton.Text = funcaoNewton.toString();
                break;
                case TipoFuncao.NEWTONRESUMIDO:
                    this.tb_funcaoDerivada.Text = funcaoNewtonDerivada.toString();
                break;
                case TipoFuncao.ZERODAFUNCAO:
                    funcaoZero = funcao;
                    this.tb_FuncaoZero.Text = funcaoZero.toString();
                break;
            }
        }

        private void gerarMaisNúmerosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                NumerosPrimos primos = NumerosPrimos.getInstance();
                //primos.numerosPrimos(primos.getNumeroMaximo() + new BigInteger(100000000));
                MessageBox.Show("Números primos gerados. Número máximo: "+primos.getNumeroMaximo());
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void bt_editarFuncao_Click(object sender, EventArgs e)
        {
            try{
                CadastroDeFuncao funcao = new CadastroDeFuncao(this, this.funcaoNewton, TipoFuncao.NEWTON);
                funcao.Show();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void bt_derivar_Click(object sender, EventArgs e)
        {
            try{
                if (funcaoNewton != null)
                {
                    funcaoNewtonDerivada = funcaoNewton.derivar();
                    this.atualizarFuncao(funcaoNewtonDerivada, TipoFuncao.NEWTONRESUMIDO);
                }
                else
                    throw new Exception("Por favor, inserir uma função para gerar a derivada!");
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void bt_calcularNewton_Click(object sender, EventArgs e)
        {
            tabelaNEwton.Rows.Clear();

            try
            {
                #region Verificando dados
                bool aconteceuErro = false;

                decimal erroRelativo;
                try
                {
                    erroRelativo = decimal.Parse(tb_erroNewton.Text);
                }
                catch (Exception ee)
                {
                    aconteceuErro = true;
                    throw new Exception("Erro no valor do erro relativo!");
                }
                int a, b;
                try
                {
                    a = int.Parse(tb_intervaloA.Text);
                    b = int.Parse(tb_intervaloB.Text);
                }
                catch (Exception ee)
                {
                    aconteceuErro = true;
                    throw new Exception("Os valores do intervalo devem ser numeros inteiros!");
                }
                if (!aconteceuErro)
                    if (a > b)
                        throw new Exception("O valor de A não pode ser maior que o valor de B!");

                if (funcaoNewton == null)
                {
                    aconteceuErro = true;
                    throw new Exception("Por favor, cadastrar alguma função!");
                }
                if (funcaoNewtonDerivada == null)
                {
                    aconteceuErro = true;
                    throw new Exception("Por favor, cadastrar alguma função e gerar a derivada!");
                }
                #endregion

                if (!aconteceuErro)
                {
                    MetodoNewton metodo = new MetodoNewton(funcaoNewtonDerivada,funcaoNewton, new Newton.ErroRelativo(erroRelativo));
                    metodo.addTabelaResultado(tabelaNEwton);
                    metodo.iniciarMetodoNewton(a, b);
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void bt_editarFuncaoZero_Click(object sender, EventArgs e)
        {
            try
            {
                CadastroDeFuncao funcao = new CadastroDeFuncao(this, this.funcaoZero, TipoFuncao.ZERODAFUNCAO);
                funcao.Show();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void bt_CalcularZero_Click(object sender, EventArgs e)
        {
            tabelaDeZerosDaFuncao.Rows.Clear();

            try
            {
                #region Verificando dados
                bool aconteceuErro = false;

                int a, b;
                try
                {
                    a = int.Parse(valorInicialZero.Text);
                    b = int.Parse(valorFinalZero.Text);
                }
                catch (Exception ee)
                {
                    aconteceuErro = true;
                    throw new Exception("Os valores do intervalo devem ser numeros inteiros!");
                }
                if (!aconteceuErro)
                    if (a > b)
                        throw new Exception("O valor de A não pode ser maior que o valor de B!");

                if (funcaoZero == null)
                {
                    aconteceuErro = true;
                    throw new Exception("Por favor, cadastrar alguma função!");
                }
                #endregion

                if (!aconteceuErro)
                {
                    funcaoZero.calcularZerosDaFuncao(a,b,tabelaDeZerosDaFuncao);
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void bt_CalcularInterpolacaoLagrange_Click(object sender, EventArgs e)
        {
            tabelaInterpolacaoLagrage.Columns.Clear();
            tabelaInterpolacaoLagrage.Rows.Clear();

            try
            {
                #region Verificando dados
                bool aconteceuErro = false;

                Fracao.Fracao[] x = new Fracao.Fracao[tabelaPontos.Rows.Count-1];
                Fracao.Fracao[] y = new Fracao.Fracao[tabelaPontos.Rows.Count-1];
                    
                try
                {
                    for (int i = 0; i < tabelaPontos.Rows.Count-1; i++)
                    {
                        x[i] = new Fracao.Fracao(tabelaPontos[0,i].Value.ToString());
                        y[i] = new Fracao.Fracao(tabelaPontos[1, i].Value.ToString());
                    }
                }
                catch (Exception ee)
                {
                    aconteceuErro = true;
                    throw new Exception("Erro ao pegar os pontos! Verifique se não falta algum ponto, ou se não existe numeros decimais. É aceito apenas fração ou numeros inteiros.");
                }
                #endregion

                if (!aconteceuErro)
                {
                    Pontos pontos = new Pontos(x, y);
                    Interpolacao.Lagrange.Interpolacao interpo = new Interpolacao.Lagrange.Interpolacao(pontos);
                    interpo.addTabelaResultado(tabelaInterpolacaoLagrage);
                    interpo.iniciarCalculo();
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void btCalcularInterpolacaoNewton_Click(object sender, EventArgs e)
        {
            tabelaResultadoInterpolacaoNewton.Columns.Clear();
            tabelaResultadoInterpolacaoNewton.Rows.Clear();

            try
            {

                #region Verificando dados
                bool aconteceuErro = false;

                Fracao.Fracao[] x = new Fracao.Fracao[tabelaPontosNewton.Rows.Count - 1];
                Fracao.Fracao[] y = new Fracao.Fracao[tabelaPontosNewton.Rows.Count - 1];

                try
                {
                    for (int i = 0; i < tabelaPontosNewton.Rows.Count - 1; i++)
                    {
                        x[i] = new Fracao.Fracao(tabelaPontosNewton[0, i].Value.ToString());
                        y[i] = new Fracao.Fracao(tabelaPontosNewton[1, i].Value.ToString());
                    }
                }
                catch (Exception ee)
                {
                    aconteceuErro = true;
                    throw new Exception("Erro ao pegar os pontos! Verifique se não falta algum ponto, ou se não existe numeros decimais. É aceito apenas fração ou numeros inteiros.");
                }

                #endregion
                if (!aconteceuErro)
                {
                    Pontos pontos = new Pontos(x, y);
                    Interpolacao.Newton.Interpolacao interpolacao = new Interpolacao.Newton.Interpolacao(pontos);
                    interpolacao.addTabelaResultado(tabelaResultadoInterpolacaoNewton);
                    interpolacao.iniciarCalculo();
                }
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void gerarGraficoNewton_Click(object sender, EventArgs e)
        {
            Grafico telaGrafico = new Grafico();
            telaGrafico.Show();
        }

        private void bt_calcularTranporteVogil_Click(object sender, EventArgs e)
        {
            try
            {
                metodoVogil = null;
                for(int aba = 1; aba < abas.TabPages.Count; aba++){
                    abas.TabPages.Remove(abas.TabPages[aba]);
                }
                qtdPassos = 1;
                bt_calculaTaxa.Enabled = true;
                bt_proximoPasso.Enabled = false;
                bt_NovaColuna.Enabled = true;
                bt_derivar.Enabled = false;
                
                int c = tabelaDeCustos.Columns.Count-1;
                while(c > 0)
                {
                    tabelaDeCustos.Columns.RemoveAt(c);
                    c--;
                }
                c = tabelaDeCustos.Rows.Count-1;
                while (c > 0)
                {
                    tabelaDeCustos.Rows.RemoveAt(c);
                    c--;
                }

                tabelaDeCustos.AllowUserToAddRows = true;
                
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void bt_NovaColuna_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
                c.HeaderText = "Coluna " + (tabelaDeCustos.ColumnCount+1);
                c.Name = "coluna" + (tabelaDeCustos.ColumnCount + 1);
                c.Width = 70;

                tabelaDeCustos.Columns.Add(c);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void bt_calculaTaxa_Click(object sender, EventArgs e)
        {
            try{
                bt_NovaColuna.Enabled = false;

                System.Windows.Forms.DataGridViewCellStyle cor = new System.Windows.Forms.DataGridViewCellStyle();
                cor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                System.Windows.Forms.DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
                c.DefaultCellStyle = cor;
                c.HeaderText = "Necessidade";
                c.Name = "dataGridViewTextBoxColumn13";
                c.Width = 90;

                System.Windows.Forms.DataGridViewRow r = new DataGridViewRow();
                r.DefaultCellStyle = cor;
            
                tabelaDeCustos.AllowUserToAddRows = false;
                tabelaDeCustos.Columns.Add(c);
                tabelaDeCustos.Rows.Add(r);

                bt_calculaTaxadeDeg.Enabled = true;
                bt_calculaTaxa.Enabled = false;
                tabelaDeCustos[tabelaDeCustos.ColumnCount-1, tabelaDeCustos.RowCount-1].Style.BackColor = System.Drawing.Color.White;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void bt_calculaTaxadeDeg_Click(object sender, EventArgs e)
        {
            try
            {
                Fracao.Fracao [,] custo = pegaMatrizDeCusto(tabelaDeCustos, 2, 2);
                Fracao.Fracao [] capacidadeLinha = pegaValoresDasLinhas(tabelaDeCustos, tabelaDeCustos.ColumnCount-1);
                Fracao.Fracao [] capacidadeColuna = pegaValoresDasColunas(tabelaDeCustos, tabelaDeCustos.RowCount-1);
                metodoVogil = new Vogil(custo, capacidadeLinha, capacidadeColuna);

                                                                                            #region Add coluna
            System.Windows.Forms.DataGridViewCellStyle cor = new System.Windows.Forms.DataGridViewCellStyle();
            cor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            System.Windows.Forms.DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
            c.DefaultCellStyle = cor;
            c.HeaderText = "Taxa Deg.";
            c.Name = "dataGridViewTextBoxColumn13";
            c.Width = 90;

            System.Windows.Forms.DataGridViewRow r = new DataGridViewRow();
            r.DefaultCellStyle = cor;

            tabelaDeCustos.AllowUserToAddRows = false;
            tabelaDeCustos.Columns.Add(c);
            tabelaDeCustos.Rows.Add(r);
            tabelaDeCustos[2, 2].Style.BackColor = System.Drawing.Color.White;

            tabelaDeCustos[tabelaDeCustos.ColumnCount - 1, tabelaDeCustos.RowCount - 1].Style.BackColor = System.Drawing.Color.White;
            tabelaDeCustos[tabelaDeCustos.ColumnCount - 2, tabelaDeCustos.RowCount - 1].Style.BackColor = System.Drawing.Color.White;
            tabelaDeCustos[tabelaDeCustos.ColumnCount - 1, tabelaDeCustos.RowCount - 2].Style.BackColor = System.Drawing.Color.White;

            tabelaDeCustos.ReadOnly = true;
            bt_calculaTaxadeDeg.Enabled = false;
            #endregion

                int coluna = 0;
                foreach (CelulaTaxa celula in metodoVogil.getTaxaDegenaracaoColuna())
                {
                    tabelaDeCustos[coluna, tabelaDeCustos.RowCount - 1].Value = celula.getValor().toString();
                    coluna++;
                }

                int linha = 0;
                foreach (CelulaTaxa celula in metodoVogil.getTaxaDegenaracaoLinha())
                {
                    tabelaDeCustos[tabelaDeCustos.ColumnCount-1,linha].Value = celula.getValor().toString();
                    linha++;
                }

                bt_proximoPasso.Enabled = true;
                passoAtual = 1;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private Fracao.Fracao[] pegaValoresDasLinhas(DataGridView tabela, int coluna)
        {
            Fracao.Fracao[] r = new Fracao.Fracao[tabela.RowCount-1]; 
            for (int l = 0; l < tabela.RowCount-1; l++)
            {
                r[l] = new Fracao.Fracao(tabela[coluna, l].Value.ToString());
            }
            return r;
        }

        private Fracao.Fracao[] pegaValoresDasColunas(DataGridView tabela, int linhas)
        {
            Fracao.Fracao[] r = new Fracao.Fracao[tabela.ColumnCount - 1];
            for (int c = 0; c < tabela.ColumnCount - 1; c++)
            {
                r[c] = new Fracao.Fracao(tabela[c, linhas].Value.ToString());
            }
            return r;
        }

        private Fracao.Fracao[,] pegaMatrizDeCusto(DataGridView tabela, int linhasIgnorar, int colunasIgnorar)
        {
            Fracao.Fracao[,] retorno = new Fracao.Fracao[tabela.RowCount - linhasIgnorar+1, tabela.ColumnCount - colunasIgnorar+1];
            for (int c = 0; c < tabela.ColumnCount - colunasIgnorar+1; c++)
            {
                for (int l = 0; l < tabela.RowCount - linhasIgnorar+1; l++)
                {
                    retorno[l, c] = new Fracao.Fracao(tabela[c, l].Value.ToString());
                }
            }
            return retorno;
        }

        private void bt_proximoPasso_Click(object sender, EventArgs e)
        {
            try
            {
                switch (passoAtual)
                {
                    case 1:
                        #region
                        {
                            metodoVogil.calcularTabelaDeTransporte();
                            criaAbaParaMatrizTransporte(new LinkedList<PosicaoMatriz>(), null);
                            passoAtual++;
                            break;
                        }
                        #endregion
                    case 2:
                        #region
                        {
                            criaAbaParaMatrizDoLeC();
                            break;
                        }
                        #endregion

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            qtdPassos++;
        }

        private void criaAbaParaMatrizDoLeC()
        {
            TabPage page = new TabPage();

            #region monta a aba
            DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            
            DataGridViewCellStyle cell = new System.Windows.Forms.DataGridViewCellStyle();
            cell.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            
            DataGridView tabelaValoresDeFora = new System.Windows.Forms.DataGridView();
            tabelaValoresDeFora.DefaultCellStyle = cell;
            tabelaValoresDeFora.Location = new System.Drawing.Point(6, 19);
            tabelaValoresDeFora.Name = "tabelaDosLeC";
            tabelaValoresDeFora.RowHeadersWidth = 30;
            tabelaValoresDeFora.RowTemplate.Height = 20;
            tabelaValoresDeFora.Size = new System.Drawing.Size(210, 280);
            tabelaValoresDeFora.TabIndex = 10;
            tabelaValoresDeFora.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            tabelaValoresDeFora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            
            System.Windows.Forms.DataGridViewTextBoxColumn cAux = new DataGridViewTextBoxColumn();
            cAux.HeaderText = "Posição";
            cAux.Name = "po";
            cAux.Width = 70;
            tabelaValoresDeFora.Columns.Add(cAux);

            cAux = new DataGridViewTextBoxColumn();
            cAux.HeaderText = "Valor";
            cAux.Name = "va";
            cAux.Width = 70;
            tabelaValoresDeFora.Columns.Add(cAux);

            GroupBox box = new System.Windows.Forms.GroupBox();
            box.Controls.Add(tabelaValoresDeFora);
            box.Location = new System.Drawing.Point(616, 97);
            box.Name = "groupBox"+qtdPassos;
            box.Size = new System.Drawing.Size(228, 310);
            box.TabIndex = 21;
            box.TabStop = false;
            box.Text = "Variaveis de fora";

            DataGridView tabelaParaMostrarLeC = new System.Windows.Forms.DataGridView();
            tabelaParaMostrarLeC.DefaultCellStyle = cell;
            tabelaParaMostrarLeC.Location = new System.Drawing.Point(9, 97);
            tabelaParaMostrarLeC.Name = "tabelaParaMostrarLeC";
            tabelaParaMostrarLeC.Size = new System.Drawing.Size(601, 310);
            tabelaParaMostrarLeC.TabIndex = 20;

            DataGridView tabelaDosLeC = new System.Windows.Forms.DataGridView();
            tabelaDosLeC.DefaultCellStyle = cell;
            tabelaDosLeC.Location = new System.Drawing.Point(6, 19);
            tabelaDosLeC.Name = "tabelaDosLeC";
            tabelaDosLeC.RowHeadersWidth = 30;
            tabelaDosLeC.RowTemplate.Height = 20;
            tabelaDosLeC.Size = new System.Drawing.Size(835, 62);
            tabelaDosLeC.TabIndex = 10;
            tabelaDosLeC.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            tabelaDosLeC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            
            GroupBox boxDaTabela = new System.Windows.Forms.GroupBox();
            boxDaTabela.Controls.Add(tabelaDosLeC);
            boxDaTabela.Location = new System.Drawing.Point(3, 3);
            boxDaTabela.Name = "boxDaTabela";
            boxDaTabela.Size = new System.Drawing.Size(847, 88);
            boxDaTabela.TabIndex = 0;
            boxDaTabela.TabStop = false;
            boxDaTabela.Text = "L e C";
            

            page.Controls.Add(box);
            page.Controls.Add(tabelaParaMostrarLeC);
            page.Controls.Add(boxDaTabela);
            page.Location = new System.Drawing.Point(4, 22);
            page.Size = new System.Drawing.Size(853, 421);
            page.TabIndex = qtdPassos;
            page.Text = "Variveis "+ qtdPassos;
            page.Name = "Variveis " + qtdPassos;
            page.UseVisualStyleBackColor = true;

            abas.TabPages.Add(page);
            #endregion

            PosicaoMatriz posicao = metodoVogil.calcularVariaveisLinhasColunas(tabelaParaMostrarLeC, tabelaDosLeC, tabelaValoresDeFora);
            qtdPassos++;
            if (posicao == null)
            {
                bt_proximoPasso.Enabled = false;
                //Mostra custos
                criaAbaParaMatrizECustos();
            }
            else
            {
                LinkedList<PosicaoMatriz> circuito = metodoVogil.montarCircuitoEAjustarValor(posicao);
                criaAbaParaMatrizTransporte(circuito, posicao);
            }
        }

        private void criaAbaParaMatrizECustos()
        {
            TabPage page = new TabPage();
            
            #region monta a aba
            DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            DataGridViewCellStyle cell = new System.Windows.Forms.DataGridViewCellStyle();
            cell.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;

            DataGridView tabelaValoresDeFora = new System.Windows.Forms.DataGridView();
            tabelaValoresDeFora.DefaultCellStyle = cell;
            tabelaValoresDeFora.Location = new System.Drawing.Point(6, 19);
            tabelaValoresDeFora.Name = "tabelaDosLeC";
            tabelaValoresDeFora.RowHeadersWidth = 30;
            tabelaValoresDeFora.RowTemplate.Height = 20;
            tabelaValoresDeFora.Size = new System.Drawing.Size(210, 380);
            tabelaValoresDeFora.TabIndex = 10;
            tabelaValoresDeFora.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            tabelaValoresDeFora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            System.Windows.Forms.DataGridViewTextBoxColumn cAux = new DataGridViewTextBoxColumn();
            cAux.HeaderText = "Posição";
            cAux.Name = "po";
            cAux.Width = 70;
            tabelaValoresDeFora.Columns.Add(cAux);

            cAux = new DataGridViewTextBoxColumn();
            cAux.HeaderText = "Custo";
            cAux.Name = "va";
            cAux.Width = 70;
            tabelaValoresDeFora.Columns.Add(cAux);

            GroupBox box = new System.Windows.Forms.GroupBox();
            box.Controls.Add(tabelaValoresDeFora);
            box.Location = new System.Drawing.Point(616, 6);
            box.Name = "groupBox" + qtdPassos;
            box.Size = new System.Drawing.Size(228, 410);
            box.TabIndex = 21;
            box.TabStop = false;
            box.Text = "Custo Total";

            DataGridView tabelaParaMostrarLeC = new System.Windows.Forms.DataGridView();
            tabelaParaMostrarLeC.DefaultCellStyle = cell;
            tabelaParaMostrarLeC.Location = new System.Drawing.Point(6, 6);
            tabelaParaMostrarLeC.Name = "tabelaParaMostrarLeC";
            tabelaParaMostrarLeC.Size = new System.Drawing.Size(601, 410);
            tabelaParaMostrarLeC.TabIndex = 20;
            tabelaParaMostrarLeC.AllowUserToAddRows = false;

            page.Controls.Add(box);
            page.Controls.Add(tabelaParaMostrarLeC);
            page.Location = new System.Drawing.Point(4, 22);
            page.Size = new System.Drawing.Size(853, 421);
            page.TabIndex = qtdPassos;
            page.Text = "Custo Total " + qtdPassos;
            page.Name = "Custo Total " + qtdPassos;
            page.UseVisualStyleBackColor = true;

            abas.TabPages.Add(page);
            #endregion

            #region monta todas as colunas
            for (int col = 0; col < metodoVogil.getMatrizTransporte().GetLength(1); col++)
            {
                cAux = new DataGridViewTextBoxColumn();
                cAux.HeaderText = "Coluna " + (col + 1);
                cAux.Name = "coluna" + (col + 1);
                cAux.Width = 70;

                tabelaParaMostrarLeC.Columns.Add(cAux);
            }
            #endregion

            #region imprimir dados
            for (int linha = 0; linha < metodoVogil.getMatrizTransporte().GetLength(0); linha++)
            {
                tabelaParaMostrarLeC.Rows.Add();
                for (int coluna = 0; coluna < metodoVogil.getMatrizTransporte().GetLength(1); coluna++)
                {
                    if (metodoVogil.getMatrizTransporte()[linha, coluna] != null)
                        tabelaParaMostrarLeC[coluna, linha].Value = metodoVogil.getMatrizTransporte()[linha, coluna].getValor().toString();
                }
            }
            #endregion

            metodoVogil.calcularCusto(tabelaValoresDeFora);
        }

        private void criaAbaParaMatrizTransporte(LinkedList<PosicaoMatriz> circuito, PosicaoMatriz posicaoPrincipal)
        {
            TabPage page = new TabPage();
            DataGridView tabela = new System.Windows.Forms.DataGridView();

            #region cria tabela e a aba
            page.Controls.Add(tabela);
            page.Location = new System.Drawing.Point(4, 22);
            page.Name = "Transporte "+qtdPassos;
            page.Size = new System.Drawing.Size(853, 421);
            page.TabIndex = qtdPassos;
            page.Text = "Transporte " + qtdPassos;
            page.UseVisualStyleBackColor = true;

            DataGridViewCellStyle cell = new System.Windows.Forms.DataGridViewCellStyle();
            cell.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            tabela.AlternatingRowsDefaultCellStyle = cell;
            tabela.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            tabela.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            tabela.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            tabela.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            System.Windows.Forms.DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
            c.HeaderText = "Coluna 1";
            c.Name = "coluna1";
            c.Width = 70;
            tabela.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {c});
            DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle50.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle50.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle50.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle50.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle50.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            tabela.DefaultCellStyle = dataGridViewCellStyle50;
            tabela.Location = new System.Drawing.Point(9, 12);
            tabela.Name = "dataGridView1";
            tabela.Size = new System.Drawing.Size(841, 394);
            tabela.TabIndex = 20;
            abas.TabPages.Add(page);
            #endregion

            #region monta todas as colunas
            for (int col = 1; col < metodoVogil.getMatrizTransporte().GetLength(1); col++)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn cAux = new DataGridViewTextBoxColumn();
                cAux.HeaderText = "Coluna "+(col+1);
                cAux.Name = "coluna"+(col+1);
                cAux.Width = 70;

                tabela.Columns.Add(cAux);
            }
            #endregion

            #region imprimir dados
            for (int linha = 0; linha < metodoVogil.getMatrizTransporte().GetLength(0); linha++)
            {
                tabela.Rows.Add();
                for (int coluna = 0; coluna < metodoVogil.getMatrizTransporte().GetLength(1); coluna++)
                {
                    if (metodoVogil.getMatrizTransporte()[linha, coluna] != null)
                        tabela[coluna, linha].Value = metodoVogil.getMatrizTransporte()[linha, coluna].getValor().toString();
                }
            }
            #endregion

            #region imprimi circuito
            foreach (PosicaoMatriz pos in circuito)
            {
                tabela[pos.getColuna(), pos.getLinha()].Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));          
            }
            if (posicaoPrincipal != null)
                tabela[posicaoPrincipal.getColuna(), posicaoPrincipal.getLinha()].Style.BackColor = System.Drawing.Color.Red;          
            #endregion

            tabela.AllowUserToAddRows = false;
            tabela.ReadOnly = true;

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string var = grid_variaveis[e.ColumnIndex, e.RowIndex].Value.ToString();
                foreach(Variavel v in variaveis){
                    if (v.letraVarivel == var)
                    {
                        MessageBox.Show("Já possui uma variavel com a mesma letra.");
                        grid_variaveis[e.ColumnIndex, e.RowIndex].Value = "";
                        return;
                    }
                }
            }
            if (variaveis.Count > e.RowIndex)
            {
                if (e.ColumnIndex == 0)
                    variaveis[e.RowIndex].letraVarivel = (grid_variaveis[e.ColumnIndex, e.RowIndex].Value == null?"": grid_variaveis[e.ColumnIndex, e.RowIndex].Value.ToString());
                else
                    variaveis[e.RowIndex].descricao = (grid_variaveis[e.ColumnIndex, e.RowIndex].Value == null?"": grid_variaveis[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
            else
            {
                Variavel v = new Variavel();
                if (e.ColumnIndex == 0)
                    v.letraVarivel = grid_variaveis[e.ColumnIndex, e.RowIndex].Value.ToString();
                else
                    v.descricao = grid_variaveis[e.ColumnIndex, e.RowIndex].Value.ToString();
                variaveis.Insert(e.RowIndex, v);
            }
            atualizaGrid(grid_funcaoObjetivo);
            atualizaGrid(grid_restricao);
            atualizaGridRestricoes(grid_restricoes);
        }

        private void atualizaGridRestricoes(DataGridView data)
        {
            for (int i = data.ColumnCount; i > 0; i--)
                data.Columns.RemoveAt(i - 1);

            foreach (Variavel v in variaveis)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn x = new System.Windows.Forms.DataGridViewTextBoxColumn();
                x.HeaderText = v.letraVarivel;
                x.Width = 60;

                data.Columns.Add(x);
            }

        }

        private void atualizaGrid(DataGridView data)
        {
            for (int i = data.ColumnCount; i > 0; i--)
                data.Columns.RemoveAt(i - 1);

            if (data.RowCount > 0)
                data.Rows.RemoveAt(0);

            bool primeraVez = true;
            foreach(Variavel v in variaveis){
                System.Windows.Forms.DataGridViewTextBoxColumn Coef = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Coef.HeaderText = "Coef.";
                Coef.Width = 60;

                System.Windows.Forms.DataGridViewTextBoxColumn Var = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Var.HeaderText = "Var";
                Var.ReadOnly = true;
                Var.Width = 40;
            
                Coef.Name = "Coef" + v.letraVarivel;
                Var.Name = "Var" + v.letraVarivel;
                data.Columns.Add(Coef);
                data.Columns.Add(Var);
                if (primeraVez)
                {
                    data.Rows.Add();
                    primeraVez = false;
                }
                data[data.ColumnCount - 1, 0].Value = v.letraVarivel;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tb_sinal.SelectedIndex == -1)
            {
                MessageBox.Show("Escolher uma das opções: >, < ou = ");
                return;
            }
            if (tb_resultadoRestricao.Text == "")
            {
                MessageBox.Show("Digitar um valor no resultado!");
                return;
            }
            if (grid_restricoes.ColumnCount == variaveis.Count)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
                c.HeaderText = "Sinal";
                c.Width = 70;
                grid_restricoes.Columns.Add(c);
 
                System.Windows.Forms.DataGridViewTextBoxColumn r = new DataGridViewTextBoxColumn();
                r.HeaderText = "Resultado";
                r.Width = 70;
                grid_restricoes.Columns.Add(r);
            
            }
            string [] vetor = new string [(grid_restricao.Columns.Count/2)+2];
            int j = 0;
            for (int i = 0; i < grid_restricao.Columns.Count; i += 2)
            {
                vetor[j] = grid_restricao[i, 0].Value.ToString();
                j++;
            }
            vetor[j] = tb_sinal.SelectedItem.ToString(); j++;
            vetor[j] = tb_resultadoRestricao.Text;    
            grid_restricoes.Rows.Add(vetor);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string erroAo = "";
            try
            {
                CalculoLinear calculo = new CalculoLinear();
                #region Exemplos usado para testes
                #region EX 1
                //#region FuncaoObjetivo
                //FuncaoObjetivo funcaoObjetivo = new FuncaoObjetivo();
                //List<Fracao.Fracao> coef = new List<Fracao.Fracao>();
                //coef.Add(new Fracao.Fracao(5));
                //coef.Add(new Fracao.Fracao(4));
                //funcaoObjetivo.setCoeficiente(coef);
                //List<Variavel> vari = new List<Variavel>();
                //Variavel vA = new Variavel();
                //vA.descricao = "dsfdsf";
                //vA.letraVarivel = "A";
                //vari.Add(vA);
                //Variavel vB = new Variavel();
                //vB.descricao = "dfgfjgjdfn";
                //vB.letraVarivel = "B";
                //vari.Add(vB);
                //funcaoObjetivo.setVariaveis(vari);
                //#endregion

                //#region Restricoes
                //List<Restricao> restricoes = new List<Restricao>();
                //List<Fracao.Fracao> c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(3));
                //c.Add(new Fracao.Fracao(2));
                //List<Variavel> v = new List<Variavel>();
                //v.Add(vA);
                //v.Add(vB);
                //Restricao r1 = new Restricao();
                //r1.setCoeficiente(c);
                //r1.setVariaveis(v);
                //r1.setResultadoB(new Fracao.Fracao(3));
                //r1.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(2));
                //c.Add(new Fracao.Fracao(6));
                //Restricao r2 = new Restricao();
                //r2.setCoeficiente(c);
                //r2.setVariaveis(v);
                //r2.setResultadoB(new Fracao.Fracao(3));
                //r2.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(8));
                //c.Add(new Fracao.Fracao(2));
                //Restricao r3 = new Restricao();
                //r3.setCoeficiente(c);
                //r3.setVariaveis(v);
                //r3.setResultadoB(new Fracao.Fracao(4));
                //r3.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //restricoes.Add(r1);
                //restricoes.Add(r2);
                //restricoes.Add(r3);
                //#endregion

                //calculo.tipoMaxOuMin = TipoLinear.Min;
                #endregion

                #region EX 2 - No final a segunda restrição deu diferença no s1 e a1. No valor 14/9
                //#region FuncaoObjetivo
                //FuncaoObjetivo funcaoObjetivo = new FuncaoObjetivo();
                //List<Fracao.Fracao> coef = new List<Fracao.Fracao>();
                //coef.Add(new Fracao.Fracao(3));
                //coef.Add(new Fracao.Fracao(2));
                //funcaoObjetivo.setCoeficiente(coef);
                //List<Variavel> vari = new List<Variavel>();
                //Variavel vA = new Variavel();
                //vA.descricao = "dsfdsf";
                //vA.letraVarivel = "A";
                //vari.Add(vA);
                //Variavel vB = new Variavel();
                //vB.descricao = "dfgfjgjdfn";
                //vB.letraVarivel = "B";
                //vari.Add(vB);
                //funcaoObjetivo.setVariaveis(vari);
                //#endregion

                //#region Restricoes
                //List<Restricao> restricoes = new List<Restricao>();
                //List<Fracao.Fracao> c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(3));
                //c.Add(new Fracao.Fracao(1));
                //List<Variavel> v = new List<Variavel>();
                //v.Add(vA);
                //v.Add(vB);
                //Restricao r1 = new Restricao();
                //r1.setCoeficiente(c);
                //r1.setVariaveis(v);
                //r1.setResultadoB(new Fracao.Fracao(12));
                //r1.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(3));
                //c.Add(new Fracao.Fracao(4));
                //Restricao r2 = new Restricao();
                //r2.setCoeficiente(c);
                //r2.setVariaveis(v);
                //r2.setResultadoB(new Fracao.Fracao(30));
                //r2.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(2));
                //c.Add(new Fracao.Fracao(7));
                //Restricao r3 = new Restricao();
                //r3.setCoeficiente(c);
                //r3.setVariaveis(v);
                //r3.setResultadoB(new Fracao.Fracao(28));
                //r3.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //restricoes.Add(r1);
                //restricoes.Add(r2);
                //restricoes.Add(r3);
                //#endregion

                //calculo.tipoMaxOuMin = TipoLinear.Min;
                #endregion

                #region EX 3 - OK
                //#region FuncaoObjetivo
                //FuncaoObjetivo funcaoObjetivo = new FuncaoObjetivo();
                //List<Fracao.Fracao> coef = new List<Fracao.Fracao>();
                //coef.Add(new Fracao.Fracao(3));
                //coef.Add(new Fracao.Fracao("3/2"));
                //coef.Add(new Fracao.Fracao(4));
                //funcaoObjetivo.setCoeficiente(coef);
                //List<Variavel> vari = new List<Variavel>();
                //Variavel vA = new Variavel();
                //vA.descricao = "dsfdsf";
                //vA.letraVarivel = "A";
                //vari.Add(vA);
                //Variavel vB = new Variavel();
                //vB.descricao = "dfgfjgjdfn";
                //vB.letraVarivel = "B";
                //vari.Add(vB);
                //Variavel vC = new Variavel();
                //vC.descricao = "dfgfjgjdfn";
                //vC.letraVarivel = "C";
                //vari.Add(vC);
                //funcaoObjetivo.setVariaveis(vari);
                //#endregion

                //#region Restricoes
                //List<Restricao> restricoes = new List<Restricao>();
                //List<Fracao.Fracao> c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(30));
                //c.Add(new Fracao.Fracao(3));
                //c.Add(new Fracao.Fracao(15));
                //List<Variavel> v = new List<Variavel>();
                //v.Add(vA);
                //v.Add(vB);
                //v.Add(vC);
                //Restricao r1 = new Restricao();
                //r1.setCoeficiente(c);
                //r1.setVariaveis(v);
                //r1.setResultadoB(new Fracao.Fracao(1500));
                //r1.setOperacao(new Operacao(Operacao.operacao.menor.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(12));
                //c.Add(new Fracao.Fracao("9/2"));
                //c.Add(new Fracao.Fracao(17));
                //Restricao r2 = new Restricao();
                //r2.setCoeficiente(c);
                //r2.setVariaveis(v);
                //r2.setResultadoB(new Fracao.Fracao(1200));
                //r2.setOperacao(new Operacao(Operacao.operacao.menor.GetHashCode()));

                //restricoes.Add(r1);
                //restricoes.Add(r2);
                //#endregion

                //calculo.tipoMaxOuMin = TipoLinear.Max;
                #endregion

                #region EX 4
                //#region FuncaoObjetivo
                //FuncaoObjetivo funcaoObjetivo = new FuncaoObjetivo();
                //List<Fracao.Fracao> coef = new List<Fracao.Fracao>();
                //coef.Add(new Fracao.Fracao(4));
                //coef.Add(new Fracao.Fracao(5));
                //funcaoObjetivo.setCoeficiente(coef);
                //List<Variavel> vari = new List<Variavel>();
                //Variavel vA = new Variavel();
                //vA.descricao = "dsfdsf";
                //vA.letraVarivel = "A";
                //vari.Add(vA);
                //Variavel vB = new Variavel();
                //vB.descricao = "dfgfjgjdfn";
                //vB.letraVarivel = "B";
                //vari.Add(vB);
                //funcaoObjetivo.setVariaveis(vari);
                //#endregion

                //#region Restricoes
                //List<Restricao> restricoes = new List<Restricao>();
                //List<Fracao.Fracao> c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(2));
                //c.Add(new Fracao.Fracao(3));
                //List<Variavel> v = new List<Variavel>();
                //v.Add(vA);
                //v.Add(vB);
                //Restricao r1 = new Restricao();
                //r1.setCoeficiente(c);
                //r1.setVariaveis(v);
                //r1.setResultadoB(new Fracao.Fracao(7));
                //r1.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(5));
                //c.Add(new Fracao.Fracao(2));
                //Restricao r2 = new Restricao();
                //r2.setCoeficiente(c);
                //r2.setVariaveis(v);
                //r2.setResultadoB(new Fracao.Fracao(12));
                //r2.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //restricoes.Add(r1);
                //restricoes.Add(r2);
                //#endregion

                //calculo.tipoMaxOuMin = TipoLinear.Min;
                #endregion

                #region EX 5 - OK
                //#region FuncaoObjetivo
                //FuncaoObjetivo funcaoObjetivo = new FuncaoObjetivo();
                //List<Fracao.Fracao> coef = new List<Fracao.Fracao>();
                //coef.Add(new Fracao.Fracao(6));
                //coef.Add(new Fracao.Fracao(4));
                //funcaoObjetivo.setCoeficiente(coef);
                //List<Variavel> vari = new List<Variavel>();
                //Variavel vA = new Variavel();
                //vA.descricao = "dsfdsf";
                //vA.letraVarivel = "A";
                //vari.Add(vA);
                //Variavel vB = new Variavel();
                //vB.descricao = "dfgfjgjdfn";
                //vB.letraVarivel = "B";
                //vari.Add(vB);
                //funcaoObjetivo.setVariaveis(vari);
                //#endregion

                //#region Restricoes
                //List<Restricao> restricoes = new List<Restricao>();
                //List<Fracao.Fracao> c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(3));
                //c.Add(new Fracao.Fracao(1));
                //List<Variavel> v = new List<Variavel>();
                //v.Add(vA);
                //v.Add(vB);
                //Restricao r1 = new Restricao();
                //r1.setCoeficiente(c);
                //r1.setVariaveis(v);
                //r1.setResultadoB(new Fracao.Fracao(24));
                //r1.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(1));
                //c.Add(new Fracao.Fracao(1));
                //Restricao r2 = new Restricao();
                //r2.setCoeficiente(c);
                //r2.setVariaveis(v);
                //r2.setResultadoB(new Fracao.Fracao(16));
                //r2.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //c = new List<Fracao.Fracao>();
                //c.Add(new Fracao.Fracao(2));
                //c.Add(new Fracao.Fracao(6));
                //Restricao r3 = new Restricao();
                //r3.setCoeficiente(c);
                //r3.setVariaveis(v);
                //r3.setResultadoB(new Fracao.Fracao(48));
                //r3.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));

                //restricoes.Add(r1);
                //restricoes.Add(r2);
                //restricoes.Add(r3);
                //#endregion

                //calculo.tipoMaxOuMin = TipoLinear.Min;
                #endregion

                #endregion

                #region Pegando os dados da tela

                #region Variaveis
                List<Variavel> vari = new List<Variavel>();
                erroAo = "buscar as variaveis";
                for (int i = 0; i < grid_variaveis.Rows.Count-1; i++)
                {
                    Variavel v = new Variavel();
                    v.descricao = grid_variaveis[1, i].Value.ToString();
                    v.letraVarivel = grid_variaveis[0, i].Value.ToString();
                    vari.Add(v);
                }
                if (vari.Count == 0)
                {
                    MessageBox.Show("Deve preencher alguma variavel!");
                    return;
                }
                #endregion

                #region FuncaoObjetivo
                FuncaoObjetivo funcaoObjetivo = new FuncaoObjetivo();
                List<Fracao.Fracao> coef = new List<Fracao.Fracao>();
                erroAo = " buscar os valores da função objetivo";
                for (int i = 0; i < grid_funcaoObjetivo.Columns.Count; i+=2)
                {
                    coef.Add(new Fracao.Fracao(grid_funcaoObjetivo[i, 0].Value.ToString()));
                }
                funcaoObjetivo.setCoeficiente(coef);
                funcaoObjetivo.setVariaveis(vari);
                #endregion

                #region Restricoes
                List<Restricao> restricoes = new List<Restricao>();
                for (int restricao = 0; restricao < grid_restricoes.Rows.Count; restricao++)
                {
                    List<Fracao.Fracao> c = new List<Fracao.Fracao>();
                    for (int coluna = 0; coluna < grid_restricoes.Columns.Count-2; coluna++)
                    {
                        c.Add(new Fracao.Fracao(grid_restricoes[coluna, restricao].Value.ToString()));
                    }
                    
                    Restricao r1 = new Restricao();
                    r1.setCoeficiente(c);
                    r1.setVariaveis(vari);
                    r1.setResultadoB(new Fracao.Fracao(grid_restricoes[grid_restricoes.Columns.Count-1, restricao].Value.ToString()));
                    switch (grid_restricoes[grid_restricoes.Columns.Count-2, restricao].Value.ToString())
                    {
                        case ">":
                        {
                            r1.setOperacao(new Operacao(Operacao.operacao.maior.GetHashCode()));
                            break;
                        }
                        case "<":
                        {
                            r1.setOperacao(new Operacao(Operacao.operacao.menor.GetHashCode()));
                            break;
                        }
                        case "=":
                        {
                            r1.setOperacao(new Operacao(Operacao.operacao.igual.GetHashCode()));
                            break;
                        }
                    }
                    
                    restricoes.Add(r1);
                }
                #endregion
                erroAo = "buscar o tipo da função (Máx ou Mín)";
                if (tipoFuncao.SelectedIndex == 0)
                    calculo.tipoMaxOuMin = TipoLinear.Max;
                else
                    calculo.tipoMaxOuMin = TipoLinear.Min;
                #endregion

                calculo.funcaoObjetivoInicical = funcaoObjetivo;
                calculo.restricoesInicial = restricoes;

                Fracao.Fracao maiorValor = calculo.funcaoObjetivoInicical.maiorValorCoeficiente();
                #region Maior valor para testes
                //EX 1 maiorValor = new Fracao.Fracao(100);
                //EX 2 maiorValor = new Fracao.Fracao(10);
                //EX 3 maiorValor = new Fracao.Fracao(0);
                //EX 4 maiorValor = new Fracao.Fracao(1000000);
                //EX 5 maiorValor = new Fracao.Fracao(1000000);
                #endregion
                erroAo = "buscar maior valor";
                maiorValor = new Fracao.Fracao(tb_maiorValor.Text);
                calculo.maiorValoDaFuncaoObjeto = maiorValor;
                calculo.montaFOComMinOuMax();
                List<Restricao> newList = calculo.restricoesInicial.GetRange(0, calculo.restricoesInicial.Count);
                calculo.restricoesModificada = newList;
                calculo.inseriVariaveisDeFolgaNasRestricoes();

                calculo.inseriVariaveisDeDentro();
                calculo.inseriProdutorioZ();
                calculo.inseriCMenosZ();
                while (calculo.verificaSeTemCMenosZPositivo())
                {
                    calculo.buscaPivot();
                    calculo.inseriBDivididoPorA();
                    criarTabPage(calculo);
                    calculo.buscaVariavelSaida();
                    calculo.realizaTrocaDeVariaveis();
                    calculo.inseriProdutorioZ();
                    calculo.inseriCMenosZ();
                }
                criarTabPage(calculo);
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao " + erroAo + ". Erro: " + erro.Message);
            }
        }

        private void criarTabPage(CalculoLinear calculo)
        {
            System.Windows.Forms.TabPage tabPage9 = new System.Windows.Forms.TabPage();
            System.Windows.Forms.GroupBox groupBox6 = new System.Windows.Forms.GroupBox();
            System.Windows.Forms.DataGridView grid_funcaoPasso = new System.Windows.Forms.DataGridView();
            System.Windows.Forms.DataGridView grid_varDentroPasso = new System.Windows.Forms.DataGridView();
            System.Windows.Forms.DataGridView grid_bPasso = new System.Windows.Forms.DataGridView();
            System.Windows.Forms.DataGridView grid_restricoesPasso = new System.Windows.Forms.DataGridView();
            System.Windows.Forms.DataGridView grid_zMenosCPasso = new System.Windows.Forms.DataGridView();
            
            groupBox6.Controls.Add(grid_funcaoPasso);
            groupBox6.Location = new System.Drawing.Point(3, 3);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new System.Drawing.Size(847, 87);
            groupBox6.TabIndex = 22;
            groupBox6.TabStop = false;
            groupBox6.Text = "Função objetivo - FO";

            DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            
            grid_funcaoPasso.AllowUserToAddRows = false;
            grid_funcaoPasso.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle48;
            grid_funcaoPasso.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            grid_funcaoPasso.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            grid_funcaoPasso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_funcaoPasso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_funcaoPasso.DefaultCellStyle = dataGridViewCellStyle48;
            grid_funcaoPasso.Location = new System.Drawing.Point(6, 19);
            grid_funcaoPasso.Name = "dataGridView1";
            grid_funcaoPasso.RowHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_funcaoPasso.Size = new System.Drawing.Size(835, 62);
            grid_funcaoPasso.TabIndex = 22;
            // 
            // dataGridView2
            // 
            grid_varDentroPasso.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle48;
            grid_varDentroPasso.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            grid_varDentroPasso.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            grid_varDentroPasso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_varDentroPasso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_varDentroPasso.DefaultCellStyle = dataGridViewCellStyle48;
            grid_varDentroPasso.Location = new System.Drawing.Point(9, 96);
            grid_varDentroPasso.Name = "dataGridView2";
            grid_varDentroPasso.RowHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_varDentroPasso.Size = new System.Drawing.Size(118, 260);
            grid_varDentroPasso.TabIndex = 23;
            // 
            // dataGridView3
            // 
            grid_bPasso.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle48;
            grid_bPasso.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            grid_bPasso.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            grid_bPasso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_bPasso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_bPasso.DefaultCellStyle = dataGridViewCellStyle48;
            grid_bPasso.Location = new System.Drawing.Point(726, 96);
            grid_bPasso.Name = "dataGridView3";
            grid_bPasso.RowHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_bPasso.Size = new System.Drawing.Size(118, 260);
            grid_bPasso.TabIndex = 24;
            // 
            // dataGridView4
            // 
            grid_restricoesPasso.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle48;
            grid_restricoesPasso.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            grid_restricoesPasso.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            grid_restricoesPasso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_restricoesPasso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_restricoesPasso.DefaultCellStyle = dataGridViewCellStyle48;
            grid_restricoesPasso.Location = new System.Drawing.Point(133, 96);
            grid_restricoesPasso.Name = "dataGrifdgdView4";
            grid_restricoesPasso.RowHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_restricoesPasso.Size = new System.Drawing.Size(587, 260);
            grid_restricoesPasso.TabIndex = 25;
            // 
            // dataGridView5
            // 
            grid_zMenosCPasso.AllowUserToAddRows = false;
            grid_zMenosCPasso.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle48;
            grid_zMenosCPasso.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            grid_zMenosCPasso.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            grid_zMenosCPasso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_zMenosCPasso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid_zMenosCPasso.DefaultCellStyle = dataGridViewCellStyle48;
            grid_zMenosCPasso.Location = new System.Drawing.Point(9, 359);
            grid_zMenosCPasso.Name = "dataGridView5";
            grid_zMenosCPasso.RowHeadersDefaultCellStyle = dataGridViewCellStyle48;
            grid_zMenosCPasso.Size = new System.Drawing.Size(835, 59);
            grid_zMenosCPasso.TabIndex = 26;

            tabPage9.Controls.Add(grid_zMenosCPasso);
            tabPage9.Controls.Add(grid_restricoesPasso);
            tabPage9.Controls.Add(grid_bPasso);
            tabPage9.Controls.Add(grid_varDentroPasso);
            tabPage9.Controls.Add(groupBox6);
            tabPage9.Location = new System.Drawing.Point(4, 22);
            tabPage9.Name = "tabPage9";
            tabPage9.Size = new System.Drawing.Size(853, 421);
            tabPage9.TabIndex = 1;
            tabPage9.Text = "Passo "+tabControl2.TabCount;
            tabPage9.UseVisualStyleBackColor = true;

            this.tabControl2.Controls.Add(tabPage9);

            #region Funcao Objetivo
            for (int i = 0; i < calculo.funcaoObjetivoModificada.getVariaveis().Count; i++)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn Var = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Var.HeaderText = calculo.funcaoObjetivoModificada.getVariaveis()[i].letraVarivel;
                Var.ReadOnly = true;
                Var.Width = 80;

                grid_funcaoPasso.Columns.Add(Var);

            }
            string [] xx  = new string [calculo.funcaoObjetivoModificada.getCoeficiente().Count];
            for (int i = 0; i < calculo.funcaoObjetivoModificada.getCoeficiente().Count; i++)
            {
                xx[i] = calculo.funcaoObjetivoModificada.getCoeficiente()[i].toString() + " = " + calculo.funcaoObjetivoModificada.getCoeficiente()[i].toStringDecimal();
                
            }
            grid_funcaoPasso.Rows.Add(xx);
            #endregion

            #region Var de dentro
            System.Windows.Forms.DataGridViewTextBoxColumn Var2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Var2.HeaderText = "Variaveis";
            Var2.ReadOnly = true;
            Var2.Width = 80;

            grid_varDentroPasso.Columns.Add(Var2);

            System.Windows.Forms.DataGridViewTextBoxColumn Var3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Var3.HeaderText = "Valor";
            Var3.ReadOnly = true;
            Var3.Width = 80;

            grid_varDentroPasso.Columns.Add(Var3);

            for (int i = 0; i < calculo.varDentro.Count; i++)
            {
                string[] xxx = new string[2];
                xxx[0] = calculo.varDentro[i].getVariavel().letraVarivel;
                xxx[1] = calculo.varDentro[i].getValor().toString() + " = " + calculo.varDentro[i].getValor().toStringDecimal();
                grid_varDentroPasso.Rows.Add(xxx);
            }
            #endregion

            #region Var do canto da tela
            Var2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Var2.HeaderText = "b";
            Var2.ReadOnly = true;
            Var2.Width = 80;

            grid_bPasso.Columns.Add(Var2);

            Var3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Var3.HeaderText = "b/a";
            Var3.ReadOnly = true;
            Var3.Width = 80;

            grid_bPasso.Columns.Add(Var3);

            for (int i = 0; i < calculo.restricoesModificada.Count; i++)
            {
                string[] xxx = new string[2];
                xxx[0] = calculo.restricoesModificada[i].getResultadoB().toString() + " = " + calculo.restricoesModificada[i].getResultadoB().toStringDecimal();
                if (calculo.bDivididoPorA.Count > 0)
                    xxx[1] = calculo.bDivididoPorA[i].toString() + " = " + calculo.bDivididoPorA[i].toStringDecimal();
                else
                    xxx[1] = "";
                grid_bPasso.Rows.Add(xxx);
            }
            #endregion

            #region restricoes

            if (calculo.restricoesModificada.Count > 0)
            {
                for (int i = 0; i < calculo.restricoesModificada[0].getVariaveis().Count; i++)
                {
                    Var2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    Var2.HeaderText = calculo.restricoesModificada[0].getVariaveis()[i].letraVarivel;
                    Var2.ReadOnly = true;
                    Var2.Width = 80;

                    grid_restricoesPasso.Columns.Add(Var2);
                }
                for (int i = 0; i < calculo.restricoesModificada[0].getVariaveisDeFolga().Count; i++)
                {
                    Var2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    Var2.HeaderText = calculo.restricoesModificada[0].getVariaveisDeFolga()[i].letraVarivel;
                    Var2.ReadOnly = true;
                    Var2.Width = 80;

                    grid_restricoesPasso.Columns.Add(Var2);
                }
            }
            for (int res = 0; res < calculo.restricoesModificada.Count; res++)
            {
                string[] xy = new string[calculo.restricoesModificada[res].getVariaveis().Count + calculo.restricoesModificada[res].getVariaveisDeFolga().Count];
                int cc = 0;
                for (int i = 0; i < calculo.restricoesModificada[res].getVariaveis().Count; i++)
                {
                    xy[cc] = calculo.restricoesModificada[res].getCoeficiente()[i].toString() + " = " + calculo.restricoesModificada[res].getCoeficiente()[i].toStringDecimal();
                    cc++;
                }
                for (int i = 0; i < calculo.restricoesModificada[res].getVariaveisDeFolga_Valor().Count; i++)
                {
                    xy[cc] = calculo.restricoesModificada[res].getVariaveisDeFolga_Valor()[i].toString() + " = " + calculo.restricoesModificada[res].getVariaveisDeFolga_Valor()[i].toStringDecimal();
                    cc++;
                }
                grid_restricoesPasso.Rows.Add(xy);
            }
            #endregion


            #region c menor z
            if (calculo.restricoesModificada.Count > 0)
            {
                for (int i = 0; i < calculo.restricoesModificada[0].getVariaveis().Count; i++)
                {
                    Var2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    Var2.HeaderText = calculo.restricoesModificada[0].getVariaveis()[i].letraVarivel;
                    Var2.ReadOnly = true;
                    Var2.Width = 80;

                    grid_zMenosCPasso.Columns.Add(Var2);
                }
                for (int i = 0; i < calculo.restricoesModificada[0].getVariaveisDeFolga().Count; i++)
                {
                    Var2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    Var2.HeaderText = calculo.restricoesModificada[0].getVariaveisDeFolga()[i].letraVarivel;
                    Var2.ReadOnly = true;
                    Var2.Width = 80;

                    grid_zMenosCPasso.Columns.Add(Var2);
                }
            }
            string[] xyf = new string[calculo.restricoesModificada[0].getVariaveis().Count + calculo.restricoesModificada[0].getVariaveisDeFolga().Count];
            for (int i = 0; i < calculo.produtorioZ.getCoeficiente().Count; i++)
            {
                xyf[i] = calculo.produtorioZ.getCoeficiente()[i].toString() + " = " + calculo.produtorioZ.getCoeficiente()[i].toStringDecimal();
            }
            grid_zMenosCPasso.Rows.Add(xyf);

            xyf = new string[calculo.restricoesModificada[0].getVariaveis().Count + calculo.restricoesModificada[0].getVariaveisDeFolga().Count];
            for (int i = 0; i < calculo.cMenosZ.getCoeficiente().Count; i++)
            {
                xyf[i] = calculo.cMenosZ.getCoeficiente()[i].toString() + " = " + calculo.cMenosZ.getCoeficiente()[i].toStringDecimal();
            }
            grid_zMenosCPasso.Rows.Add(xyf);
            #endregion
        }

    }
    public enum TipoFuncao
    {
        NEWTON,
        FUNCAOBISSECAO,
        NEWTONRESUMIDO,
        ZERODAFUNCAO
    }
}
