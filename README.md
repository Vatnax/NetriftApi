# NETRIFT API :soccer:

This is a WebAPI for creating, managing and simulating football matches.

# Table of contents
- [Description](#description)
- [Why does this project exist?](#why-does-this-project-exist)
- [Features](#features)
- [Getting started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Running locally](#running-locally)
  * [Pulling a Docker image](#pulling-a-docker-image)
  * [Building a Docker image](#building-a-docker-image)
  * [SSL/TLS](#ssl-tls)
- [Useful links](#useful-links)
- [Contact](#contact)

# Description

Netrift WebAPI provides functionality for simulating football tournaments. It allows to create, manage, delete and simulate a cup using all countries that are FIFA members. The number of teams in a single tournament is flexible - a user can choose to create a tournament with only 4 teams up to 128! The API automatically creates groups and finals stage. The API provides full cookie-based authentication and authorization.

# Why does this project exist?

Here's a little anegdote - when I was young I used to simulate a tournament in my notebook. Results of the matches would be determined by rolling a dice. Since now I know how to code, I decided to use my polymorphic skills to turn my notebook into a computer program. No more rolling a dice! :game_die:

# Features
  - Creating, managing, deleting and simulating football tournaments
  - Looting system
  - Gathering statistics about tournaments and putting them together
  - Cookie-based authentication and authorization

# Getting started

### Prerequisites
- _Dotnet SDK (version defined in global.json)_<br>
- _Docker_<br>

### Running locally
```git clone https://github.com/Vatnax/NetriftApi.git```<br>
```dotnet dev-certs https --trust```
```dotnet run --project source\Api --launch-profile dev-https```<br>
<br>
The above commands will clone the repository, trust a self-signed certificate, compile and run the program locally on your computer in the development mode.

### Pulling a Docker image
```git clone https://github.com/Vatnax/NetriftApi.git```<br>
```docker-compose -f docker-compose.yml -f docker-compose.image.yml up -d```<br>
<br>
The above commands will clone the repository, pull the latest Docker image from a registry, set up other dependencies (volumes, networks, external software etc.) and run the container in a detached mode.

### Building a Docker image
```git clone https://github.com/Vatnax/NetriftApi.git```<br>
```docker-compose -f docker-compose.yml -f docker-compose.build.yml up -d```<br>
<br>
The above commands will clone the repository, build a Docker image, set up other dependencies (volumes, networks, external software etc.) and run the container in a detached mode.

### SSL/TLS
This API is not secured when running in a Docker container. Although it is possible to secure the connection via SSL/TLS, it would require you to make several additional changes in the Docker configuration files.

# Useful links
- API documentation: [soon]
- Netrift website: [soon]

# Contact

If you have any questions, feel free to contact me at [soon]
