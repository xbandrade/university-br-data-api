# BR University Data API

## ➡️ Uma API ASP.NET API que permite a obtenção de dados de universidades brasileiras

## ▶️ Vídeo Youtube: [BR University API](https://www.youtube.com/watch?v=XWZ0S-GbTsw)

## 💻 Tecnologias utilizadas:
  - ASP.NET 7.0
  - MySQL 8.1.0
  - C++17
  - Qt 5

## ⚙️ Setup API
#### Com Docker:
    - Navegue até a pasta API/ no diretório raiz do projeto
    - Altere as preferências no arquivo `docker-compose.yml` se necessário
    - Ative os serviços Docker e execute o comando `docker-compose up`
    - A API vai iniciar e vai escutar a porta `5000` por padrão
    
#### Sem Docker:
    - Tecnologias necessárias: .NET 7.0, MySQL
    - Navegue até a pasta API/ no diretório raiz do projeto
    - Execute o comando `dotnet run` para instalar as dependências e rodar a aplicação
    - A aplicação usa uma base de dados MySQL hospedada em `db4free.net` por padrão, portanto a conexão pode ser mais lenta inicialmente
      - Se necessário, configure seu ambiente MySQL e modifique a `connectionString` no arquivo de Controller
    
## ⚙️ Setup Client
#### Com Visual Studio:
    - Tecnologias necessárias: Microsoft C++, Qt5
    - Navegue até a pasta Client/ no diretório raiz do projeto
    - Abra a solução `.sln` com o Visual Studio
    - Compile o projeto, e o executável poderá ser localizado nas pastas Debug/ ou Release/

## 📊 Estrutura Database MySQL
#### A database possui os seguintes campos:
    - `id` ─ INT, AUTO_INCREMENT, PRIMARY KEY
    - `name` ─ VARCHAR(255), UNIQUE
    - `state` ─ VARCHAR(255)
    - `webPages` ─ VARCHAR(255)
    - `domains` ─ VARCHAR(255)
  ❕Múltiplos `webPages` e `domains` podem ser armazenados como `string` separados por vírgula

## 💻 Funcionalidades da API
  #### Esta API consome dados da [University Domains and Names API](https://github.com/Hipo/university-domains-list-api), e expõe endpoints para obtenção de dados de universidades brasileiras.
  Estes são os endpoints disponíveis para a API:
  - `GET` ➔ `/uni-br/search` ─ Obtém dados de todas as universidades no banco de dados. Se o banco de dados estiver vazio, haverá uma tentativa de povoar a database com a API base.
      - Parâmetros: `page` ─ Padrão: 1, `pageSize` ─ Padrão: 10
  - `GET` ➔ `/uni-br/search/{pk}` ─ Obtém dados de uma universidade específica.
  - `POST` ➔ `/uni-br/create` ─ Cria uma nova entrada de dados de uma universidade no banco de dados.
    - Request Body: `name`: string, `state`: string, `webPages`: string, `domains`: string
  - `POST` ➔ `/uni-br/update-db` ─ Atualiza a database de universidades. Se o banco de dados estiver vazio, haverá uma tentativa de povoar a database com a API base.

  #### Todos esses endpoints também podem ser checados e testados no endpoint `/swagger`.

  

      



    
