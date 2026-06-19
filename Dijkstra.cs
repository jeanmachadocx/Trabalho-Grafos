public static class Dijkstra
{
    public static (int custo, List<int> caminho) Executar(Grafo grafo, int origem, int destino)
    {
        int n = grafo.QuantidadeVertices;

        if (origem < 1 || origem > n)
            throw new ArgumentOutOfRangeException(nameof(origem), $"Hub de origem inválido. Informe um valor entre 1 e {n}.");

        if (destino < 1 || destino > n)
            throw new ArgumentOutOfRangeException(nameof(destino), $"Hub de destino inválido. Informe um valor entre 1 e {n}.");

        int[] dist = new int[n + 1];
        int[] anterior = new int[n + 1];
        Array.Fill(dist, int.MaxValue);
        Array.Fill(anterior, -1);

        dist[origem] = 0;

        // Fila de prioridade: (custo acumulado, vértice)
        PriorityQueue<int, int> fila = new PriorityQueue<int, int>();
        fila.Enqueue(origem, 0);

        while (fila.Count > 0)
        {
            int atual = fila.Dequeue();

            // Se já chegamos ao destino, podemos parar
            if (atual == destino)
                break;

            foreach (var (vizinho, peso, _) in grafo.GetVizinhos(atual))
            {
                if (dist[atual] == int.MaxValue)
                    continue;

                int novaDist = dist[atual] + peso;

                if (novaDist < dist[vizinho])
                {
                    dist[vizinho] = novaDist;
                    anterior[vizinho] = atual;
                    fila.Enqueue(vizinho, novaDist);
                }
            }
        }

        // Reconstrói o caminho de trás para frente
        List<int> caminho = new List<int>();

        if (dist[destino] == int.MaxValue)
            return (-1, caminho); // Sem caminho

        int v = destino;
        while (v != -1)
        {
            caminho.Add(v);
            v = anterior[v];
        }

        caminho.Reverse();

        return (dist[destino], caminho);
    }
}
