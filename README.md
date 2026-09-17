Portfólio pessoal desenvolvido em ASP.NET Core MVC com EF Core e SQL Server. Conteúdo totalmente gerenciável por painel administrativo autenticado, com upload de imagens e layout responsivo. Publicado em produção com configuração segura de credenciais e migrações automáticas.

Aplicação web full-stack desenvolvida para apresentar minha trajetória profissional, habilidades e projetos de forma dinâmica e gerenciável. Diferente de um site estático, todo o conteúdo (perfil, projetos, habilidades e imagens) é armazenado em banco de dados e administrado por um painel próprio, sem necessidade de alterar código para atualizar informações.

Objetivo

Criar um portfólio profissional que funcionasse também como projeto de demonstração técnica, cobrindo o ciclo completo de desenvolvimento de software: modelagem de dados, back-end, front-end responsivo, autenticação, segurança de credenciais, controle de versão e deploy em ambiente de produção.

Principais funcionalidades

Página inicial com apresentação pessoal, resumo profissional e links de contato (LinkedIn, GitHub, e-mail).
Listagem de projetos com descrição, tecnologias utilizadas, imagem de capa e links para repositório e demonstração.
Seção de habilidades técnicas organizadas por categoria.
Download do currículo em PDF.
Painel administrativo protegido por login, permitindo criar, editar e excluir projetos, habilidades e dados do perfil.
Upload de imagens diretamente pelo painel, com armazenamento no servidor.
Layout responsivo, adaptado a desktop, tablet e celular.
Meta tags Open Graph para exibição de prévia ao compartilhar o link em redes sociais.
Tecnologias e ferramentas

Back-end: C#, ASP.NET Core MVC (.NET 8)
Acesso a dados: Entity Framework Core (Code First, Migrations)
Banco de dados: SQL Server
Front-end: Razor Views, HTML5, CSS3, Bootstrap, JavaScript
Autenticação e autorização: Cookie Authentication, atributos [Authorize] e [AllowAnonymous]
Configuração e segurança: User Secrets em desenvolvimento, variáveis de ambiente em produção, .gitignore para exclusão de arquivos sensíveis
Controle de versão: Git e GitHub
Hospedagem: MonsterASP.NET (Windows/IIS), publicação via Visual Studio
IDE: Visual Studio 2022
Arquitetura e decisões técnicas

Padrão MVC com separação clara entre Models, Controllers e Views.
Modelagem do banco via EF Core Code First; as tabelas são criadas e atualizadas automaticamente em produção com Database.Migrate() na inicialização da aplicação.
Credenciais (string de conexão e acesso ao painel admin) nunca são versionadas: em desenvolvimento ficam no secrets.json; em produção são injetadas por variáveis de ambiente, aproveitando o sistema de configuração em camadas do ASP.NET Core.
Área administrativa isolada em um controller próprio, com autorização aplicada em nível de classe e exceção apenas para as rotas de login.
Validação de configurações obrigatórias no startup, impedindo que a aplicação suba com credenciais ausentes.
Desafios e aprendizados

Configuração do ambiente de produção no IIS (web.config, módulo AspNetCoreModuleV2, variáveis de ambiente) e diagnóstico de erros de inicialização (HTTP 500.30) por meio de logs de stdout.
Entendimento prático da hierarquia de configuração do .NET (appsettings, User Secrets, variáveis de ambiente) e boas práticas para proteger dados sensíveis em repositórios públicos.
Fluxo completo de publicação: desenvolvimento local → commit no GitHub → publicação no servidor → migrações aplicadas automaticamente.
Próximos passos

Transformar o site em PWA (Progressive Web App), permitindo instalação no celular.
Galeria de imagens por projeto com visualização em lightbox.
Upload do currículo em PDF diretamente pelo painel administrativo.
Automação do deploy com GitHub Actions.
