FROM mcr.microsoft.com/dotnet/core/sdk:2.2
# no telemetry
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
#ENV HTTP_PROXY=http://your-company-proxy-server:proxy-port
#ENV HTTPS_PROXY=http://your-company-proxy-server:proxy-port
#AWS by pass AWS Meta URL
#ENV NO_PROXY=169.254.169.254,your-company.com
# I want to run as my own user id not under root
# If you are not bruno or have different id, you can --build-arg to change the default settings here
ARG UID=1000
ARG GID=1000
ARG USERNAME=bruno
ARG GROUPNAME=bruno
# use --build-arg UID=$(id -u) --build-arg GID=$(id -g) --build-arg USERNAME=notbruno --build-arg GROUPNAME=notbruno
# Create a new user that matches my account
RUN groupadd -g ${GID} ${GROUPNAME} \
&& useradd -m -u ${UID} -g ${GROUPNAME} ${USERNAME}
# switch to the new user
USER ${USERNAME}
# install some tools
RUN dotnet tool install -g cake.tool \
&& dotnet tool install -g dotnet-script
# setup tool path
ENV PATH=/home/${USERNAME}/.dotnet/tools:$PATH
# to build, run: docker build -t yin/dotnet:2.2 .
#
# if you are not bruno, run:
# docker build --build-arg UID=$(id -u) --build-arg GID=$(id -g) --build-arg USERNAME=notbruno --build-arg GROUPNAME=notbruno -t you/dotnet:2.2 .
#
# if you need access to resources outside from a container and are behind a firewall
# you need to provide DNS with --dns and set up proxy with -e HTTP_PROXY=http://your-company-proxy-server:proxy-port
#
# cd $your_dotnet_project, in docker run command, mount the current directory 
# and then make it the working directory in the container, and do your dotnet run, test, publish, ...
# ---command line, if you are in Powershell use ` instead of \ for line continuation ===
# docker run -it --dns x.x.x.x \
# -e HTTP_PROXY=http://your-company-proxy-server:proxy-port \
# -e HTTPS_PROXY=http://your-company-proxy-server:proxy-port \
# -e NO_PROXY=169.254.169.254,your-company.com \
# -v $PWD:/app \
# --workdir /app \
# yin/dotnet:2.2 \
# dotnet run