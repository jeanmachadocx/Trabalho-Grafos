public static class Euler
{
    // Verifica a existência do Ciclo Euleriano e, se existir, constrói o percurso.
    public static (bool existe, List<int> percurso, string motivo) Executar(Grafo grafo, int inicio)
    {
        int n = grafo.QuantidadeVertices;

        if (inicio < 1 || inicio > n)
            throw new ArgumentOutOfRangeException(nameof(inicio), $"Hub inicial inválido. Informe um valor entre 1 e {n}.");

        for (int v = 1; v <= n; v++)
        {
            int entrada = grafo.GetGrauEntrada(v);
            int saida = grafo.GetGrauSaida(v);

            if (entrada != saida)
                return (false, new List<int>(),
                    $"o hub {v} tem grau de entrada ({entrada}) diferente do grau de saída ({saida}).");
        }

        if (!FortementeConexo_Kosaraju(grafo))
            return (false, new List<int>(),
                "as rotas não formam um único componente fortemente conexo.");

        List<int> percurso = Fleury(grafo, inicio);
        return (true, percurso, string.Empty);
    }

    private static bool FortementeConexo_Kosaraju(Grafo grafo)
    {
        int n = grafo.QuantidadeVertices;

        List<int>[] adj = new List<int>[n + 1];
        List<int>[] adjTransposto = new List<int>[n + 1];
        for (int i = 0; i <= n; i++)
        {
            adj[i] = new List<int>();
            adjTransposto[i] = new List<int>();
        }

        foreach (var a in grafo.Arestas)
        {
            adj[a.Origem].Add(a.Destino);
            adjTransposto[a.Destino].Add(a.Origem);
        }

        int partida = -1;
        for (int v = 1; v <= n; v++)
        {
            if (grafo.GetGrauSaida(v) > 0 || grafo.GetGrauEntrada(v) > 0)
            {
                partida = v;
                break;
            }
        }

        if (partida == -1)
            return false;

        // 1ª passagem (DFS no grafo original) — preenche a pilha por ordem de término
        bool[] visitado = new bool[n + 1];
        Stack<int> ordem = new Stack<int>();
        for (int v = 1; v <= n; v++)
        {
            if (!visitado[v] && (adj[v].Count > 0 || adjTransposto[v].Count > 0))
                PreencherOrdem(v, adj, visitado, ordem);
        }

        // 2ª passagem (DFS no grafo transposto) — conta os componentes
        Array.Clear(visitado, 0, visitado.Length);
        int componentes = 0;
        while (ordem.Count > 0)
        {
            int v = ordem.Pop();
            if (!visitado[v])
            {
                componentes++;
                Buscar(v, adjTransposto, visitado);
            }
        }

        return componentes == 1;
    }

    private static void PreencherOrdem(int v, List<int>[] adj, bool[] visitado, Stack<int> ordem)
    {
        visitado[v] = true;
        foreach (int w in adj[v])
        {
            if (!visitado[w])
                PreencherOrdem(w, adj, visitado, ordem);
        }
        ordem.Push(v);
    }

    private static void Buscar(int v, List<int>[] adjTransposto, bool[] visitado)
    {
        visitado[v] = true;
        foreach (int w in adjTransposto[v])
        {
            if (!visitado[w])
                Buscar(w, adjTransposto, visitado);
        }
    }

    private static List<int> Fleury(Grafo grafo, int inicio)
    {
        int n = grafo.QuantidadeVertices;

        List<int>[] saidas = new List<int>[n + 1];
        for (int i = 0; i <= n; i++)
            saidas[i] = new List<int>();

        foreach (var a in grafo.Arestas)
            saidas[a.Origem].Add(a.Destino);

        int arestasRestantes = grafo.Arestas.Count;

        List<int> circuito = new List<int> { inicio };
        int atual = inicio;

        while (arestasRestantes > 0)
        {
            List<int> destinos = saidas[atual];

            int escolhido = 0;
            for (int i = 0; i < destinos.Count; i++)
            {
                if (!EhPonte(saidas, atual, i, arestasRestantes))
                {
                    escolhido = i;
                    break;
                }
            }

            int proximo = destinos[escolhido];
            destinos.RemoveAt(escolhido);
            arestasRestantes--;

            circuito.Add(proximo);
            atual = proximo;
        }

        return circuito;
    }

    private static bool EhPonte(List<int>[] saidas, int aDestino, int indice, int arestasRestantes)
    {
        if (saidas[aDestino].Count == 1)
            return false;

        int destino = saidas[aDestino][indice];

        saidas[aDestino].RemoveAt(indice);

        bool[] visitado = new bool[saidas.Length];
        int alcancaveis = ContarArestasAlcancaveis(destino, saidas, visitado);

        saidas[aDestino].Insert(indice, destino);

        return alcancaveis < arestasRestantes - 1;
    }

    private static int ContarArestasAlcancaveis(int v, List<int>[] saidas, bool[] visitado)
    {
        visitado[v] = true;
        int total = saidas[v].Count; 

        foreach (int w in saidas[v])
        {
            if (!visitado[w])
                total += ContarArestasAlcancaveis(w, saidas, visitado);
        }

        return total;
    }
}
