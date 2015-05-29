using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interpolacao;

namespace InterpolacaoNewton
{
    public class DiferencaDividida
    {
        private Fracao.Fracao[] diferencas;
        private int index = 0;
        private int diferencaEntreX = 1;

        public DiferencaDividida(Interpolacao.Pontos pontos)
        {
            diferencas = new Fracao.Fracao[pontos.getPontosY().Length];
            calcularDiferenca(pontos.getPontosY(), pontos.getPontosX());
        }

        private void calcularDiferenca(Fracao.Fracao[] pontos, Fracao.Fracao[] pontoX)
        {
            diferencas[index] = pontos[0];
            index++;
            Fracao.Fracao [] proximoVetor = new Fracao.Fracao[pontos.Length-1];
            
            for (int i = 0; i < proximoVetor.Length; i++)
            {
                proximoVetor[i] = diferencasValor(pontos[i], pontos[i + 1], pontoX[i], pontoX[i+diferencaEntreX]);
            }
            diferencaEntreX++;

            if (proximoVetor.Length > 0)
                calcularDiferenca(proximoVetor, pontoX);
        }

        private Fracao.Fracao diferencasValor(Fracao.Fracao valor1, Fracao.Fracao valor2, Fracao.Fracao x1, Fracao.Fracao x2)
        {
            return valor2.subtrair(valor1).dividir(x2.subtrair(x1));
        }

        public Fracao.Fracao[] getDiferenca()
        {
            return diferencas;
        }
    }
}
