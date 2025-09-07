using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CRM.Data;

namespace CRM.Middleware
{
    public class SessaoMiddleware
    {
        private readonly RequestDelegate _next;

        public SessaoMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            if (context.User.Identity?.IsAuthenticated ?? false)
            {
                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var sessao = await db.SessoesUsuarios.FirstOrDefaultAsync(s => s.Token == token);

                if (sessao == null || sessao.DataExpiracao <= DateTime.Now)
                {
                    if (sessao != null)
                    {
                        db.SessoesUsuarios.Remove(sessao);
                        await db.SaveChangesAsync();
                    }

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Sessão expirada ou inválida.");
                    return;
                }

                // ⏳ Verifica inatividade de 15 minutos
                if ((DateTime.Now - sessao.UltimaRequisicao).TotalMinutes > 15)
                {
                    db.SessoesUsuarios.Remove(sessao);
                    await db.SaveChangesAsync();

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Sessão expirada por inatividade.");
                    return;
                }

                // Atualiza última requisição
                sessao.UltimaRequisicao = DateTime.Now;
                db.SessoesUsuarios.Update(sessao);
                await db.SaveChangesAsync();
            }

            await _next(context);
        }
    }

}
