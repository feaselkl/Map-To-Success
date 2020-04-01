FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as core

ENV \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since icu_libs isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# Install Mono.  This will be required for FSharp.Data.SqlClient.
RUN apt install apt-transport-https dirmngr gnupg ca-certificates
RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb https://download.mono-project.com/repo/debian stable-buster main" | tee /etc/apt/sources.list.d/mono-official-stable.list
RUN apt update
RUN apt install mono-devel --assume-yes

# Install the F# compiler.
RUN apt-get install fsharp --assume-yes

RUN apt-get update
RUN apt-get install git --assume-yes

COPY run_fsharp_demo /root/run_fsharp_demo
