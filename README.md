
# Tribal coding challenge - Backend

Coding challenge to measure programming skills in order to join Tribal as a Software Engineer.




## Run Locally

Clone the project

```bash
  git clone https://github.com/cesarcigarroa98/tribal-coding-challenge.git
```

Create PostgreSQL DB using docker

```bash
  docker run --name creditline -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=secret -p 5432:5432 -d postgres:latest
```

Run application

```bash
  cd CreditLine
  dotnet restore CreditLine.sln
  dotnet run
```




## Running Tests

To run tests, run the following commands

```bash
  cd CreditLine.Tests
  dotnet test
```


## API Reference

#### Calculate credit line

```http
  POST /api/creditline/getcredit
```

#### JSON request object 
```http
{
  "foundingType": "SME",
  "cashBalance": 435.30,
  "monthlyRevenue": 4235.45,
  "requestedCreditLine": 100,
  "requestedDate": "2022-01-07T19:38:06.606Z"
}
```

## Status codes

| Status code | Description       |
| :---:       | :-:               |
| 200         | OK                |
| 400         | BAD REQUEST       |
| 429         | TOO MANY REQUESTS |





## Arquitecture

The solution contains 5 projects written in C#:

* CreditLine.Tests: test cases
* API: contains the controller
* Aplication: application's business logic
* Persistence: DB connection
* Domain: DB classes


Projects' references:

* CreditLineTests --> API
* API --> Application
* Application --> Persistence
* Persistence --> Domain

Using this arquitecture allows to separate concerns and organize code in a better way.




## Authors

- [@cesarcigarroa](https://www.github.com/cesarcigarroa98)

