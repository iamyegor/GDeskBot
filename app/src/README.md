Как создать бота из этого шаблона:

1. Заходите в [Botfather](https://t.me/BotFather)
2. Создаете нового бота с помощью команды /newbot
3. В appsettings.local.json rопируете токен в "BotConfiguration":"Token"
4. Генерируете новый Guid и копируете его в "BotConfiguration":"WebhookToken"
5. Запускаете [ngrok](https://ngrok.com/download) при помощи команды ./ngrok http 1402
6. Запускаете docker compose up
7. Запускаете вашего бота.

Ваш бот готов к работе!