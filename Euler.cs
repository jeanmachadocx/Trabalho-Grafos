public static class Euler
{
    // Verifica a existência do Ciclo Euleriano e, se existir, constrói o percurso.
    // Grafo direcionado: precisa de grauEntrada == grauSaida em todo vértice
    // e de todos os vértices com aresta dentro de um único componente fortemente conexo.
    public static (bool existe, List<int> percurso, string motivo) Executar(Grafo grafo, int inicio)
    {
        int n = grafo.QuantidadeVertices;

        if (inicio < 1 || inicio > n)
            throw new ArgumentOutOfRangeException(nameof(inicio), $"Hub inicial inválido. Informe um valor entre 1 e {n}.");

        // Condição 1: balanceamento de grau (entrada == saída)
        for (int v = 1; v <= n; v++)
        {
            int entrada = grafo.GetGrauEntrada(v);
            int saida = grafo.GetGrauSaida(v);

            if (entrada != saida)
                return (false, new List<int>(),
                    $"o hub {v} tem grau de entrada ({entrada}) diferente do grau de saída ({saida}).");
        }

        // Condição 2: conectividade forte dos vértices que possuem arestas (Kosaraju)
        if (!FortementeConexo(grafo))
            return (false, new List<int>(),
                "as rotas não formam um único componente fortemente conexo.");

        // Ambas as condições satisfeitas: constrói o percurso com Hierholzer
        List<int> percurso = Hierholzer(grafo, inicio);
        return (true, percurso, string.Empty);
    }

    // Kosaraju: verifica se todos os vértices com grau != 0 estão em um único SCC.
    private static bool FortementeConexo(Grafo grafo)
    {
        int n = grafo.QuantidadeVertices;

        // Adjacência normal e transposta montadas a partir da lista de arestas
        List<int>[] adj = new List<int>[n + 1];
        List<int>[] adjT = new List<int>[n + 1];
        for (int i = 0; i <= n; i++)
        {
            adj[i] = new List<int>();
            adjT[i] = new List<int>();
        }

        foreach (var a in grafo.Arestas)
        {
            adj[a.Origem].Add(a.Destino);
            adjT[a.Destino].Add(a.Origem);
        }

        // Escolhe um vértice de partida que possua arestas (ignora isolados)
        int partida = -1;
        for (int v = 1; v <= n; v++)
        {
            if (grafo.GetGrauSaida(v) > 0 || grafo.GetGrauEntrada(v) > 0)
            {
                partida = v;
                break;
            }
        }

        // Sem arestas: não há rotas para inspecionar
        if (partida == -1)
            return false;

        // 1ª passagem (DFS no grafo original) — preenche a pilha por ordem de término
        bool[] visitado = new bool[n + 1];
        Stack<int> ordem = new Stack<int>();
        for (int v = 1; v <= n; v++)
        {
            if (!visitado[v] && (adj[v].Count > 0 || adjT[v].Count > 0))
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
                Buscar(v, adjT, visitado);
            }
        }

        // Fortemente conexo (para fins eulerianos) se todos os vértices com aresta
        // formam exatamente um componente.
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

    private static void Buscar(int v, List<int>[] adj, bool[] visitado)
    {
        visitado[v] = true;
        foreach (int w in adj[v])
        {
            if (!visitado[w])
                Buscar(w, adj, visitado);
        }
    }

    // Hierholzer: monta uma adjacência local consumível (suporta arestas paralelas)
    // e percorre o grafo formando o circuito euleriano.
    private static List<int> Hierholzer(Grafo grafo, int inicio)
    {
        int n = grafo.QuantidadeVertices;

        // Pilha de destinos não usados por vértice
        Stack<int>[] saidas = new Stack<int>[n + 1];
        for (int i = 0; i <= n; i++)
            saidas[i] = new Stack<int>();

        foreach (var a in grafo.Arestas)
            saidas[a.Origem].Push(a.Destino);

        List<int> circuito = new List<int>();
        Stack<int> caminho = new Stack<int>();
        caminho.Push(inicio);

        while (caminho.Count > 0)
        {
            int u = caminho.Peek();

            if (saidas[u].Count > 0)
            {
                int proximo = saidas[u].Pop();
                caminho.Push(proximo);
            }
            else
            {
                circuito.Add(u);
                caminho.Pop();
            }
        }

        circuito.Reverse();
        return circuito;
    }
}
