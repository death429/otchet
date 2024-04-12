from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import psycopg2
from psycopg2 import Error


# Создаем подключение к базе данных PostgreSQL
def create_db_connection():
    try:
        connection = psycopg2.connect(user="postgres",
                                      password="Hit1337",
                                      host="localhost",
                                      port="5432",
                                      database="user")
        return connection
    except (Exception, Error) as error:
        print("Ошибка при подключении к PostgreSQL:", error)


# Модель данных для запросов
class User(BaseModel):
    username: str
    email: str
    age: int


app = FastAPI()


@app.get("/")
async def read_root():
    return {"message": "Привет, мир!"}


# Обработчик для приема данных и записи в базу данных
@app.post("/users/")
async def create_user(user: User):
    # Создаем подключение к базе данных
    connection = create_db_connection()
    if connection is None:
        raise HTTPException(status_code=500, detail="Ошибка подключения к базе данных")

    try:
        cursor = connection.cursor()

        # Проверяем уникальность имени пользователя
        check_username_query = "SELECT * FROM users WHERE username = %s"
        cursor.execute(check_username_query, (user.username,))
        existing_user = cursor.fetchone()
        if existing_user:
            raise HTTPException(status_code=400, detail="Пользователь с таким именем уже существует")

        # Проверяем уникальность электронной почты
        check_email_query = "SELECT * FROM users WHERE email = %s"
        cursor.execute(check_email_query, (user.email,))
        existing_user = cursor.fetchone()
        if existing_user:
            raise HTTPException(status_code=400, detail="Пользователь с такой электронной почтой уже существует")

        # Выполняем запрос для вставки данных в таблицу users
        insert_query = "INSERT INTO users (username, email, age) VALUES (%s, %s, %s)"
        cursor.execute(insert_query, (user.username, user.email, user.age))
        # Подтверждаем транзакцию
        connection.commit()

        # Закрываем курсор и соединение
        cursor.close()
        connection.close()

        return {"message": "Пользователь успешно создан"}
    except (Exception, Error) as error:
        print("Ошибка при вставке данных в PostgreSQL:", error)
        connection.rollback()  # Откатываем транзакцию в случае ошибки
        raise HTTPException(status_code=500, detail="Ошибка при создании пользователя")
# Обработчик для получения всех пользователей из базы данных
@app.get("/users/")
async def read_users():
    # Создаем подключение к базе данных
    connection = create_db_connection()
    if connection is None:
        raise HTTPException(status_code=500, detail="Ошибка подключения к базе данных")

    try:
        cursor = connection.cursor()

        # Выполняем запрос для получения всех пользователей
        select_all_users_query = "SELECT * FROM users"
        cursor.execute(select_all_users_query)
        users = cursor.fetchall()

        # Формируем список словарей с данными пользователей
        user_list = []
        for user in users:
            user_data = {
                "id": user[0],
                "username": user[1],
                "email": user[2],
                "age": user[3]
            }
            user_list.append(user_data)

        # Закрываем курсор и соединение
        cursor.close()
        connection.close()

        return {"users": user_list}
    except (Exception, Error) as error:
        print("Ошибка при получении данных из PostgreSQL:", error)
        raise HTTPException(status_code=500, detail="Ошибка при получении пользователей")

if __name__ == "__main__":
    import uvicorn

    uvicorn.run(app, host="localhost", port=8000)
