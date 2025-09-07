using CRM.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace CRM.Services.Limpeza
{
    public class SessaoCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SessaoCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var agora = DateTime.UtcNow;

                // Primeiro filtra o que o EF entende (DataExpiracao)
                var sessoes = await db.SessoesUsuarios
                    .Where(s => s.DataExpiracao <= agora)
                    .ToListAsync(stoppingToken);

                // Agora adiciona à lista os que passaram de 20 min sem uso
                var inativos = await db.SessoesUsuarios
                    .AsNoTracking()
                    .ToListAsync(stoppingToken);

                var inativosPorTempo = inativos
                    .Where(s => (agora - s.UltimaRequisicao).TotalMinutes > 20)
                    .ToList();

                // Junta tudo (evita duplicatas)
                var expiradas = sessoes
                    .Union(inativosPorTempo)
                    .Distinct()
                    .ToList();

                if (expiradas.Any())
                {
                    db.SessoesUsuarios.RemoveRange(expiradas);
                    await db.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
