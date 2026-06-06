public static class LeitorDIMACS
{
    public static Grafo Ler(string caminho)
    {
        string[] linhas = File.ReadAllLines(caminho);

        // Primeira linha: número de vértices e número de arestas
        string[] primeira = linhas[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int vertices = int.Parse(primeira[0]);
        int totalArestas = int.Parse(primeira[1]);

        Grafo grafo = new Grafo(vertices, totalArestas);

        for (int i = 1; i < linhas.Length; i++)
        {
            string linha = linhas[i].Trim();

            if (string.IsNullOrWhiteSpace(linha))
                continue;

            string[] dados = linha.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int origem     = int.Parse(dados[0]);
            int destino    = int.Parse(dados[1]);
            int peso       = int.Parse(dados[2]);
            int capacidade = int.Parse(dados[3]);

            grafo.AdicionarAresta(origem, destino, peso, capacidade);
        }

        return grafo;
    }
}
