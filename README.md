# Projeto Blog Pessoal

<p>Projeto Blog Pessoal desenvolvido em ASP.NET - Core 7</p>

<br />

<div align="center">
    <img src="https://i.imgur.com/itDBUty.png" title="source: imgur.com" width="70%"/> 
</div>

<br /><br />

## Diagrama de Classes

```mermaid
classDiagram
class Tema {
  - id : Long
  - descricao : String
  - postagem : List ~Postagem~
  + getAll()
  + getById(Long id)
  + getByDescricao(String descricao)
  + postTema(Tema tema)
  + putTema(Tema tema)
  + deleteTema(Long id)
}
class Postagem {
  - id : Long
  - titulo : String
  - texto: String
  - data: LocalDateTime
  - tema : Tema
  - usuario : Usuario
  + getAll()
  + getById(Long id)
  + getByTitulo(String titulo)
  + postPostagem(Postagem postagem)
  + putPostagem(Postagem postagem)
  + deleteTema(Long id)
}
class User {
  - id : Long
  - nome : String
  - usuario : String
  - senha : String
  - foto : String
  - postagem : List ~Postagem~
  + getAll()
  + getById(Long id)
  + autenticarUsuario(UsuarioLogin usuarioLogin)
  + cadastrarUsuario(Usuario usuario)
  + atualizarUsuario(Usuario usuario)
}
class UsuarioLogin{
  - id : Long
  - nome : String
  - usuario : String
  - senha : String
  - foto : String
  - token : String
}
Tema --> Postagem
Usuario --> Postagem
```

<br /><br />

## Bibliotecas

- API
    - Entity Framework
    - SQL Server Client
    - Newton Soft JSON
    - Fluent Validation
    - Open API - Swagger
    - JWT - Bearer
    - Authentication JWT
    - Bcrypt
- Testes
    - xUnit (Testes de Integração)
    - inMemory Database (Mock do Banco de dados)
    - MVC Testing
- Deploy
    - NPGSQL - Postgres (Deploy no Render)
