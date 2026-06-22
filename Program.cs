class Program
{
    static Grafo grafo = null;

    static void Main()
    {
        Console.WriteLine("=== Sistema de Otimização de Rotas Logísticas (SORL) ===\n");

        CarregarGrafo();

        bool rodando = true;
        while (rodando)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1 - Roteamento de Menor Custo (Dijkstra)");
            Console.WriteLine("2 - Capacidade Máxima de Escoamento (Ford-Fulkerson)");
            Console.WriteLine("3 - Expansão da Rede de Comunicação (Kruskal)");
            Console.WriteLine("4 - Agendamento de Manutenções sem Conflito (Welsh-Powell)");
            Console.WriteLine("5 - Rota Única de Inspeção (Euler/Hamilton)");
            Console.WriteLine("6 - Carregar outro arquivo");
            Console.WriteLine("0 - Sair");
            Console.Write("\nEscolha: ");

            string entrada = Console.ReadLine();
            Console.WriteLine();

            switch (entrada)
            {
                case "1":
                    MenuDijkstra();
                    break;

                case "2":
                    MenuFordFulkerson();
                    break;

                case "3":
                    MenuKruskal();
                    break;

                case "4":
                    MenuWelshPowell();
                    break;

                case "5":
                    MenuRotaInspecao();
                    break;

                case "6":
                    CarregarGrafo();
                    break;

                case "0":
                    rodando = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    static void CarregarGrafo()
    {
        Console.Write("Informe o caminho do arquivo DIMACS: ");
        string caminho = Console.ReadLine().Trim();

        if (!File.Exists(caminho))
        {
            Console.WriteLine("Arquivo não encontrado.");
            return;
        }

        grafo = LeitorDIMACS.Ler(caminho);

        string nomeArquivo = Path.GetFileNameWithoutExtension(caminho);
        Logger.Iniciar(nomeArquivo);

        Logger.Registrar($"Grafo carregado: {caminho}");
        grafo.ExibirEstrutura();
        Logger.Registrar($"Vértices: {grafo.QuantidadeVertices} | Arestas: {grafo.QuantidadeArestas}");
        Logger.Separador();
    }

    static void MenuDijkstra()
    {
        if (grafo == null)
        {
            Console.WriteLine("Carregue um grafo primeiro.");
            return;
        }

        int origem = LerHubValido("Hub de origem: ");
        int destino = LerHubValido("Hub de destino: ");

        try
        {
            var (custo, caminho) = Dijkstra.Executar(grafo, origem, destino);

            Logger.Separador();
            Logger.Registrar("[Dijkstra] Roteamento de Menor Custo");
            Logger.Registrar($"Origem: {origem} | Destino: {destino}");

            if (custo == -1)
            {
                Logger.Registrar("Não existe caminho entre os hubs informados.");
            }
            else
            {
                Logger.Registrar($"Custo total: R$ {custo}");
                Logger.Registrar($"Caminho: {string.Join(" -> ", caminho)}");
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void MenuFordFulkerson()
    {
        if (grafo == null)
        {
            Console.WriteLine("Carregue um grafo primeiro.");
            return;
        }

        int s = LerHubValido("Hub S (origem): ");
        int t = LerHubValido("Hub T (destino): ");

        try
        {
            var (fluxo, corte) = FordFulkerson.Executar(grafo, s, t);

            Logger.Separador();
            Logger.Registrar("[Ford-Fulkerson] Capacidade Máxima de Escoamento");
            Logger.Registrar($"S: {s} | T: {t}");
            Logger.Registrar($"Fluxo máximo: {fluxo} toneladas/dia");
            Logger.Registrar("Corte mínimo (arestas gargalo):");

            foreach (var (u, v) in corte)
                Logger.Registrar($"  {u} -> {v}");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void MenuKruskal()
    {
        if (grafo == null)
        {
            Console.WriteLine("Carregue um grafo primeiro.");
            return;
        }

        var (custo, arvore) = Kruskal.Executar(grafo);

        Logger.Separador();
        Logger.Registrar("[Kruskal] Expansão da Rede de Comunicação");

        if (custo == -1)
        {
            Logger.Registrar("Grafo desconexo — não é possível interligar todos os hubs.");
            Logger.Registrar($"Conexões parciais encontradas: {arvore.Count} de {grafo.QuantidadeVertices - 1} necessárias.");
        }
        else
        {
            Logger.Registrar($"Conexões selecionadas: {arvore.Count}");
            Logger.Registrar($"Custo total da rede: R$ {custo}");
            Logger.Registrar("Conexões de fibra óptica:");

            foreach (var a in arvore)
                Logger.Registrar($"  {a.Origem} -- {a.Destino}  |  Custo: R$ {a.Peso}");

            Logger.Registrar($"Todos os {grafo.QuantidadeVertices} hubs estão interligados com custo mínimo.");
        }
    }

    static void MenuWelshPowell()
    {
        if (grafo == null)
        {
            Console.WriteLine("Carregue um grafo primeiro.");
            return;
        }

        if (grafo.QuantidadeArestas == 0)
        {
            Console.WriteLine("Nenhuma rota disponível para agendamento.");
            return;
        }

        var (numTurnos, cores) = WelshPowell.Executar(grafo);
        List<Aresta> rotas = grafo.Arestas;

        Logger.Separador();
        Logger.Registrar("[Welsh-Powell] Agendamento de Manutenções sem Conflito");
        Logger.Registrar($"Rotas analisadas: {rotas.Count}");
        Logger.Registrar($"Turnos necessários: {numTurnos}");

        for (int turno = 0; turno < numTurnos; turno++)
        {
            Logger.Separador();
            Logger.Registrar($"Turno {turno + 1}:");

            for (int i = 0; i < rotas.Count; i++)
                if (cores[i] == turno)
                    Logger.Registrar($"  Rota {i + 1}: {rotas[i].Origem} -> {rotas[i].Destino}  |  Peso: {rotas[i].Peso}");
        }

        Logger.Separador();
        Logger.Registrar($"Com {numTurnos} turno(s) é possível executar todas as manutenções sem conflito de recursos.");
    }

    static void MenuRotaInspecao()
    {
        if (grafo == null)
        {
            Console.WriteLine("Carregue um grafo primeiro.");
            return;
        }

        Console.WriteLine("--- Rota Única de Inspeção ---");
        Console.WriteLine("A - Percurso de Rotas  (todas as arestas uma vez) [Euler]");
        Console.WriteLine("B - Percurso de Hubs   (todos os vértices uma vez) [Hamilton]");
        Console.Write("\nEscolha o cenário: ");

        string cenario = Console.ReadLine()?.Trim().ToUpper();
        Console.WriteLine();

        switch (cenario)
        {
            case "A":
                MenuEuler();
                break;

            case "B":
                MenuHamilton();
                break;

            default:
                Console.WriteLine("Cenário inválido.");
                break;
        }
    }

    static void MenuEuler()
    {
        int inicio = LerHubValido("Hub inicial: ");

        try
        {
            var (existe, percurso, motivo) = Euler.Executar(grafo, inicio);

            Logger.Separador();
            Logger.Registrar("[Euler] Percurso de Rotas (Ciclo Euleriano)");
            Logger.Registrar($"Início: {inicio}");

            if (existe)
            {
                Logger.Registrar("Resultado: Percurso possível.");
                Logger.Registrar($"Sequência: {string.Join(" -> ", percurso)}");
            }
            else
            {
                Logger.Registrar($"Resultado: Não é possível. Motivo: {motivo}");
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void MenuHamilton()
    {
        int inicio = LerHubValido("Hub inicial: ");

        try
        {
            var (existe, caminho) = Hamilton.Executar(grafo, inicio);

            Logger.Separador();
            Logger.Registrar("[Hamilton] Percurso de Hubs (Ciclo Hamiltoniano)");
            Logger.Registrar($"Início: {inicio}");

            if (existe)
            {
                Logger.Registrar("Resultado: Percurso possível.");
                Logger.Registrar($"Sequência: {string.Join(" -> ", caminho)}");
            }
            else
            {
                Logger.Registrar("Resultado: Não é possível visitar todos os hubs exatamente uma vez e retornar ao inicial.");
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static int LerHubValido(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out int valor))
            {
                Console.WriteLine("Digite um número inteiro válido.");
                continue;
            }

            if (grafo == null || valor < 1 || valor > grafo.QuantidadeVertices)
            {
                Console.WriteLine($"Digite um valor entre 1 e {grafo.QuantidadeVertices}.");
                continue;
            }

            return valor;
        }
    }
}
