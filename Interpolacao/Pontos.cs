using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fracao;

namespace Interpolacao
{
    public class Pontos
    {
        private Fracao.Fracao[] pontosX;
        private Fracao.Fracao[] pontosY;

        public Pontos(Fracao.Fracao[] x, Fracao.Fracao[] y)
        {
            if (x.Length != y.Length)
                throw new Exception("Os vetores de pontos do x  tem q ter a mesma quantidade de pontos no vetor y");
            if (x == null)
                throw new Exception("Vetor x está nulo!");
            if (y == null)
                throw new Exception("Vetor y está nulo!");
            if (x.Length < 1)
                throw new Exception("É necessario pelo menos um ponto no vetor x!");
            if (y.Length < 1)
                throw new Exception("É necessario pelo menos um ponto no vetor y!");
            this.pontosX = x;
            this.pontosY = y;
        }

        public Fracao.Fracao[] getPontosX()
        {
            return this.pontosX;
        }

        public Fracao.Fracao[] getPontosY()
        {
            return this.pontosY;
        }
    }
}
