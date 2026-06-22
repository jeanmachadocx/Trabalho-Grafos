public static class Kruskal
{
    public static (int custoTotal, List<Aresta> arvore) Executar(Grafo grafo)
    {
        int n = grafo.QuantidadeVertices;

        List<Aresta> arestas = new List<Aresta>(grafo.Arestas);
        arestas.Sort((a, b) => a.Peso.CompareTo(b.Peso));

        int[] pai = new int[n + 1];
        for (int i = 0; i <= n; i++) pai[i] = i;

        int custo = 0;
        List<Aresta> arvore = new List<Aresta>();

        foreach (var aresta in arestas)
        {
            int raizA = Encontrar(pai, aresta.Origem);
            int raizB = Encontrar(pai, aresta.Destino);

            if (raizA == raizB)
                continue;

            pai[raizA] = raizB;
            arvore.Add(aresta);
            custo += aresta.Peso;

            if (arvore.Count == n - 1)
                break;
        }

        if (arvore.Count < n - 1)
            return (-1, arvore);

        return (custo, arvore);
    }

    private static int Encontrar(int[] pai, int x)
    {
        while (pai[x] != x)
        {
            pai[x] = pai[pai[x]]; 
            x = pai[x];
        }
        return x;
    }
}
