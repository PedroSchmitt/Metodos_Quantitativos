using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Fracao;
using System.Numerics;
using Bissecao;

namespace MetodosQuantitativo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TelaPrincipal());

            /*
            3/5 + 2/3 = (3*3 + 5*2) / 5*3 = 19/15

            3/5 - 2/3 = (3*3 - 5*2) / 5*3 = (9 - 10) / 15 = -1/15

             */
            /*Fracao.Fracao f = new Fracao.Fracao(new BigInteger(3), new BigInteger(5));
            Fracao.Fracao f2 = new Fracao.Fracao(new BigInteger(2), new BigInteger(3));
            string x = f.adicionar(f2).toString();
            x = f.subtrair(f2).toString();

            f = new Fracao.Fracao(new BigInteger(2), new BigInteger(1));
            f2 = new Fracao.Fracao(new BigInteger(4), new BigInteger(5));
            x = f.dividir(f2).toString();
            
            // 2/3 / 4/5 = 2/3 * 5/4 = 10/12

            //(3/4)² = 3/4 * 3/4 = 9 / 16
            f = new Fracao.Fracao(new BigInteger(3), new BigInteger(4));
            x = f.elevarPotencia(3).toString();
            */
            NumerosPrimos n = NumerosPrimos.getInstance();
            BigInteger b = new BigInteger(10);
            //List<BigInteger> l = n.numerosPrimos(b);
            //List<BigInteger> l2 = n.numerosPrimos(new BigInteger(10));
            //List<BigInteger> l3 = n.numerosPrimos(new BigInteger(100));
            
        }
    }
}
