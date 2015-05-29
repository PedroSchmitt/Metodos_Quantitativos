using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fracao;

namespace Bissecao
{
    public class MetodoBissecao
    {
        private Funcao funcao;
        private System.Windows.Forms.DataGridView passosDoMetodo = null;
        private ErroRelativo erro;

        public MetodoBissecao(Funcao funcao, ErroRelativo erro)
        {
            this.funcao = funcao;
            this.erro = erro;
        }

        public void iniciarMetodoBissecao( int a, int b)
        {
            Fracao.Fracao[] positivoNegativo = { new Fracao.Fracao(a), new Fracao.Fracao(b) };//1,421875
            //Com a e b iniciar o calculo
            if (passosDoMetodo == null)
            {
                throw new Exception("Falta inserir uma tabela para armazenar os dados.");
            }
            CalculoDoErro e = new CalculoDoErro(positivoNegativo[0], positivoNegativo[1]);
            CalculoDoErro prox = null;
            CalculoDoErro ultimaCalculo = null;
            int count = 1;
            bool sair = false;
            while (!sair)
            {
                if (prox != null && this.erro.getErro().comparar(prox.getErro().getErro()) != -1)
                    sair = true;

                prox = e.calcularResultados(this.funcao);
                if (prox == null)
                {
                    sair = true;
                }else
                    prox.calcularErro(e);
            
                passosDoMetodo.Rows.Add(e.toColum(count));
                ultimaCalculo = e;
                e = prox;
                count++;    
            }
            if (ultimaCalculo != null)
                passosDoMetodo.Rows.Add(ultimaCalculo.toColumResultado());

        }

        public void addTabelaResultado(System.Windows.Forms.DataGridView tabelaErroRelativo)
        {
            passosDoMetodo = tabelaErroRelativo;
        }
    }
}
