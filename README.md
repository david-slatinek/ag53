# Description

<div align="center">
    <img alt=".NET" src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
    <img alt="PostgreSQL" src="https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white"/>
    <img alt="MinIO" src="https://img.shields.io/badge/minio-C72E49?style=for-the-badge&logo=minio&logoColor=white"/>
    <img alt="Docker" src="https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white"/>
</div>

Project for managing actors and movies made with ASP.NET Core 8.0, PostgreSQL, and MinIO.

## Prerequisites

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

## How to run

1. Open a terminal, navigate to the `db` directory, and run the following commands:
   ```bash
   echo "POSTGRES_USER=david" >> .env
   echo "POSTGRES_PASSWORD=david" >> .env
   echo "POSTGRES_DB=ag53-db" >> .env
   ```
   If you change these values, you also need to change the connection string for all services by updating
   the `DefaultConnection` in `appsettings.json` files.

   If you want to see the database, open a web browser, navigate to [http://localhost:7000](http://localhost:7000), and
   login with the credentials you set in step 1.
2. Run the following command to start the database:
   ```bash
   docker-compose up
   ```

3. Open another terminal, navigate to the `image-storage` directory, and run the following commands:
   ```bash
   echo "MINIO_ROOT_USER=<YOUR USERNAME>" >> .env
   echo "MINIO_ROOT_PASSWORD=<YOUR PASSWORD" >> .env
   ```
4. Run the following command to start the image storage:
    ```bash
   docker-compose up
    ```
5. Open a web browser, navigate to [http://localhost:9000](http://localhost:9000), and login with the credentials you
   set in step 3.
6. Create a new access key and secret key.
7. Open `movies-service/appsettings.json` and replace the values of `MinIOAccessKey` and `MinIOSecretKey` with the
   values you set in step 6.
8. Open a terminal, navigate to the root directory, and run the following command:
   ```bash
   docker-compose up
   ```

## How to use

Open a web browser and navigate to:

- [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)
    - for **Actors service**
- [http://localhost:8081/swagger/index.html](http://localhost:8081/swagger/index.html)
    - for **Movies service**
- [http://localhost:8082/swagger/index.html](http://localhost:8082/swagger/index.html)
    - for **Acting service**

## Migration

1. Open the terminal, navigate to the `db` directory, and run the following commands:
   ```bash
   echo "POSTGRES_USER=david" >> .env
   echo "POSTGRES_PASSWORD=david" >> .env
   echo "POSTGRES_DB=ag53-db" >> .env
   ```
   If you change these values, you also need to change the connection string for all services by updating
   the `DefaultConnection` in `appsettings.json` files.

   If you want to see the database, open a web browser, navigate to [http://localhost:7000](http://localhost:7000), and
   login with the credentials you set in step 1.
2. Run the following command to start the database:
   ```bash
   docker-compose up
   ```
3. Open another terminal, navigate to the `actors-service` directory, and run the following command:
   ```bash
   ./migration.sh
   ```
   If you are on Windows, manually run the commands in that file.
4. Open another terminal, navigate to the `movies-service` directory, and run the following command:
   ```bash
   ./migration.sh
   ```
   If you are on Windows, manually run the commands in that file.
