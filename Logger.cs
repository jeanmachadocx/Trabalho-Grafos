public static class Logger
{
    private static string _arquivo = "log.txt";

    public static void Iniciar(string nomeGrafo)
    {
        _arquivo = $"log_{nomeGrafo}.txt";
        File.WriteAllText(_arquivo, $"=== Log de Execução - {nomeGrafo} ===\n\n");
    }

    public static void Registrar(string mensagem)
    {
        Console.WriteLine(mensagem);
        File.AppendAllText(_arquivo, mensagem + "\n");
    }

    public static void Separador()
    {
        Registrar("--------------------------------------------------");
    }
}
