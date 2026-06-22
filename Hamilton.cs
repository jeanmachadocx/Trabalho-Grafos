public static class Hamilton
{
    // Verifica a existência de um Ciclo Hamiltoniano (visita todos os hubs
    // exatamente uma vez e retorna ao inicial) usando backtracking.
    // Problema NP-completo: viável para os grafos pequenos do trabalho.
    public static (bool existe, List<int> caminho) Executar(Grafo grafo, int inicio)
    {
        int n = grafo.QuantidadeVertices;

        if (inicio < 1 || inicio > n)
            throw new ArgumentOutOfRangeException(nameof(inicio), $"Hub inicial inválido. Informe um valor entre 1 e {n}.");

        bool[] visitado = new bool[n + 1];
        List<int> caminho = new List<int> { inicio };
        visitado[inicio] = true;

        if (Backtrack(grafo, inicio, inicio, 1, n, visitado, caminho))
            return (true, caminho);

        return (false, new List<int>());
    }

    // atual: vértice atual | inicio: hub de partida | visitados: quantos já foram incluídos
    private static bool Backtrack(Grafo grafo, int atual, int inicio, int visitados, int n,
        bool[] visitado, List<int> caminho)
    {
        // Todos os hubs visitados: o ciclo fecha se existir aresta de volta ao início
        if (visitados == n)
        {
            foreach (var (vizinho, _, _) in grafo.GetVizinhos(atual))
            {
                if (vizinho == inicio)
                {
                    caminho.Add(inicio);
                    return true;
                }
            }
            return false;
        }

        foreach (var (vizinho, _, _) in grafo.GetVizinhos(atual))
        {
            if (!visitado[vizinho])
            {
                visitado[vizinho] = true;
                caminho.Add(vizinho);

                if (Backtrack(grafo, vizinho, inicio, visitados + 1, n, visitado, caminho))
                    return true;

                // Backtrack: desfaz a escolha
                visitado[vizinho] = false;
                caminho.RemoveAt(caminho.Count - 1);
            }
        }

        return false;
    }
}
