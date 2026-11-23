# Product backlog

This file records future portfolio stages that are intentionally outside the completed Junior release. Items remain proposals until their technical claims, package choices, and learning outcomes are validated against current primary documentation.

## Middle and Senior

- Complete the bilingual Middle learning path, laboratory, and interview simulator before marking the level available.
- Complete the bilingual Senior learning path, laboratory, and interview simulator before marking the level available.
- Preserve one shared UI and domain model; runtime capabilities must continue to be selected through configuration and dependency injection.

## Cross-cutting AI evolution

Junior already teaches the safe baseline: server-side secrets, minimum context, constrained output, fallback, and human review. Later levels should deepen that boundary instead of duplicating the Junior module.

### Middle candidate outcomes

- Replace the deterministic provider through the existing `IAiOrderSuggestionService` boundary without coupling UI or domain rules to one vendor.
- Evaluate `Microsoft.Extensions.AI`, Semantic Kernel, Azure AI Foundry, OpenAI-compatible APIs, and local models from their current official documentation before choosing dependencies.
- Teach embeddings, vector search, retrieval-augmented generation, structured output, function calling, and evaluation with explicit trade-offs.
- Add an integration laboratory with provider fakes, contract tests, timeouts, cancellation, and deterministic fallbacks.

### Senior and architecture candidate outcomes

- Operate AI workloads with observability, privacy-safe logging, security controls, rate limits, budgets, performance measurements, and CI/CD evaluation gates.
- Compare tool calling, MCP, agents, and multi-agent orchestration only where they solve a demonstrated problem; avoid presenting orchestration complexity as a default.
- Cover cloud-native deployment, managed identity and secret storage, failure isolation, model/provider portability, and cost-aware routing.
- Build a portfolio-grade AI case whose behavior remains understandable and testable without requiring visitors to provide credentials.

## Interview simulator expansion

Before adding questions, compare each candidate against the existing Junior catalog to prevent duplicate prompts. Middle and Senior questions must evaluate reasoning through a clear explanation, a code or query example, and a realistic use case rather than memorized definitions.

### Candidate topic groups

- C# and runtime: `const` vs `readonly` vs `static readonly`, interface vs abstract class, boxing and unboxing, and `IEnumerable` vs `IQueryable`.
- ASP.NET Core and Web API: middleware execution, DI lifetimes, authentication vs authorization, PUT vs PATCH, and centralized exception handling.
- SQL Server: clustered vs non-clustered indexes, second-highest-value queries, `WHERE` vs `HAVING`, execution plans, and SQL injection prevention.
- Entity Framework Core: N+1 queries, `AsNoTracking`, eager vs lazy loading, migrations, and diagnosing slow database queries.
- Real-world engineering: slow API diagnosis, nullable database values, SOLID application, business logic boundaries, and production-only incident debugging.

These groups are curriculum candidates, not a commitment to exactly 25 questions. Difficulty and ownership should be assigned only after checking whether a topic belongs to Junior review, Middle design, Senior production, or the later architecture track.

## Reference material

![.NET AI Engineer roadmap reference](docs/references/dotnet-ai-engineer-roadmap-2026-27.png)

The image above was supplied by the project owner on 2026-11-19 as planning inspiration. It is preserved for later curriculum analysis, not treated as an authoritative source. Attribution visible in the original image remains intact; every topic and version must be verified before implementation.

![Full-stack .NET interview checklist reference](docs/references/full-stack-dotnet-interview-checklist.png)

This interview checklist was supplied by the project owner on 2026-11-19. It is retained to compare future Middle and Senior interview coverage. The visible attribution remains intact; question wording and technical answers must be reviewed against current Microsoft and database documentation before use.
