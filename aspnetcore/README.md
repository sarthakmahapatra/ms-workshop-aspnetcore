# ASP.NET Core WebAPI Containerization

> NOTE: Code snippet contains comment starting with a # [hash] symbol, remove the comment to avoid execution issues.

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
# Deploy ASP.NET Core Applications to Azure Container Instances

## Create ACR Registry

Create an ACR registry per the instructions at [Push Docker Images to Azure Container Registry](../dotnetapp/push-image-to-acr.md). The following is a summarized version of those instructions.

> Note: Change the password location and the user account ("rich" and "richlander") example values in your environment.

```console
az login
az group create --name demo-containers --location eastus
az acr create --name [acrdemo-name] --resource-group demo-containers --sku Basic
```

## Login to Azure Container Registry

First, "admin-enable" your session, an ACR credentials access prerequisite for the subsequent command.

```console
az acr update -n [acrdemo-name] --admin-enabled true
```

Now login to ACR via the docker cli, an ACR push prerequisite:

```console
az acr credential show -n [acrdemo-name] --query passwords[0].value --output tsv | docker login [acrdemo-name].azurecr.io -u [acrdemo-name] --password-stdin
```

## Push Image for Azure Container Registry (ACR)

Use the following instructions to tag the image for your registry and push the image. If you automate these instructions, build the image with the correct name initially.

```console
docker tag corewebapi [acrdemo-name].azurecr.io/corewebapi
docker push [acrdemo-name].azurecr.io/corewebapi
```

## Deploy Image to Azure Container Instance (ACI)

During deployment, you'll need to enter your password. Type or copy/paste it in. Get your password beforehand from the following command:

```console
az acr credential show -n [acrdemo-name] --query passwords[0].value --output tsv
```

You can deploy Linux images with the following command:

```console
az container create --name corewebapi --image [acrdemo-name].azurecr.io/corewebapi --resource-group demo-containers --ip-address public
```

You can deploy Windows images with the following command, which includes `--os-type Windows`:

```console
az container create --name corewebapi --image [acrdemo-name].azurecr.io/aspnetapp --resource-group demo-containers --ip-address public --os-type Windows
```

> Note: Azure Container Instances only supports Windows Server 2016 Nano Server and Server Core images, not Windows Server, version 1709 or later.

## Running the Image

The last step -- `az container show` -- will need to be repeated until `provisioningState` moves to `Succeeded`.

```console
az container show --name corewebapi --resource-group demo-containers 
```

Once the `provisioningState` moves to `Succeeded`, collect the IP address from the `ip` field, as you can see in the following image, and then copy/paste the IP address into your browser. You should see the sample running.

![az container show -- successfully provisioned app](https://user-images.githubusercontent.com/2608468/29669868-b492c4e8-8899-11e7-82cc-d3ae1262a080.png)

## Cleanup

When these containers aren't needed, delete the resource group to reclaim all exercise container resources.

```console
az group delete --name demo-containers
az group exists --name demo-containers
```

## More Samples

* [.NET Core Docker Samples](../README.md)
* [.NET Framework Docker Samples](https://github.com/microsoft/dotnet-framework-docker-samples/)
