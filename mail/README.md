# Почтовый сервер для тестирования веб-приложения

Образ виртуальной машины с почтовым сервером лежит на гугл диске - https://drive.google.com/file/d/1kUz2M_NRZKqPzum4lOrly8e1nJeKXfKl/view?usp=sharing

___
## Краткое описание проекта
Почтовый сервер который использует доменное имя test.ru, сам же сервер mail.test.ru, сертификат на это имя находиться так же в директории, его нужно установить на сервер с которого будет производиться отправка писем.

Созданы 2 тестовые учетные записи  
Логин | пароль  
usertest1@test.ru | testUser1!  
usertest2@test.ru | testUser2!

Учетная запись для отправки почты  
genericnotification@test.ru | genericNotification1!

Доступ к веб-интерфейсу произвоиться по адресу - {ip-address}/mail