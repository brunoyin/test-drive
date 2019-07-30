### Running Rabbitmq in a Docker container on Windows 10

You must install and configure Docker first. Please see notes below

```powershell
$params = @(
    '--hostname', "rabbit-test-server",
    '--name', 'rabbit-test-server',
    '-e', 'RABBITMQ_DEFAULT_USER=bunny',
     '-e', 'RABBITMQ_DEFAULT_PASS=bugs',
     '-p', '8080:15672',
     '-p', '5672:5672',
    # '-P',
    '-d', 
    'rabbitmq:3.7.16-management-alpine'
)
docker run @params
```
### Installing Docker Desktop on Windows 10

You cannot run Windows containers and Linux containers simultaneously

1. download and install Docker Desktop
1. start Docker App and log in
1. Run Powershell as Administrator
1. Switch to Windows Containers
1. Switch to Linux containers
 
### Running Docker on Windows Server 2019 data center

Linux container support is experimental.

You can run Windows containers and Linux containers simultaneously only if you enable 
experimental feature.

1. [Install Docker Engine - Enterprise on Windows Servers](https://docs.docker.com/install/windows/docker-ee/)
1. [Enable Linux container support experimental feature](https://www.b2-4ac.com/lcow-linux-containers-on-windows-server/)
 
```powershell
Install-Module DockerMsftProvider -Force
Install-Package Docker -ProviderName DockerMsftProvider -Force

Set-Content -Value "`{`"experimental`":true`}" -Path C:\ProgramData\docker\config\daemon.json
iwr https://github.com/linuxkit/lcow/releases/download/v4.14.35-v0.3.9/release.zip -OutFile release.zip
ls 'C:\Program Files\Linux Containers'
md 'C:\Program Files\Linux Containers'
cd 'C:\Program Files\Linux Containers'
Expand-Archive -DestinationPath . C:\Users\Administrator\release.zip
# use --platform=linux when running Linux container
docker run -it --rm --platform=linux busybox
docker images
# running Windows containers
docker run -it --rm hello-world:nanoserver
```