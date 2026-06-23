# Sistema de Otimização de Rotas Logísticas (SORL)

Trabalho Prático da disciplina **Algoritmos em Grafos** — PUC Minas 2026/01.

O sistema analisa e otimiza uma rede logística representada por um grafo direcionado e ponderado, respondendo perguntas estratégicas da empresa fictícia **Entrega Máxima Logística S.A.**

Link RELATÓRIO TECNICO: https://docs.google.com/document/d/1xq4tDxqpBReh_O67DIqIpmNGQF3-5bwBtxB7ldHG5lY/edit?usp=sharing

---

## Estrutura do Projeto

```
ProjetoLogistica/
├── Aresta.cs               # Representa uma aresta (origem, destino, peso, capacidade)
├── Grafo.cs                # Estrutura do grafo com seleção automática de representação
├── LeitorDIMACS.cs         # Leitura de arquivos no formato DIMACS
├── Dijkstra.cs             # Problema I  — Roteamento de Menor Custo
├── FordFulkerson.cs        # Problema II — Capacidade Máxima de Escoamento
├── AlgoritmosRestantes.cs  # Problemas III, IV e V (Kruskal, Coloração, Euler, Hamilton)
├── Logger.cs               # Geração de arquivo de log por grafo
└── Program.cs              # Menu interativo
```

---

## Problemas e Algoritmos

| # | Problema | Algoritmo |
|---|----------|-----------|
| I | Roteamento de Menor Custo | Dijkstra |
| II | Capacidade Máxima de Escoamento | Ford-Fulkerson (Edmonds-Karp) |
| III | Expansão da Rede de Comunicação | Kruskal |
| IV | Agendamento de Manutenções sem Conflito | Coloração de Grafos |
| V-A | Percurso de Rotas (arestas) | Ciclo Euleriano (Fleury) |
| V-B | Percurso de Hubs (vértices) | Ciclo Hamiltoniano (Backtracking) |

---

## Como Executar

**Pré-requisito:** .NET 8 SDK instalado.

```bash
git clone https://github.com/jeanmachadocx/Trabalho-Grafos.git
cd Trabalho-Grafos
dotnet run
```

Ao iniciar, o sistema pedirá o caminho de um arquivo no formato DIMACS.

---

## Formato de Entrada (DIMACS)

```
5 6
1 2 10 20
1 3 15 10
2 4 5 30
...
```

- **Linha 1:** número de vértices e número de arestas  
- **Demais linhas:** `origem destino peso capacidade`

---

## Representação do Grafo

O sistema identifica automaticamente a densidade do grafo e escolhe a estrutura mais eficiente:

- **Grafo esparso** (densidade < 50%) → Lista de Adjacência  
- **Grafo denso** (densidade ≥ 50%) → Matriz de Adjacência

---

## Log de Execução

Cada execução gera um arquivo `log_<nome_do_grafo>.txt` com todos os resultados: caminhos, custos, fluxos, cortes mínimos e percursos.

---

## Tecnologia

- Linguagem: **C#**
- Plataforma: **.NET 8**
