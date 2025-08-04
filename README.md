# DesafioFullStack--ASP.NET-SAPUI5-
Desafio de um FullStack com direito a back-end de ASP.NET, utilizando bibliotecas como IMapper e frameworks como Entity Framework Core. Já o front-end foi realizado no frameword SAPUI5, bom aproveito do código.

Para funcionamento do projeto, é necessário Visual Studio 2022 preview para o back e Visual Studio Code para font. Ao abrir o projeto do back no Visual roxo, instala as dependencias necessárias através do console de pacote Nugget, cria um banco de dados SQL local, e utiliza a ConectionString gerada na appsettings.json e substitua "LocalSqlDB", abra o console Nugget e rode o comando 'Update-Database', assim sera gerado seu banco com as tabelas necessárias. O back possui Swagger e um programa de Teste caso queira utilizar os métodos http. 

Ja no front, abre o projeto no Visual azul, lembra que é necessário Node.js na sua máquina para rodar. Abra um terminal no projeto e execute os seguintes comandos: 

No back-end foi utilizado algumas Dtos (Data Transfer Object) para melhor manuseio de dados e com isso foi utilizado a biblioteca IMapper para facilitar tal.

A ideia era pegar uma lista de dados de uma API, popular um banco de dados e manipular elas já inseridas, tarefas no formato: 'npm install --global @ui5/cli' isso permite rodar ui5 serve e outros comandos do SAPUI5 localmente e depois 'npm i' para instalar as depenências.

Agora precisa haver uma ponte entre o back e o front, eu particularmente gosto de rodar em https o back, assim tenha certeza de pegar a porta que disponibilizará e insira na variável baseUrl, assim irá para toda aplicação e para inicar seu front, só digitar 'npm start' no terminal da sua pasta (Tenha certeza que o diretório do terminal esteje correto) e utilize a porta que irá gerar no arquivo Program.cs e inserir a url na variável BaseUrl.

Segue o modelo de item que a API nos fornece:

{
    "userId": int,
    "id": int,
    "title": string,
    "completed": bool
}

Assim, modelos criados de Usuario e Tarefas, advém de arquivos Create, Read e Update que são Dtos necessárias para seus modelos de acordo com o que o desafio foi proposto. 

Com isso foi criado 4 métodos:

GET: /todos
Tem objetivo listar Tarefas presentes dentro do banco. Possui parâmetros opcionais de página (int page = 1), tamanho da página (int pageSize = 10), tíutlo (string title = null), organização (string sort = 'title')  e ordem (string order = 'asc'). [Respectivos parenteses representa o padrão se não for inserido];

GET: /todos/{id}
Retorna uma tarefa na qual é procurada através de ID.

PUT: /todos/{id}
Recebe Id e um corpo body referente a única mudança possível:

{
  "completed": true or false
}

Possui regra de negócio na qual se um usuário tiver 5 ou mais tarefas incompletas, a função não é concluída.

POST: /sync
Popula o banco conm dados de uma API. Tem função de sicronizar, ou seja, apaga informações em banco e insere os dados novamente.


Já o front-ent foi desenvolvido com SAPUI5, possui todas essas funcionalidades em tela com filtros, botão de detalhes e muito mais.
