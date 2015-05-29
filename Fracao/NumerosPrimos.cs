using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace Fracao
{
    public class NumerosPrimos
    {
        private static NumerosPrimos numeroPrimoClass = null;
        private BigInteger numeroMaximo = 0;
        private static List<BigInteger> numeros = new List<BigInteger>();

        private NumerosPrimos()
        {
            try
            {
                ArquivoTexto arq = new ArquivoTexto(ArquivoTexto.Tipo.Ler);
                string s = arq.lerLinha();
                while (s != null)
                {
                    numeros.Add(new BigInteger(decimal.Parse(s)));
                    s = arq.lerLinha();
                }
                if (numeros.Count > 0)
                    numeroMaximo = numeros[numeros.Count - 1];

                arq.fechar();
            }
            catch (Exception e) { }
        }

        public static NumerosPrimos getInstance()
        {
            if (numeroPrimoClass == null)
            {
                numeroPrimoClass = new NumerosPrimos();
            }
            return numeroPrimoClass;
        }

        public BigInteger getNumeroMaximo()
        {
            return numeroMaximo;
        }

        public BigInteger numerosPrimos(BigInteger numero1, BigInteger numero2)
        {
            //Retorna o numero divisevel pelo dois numeros
            #region Procura um divisor e divide o numerador e denominador
            bool achou = false;
            for (int i = 0; i < numeros.Count && !achou && numeros[i] < numero1; i++)
            {
                if (numero1 % numeros[i] == 0 && numero2 % numeros[i] == 0)
                {
                    achou = true;
                    return numeros[i];
                }
            }
            return new BigInteger(1);
            #endregion
            
            //if (numeroMaximo > this.numeroMaximo)
            //{
                /*BigInteger i = this.numeroMaximo;

                ArquivoTexto arq = new ArquivoTexto(ArquivoTexto.Tipo.Escrever);

                if (i < 2)
                {
                    numeros.Add(2);
                    arq.escrever(new BigInteger(2));
                }

                if ((i / 2) * 2 == i)
                    i++;

                int cincoMil = 0;
                while (i <= numeroMaximo)
                {
                    if (verificaEhNumeroPrimo(i))
                    {
                        numeros.Add(i);
                        arq.escrever(i);
                    }
                    i++; i++;
                    cincoMil++;
                    if (cincoMil >= 5000)
                    {
                        arq.fechar();
                        cincoMil = 0;
                        arq = new ArquivoTexto(ArquivoTexto.Tipo.Escrever);
                    }
                }
                arq.fechar();
                this.numeroMaximo = numeroMaximo;*/
            //    return numeros;
            //}
            //return numeros.FindAll(y => y <= numeroMaximo);
        }

        private bool verificaEhNumeroPrimo(BigInteger numero)
        {
            if (numero < 0)
                throw new Exception("O numero não pode ser negativo para realizar a simplificação!");


            if (numero == 2 || numero == 3 || numero == 5 || numero == 7)
                return true;

            string porcinco = numero + "";
            if (porcinco.Substring(porcinco.Length - 1) == "5")
                return false;

            if (numero == 1)
                return false;

            double dou = double.Parse(numero.ToString());
            int r = (int) Math.Sqrt(dou);
            BigInteger raiz = new BigInteger(r);
            //List<int> t = numeros.FindAll(y => y <= numero);

            foreach (BigInteger n in numeros)
            {
                if (n > raiz)
                    break;
                if (n != 1 && n != numero)
                    if ((numero / n) * n == numero)
                    //if (numero % n == 0)
                        return false;
            }



            //if (numero == j)
            return true;
            //return false;
        }


    }

    //public class NumerosPrimos2
    //{
    //    private static NumerosPrimos2 numeroPrimoClass = null;
    //    private BigInteger numeroMaximo = 0;
    //    private static List<BigInteger> numeros = new List<BigInteger>();

    //    private NumerosPrimos2()
    //    {
    //    }

    //    public static NumerosPrimos2 getInstance()
    //    {
    //        if (numeroPrimoClass == null)
    //        {
    //            numeroPrimoClass = new NumerosPrimos2();

    //            try
    //            {
    //                XElement xml = XElement.Load("NumerosPrimos.xml");
    //                foreach (XElement x in xml.Elements())
    //                {
    //                    BigInteger p = new BigInteger(long.Parse(x.Attribute("num").Value.ToString()));
    //                    numeros.Add(p);
    //                }
    //                numeroPrimoClass.numeroMaximo = numeros[numeros.Count - 1];
    //            }
    //            catch (Exception e) { }

    //        }
    //        return numeroPrimoClass;
    //    }

    //    public List<BigInteger> numerosPrimos(BigInteger numeroMaximo)
    //    {
    //        if (numeroMaximo > this.numeroMaximo)
    //        {
    //            BigInteger i = this.numeroMaximo;
    //            if (i % 2 == 0)
    //                i++;
    //            if (i < 2)
    //                numeros.Add(2);
    //            while (i <= numeroMaximo)
    //            {
    //                if (verificaEhNumeroPrimo(i))
    //                    numeros.Add(i);

    //                    i++; i++;
                    
    //            }
    //            this.numeroMaximo = numeroMaximo;
    //        }
    //        return numeros.FindAll(y => y <= numeroMaximo);
    //    }

    //    private bool verificaEhNumeroPrimo(BigInteger numero)
    //    {
    //        if (numero < 0)
    //            throw new Exception("O numero não pode ser negativo para realizar a simplificação!");

    //        int j = 2;

    //        if (numero == 1)
    //            return false;
    //        if (numero == 2)
    //            return true;

    //        bool sair = false;
    //        while (!sair)
    //        {
    //            if (numero % j == 0)
    //                return false;

    //            j++;
    //            if (j >= numero)
    //                sair = true;
    //        }

    //        //if (numero == j)
    //        return true;
    //        //return false;
    //    }

    //    public void salvarNumeros()
    //    {
    //        try
    //        {
    //            XElement xml = new XElement("NumerosPrimos");

    //            foreach (BigInteger i in numeros)
    //            {
    //                XElement x = new XElement("primo");
    //                x.Add(new XAttribute("num", i.ToString()));
    //                xml.Add(x);
    //            }
    //            xml.Save("NumerosPrimos.xml");
    //        }
    //        catch (Exception rrr) { }
    //    }
    //}
}
