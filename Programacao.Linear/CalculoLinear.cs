using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Programacao.Linear
{
    public class CalculoLinear
    {
        public FuncaoObjetivo funcaoObjetivoInicical;
        public List<Restricao> restricoesInicial;
        public TipoLinear tipoMaxOuMin;
        public Fracao.Fracao maiorValoDaFuncaoObjeto;
        
        public FuncaoObjetivo funcaoObjetivoModificada;
        public List<Restricao> restricoesModificada;
        public List<VariavelDentro> varDentro = new List<VariavelDentro>();
        public List<Fracao.Fracao> bDivididoPorA = new List<Fracao.Fracao>();

        public FuncaoObjetivo produtorioZ = new FuncaoObjetivo();
        public FuncaoObjetivo cMenosZ = new FuncaoObjetivo();
        private int indexDoPivo;
        private int indexRestricaoSaida;

        public void inseriVariaveisDeFolgaNasRestricoes()
        {
            int qtdA = 1, qtdS = 1;
            foreach (Restricao r in restricoesModificada)
            {
                if (r.getOperacao().oper.GetHashCode() == Operacao.operacao.igual.GetHashCode())
                {
                    Variavel vA = new Variavel();
                    vA.letraVarivel = "A" + qtdA;
                    Fracao.Fracao valor = new Fracao.Fracao(new System.Numerics.BigInteger(1));
                    r.adicionaVariavel(vA, valor);
                    funcaoObjetivoModificada.adicionarVariavel(vA, maiorValoDaFuncaoObjeto);
                    adicionaVariavelFolgaEmTodasRestricoes(vA, new Fracao.Fracao(new System.Numerics.BigInteger(0)), r);
                    qtdA++;
                }
                else if (r.getOperacao().oper.GetHashCode() == Operacao.operacao.maior.GetHashCode())
                {
                    Variavel vA = new Variavel();
                    vA.letraVarivel = "A" + qtdA;
                    Variavel vS = new Variavel();
                    vS.letraVarivel = "-S" + qtdS;
                    r.adicionaVariavel(vA, new Fracao.Fracao(new System.Numerics.BigInteger(1)));
                    r.adicionaVariavel(vS, new Fracao.Fracao(new System.Numerics.BigInteger(-1)));
                    funcaoObjetivoModificada.adicionarVariavel(vA, maiorValoDaFuncaoObjeto);
                    funcaoObjetivoModificada.adicionarVariavel(vS, new Fracao.Fracao(0));
                    adicionaVariavelFolgaEmTodasRestricoes(vA, new Fracao.Fracao(new System.Numerics.BigInteger(0)), r);
                    adicionaVariavelFolgaEmTodasRestricoes(vS, new Fracao.Fracao(new System.Numerics.BigInteger(0)), r);
                    qtdA++;
                    qtdS++;
                }
                else
                {
                    Variavel vS = new Variavel();
                    vS.letraVarivel = "S" + qtdS;
                    r.adicionaVariavel(vS, new Fracao.Fracao(new System.Numerics.BigInteger(1)));
                    funcaoObjetivoModificada.adicionarVariavel(vS, new Fracao.Fracao(0));
                    adicionaVariavelFolgaEmTodasRestricoes(vS, new Fracao.Fracao(new System.Numerics.BigInteger(0)), r);
                    qtdS++;
                
                }
            }
        }

        private void adicionaVariavelFolgaEmTodasRestricoes(Variavel vA, Fracao.Fracao fracao, Restricao r)
        {
            foreach (Restricao res in restricoesModificada)
            {
                if (!res.Equals(r))
                {
                    res.adicionaVariavel(vA, fracao);
                }
            }
        }

        public void montaFOComMinOuMax()
        {
            Fracao.Fracao multiplicar = new Fracao.Fracao(1);
            if (tipoMaxOuMin == TipoLinear.Min)
                multiplicar = new Fracao.Fracao(-1);
            
            funcaoObjetivoModificada = new FuncaoObjetivo();
            List<Fracao.Fracao> coefModif = funcaoObjetivoInicical.getCoeficiente().GetRange(0, funcaoObjetivoInicical.getCoeficiente().Count);
            for (int i  = 0; i < coefModif.Count; i ++)
            {
                coefModif[i] = coefModif[i].multiplicar(multiplicar); 
            }
            funcaoObjetivoModificada.setCoeficiente(coefModif);
            List<Variavel> l = funcaoObjetivoInicical.getVariaveis().GetRange(0, funcaoObjetivoInicical.getVariaveis().Count);
            funcaoObjetivoModificada.setVariaveis(l);
            maiorValoDaFuncaoObjeto = maiorValoDaFuncaoObjeto.multiplicar(multiplicar);
        }

        public void inseriVariaveisDeDentro()
        {
            for (int restricao = 0; restricao < restricoesModificada.Count; restricao++)
            {
                List<Fracao.Fracao> listaVariaveis = restricoesModificada[restricao].getVariaveisDeFolga_Valor();
                for (int i = 0; i < listaVariaveis.Count; i++)
                {
                    if (listaVariaveis[i].comparar(new Fracao.Fracao(1)) == 0)
                    {
                        VariavelDentro v = new VariavelDentro();
                        v.setVariavel(restricoesModificada[restricao].getVariaveisDeFolga()[i]);
                        v.setValor(funcaoObjetivoModificada.getCoeficiente()[funcaoObjetivoInicical.getCoeficiente().Count + i]);
                        varDentro.Add(v);
                        i = listaVariaveis.Count;
                    }
                }
            }
        }

        public void inseriProdutorioZ()
        {
            produtorioZ = new FuncaoObjetivo();
            produtorioZ.setVariaveis(funcaoObjetivoModificada.getVariaveis().GetRange(0, funcaoObjetivoModificada.getVariaveis().Count));
            List<Fracao.Fracao> listZeros = new List<Fracao.Fracao>();
            for(int i = 0; i < funcaoObjetivoModificada.getVariaveis().Count; i++)
            {
                listZeros.Add(new Fracao.Fracao(0));
            }
            produtorioZ.setCoeficiente(listZeros);
            for (int restricao = 0; restricao < varDentro.Count; restricao++)
            {
                for (int coeficiente = 0; coeficiente < produtorioZ.getCoeficiente().Count; coeficiente++)
                {
                    Fracao.Fracao multiplicacao = new Fracao.Fracao(0);
                    if (coeficiente < restricoesModificada[restricao].getCoeficiente().Count)
                        multiplicacao = restricoesModificada[restricao].getCoeficiente()[coeficiente].multiplicar(varDentro[restricao].getValor());
                    else
                        multiplicacao = restricoesModificada[restricao].getVariaveisDeFolga_Valor()[coeficiente - restricoesModificada[restricao].getCoeficiente().Count].multiplicar(varDentro[restricao].getValor());
                    
                    produtorioZ.coeficiente[coeficiente] = produtorioZ.coeficiente[coeficiente].adicionar(multiplicacao);
                }
            }
        }

        public void inseriCMenosZ()
        {
            cMenosZ = new FuncaoObjetivo();
            cMenosZ.setVariaveis(funcaoObjetivoModificada.getVariaveis().GetRange(0, funcaoObjetivoModificada.getVariaveis().Count));
            List<Fracao.Fracao> listZeros = new List<Fracao.Fracao>();
            for (int i = 0; i < funcaoObjetivoModificada.getVariaveis().Count; i++)
            {
                listZeros.Add(new Fracao.Fracao(0));
            }
            cMenosZ.setCoeficiente(listZeros);
            for (int coeficiente = 0; coeficiente < cMenosZ.getCoeficiente().Count; coeficiente++)
            {
                cMenosZ.coeficiente[coeficiente] = funcaoObjetivoModificada.coeficiente[coeficiente].subtrair(produtorioZ.coeficiente[coeficiente]);
            }
        }

        public bool verificaSeTemCMenosZPositivo()
        {
            foreach (Fracao.Fracao f in cMenosZ.getCoeficiente())
            {
                if (f.comparar(new Fracao.Fracao(0)) == 1)
                    return true;
            }
            return false;
        }

        public void buscaPivot()
        {
            Fracao.Fracao maior = null;
            indexDoPivo = -1;
            for (int i = 0; i < cMenosZ.getCoeficiente().Count; i++ )
            {
                if (cMenosZ.getCoeficiente()[i].comparar(new Fracao.Fracao(0)) == 1)
                {
                    if (maior == null)
                    {
                        maior = cMenosZ.getCoeficiente()[i];
                        indexDoPivo = i;
                    }
                    else
                    {
                        if (cMenosZ.getCoeficiente()[i].comparar(maior) == 1)
                        {
                            maior = cMenosZ.getCoeficiente()[i];
                            indexDoPivo = i;
                        }
                    }
                }
            }
        }

        public void inseriBDivididoPorA()
        {
            bDivididoPorA = new List<Fracao.Fracao>();
            //11 - Preenchemos a última coluna com a divisão entre os valores de B (inteiros das restrições) pelos Pivot de cada linha.
            for (int restricao = 0; restricao < restricoesModificada.Count; restricao++)
            {
                Fracao.Fracao a = new Fracao.Fracao(1); 
                if (indexDoPivo < restricoesModificada[restricao].getCoeficiente().Count)
                    a = restricoesModificada[restricao].getCoeficiente()[indexDoPivo];
                else
                    a = restricoesModificada[restricao].getVariaveisDeFolga_Valor()[indexDoPivo - restricoesModificada[restricao].getCoeficiente().Count];
                    
                Fracao.Fracao b = restricoesModificada[restricao].getResultadoB();
                if (a.comparar(new Fracao.Fracao(0)) == 0)
                    bDivididoPorA.Add(new Fracao.Fracao(0));
                else
                    bDivididoPorA.Add(b.dividir(a));
            }
        }

        public void buscaVariavelSaida()
        {
            //menor valor sendo positivos e maiores que zero 
            Fracao.Fracao menor = null;
            indexRestricaoSaida = -1;
            for (int i = 0; i < bDivididoPorA.Count; i++)
            {
                if (bDivididoPorA[i].comparar(new Fracao.Fracao(0)) == 1)
                {
                    if (menor == null)
                    {
                        menor = bDivididoPorA[i];
                        indexRestricaoSaida = i;
                    }
                    else
                    {
                        if (bDivididoPorA[i].comparar(menor) == -1)
                        {
                            menor = bDivididoPorA[i];
                            indexRestricaoSaida = i;
                        }
                    }
                }
            }
        }

        public void realizaTrocaDeVariaveis()
        {
            List<Restricao> restricoesModificada_V2 = restricoesModificada.GetRange(0, restricoesModificada.Count);
            List<VariavelDentro> varDentro_V2 = varDentro.GetRange(0, varDentro.Count);
            
            //Trocamos o nome e o coeficiente da variável que sai pela variável que entra.
            varDentro_V2[indexRestricaoSaida].setVariavel(funcaoObjetivoModificada.getVariaveis()[indexDoPivo]);
            varDentro_V2[indexRestricaoSaida].setValor(funcaoObjetivoModificada.getCoeficiente()[indexDoPivo]);

            //Dividimos todas as células da linha de entrada por seu pivot
            Fracao.Fracao pivot = null;
            if (indexDoPivo >= restricoesModificada[indexRestricaoSaida].getCoeficiente().Count)
            {
                pivot = restricoesModificada[indexRestricaoSaida].getVariaveisDeFolga_Valor()[indexDoPivo - restricoesModificada[indexRestricaoSaida].getCoeficiente().Count];
            }
            else
                pivot  = restricoesModificada[indexRestricaoSaida].getCoeficiente()[indexDoPivo];

            restricoesModificada_V2[indexRestricaoSaida].dividirPeloPivot(pivot);

            for (int i = 0; i < restricoesModificada.Count; i++)
            {
                Fracao.Fracao p = null;
                if (indexDoPivo >= restricoesModificada_V2[i].getCoeficiente().Count)
                {
                    p = restricoesModificada_V2[i].getVariaveisDeFolga_Valor()[indexDoPivo - restricoesModificada_V2[i].getCoeficiente().Count];
                }
                else
                    p = restricoesModificada_V2[i].getCoeficiente()[indexDoPivo];

                if (i != indexRestricaoSaida)
                {
                    restricoesModificada_V2[i].aplicaFormulaPivot(p, restricoesModificada_V2[indexRestricaoSaida]);
                }
            }
            restricoesModificada = restricoesModificada_V2.GetRange(0, restricoesModificada_V2.Count);
            varDentro = varDentro_V2.GetRange(0, varDentro_V2.Count);

        }
    }
    public enum TipoLinear
    {
        Max = 1,
        Min = 2
    }
}
