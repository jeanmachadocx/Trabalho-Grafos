public static class WelshPowell
{
    public static (int numTurnos, int[] cores) Executar(Grafo grafo)
    {
        List<Aresta> rotas = grafo.Arestas;
        int numRotas = rotas.Count;

        if (numRotas == 0)
            return (0, Array.Empty<int>());

        List<int>[] conflitos = new List<int>[numRotas];
        for (int i = 0; i < numRotas; i++) conflitos[i] = new List<int>();

        for (int i = 0; i < numRotas; i++)
        {
            for (int j = i + 1; j < numRotas; j++)
            {
                bool compartilham = rotas[i].Origem == rotas[j].Origem
                                 || rotas[i].Origem == rotas[j].Destino
                                 || rotas[i].Destino == rotas[j].Origem
                                 || rotas[i].Destino == rotas[j].Destino;

                if (compartilham)
                {
                    conflitos[i].Add(j);
                    conflitos[j].Add(i);
                }
            }
        }

        int[] ordem = new int[numRotas];
        for (int i = 0; i < numRotas; i++) ordem[i] = i;
        Array.Sort(ordem, (a, b) => conflitos[b].Count.CompareTo(conflitos[a].Count));

        int[] cores = new int[numRotas];
        Array.Fill(cores, -1);

        foreach (int i in ordem)
        {
            bool[] turnosUsados = new bool[numRotas + 1];

            foreach (int j in conflitos[i])
                if (cores[j] >= 0)
                    turnosUsados[cores[j]] = true;

            int turno = 0;
            while (turnosUsados[turno]) turno++;
            cores[i] = turno;
        }

        int numTurnos = 0;
        foreach (int c in cores)
            if (c + 1 > numTurnos) numTurnos = c + 1;

        return (numTurnos, cores);
    }
}
