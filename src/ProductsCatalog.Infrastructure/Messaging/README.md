# Messaging

Extension point para mensageria assincrona (ex: producers/consumers Kafka)
usados por eventos que precisam sair do processo. Os Domain Events internos
(publicados via MediatR/IPublisher) ja cobrem reacoes *dentro* do mesmo
processo/transacao — este diretorio e para quando um evento precisar virar
uma mensagem em um topico externo (ex: `OrderCreatedEvent` publicando em um
topico Kafka `orders.created` para outro servico consumir).

Sugestao de organizacao ao implementar:

```
Messaging/
├── Kafka/
│   ├── KafkaProducer.cs
│   ├── KafkaConsumerHostedService.cs
│   └── KafkaOptions.cs
└── Outbox/            (Transactional Outbox Pattern, recomendado com Kafka)
```
