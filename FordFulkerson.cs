public static class FordFulkerson
{
    // BFS para achar um caminho aumentante; preenche o array 'pai'
    private static bool BFS(int[,] capacidade, int origem, int destino, int[] pai, int n)
    {
        bool[] visitado = new bool[n];
        Array.Fill(pai, -1);

        Queue<int> fila = new Queue<int>();
        fila.Enqueue(origem);
        visitado[origem] = true;

        while (fila.Count > 0)
        {
            int u = fila.Dequeue();

            for (int v = 0; v < n; v++)
            {
                if (!visitado[v] && capacidade[u, v] > 0)
                {
                    fila.Enqueue(v);
                    pai[v] = u;
                    visitado[v] = true;
                }
            }
        }

        return visitado[destino];
    }

    public static (int fluxoMaximo, List<(int, int)> corteMinimo) Executar(
        Grafo grafo, int origem, int destino)
    {
        int n = grafo.QuantidadeVertices + 1;
        int[,] capacidade = grafo.GetMatrizCapacidade();

        int[] pai = new int[n];
        int fluxoMax = 0;

        // Enquanto existir caminho aumentante
        while (BFS(capacidade, origem, destino, pai, n))
        {
            // Acha o gargalo 
            int gargalo = int.MaxValue;

            for (int v = destino; v != origem; v = pai[v])
            {
                int u = pai[v];
                gargalo = Math.Min(gargalo, capacidade[u, v]);
            }

            // Atualiza capacidades residuais
            for (int v = destino; v != origem; v = pai[v])
            {
                int u = pai[v];
                capacidade[u, v] -= gargalo;
                capacidade[v, u] += gargalo;
            }

            fluxoMax += gargalo;
        }

        // Identifica o corte mínimo:
        // Faz BFS final a partir da origem no grafo residual
        // Os vértices alcancaveis formam o lado S do corte
        bool[] alcancavel = new bool[n];
        Queue<int> bfsCorte = new Queue<int>();
        bfsCorte.Enqueue(origem);
        alcancavel[origem] = true;

        while (bfsCorte.Count > 0)
        {
            int u = bfsCorte.Dequeue();
            for (int v = 0; v < n; v++)
            {
                if (!alcancavel[v] && capacidade[u, v] > 0)
                {
                    alcancavel[v] = true;
                    bfsCorte.Enqueue(v);
                }
            }
        }

        // Arestas do corte minimo: partem de vértice alcançável para não alcançável
        List<(int, int)> corte = new List<(int, int)>();
        int[,] capOriginal = grafo.GetMatrizCapacidade();

        for (int u = 0; u < n; u++)
        {
            for (int v = 0; v < n; v++)
            {
                if (alcancavel[u] && !alcancavel[v] && capOriginal[u, v] > 0)
                    corte.Add((u, v));
            }
        }

        return (fluxoMax, corte);
    }
}
