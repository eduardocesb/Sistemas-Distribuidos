# API REST - Email System

## How to run

The Server and the Client is running in `.NET 5.0`.

#### Installing `.NET 5.0` on Ubuntu 20.04

```bash
sudo apt-get update
sudo apt-get install snapd
sudo snap install dotnet-sdk --classic
```

### Cloning the repository

```bash
sudo apt-get install git
git clone https://github.com/eduardocesb/Sistemas-Distribuidos.git
```

### Running the Server

```bash
cd Sistemas-Distribuidos/Server/
dotnet run
```

By default, the server will run at the following address: `localhost:5000`

After the server run, you can do some `HTTP Requests`, to see more, visit the Postam Collection

### Running the Client

```bash
cd Sistemas-Distribuidos/Client/
dotnet run
```

You can use this simple Client to do some commands in the terminal, to validate the API.

Ps: You need have the Server running to do it

## Postman Collection

* [Collection](https://github.com/eduardocesb/Sistemas-Distribuidos/tree/master/Postman)