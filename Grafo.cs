public class Grafo
{
    public int QuantidadeVertices { get; private set; }
    public int QuantidadeArestas { get; private set; }
    public bool UsarMatriz { get; private set; }

    // Lista de adjacência: cada posição guarda os vizinhos (destino, peso, capacidade)
    private List<(int destino, int peso, int capacidade)>[] _listaAdj;

    // Matriz de adjacência: [origem, destino] = peso  (0 = sem aresta)
    private int[,] _matrizPeso;
    private int[,] _matrizCapacidade;

    // Lista com todas as arestas (usada pelo Kruskal e outros)
    public List<Aresta> Arestas { get; private set; }

    // Densidade a partir da qual usamos matriz (ajuste se necessário)
    private const double LimiteDensidade = 0.5;

    public Grafo(int vertices, int arestas)
    {
        QuantidadeVertices = vertices;
        QuantidadeArestas = arestas;
        Arestas = new List<Aresta>();

        double densidade = (double)arestas / ((double)vertices * (vertices - 1));
        UsarMatriz = densidade >= LimiteDensidade;

        if (UsarMatriz)
        {
            _matrizPeso = new int[vertices + 1, vertices + 1];
            _matrizCapacidade = new int[vertices + 1, vertices + 1];
        }
        else
        {
            _listaAdj = new List<(int, int, int)>[vertices + 1];
            for (int i = 0; i <= vertices; i++)
                _listaAdj[i] = new List<(int, int, int)>();
        }
    }

    public void AdicionarAresta(int origem, int destino, int peso, int capacidade)
    {
        Arestas.Add(new Aresta(origem, destino, peso, capacidade));

        if (UsarMatriz)
        {
            _matrizPeso[origem, destino] = peso;
            _matrizCapacidade[origem, destino] = capacidade;
        }
        else
        {
            _listaAdj[origem].Add((destino, peso, capacidade));
        }
    }

    // Retorna vizinhos de um vértice como lista (independente da estrutura interna)
    public List<(int destino, int peso, int capacidade)> GetVizinhos(int vertice)
    {
        if (UsarMatriz)
        {
            var vizinhos = new List<(int, int, int)>();
            for (int v = 1; v <= QuantidadeVertices; v++)
            {
                if (_matrizPeso[vertice, v] > 0)
                    vizinhos.Add((v, _matrizPeso[vertice, v], _matrizCapacidade[vertice, v]));
            }
            return vizinhos;
        }
        else
        {
            return _listaAdj[vertice];
        }
    }

    // Grau de saída: quantidade de arestas que partem do vértice
    public int GetGrauSaida(int vertice)
    {
        return GetVizinhos(vertice).Count;
    }

    // Grau de entrada: quantidade de arestas que chegam ao vértice
    public int GetGrauEntrada(int vertice)
    {
        int grau = 0;
        foreach (var a in Arestas)
        {
            if (a.Destino == vertice)
                grau++;
        }
        return grau;
    }

    // Retorna a matriz de capacidade (cópia) para o Ford-Fulkerson
    public int[,] GetMatrizCapacidade()
    {
        int n = QuantidadeVertices + 1;
        int[,] cap = new int[n, n];

        foreach (var a in Arestas)
            cap[a.Origem, a.Destino] = a.Capacidade;

        return cap;
    }

    public void ExibirEstrutura()
    {
        double densidade = (double)QuantidadeArestas / ((double)QuantidadeVertices * (QuantidadeVertices - 1));
        Console.WriteLine($"Vértices: {QuantidadeVertices} | Arestas: {QuantidadeArestas}");
        Console.WriteLine($"Densidade: {densidade:P1} => Usando {(UsarMatriz ? "Matriz" : "Lista")} de Adjacência");
    }
}
