# ASP.NET Core WebAPI Containerizatio

###### NOTE: Code snippet contains comment starting with a # [hash] symbol, remove the comment to avoid execution issues.

# Create a ASP.NET Core WebAPI

* [ASP.NET WebAPI with Visual Studio for Windows](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.1)
* [ASP.NET Core WebAPI with Visual Studio for Mac](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api-mac?view=aspnetcore-2.1)

Create a new ASP.NET WebAPI project and name it `CoreWebAPI`

# Create a Dockerfile for an ASP.NET Core application
1. Create a Dockerfile in your project folder.
2. Add the text below to your Dockerfile for either Linux or Windows Containers. The tags below are multi-arch meaning they pull either Windows or Linux containers depending on what mode is set in Docker for Windows. Read more on switching containers.
3. The Dockerfile assumes that your application is called aspnetapp. Change the Dockerfile to use the DLL file of your project.

```js
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT dotnet CoreWebAPI.dll
```
To make your build context as small as possible add a .dockerignore file to your project folder and copy the following into it.

```js
bin\
obj\

```
# Build and run the Docker image
Open a command prompt and navigate to your project folder. Use the following commands to build and run your Docker image:

```js
docker build -t corewebapi . # (. [dot] is required not a typo)
docker run -d -p 8080:80 --name [image-name] corewebapi
```
# View the web page running from a container

Go to http://localhost:8080 to access your app in a web browser. If you are using the Nano Windows Container and have not updated to the Windows Creator Update there is a bug affecting how Windows 10 talks to Containers via “NAT” (Network Address Translation). You must hit the IP of the container directly. You can get the IP address of your container with the following steps:

```console
Run docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" image-name
```

Copy the container ip address and paste into your browser. (For example, 172.16.240.197)

# Edit application and refresh image

```js
docker build -t corewebapi .
docker stop [container-name]
docker rm [container-name]
docker run -d -p 8080:80 --name [image-name] corewebapi
```
# Push Images to a Container Registry
