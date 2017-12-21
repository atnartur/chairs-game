FROM microsoft/aspnetcore-build:2.0.0-jessie

RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && \
    apt-get install nodejs -y --no-install-recommends

WORKDIR /app
EXPOSE 5000

COPY ./ChairsGame.sln .
RUN dotnet restore ./ChairsGame.sln
COPY . .

WORKDIR /app/ChairsGame/wwwroot
RUN npm i -q

WORKDIR /app

RUN dotnet build ./ChairsGame.sln -o ./build

WORKDIR /app/ChairsGame

CMD dotnet run --server.urls=http://0.0.0.0:5000
