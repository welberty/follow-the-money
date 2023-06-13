# O problema
Um comerciante precisa controlar o seu fluxo de caixa diário com os lançamentos (débitos e créditos), também precisa de um relatório que disponibilize o saldo diário consolidado.

# Requisitos de negócio
- Serviço que faça o controle de lançamentos
- Serviço do consolidado diário
  
# Desenho da Solução
Para o diagrama da solução foi adotado o padrão C4 Model, a documentação do mesmo pode ser encontrada [aqui](https://c4model.com/).

## Contexto de Sistema - C4 Nível 1

![Contexto de Sistema](image.png)

## Container -  C4 Nível 2
![Container](image-2.png)

# Padrões e boas práticas adotadas
- Observability ([Opentelemetry](https://opentelemetry.io) + [jaegertracing](https://www.jaegertracing.io), [Serilog](https://serilog.net) + [Elasticsearch](https://www.elastic.co/pt/what-is/elasticsearch))
- Bearer Authentication + JWT
- Arquitetura Exagonal (Port and adapters)
- SOLID
- Repository Pattern
- Mediatr
- Domain Notification
- Domain Events
- BDD (Behavior Driven Development - [Specflow](https://specflow.org))

Detalhes sobre os padrões e boas práticas descritos acima podem ser consultados clicando [aqui](https://refactoring.guru/pt-br).

# Dpendências para rodar o projeto
- Docker version 23.0.1-rd, build 393499b
- Docker Compose version v2.16.0

# Run
Após clonar o repositório certifique-se de que esteja na pasta raiz do mesmo e então rode o seguinte comando

>docker-compose up

Isso pode levar uns minutos...

Assim que o comando terminar de rodar, os serviços podem ser acessados pelos seguintes links:

- [Transactions.Api](http://localhost:8082/swagger/index.html)
>http://localhost:8082/swagger/index.html

- [Transactions.Api - Health Check](http://localhost:8082/_health-metrics)
>http://localhost:8082/_health-metrics

- [Consolidate.Api](http://localhost:8081/_health-metrics)
http://localhost:8081/_health-metrics

- [Consolidate.Api - Health Check](http://localhost:8081/swagger/index.html)
>http://localhost:8081/swagger/index.html

- [Jaeger](http://localhost:16686/search)
>http://localhost:16686/search
- [Elasticsearch](http://localhost:5601/app/home#/)
>http://localhost:5601/app/home#/
  
Para executar os serviços é necessário gerar um token JWT de autenticação, em ambos os serviços existe um endpoint para este fim, abaixo seguem os valores a que devem ser utilizados como usuário e senha:
>userName: testUser

>password: testPass

## Observação
### Datas
Os valores de datas utilizados devem seguir o seguinte formato:
> MM/dd/yyyy

### Elasticksearch
Para visualização dos logos os indices dos mesmos devem ser criados como na imagem abaixo:
![Alt text](image-3.png)
