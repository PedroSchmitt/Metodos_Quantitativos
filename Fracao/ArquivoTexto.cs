using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Numerics;

namespace Fracao
{
    public class ArquivoTexto
    {
        StreamWriter wr;
        StreamReader rd;
        Tipo tip;

        public string lerLinha()
        {
            if (tip == Tipo.Escrever)
                throw new Exception("É possivel apenas escrever no arquivo");
            if (!rd.EndOfStream)
                return rd.ReadLine();
            return null;
        }

        public void escrever(BigInteger num)
        {
            if (tip == Tipo.Ler)
                throw new Exception("É possivel apenas ler o arquivo");
            wr.WriteLine(num.ToString());
        }

        public void fechar()
        {
            if (tip == Tipo.Escrever)
                wr.Close();
            else
                rd.Close();
        }

        public ArquivoTexto(Tipo t)
        {
            string caminhoNome = @"C:\Users\User\Desktop\teste\NumerosPrimos.txt";

            tip = t;
            FileInfo aFile = new FileInfo(caminhoNome);
            if (t == Tipo.Escrever)
            {
                if (aFile.Exists)
                {
                    wr = new StreamWriter(caminhoNome, true);
                }
                else
                {
                    wr = aFile.CreateText();
                }
            }
            else 
            {
                if (aFile.Exists)
                {
                    rd = new StreamReader(caminhoNome);
                }
                else
                {
                    throw new Exception("Arquivo não existe!");
                }
            }
        }
        public enum Tipo {
            Escrever,
            Ler
        }
    }
}
