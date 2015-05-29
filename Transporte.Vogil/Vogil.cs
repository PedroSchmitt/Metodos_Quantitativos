using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Transporte.Vogil
{
    public class Vogil
    {
        private Fracao.Fracao[,] matrizCustos;
        private CelulaDaMatriz[,] matrizTransporte;

        private Fracao.Fracao[] capacidadeLinha;
        private Fracao.Fracao[] capacidadeColuna;

        private CelulaTaxa[] taxaDegenaracaoLinha;
        private CelulaTaxa[] taxaDegenaracaoColuna;

        public Vogil(Fracao.Fracao[,] custos, Fracao.Fracao[] capacidadeLinha, Fracao.Fracao[] capacidadeColuna)
        {
            // Verifica se a soma capacidade da linha é igual a soma da capacidade da coluna
            // Caso contrario criar um linha ou coluna zerada para receber da diferença
            this.matrizCustos = custos;
            this.capacidadeLinha = capacidadeLinha;
            this.capacidadeColuna = capacidadeColuna;

            calcularTaxaDegenaracao();
        }

        // this > fracao = +1
        // this < fracao = -1
        // this = fracao = 0
        
        private void calcularTaxaDegenaracao()
        {
            taxaDegenaracaoLinha = new CelulaTaxa[capacidadeLinha.Length];
            taxaDegenaracaoColuna = new CelulaTaxa[capacidadeColuna.Length];

            #region Linha
            for (int l = 0; l < taxaDegenaracaoLinha.Length; l++)
            {
                Fracao.Fracao numMenor1 = matrizCustos[l, 0];
                Fracao.Fracao numMenor2 = matrizCustos[l, 1];
                for (int c = 2; c < taxaDegenaracaoColuna.Length; c++)
                {
                    // this > fracao = +1
                    // this < fracao = -1
                    // this = fracao = 0
                    if (matrizCustos[l, c].comparar(numMenor1) == -1)
                    {
                        if (numMenor1.comparar(numMenor2) == -1)
                            numMenor2 = matrizCustos[l, c];
                        else
                            numMenor1 = matrizCustos[l, c];
                    }
                    else if (matrizCustos[l, c].comparar(numMenor2) == -1)
                        numMenor2 = matrizCustos[l, c];
                }
                CelulaTaxa celula = new CelulaTaxa();
                if (numMenor1.comparar(numMenor2) == -1)
                    celula.setValor(numMenor2.subtrair(numMenor1));
                else
                    celula.setValor(numMenor1.subtrair(numMenor2));
                taxaDegenaracaoLinha[l] = celula;
            }
            #endregion

            #region Coluna
            for (int c = 0; c < taxaDegenaracaoColuna.Length; c++)
            {
                Fracao.Fracao numMenor1 = matrizCustos[0, c];
                Fracao.Fracao numMenor2 = matrizCustos[1, c];
                for (int l = 2; l < taxaDegenaracaoLinha.Length; l++)
                {
                    // this > fracao = +1
                    // this < fracao = -1
                    // this = fracao = 0
                    if (matrizCustos[l, c].comparar(numMenor1) == -1)
                    {
                        if (numMenor1.comparar(numMenor2) == -1)
                            numMenor2 = matrizCustos[l, c];
                        else
                            numMenor1 = matrizCustos[l, c];
                    }
                    else if (matrizCustos[l, c].comparar(numMenor2) == -1)
                        numMenor2 = matrizCustos[l, c];
                }
                CelulaTaxa celula = new CelulaTaxa();
                if (numMenor1.comparar(numMenor2) == -1)
                    celula.setValor(numMenor2.subtrair(numMenor1));
                else
                    celula.setValor(numMenor1.subtrair(numMenor2));
                taxaDegenaracaoColuna[c] = celula;

            }
            #endregion
        }

        public CelulaTaxa[] getTaxaDegenaracaoLinha()
        {
            return taxaDegenaracaoLinha;
        }

        public CelulaTaxa[] getTaxaDegenaracaoColuna()
        {
            return taxaDegenaracaoColuna;
        }

        public CelulaDaMatriz[,] getMatrizTransporte()
        {
            return matrizTransporte;
        }

        public void calcularTabelaDeTransporte()
        {
            matrizTransporte = new CelulaDaMatriz[matrizCustos.GetLength(0), matrizCustos.GetLength(1)];
            Fracao.Fracao[] capacidadeLinhaFaltante = capacidadeLinha;
            Fracao.Fracao[] capacidadeColunaFaltante =  capacidadeColuna;

            while (!capacidadeLinhaColunaEhZerados(capacidadeColunaFaltante, capacidadeLinhaFaltante))
            {
                PosicaoMatriz melhorPosicao = maiorTaxaDegeneracaoComMenorCusto();
                if (melhorPosicao != null)
                {
                    insereCapacidadeMaximaNaPosicao(melhorPosicao, ref capacidadeColunaFaltante, ref capacidadeLinhaFaltante);
                }
                else
                {
                    //Basta verifica se todos possuem os valores zerados ou lança execeção
                    melhorPosicao = linhaColunaComCapacidadeFaltante(capacidadeColunaFaltante, capacidadeLinhaFaltante);
                    if (melhorPosicao != null)
                    {
                        insereCapacidadeMaximaNaPosicao(melhorPosicao, ref capacidadeColunaFaltante, ref capacidadeLinhaFaltante);
                    }
                    else
                    {
                        throw new Exception("Erro ao calcular tabela de transporte. Matriz não balanceada!");
                    }
                }
            }

        }

        private PosicaoMatriz linhaColunaComCapacidadeFaltante(Fracao.Fracao[] capacidadeColunaFaltante, Fracao.Fracao[] capacidadeLinhaFaltante)
        {
            PosicaoMatriz posicao = new PosicaoMatriz();
            for (int i = 0; i < capacidadeLinhaFaltante.Length && posicao.getLinha() == -1; i++)
            {
                if (capacidadeLinhaFaltante[i].comparar(new Fracao.Fracao(0)) != 0)
                    posicao.setLinha(i);
            }
            for (int i = 0; i < capacidadeColunaFaltante.Length && posicao.getColuna() == -1; i++)
            {
                if (capacidadeColunaFaltante[i].comparar(new Fracao.Fracao(0)) != 0)
                    posicao.setColuna(i);
            }

            if (posicao.getColuna() == -1 && posicao.getLinha() == -1)
                return null;
            else if (posicao.getColuna() != -1 && posicao.getLinha() == -1)
                return null;
            else if (posicao.getColuna() == -1 && posicao.getLinha() != -1)
                return null;

            return posicao;
        }

        private void insereCapacidadeMaximaNaPosicao(PosicaoMatriz melhorPosicao, ref Fracao.Fracao[] capacidadeColunaFaltante, ref Fracao.Fracao[] capacidadeLinhaFaltante)
        {
            Fracao.Fracao valor;
            if (capacidadeColunaFaltante[melhorPosicao.getColuna()].comparar(capacidadeLinhaFaltante[melhorPosicao.getLinha()]) == -1)
                valor = capacidadeColunaFaltante[melhorPosicao.getColuna()];
            else
                valor = capacidadeLinhaFaltante[melhorPosicao.getLinha()];
            
            capacidadeColunaFaltante[melhorPosicao.getColuna()] = capacidadeColunaFaltante[melhorPosicao.getColuna()].subtrair(valor);
            capacidadeLinhaFaltante[melhorPosicao.getLinha()] = capacidadeLinhaFaltante[melhorPosicao.getLinha()].subtrair(valor);

            CelulaDaMatriz celula = new CelulaDaMatriz(valor);
            if (matrizTransporte[melhorPosicao.getLinha(), melhorPosicao.getColuna()] == null && !valor.equals(new Fracao.Fracao(0),true))
            {
                matrizTransporte[melhorPosicao.getLinha(), melhorPosicao.getColuna()] = celula;
            }

        }

        private bool capacidadeLinhaColunaEhZerados(Fracao.Fracao[] capacidadeColunaFaltante, Fracao.Fracao[] capacidadeLinhaFaltante)
        {
            for (int i = 0; i < capacidadeLinhaFaltante.Length; i++)
            {
                if (capacidadeLinhaFaltante[i].comparar(new Fracao.Fracao(0)) != 0)
                    return false;
            }
            for (int i = 0; i < capacidadeColunaFaltante.Length; i++)
            {
                if (capacidadeColunaFaltante[i].comparar(new Fracao.Fracao(0)) != 0)
                    return false;
            }
            return true;
        }

        private PosicaoMatriz maiorTaxaDegeneracaoComMenorCusto()
        {
            #region Coluna
            PosicaoValor maiorTaxaColuna = null;
            
            for (int i = 0; i < taxaDegenaracaoColuna.Length; i++)
            {
                if (!taxaDegenaracaoColuna[i].getUsado())
                {
                    if (maiorTaxaColuna == null)
                    {
                        maiorTaxaColuna = new PosicaoValor();
                        maiorTaxaColuna.setValor(taxaDegenaracaoColuna[i].getValor());
                        maiorTaxaColuna.setPosicao(i);
                    }
                    else
                    {
                        if (taxaDegenaracaoColuna[i].getValor().comparar(maiorTaxaColuna.getValor()) == 1)
                        {
                            maiorTaxaColuna.setValor(taxaDegenaracaoColuna[i].getValor());
                            maiorTaxaColuna.setPosicao(i);
                        }
                        else if (taxaDegenaracaoColuna[i].getValor().comparar(maiorTaxaColuna.getValor()) == 0)
                        {
                            //Qual possui o menor custo, a coluna da variavel 'coluna' ou da coluna da variavel 'i'
                            PosicaoValor menorCustoi = menorCustoDaColuna(i);
                            PosicaoValor menorCustoColuna = menorCustoDaColuna(maiorTaxaColuna.getPosicao());

                            if (menorCustoi.getValor().comparar(menorCustoColuna.getValor()) == -1)
                            {
                                maiorTaxaColuna.setValor(taxaDegenaracaoColuna[i].getValor());
                                maiorTaxaColuna.setPosicao(i);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Linha
            PosicaoValor maiorTaxaLinha = null;
            
            for (int i = 0; i < taxaDegenaracaoLinha.Length; i++)
            {
                if (!taxaDegenaracaoLinha[i].getUsado())
                {
                    if (maiorTaxaLinha == null)
                    {
                        maiorTaxaLinha = new PosicaoValor();
                        maiorTaxaLinha.setValor(taxaDegenaracaoLinha[i].getValor());
                        maiorTaxaLinha.setPosicao(i);
                    }
                    else
                    {
                        if (taxaDegenaracaoLinha[i].getValor().comparar(maiorTaxaLinha.getValor()) == 1)
                        {
                            maiorTaxaLinha.setValor(taxaDegenaracaoLinha[i].getValor());
                            maiorTaxaLinha.setPosicao(i);
                        }
                        else if (taxaDegenaracaoLinha[i].getValor().comparar(maiorTaxaLinha.getValor()) == 0)
                        {
                            //Qual possui o menor custo, a coluna da variavel 'coluna' ou da coluna da variavel 'i'
                            PosicaoValor menorCustoi = menorCustoDaLinha(i);
                            PosicaoValor menorCustoLinha = menorCustoDaLinha(maiorTaxaLinha.getPosicao());

                            if (menorCustoi.getValor().comparar(menorCustoLinha.getValor()) == -1)
                            {
                                maiorTaxaLinha.setValor(taxaDegenaracaoLinha[i].getValor());
                                maiorTaxaLinha.setPosicao(i);
                            }
                        }
                    }
                }
            }
            #endregion

            PosicaoMatriz posicao = new PosicaoMatriz();
            if (maiorTaxaColuna == null && maiorTaxaLinha != null)
            {
                posicao.setLinha(maiorTaxaLinha.getPosicao());
                posicao.setColuna(menorCustoDaLinha(maiorTaxaLinha.getPosicao()).getPosicao());
                taxaDegenaracaoLinha[maiorTaxaLinha.getPosicao()].setUsado(true);
            }
            else if (maiorTaxaLinha == null && maiorTaxaColuna != null)
            {
                posicao.setColuna(maiorTaxaColuna.getPosicao());
                posicao.setLinha(menorCustoDaColuna(maiorTaxaColuna.getPosicao()).getPosicao());
                taxaDegenaracaoColuna[maiorTaxaColuna.getPosicao()].setUsado(true);
            }
            else if (maiorTaxaLinha == null && maiorTaxaColuna == null)
            {
                posicao = null;
            }
            else if (maiorTaxaLinha.getValor().comparar(maiorTaxaColuna.getValor()) == 1)
            {
                //Linha maior
                posicao.setLinha(maiorTaxaLinha.getPosicao());
                posicao.setColuna(menorCustoDaLinha(maiorTaxaLinha.getPosicao()).getPosicao());
                taxaDegenaracaoLinha[maiorTaxaLinha.getPosicao()].setUsado(true);
            }
            else if (maiorTaxaLinha.getValor().comparar(maiorTaxaColuna.getValor()) == -1)
            {
                //Coluna maior
                posicao.setColuna(maiorTaxaColuna.getPosicao());
                posicao.setLinha(menorCustoDaColuna(maiorTaxaColuna.getPosicao()).getPosicao());
                taxaDegenaracaoColuna[maiorTaxaColuna.getPosicao()].setUsado(true);
            }
            else
            {
                // Igual
                PosicaoValor menorCustoLinha = menorCustoDaLinha(maiorTaxaLinha.getPosicao());
                PosicaoValor menorCustoColuna = menorCustoDaColuna(maiorTaxaColuna.getPosicao());

                if (menorCustoLinha.getValor().comparar(menorCustoColuna.getValor()) == -1)
                {
                    //Linha é menor
                    posicao.setColuna(menorCustoLinha.getPosicao());
                    posicao.setLinha(maiorTaxaLinha.getPosicao());
                    taxaDegenaracaoLinha[maiorTaxaLinha.getPosicao()].setUsado(true);
                }
                else
                {
                    //Coluna é menor
                    posicao.setLinha(menorCustoColuna.getPosicao());
                    posicao.setColuna(maiorTaxaColuna.getPosicao());
                    taxaDegenaracaoColuna[maiorTaxaColuna.getPosicao()].setUsado(true);
                }

            }
            return posicao;
        }

        private PosicaoValor menorCustoDaLinha(int linha)
        {
            PosicaoValor menor = null;
            
            for (int l = 0; l < matrizCustos.GetLength(1); l++)
            {
                if (menor == null)
                {
                    if (matrizTransporte[linha, l] == null)
                    {
                        menor = new PosicaoValor();
                        menor.setValor(matrizCustos[linha, l]);
                        menor.setPosicao(l);
                    }
                }
                else if (matrizCustos[linha, l].comparar(menor.getValor()) == -1 && matrizTransporte[linha, l] == null)
                {
                    menor.setValor(matrizCustos[linha, l]);
                    menor.setPosicao(l);
                }
            }
            return menor;
        }

        private PosicaoValor menorCustoDaColuna(int coluna)
        {
            PosicaoValor menor = null;
            
            for (int l = 0; l < matrizCustos.GetLength(0); l++)
            {
                if (menor == null)
                {
                    if (matrizTransporte[l, coluna] == null)
                    {
                        menor = new PosicaoValor();
                        menor.setValor(matrizCustos[l, coluna]);
                        menor.setPosicao(l);
                    }
                }
                else if (matrizCustos[l, coluna].comparar(menor.getValor()) == -1 && matrizTransporte[l,coluna] == null)
                {
                    menor.setValor(matrizCustos[l, coluna]);
                    menor.setPosicao(l);
                }
            }
            return menor;
        }

        public PosicaoMatriz calcularVariaveisLinhasColunas(DataGridView tabela, DataGridView tabelaLeC, DataGridView tabelaValoresDeFora)
        {
            Fracao.Fracao[] linha = new Fracao.Fracao[matrizTransporte.GetLength(0)];
            Fracao.Fracao[] coluna = new Fracao.Fracao[matrizTransporte.GetLength(1)];
            linha[0] = new Fracao.Fracao(0);

            #region Configura tabela
            for (int col = 0; col < matrizTransporte.GetLength(1); col++)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn cAux = new DataGridViewTextBoxColumn();
                cAux.HeaderText = "Coluna " + (col + 1);
                cAux.Name = "coluna" + (col + 1);
                cAux.Width = 70;

                tabela.Columns.Add(cAux);
            }
            for (int lin = 0; lin < matrizTransporte.GetLength(0); lin++)
            {
                tabela.Rows.Add();
            }
            tabela.ReadOnly = true;
            tabela.AllowUserToAddRows = false;
            #endregion

            bool temQterVar = false;

            PosicaoMatriz pos = null;
            LinkedList<PosicaoMatriz> posTestadas = new LinkedList<PosicaoMatriz>();
            do
            {
                #region numeros de dentro
                for (int c = 0; c < matrizTransporte.GetLength(1); c++)
                {
                    if (matrizTransporte[0, c] != null)
                    {
                        coluna[c] = matrizCustos[0, c];
                        tabela[c, 0].Value = coluna[c].toString();
                        calculaVariaveisLinhas(0, c, coluna[c], ref linha, ref coluna, ref tabela);
                    }
                }
                #endregion

                temQterVar = false;
                #region VariavelArtificial
                for (int i = 0; i < linha.Length && !temQterVar; i++)
                {
                    if (linha[i] == null)
                        temQterVar = true;
                }
                for (int i = 0; i < coluna.Length && !temQterVar; i++)
                {
                    if (coluna[i] == null)
                        temQterVar = true;
                }
                #endregion

                if (pos == null)
                {
                    pos = new PosicaoMatriz();
                    pos.setLinha(0);
                    pos.setColuna(0);
                }
                else
                {
                    if (temQterVar)
                        removeVariavel(pos);
                }

                if (temQterVar && pos != null)
                {
                    pos = insereVariavel(pos, posTestadas);
                    posTestadas.AddFirst(pos);
                }
            } while (temQterVar);
            
            #region numeros de fora
            PosicaoMatriz posicao = null;
            Fracao.Fracao menorValorNegativo = null;

            tabelaValoresDeFora.ReadOnly = true;
            tabelaValoresDeFora.AllowUserToAddRows = false;
            int li = 0;
            for (int c = 0; c < matrizTransporte.GetLength(1); c++)
            {
                for (int l = 0; l < matrizTransporte.GetLength(0); l++)
                {
                    if (matrizTransporte[l, c] == null)
                    {
                        tabelaValoresDeFora.Rows.Add();
                        Fracao.Fracao v = matrizCustos[l, c].subtrair(linha[l]).subtrair(coluna[c]);
                        tabelaValoresDeFora[0, li].Value = "x"+(l+1)+(c+1);
                        tabelaValoresDeFora[1, li].Value = v.toString();
                        li++;
                        if (v.comparar(new Fracao.Fracao(0)) == -1)
                        {
                            tabelaValoresDeFora[0, li - 1].Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                            tabelaValoresDeFora[1, li-1].Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                            if (menorValorNegativo == null)
                            {
                                menorValorNegativo = v;
                                posicao = new PosicaoMatriz();
                                posicao.setColuna(c);
                                posicao.setLinha(l);
                            }else if (v.comparar(menorValorNegativo) == -1){
                                menorValorNegativo = v;
                                posicao.setColuna(c);
                                posicao.setLinha(l);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Configura tabela dos Ls e Cs
            tabelaLeC.ReadOnly = true;
            tabelaLeC.AllowUserToAddRows = false;
            
            for (int l = 0; l < linha.Length; l++)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn cAux = new DataGridViewTextBoxColumn();
                cAux.HeaderText = "L" + (l + 1);
                cAux.Name = "coluna" + (l + 1);
                cAux.Width = 70;

                tabelaLeC.Columns.Add(cAux);
                if (l == 0)
                    tabelaLeC.Rows.Add();
                tabelaLeC[tabelaLeC.ColumnCount-1, 0].Value = linha[l].toString();
            }
            for (int l = 0; l < coluna.Length; l++)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn cAux = new DataGridViewTextBoxColumn();
                cAux.HeaderText = "C" + (l + 1);
                cAux.Name = "coluna" + (l + 1);
                cAux.Width = 70;

                tabelaLeC.Columns.Add(cAux);
                tabelaLeC[tabelaLeC.ColumnCount-1, 0].Value = coluna[l].toString();
            }
            #endregion
            
            return posicao;
        }

        private void removeVariavel(PosicaoMatriz pos)
        {
            matrizTransporte[pos.getLinha(), pos.getColuna()] = null;
        }

        private PosicaoMatriz insereVariavel(PosicaoMatriz pos, LinkedList<PosicaoMatriz> lista)
        {
            for (int l = 0; l < matrizTransporte.GetLength(0); l++)
            {
                for (int c = 0; c < matrizTransporte.GetLength(1); c++)
                {
                    if (matrizTransporte[l, c] == null)
                    {
                        PosicaoMatriz aux = new PosicaoMatriz();
                        aux.setLinha(l);
                        aux.setColuna(c);
                        if (!contemAux(aux, lista))
                        {
                            matrizTransporte[l, c] = new CelulaDaMatriz(new Fracao.Fracao(0));
                            pos.setColuna(c);
                            pos.setLinha(l);
                            return pos;
                        }
                    }
                }
            }
            return null;
        }

        private bool contemAux(PosicaoMatriz aux, LinkedList<PosicaoMatriz> lista)
        {
            foreach (PosicaoMatriz pos in lista)
            {
                if (pos.getColuna() == aux.getColuna() && pos.getLinha() == aux.getLinha())
                    return true;
            }
            return false;
        }

        private void calculaVariaveisLinhas(int linhaPrincipal, int colunaPrincipal, Fracao.Fracao custo, ref Fracao.Fracao[] linha, ref Fracao.Fracao[] coluna, ref DataGridView tabela)
        {
            for (int i = 0; i < matrizTransporte.GetLength(0); i++)
            {
                if (linhaPrincipal != i)
                    if (matrizTransporte[i, colunaPrincipal] != null)
                    {
                        if (linha[i] == null)
                        {
                            linha[i] = matrizCustos[i, colunaPrincipal].subtrair(custo);
                            tabela[colunaPrincipal, i].Value = linha[i].toString();
                            calculaVariaveisColunas(i, colunaPrincipal, linha[i], ref linha, ref coluna, ref tabela);
                        }
                    }
            }
        }

        private void calculaVariaveisColunas(int linhaPrincipal, int colunaPrincipal, Fracao.Fracao custo, ref Fracao.Fracao[] linha, ref Fracao.Fracao[] coluna,ref DataGridView tabela)
        {
            for (int i = 0; i < matrizTransporte.GetLength(1); i++)
            {
                if (colunaPrincipal != i)
                    if (matrizTransporte[linhaPrincipal, i] != null)
                    {
                        if (coluna[i] == null)
                        {
                            coluna[i] = matrizCustos[linhaPrincipal, i].subtrair(custo);
                            tabela[i, linhaPrincipal].Value = coluna[i].toString();
                            calculaVariaveisLinhas(linhaPrincipal, i, coluna[i], ref linha, ref coluna, ref tabela);
                        }
                    }
            }
        }

        public LinkedList<PosicaoMatriz> montarCircuitoEAjustarValor(PosicaoMatriz posicao)
        {
            LinkedList<PosicaoMatriz> circuito = new LinkedList<PosicaoMatriz>();

            #region Busca o circuito
            bool retorno = false;
            for (int l = 0; l < matrizTransporte.GetLength(0) && !retorno; l++)
            {
                if (l != posicao.getLinha() && matrizTransporte[l, posicao.getColuna()] != null)
                {
                    retorno = buscaCircuitoPelasColunas(l, posicao.getColuna(), posicao, ref circuito);
                    if (retorno)
                    {
                        PosicaoMatriz p = new PosicaoMatriz();
                        p.setLinha(l);
                        p.setColuna(posicao.getColuna());
                        circuito.AddFirst(p);
                    }
                }
            }
            #endregion

            #region Balanceamento da matriz
            if (circuito.Count > 0)
            {
                Fracao.Fracao valor = null;
                if (matrizTransporte[circuito.ElementAt(0).getLinha(), circuito.ElementAt(0).getColuna()].getValor().comparar(matrizTransporte[circuito.ElementAt(circuito.Count - 1).getLinha(), circuito.ElementAt(circuito.Count - 1).getColuna()].getValor()) == -1)
                {
                    //Primeiro é menor
                    valor = matrizTransporte[circuito.ElementAt(0).getLinha(), circuito.ElementAt(0).getColuna()].getValor();
                }
                else
                {
                    //Ultimo é maior
                    valor = matrizTransporte[circuito.ElementAt(circuito.Count - 1).getLinha(), circuito.ElementAt(circuito.Count - 1).getColuna()].getValor();
                }
                CelulaDaMatriz celula = new CelulaDaMatriz(valor);
                matrizTransporte[posicao.getLinha(), posicao.getColuna()] = celula;
                bool ehSomar = false;
                for (int p = 0; p < circuito.Count; p++)
                {
                    if (ehSomar)
                        matrizTransporte[circuito.ElementAt(p).getLinha(), circuito.ElementAt(p).getColuna()].setValor(matrizTransporte[circuito.ElementAt(p).getLinha(), circuito.ElementAt(p).getColuna()].getValor().adicionar(valor));
                    else
                        matrizTransporte[circuito.ElementAt(p).getLinha(), circuito.ElementAt(p).getColuna()].setValor(matrizTransporte[circuito.ElementAt(p).getLinha(), circuito.ElementAt(p).getColuna()].getValor().subtrair(valor));
                    
                    if (matrizTransporte[circuito.ElementAt(p).getLinha(), circuito.ElementAt(p).getColuna()].getValor().equals(new Fracao.Fracao(0), true))
                        matrizTransporte[circuito.ElementAt(p).getLinha(), circuito.ElementAt(p).getColuna()] = null;

                    ehSomar = !ehSomar;
                }
            }
            #endregion

            return circuito;
        }

        private bool buscaCircuitoPelasColunas(int linha, int colunaIgnorada, PosicaoMatriz posicao, ref LinkedList<PosicaoMatriz> circuito)
        {
            bool retorno = false;
            for (int c = 0; c < matrizTransporte.GetLength(1) && !retorno; c++)
            {
                if (c != colunaIgnorada && matrizTransporte[linha, c] != null)
                {
                    if (linha == posicao.getLinha() || c == posicao.getColuna())
                    {
                        retorno = true;
                        circuito = new LinkedList<PosicaoMatriz>();
                        PosicaoMatriz p = new PosicaoMatriz();
                        p.setLinha(linha);
                        p.setColuna(c);
                        circuito.AddFirst(p);
                    }
                    else
                    {
                        bool r = buscaCircuitoPelasLinhas(c, linha, posicao, ref circuito);
                        if (r)
                        {
                            retorno = true;
                            PosicaoMatriz p = new PosicaoMatriz();
                            p.setLinha(linha);
                            p.setColuna(c);
                            circuito.AddFirst(p);
                        }
                    }
                }
                
            }
            return retorno;
        }

        private bool buscaCircuitoPelasLinhas(int coluna, int linhaIgnorada, PosicaoMatriz posicao, ref LinkedList<PosicaoMatriz> circuito)
        {
            bool retorno = false;
            for (int l = 0; l < matrizTransporte.GetLength(0) && !retorno; l++)
            {
                if (l != linhaIgnorada && matrizTransporte[l, coluna] != null)
                {
                    if (coluna == posicao.getColuna() || l == posicao.getLinha())
                    {
                        retorno = true;
                        circuito = new LinkedList<PosicaoMatriz>();
                        PosicaoMatriz p = new PosicaoMatriz();
                        p.setLinha(l);
                        p.setColuna(coluna);
                        circuito.AddFirst(p);
                    }
                    else
                    {
                        bool r = buscaCircuitoPelasColunas(l, coluna, posicao, ref circuito);
                        if (r)
                        {
                            retorno = true;
                            PosicaoMatriz p = new PosicaoMatriz();
                            p.setLinha(l);
                            p.setColuna(coluna);
                            circuito.AddFirst(p);
                        }
                    }
                }

            }
            return retorno;
        }

        public void calcularCusto(DataGridView tabelaValoresDeFora)
        {
            int li = 0;
            Fracao.Fracao total = new Fracao.Fracao(0);
            for (int c = 0; c < matrizTransporte.GetLength(1); c++)
            {
                for (int l = 0; l < matrizTransporte.GetLength(0); l++)
                {
                    if (matrizTransporte[l, c] != null)
                    {
                        tabelaValoresDeFora.Rows.Add();
                        tabelaValoresDeFora[0, li].Value = "x" + (l + 1) + (c + 1);
                        total = total.adicionar(matrizTransporte[l, c].getValor().multiplicar(matrizCustos[l, c]));
                        tabelaValoresDeFora[1, li].Value = matrizTransporte[l, c].getValor().multiplicar(matrizCustos[l,c]).toString();
                        li++;
                    }
                }
            }
            tabelaValoresDeFora.Rows.Add();
            tabelaValoresDeFora[0, li].Value = "Total";
            tabelaValoresDeFora[1, li].Value = total.toString();
            tabelaValoresDeFora.AllowUserToAddRows = false;
        }
    }

    public class CelulaTaxa
    {
        private Fracao.Fracao valor;
        private bool usado;

        public bool getUsado()
        {
            return this.usado;
        }

        public void setUsado(bool usado)
        {
            this.usado = usado;
        }

        public void setValor(Fracao.Fracao valor)
        {
            this.valor = valor;
        }

        public Fracao.Fracao getValor()
        {
            return this.valor;
        }
    }

    public class PosicaoMatriz
    {
        private int linha = -1;
        private int coluna = -1;

        public int getColuna()
        {
            return this.coluna;
        }
        public int getLinha()
        {
            return this.linha;
        }
        public void setColuna(int coluna)
        {
            this.coluna = coluna;
        }
        public void setLinha(int linha)
        {
            this.linha = linha;
        }
    }

    public class PosicaoValor
    {
        private Fracao.Fracao valor;
        private int posicao;

        public Fracao.Fracao getValor()
        {
            return this.valor;
        }
        public int getPosicao()
        {
            return this.posicao;
        }
        public void setValor(Fracao.Fracao valor)
        {
            this.valor = valor;
        }
        public void setPosicao(int posicao)
        {
            this.posicao = posicao;
        }
    }
}
