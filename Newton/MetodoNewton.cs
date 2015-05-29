using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fracao;

namespace Newton
{
    public class MetodoNewton
    {
        private Funcao funcaoNewtonDerivada;
        private Funcao funcaoNewton;
        private ErroRelativo erroRelativo;
        private System.Windows.Forms.DataGridView passosDoMetodo = null;
        

        public MetodoNewton(Funcao funcaoNewtonDerivada, Funcao funcaoNewton, ErroRelativo erroRelativo)
        {
            this.funcaoNewtonDerivada = funcaoNewtonDerivada;
            this.funcaoNewton = funcaoNewton;
            this.erroRelativo = erroRelativo;
        }

        public void iniciarMetodoNewton(int a, int b)
        {
            Fracao.Fracao[] positivoNegativo = { new Fracao.Fracao(a), new Fracao.Fracao(b) };//1,421875
            //Com a e b iniciar o calculo
            if (passosDoMetodo == null)
            {
                throw new Exception("Falta inserir uma tabela para armazenar os dados.");
            }
            CalculoDoErro e = new CalculoDoErro(positivoNegativo[1]);
            CalculoDoErro prox = null;
            CalculoDoErro ultimaCalculo = null;
            int count = 1;
            while (prox == null || this.erroRelativo.getErro().comparar(ultimaCalculo.getErro().getErro()) == -1)
            {
                prox = e.calcularResultados(this.funcaoNewton, this.funcaoNewtonDerivada);
                e.calcularErro();

                passosDoMetodo.Rows.Add(e.toColum(count));
                ultimaCalculo = e;
                e = prox;
                count++;
            }
            if (ultimaCalculo != null)
                passosDoMetodo.Rows.Add(ultimaCalculo.toColumResultado());

        }

        public void addTabelaResultado(System.Windows.Forms.DataGridView tabela)
        {
            passosDoMetodo = tabela;
        }
    }
}
