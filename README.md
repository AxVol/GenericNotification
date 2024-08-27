# Generic notifications
___
## Технологии
![C#](https://img.shields.io/badge/-Csharp_11-000278?style=for-the-badge&logo=c-sharp)
![.NET](https://img.shields.io/badge/-.NET_7-000278?style=for-the-badge&logo=.NET)
![NET](https://img.shields.io/badge/-ASP.NET_CORE-000278?style=for-the-badge&logo=.NET)
![Postgres](https://img.shields.io/badge/-postgres-000278?style=for-the-badge&logo=postgresql)
![DOCKER](https://img.shields.io/badge/-DOCKER-000278?style=for-the-badge&logo=docker)
![Redis](https://img.shields.io/badge/redis-000278?style=for-the-badge&logo=redis)
![RabbitMQ](https://img.shields.io/badge/Rabbitmq-000278?style=for-the-badge&logo=rabbitmq)
___
## Краткое описание проекта
Веб-приложение созданное для рассылки уведомлений группе пользователей, для удобства приложение написано в виде двух микросервисов. Первый отвечает за обработку поступившего уведомления и установкой его даты отправки, второй за отправку его в срок. Основная информация о уведомлениях храниться в базе данных Postgres, уведомления которые должны отправиться в текущий день хранятся в оперативной памяти по средствам Redis, а микросервисы связанны между собой брокером сообщений RabbitMQ

Поверхностная схема всего проекта
![Иллюстрация к проекту](images/Screenshot_1.png)

В данном репозитории предаставляеться сервис Notification Sender который отправляет письма на почтовый сервер
___

## Установка и запуск

___
### Подробные README
[Generic Notification](Microservices/GenericNotification/README.md)
[Notification Sender](Microservices/NotificationSender/README.md)