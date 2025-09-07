
# NEXUSCRM

Sistema de gerenciamento desenvolvido com Vue 3 + Vuetify no frontend e .NET 8 no backend, utilizando autenticação JWT e arquitetura de API REST.  
O objetivo é permitir o controle eficiente de usuários, alunos, modalidades e cursos, com segurança e escalabilidade.  

## TECNOLOGIAS UTILIZADAS

### FRONTEND
- Vue 3 (Composition API)  
- Vuetify 3  
- Axios para comunicação com a API  
- Pinia para gerenciamento de estado  
- Vue Router para rotas  
- Tema dinâmico Light/Dark (planejado para configuração por usuário)  

### BACKEND
- .NET 8 Web API  
- Entity Framework Core  
- SQL Server  
- JWT (JSON Web Token) para autenticação  
- Arquitetura em camadas (Controllers, Services, DTOs)  

## ESTRUTURA DO PROJETO

### FRONTEND
- **components** → Componentes reutilizáveis  
- **layouts** → Estrutura visual e menu lateral  
- **pages** → Páginas principais (Alunos, Usuários, Modalidades, Cursos)  
- **router** → Configuração das rotas  
- **services** → Configuração do Axios e integração com API  
- **stores** → Gerenciamento de estado (ex.: autenticação)  

### BACKEND
- **Controllers** → Endpoints da API  
- **DTOs** → Objetos de transferência de dados  
- **Models** → Modelos de entidades  
- **Services** → Lógica de negócio  
- **Program.cs** → Configuração inicial da aplicação  

## FUNCIONALIDADES IMPLEMENTADAS

### AUTENTICAÇÃO
- Login com JWT  
- Middleware para validação de sessão  
- Redirecionamento automático para login em caso de erro 401  

### GESTÃO DE USUÁRIOS
- Listagem, cadastro, edição e alteração de status (ativo/inativo)  

### GESTÃO DE ALUNOS
- Listagem, cadastro e edição  

### GESTÃO DE MODALIDADES
- Listagem, cadastro e edição  

### GESTÃO DE CURSOS
- Listagem, cadastro e edição (em desenvolvimento)  

### INTERFACE
- Menu lateral responsivo com expandir/recolher  
- Logout integrado  
- Persistência de sessão (localStorage)  
- Interceptores Axios para envio automático do Bearer Token  
- Tratamento global de erros  

## COMO EXECUTAR O PROJETO

### BACKEND
1. Clonar o repositório  
2. Entrar na pasta do backend  
3. Configurar o arquivo `appsettings.json` com a string de conexão do SQL Server  
4. Executar as migrações:  
   ```bash
   dotnet ef database update
   ```
5. Iniciar o projeto:  
   ```bash
   dotnet run
   ```

### FRONTEND
1. Entrar na pasta do frontend  
2. Instalar dependências:  
   ```bash
   npm install
   ```
3. Rodar o projeto:  
   ```bash
   npm run dev
   ```

## MELHORIAS FUTURAS
- Tema Dark/Light configurável por usuário  
- Controle de permissões e papéis  
- Dashboard com gráficos e métricas  
- Paginação e filtros  
- Upload de imagens para usuários e alunos  

---  
**Desenvolvido por Iago Lúcio**  
