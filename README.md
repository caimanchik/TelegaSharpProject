`Чтобы бот завелся нужен файл с настройками. Напишите @caimanchik в телеграме`

## Описание

Телеграм-бот, позволяющий обучающимся попросить помощи у менторов. Ученику достаточно открыть бота, выложить задание и выбрать подходящий ответ. 

У каждого ученика и ментора есть очки, можно посмотреть таблицу лидеров, информацию о себе (очки, количество выложенных задач, свой юзернэйм). 

При отправке ответа, автору задачи приходит уведомление о том, что появился новый ответ. При пометке ответа как правильный, автору ответа приходит уведомление и начисляются очки

## Примененные паттерны

### DDD

Решение разбито на три области - инфраструктурная, доменная, приложение

### OCP

Есть точки расширения. Например, можно легко добавить кнопку, всего лишь унаследовавшись от их базовых классов

### DI-контейнер

Использована библиотека `Ninject` для реализации DI-контейнера