# Credit to Scott Hansleman: https://www.hanselman.com/blog/HowToBuildAKubernetesClusterWithARMRaspberryPiThenRunNETCoreOnOpenFaas.aspx
FROM microsoft/dotnet:2.0-sdk as builder

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Optimize for Docker builder caching by adding projects first.

RUN mkdir -p /root/src/function
WORKDIR /root/src/function
COPY ./function/Function.csproj  .

WORKDIR /root/src/
COPY ./root.csproj  .
RUN dotnet restore ./root.csproj

COPY .  .

RUN dotnet publish -c release -o published

FROM mcr.microsoft.com/dotnet/core/runtime:2.1

# Influenced by https://github.com/burtonr/csharp-kestrel-template
RUN apt-get update -qy \
    && apt-get install libunwind8 \
    && apt-get install -qy curl ca-certificates --no-install-recommends \ 
    && echo "Pulling watchdog binary from Github." \
    && curl -sSLf https://github.com/openfaas-incubator/of-watchdog/releases/download/0.4.0/of-watchdog > /usr/bin/fwatchdog \
    && chmod +x /usr/bin/fwatchdog \
    && apt-get -qy remove curl \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /root/
COPY --from=builder /root/src/published .

ENV fprocess="dotnet ./root.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:5000"

EXPOSE 8080
CMD ["fwatchdog"]