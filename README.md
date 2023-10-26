# BR University Data API

🗒️🇧🇷 [README pt-BR](https://github.com/xbandrade/university-br-data-api/blob/main/README-pt-BR.md)

## ➡️ An ASP.NET API that allows you to retrieve data from Brazilian universities

## Youtube Video: [BR University API](https://www.youtube.com/watch?v=XWZ0S-GbTsw)

## 💻 Technologies used:
  - ASP.NET 7.0
  - MySQL 8.1.0
  - C++17
  - Qt 5

## ⚙️ API Setup
#### With Docker:
    - Navigate to the API/ folder in the project root directory
    - Change any preferences in the `docker-compose.yml` file if needed
    - Turn on your Docker services and execute the command `docker-compose up`
    - The API will be live and listening on port `5000` by default

#### Without Docker:
    - Required Technologies: .NET 7.0, MySQL
    - Navigate to the API/ folder in the project root directory
    - Run the command `dotnet run` to install dependencies and run the application
    - The application uses a MySQL database hosted on `db4free.net` by default, so it can run slower initially
      - If needed, configure yout MySQL environment and change the `connectionString` accordingly in the Controller file
    
## ⚙️ Client Setup
#### With Visual Studio:
    - Required Technologies: Microsoft C++, Qt5
    - Navigate to the Client/ folder in the project root directory
    - Open the `.sln` solution with Visual Studio
    - Build and compile the project, the output executable can be located in the Debug/ or Release/ folder

## 📊 MySQL Database Structure
#### The database table holds the following fields:
    - `id` ─ INT, AUTO_INCREMENT, PRIMARY KEY
    - `name` ─ VARCHAR(255), UNIQUE
    - `state` ─ VARCHAR(255)
    - `webPages` ─ VARCHAR(255)
    - `domains` ─ VARCHAR(255)
  ❕Multiple `webPages` and `domains` can be stored as `string` separated by comma


## 💻 API Features
  #### This API consumes data from the [University Domains and Names API](https://github.com/Hipo/university-domains-list-api), and exposes endpoints for Brazilian universities data retrieval.
  These are the available endpoints for the API:
  - `GET` ➔ `/uni-br/search` ─ Retrieve data from all universities in the database. If it is empty, it will try to populate the database with the base API.
    - Parameters: `page` ─ Default: 1, `pageSize` ─ Default: 10
  - `GET` ➔ `/uni-br/search/{pk}` ─ Retrieve data from a specific university.
  - `POST` ➔ `/uni-br/create` ─ Create a new entry for a university in the database.
    - Request Body: `name`: string, `state`: string, `webPages`: string, `domains`: string
  - `POST` ➔ `/uni-br/update-db` ─ Update the university database. If it is empty, it will try to populate the database with the base API.

  #### These endpoints can also be checked and tested on the `/swagger` endpoint.

      



    
