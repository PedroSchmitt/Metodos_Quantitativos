using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpolacao.Lagrange
{
    public class Coeficiente
    {
        private Fracao.Fracao resultado = new Fracao.Fracao(0);

        public Coeficiente(Pontos pontos, bool mais, int tomados, Fracao.Fracao[] bDenominador)
        {
            for (int i = 0; i < pontos.getPontosY().Length; i++)
            {
                Permutacao permuta = new Permutacao(pontos.getPontosX(), i, tomados);
                if (mais)
                    resultado = resultado.adicionar(pontos.getPontosY()[i].multiplicar(permuta.getPermutacao().dividir(bDenominador[i])));
                else
                    resultado = resultado.adicionar(pontos.getPontosY()[i].multiplicar(permuta.getPermutacao().multiplicar(new Fracao.Fracao(-1)).dividir(bDenominador[i])));
            }
        }

        public Fracao.Fracao getResultado()
        {
            return resultado;
        }
    }
}
