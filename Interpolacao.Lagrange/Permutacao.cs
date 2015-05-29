using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpolacao.Lagrange
{
    public class Permutacao
    {
        private Fracao.Fracao permutacao = new Fracao.Fracao(0);
        private Fracao.Fracao[] pontos;
        private int tomados = 0;

        public Permutacao(Fracao.Fracao[] pontosX, int pontoIgnorado, int tomados)
        {
            if (tomados > pontosX.Length)
                throw new Exception("O numero tomado não pode ser maior que o numero total de pontos!");
            if (pontoIgnorado < 0 || pontoIgnorado > pontosX.Length)
                throw new Exception("Ponto ignorado está fora do vetor de pontos");

            #region Monta o vetor sem o ponto ignorado
            int index = 0;
            pontos = new Fracao.Fracao[pontosX.Length - 1];
            for (int i = 0; i < pontosX.Length; i++)
            {
                if (i != pontoIgnorado)
                {
                    pontos[index] = pontosX[i];
                    index++;
                }
            }
            #endregion

            this.tomados = tomados;

        }

        private void permutar(int nivel, int valorAnterior, Fracao.Fracao multiplicacao, int pfim)
        {
            for (int j = valorAnterior + 1; j < pfim + 1; j++)
            {
                Fracao.Fracao mul = new Fracao.Fracao(0);
                if (nivel == 0)
                    mul = pontos[j];
                else
                    mul = multiplicacao.multiplicar(pontos[j]);
                if (nivel == tomados - 1)
                    permutacao = permutacao.adicionar(mul);
                else
                    permutar(nivel + 1, j, mul, pfim + 1);
            }
        }

        public Fracao.Fracao getPermutacao()
        {
            if (tomados == 0)
                permutacao = new Fracao.Fracao(1);
            else
                permutar(0, -1, new Fracao.Fracao(1), pontos.Length - tomados);

            return permutacao;
        }
    }
}
