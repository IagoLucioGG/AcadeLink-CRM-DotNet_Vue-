using System.Text;
using CRM.Data;
using CRM.Services.Aluno_;
using CRM.Services.Modalidade_;
using CRM.Services.Usuario_;
using CRM.Services.Util_;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using CRM.Middleware;
using CRM.Services.Limpeza;
using CRM.Services.Curso_;
using CRM.Services.CursoAluno_;
using CRM.Services.Polo_;
using CRM.Infrastructure.Middlewares;
using CRM.Services.Pagamento_; // ✅ Importa ExceptionMiddleware

var builder = WebApplication.CreateBuilder(args);


builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5000); 
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<IModalidadeService, ModalidadeService>();
builder.Services.AddScoped<ICursoAlunoService, CursoAlunoService>();
builder.Services.AddScoped<IPoloService, PoloService>();
builder.Services.AddScoped<ICursoService, CursoService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IFormaPagamentoService, FormaPagamentoService>();
builder.Services.AddScoped<Util>();
builder.Services.AddHostedService<SessaoCleanupService>();




var chaveSecreta = builder.Configuration["Jwt:Chave"] ?? "Chave-Secreta-de-32-Caracteres-Super-Dificil";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta))
    };
});

// 🔹 Controllers com autenticação global
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira 'Bearer' + seu token JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 🔹 Swagger só no desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Ordem correta dos middlewares
app.UseMiddleware<ExceptionMiddleware>(); // ✅ sempre primeiro

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<SessaoMiddleware>(); // depende da sua lógica, mas em geral pode vir depois do Auth
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
