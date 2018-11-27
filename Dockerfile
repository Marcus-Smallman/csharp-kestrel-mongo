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

RUN dotnet publish -c release -o published -r linux-arm

# Influenced by https://github.com/burtonr/csharp-kestrel-template
ADD https://github.com/openfaas-incubator/of-watchdog/releases/download/0.4.0/of-watchdog-armhf /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog

FROM microsoft/dotnet:2.0.0-runtime-stretch-arm32v7

WORKDIR /root/
COPY --from=builder /root/src/published .
COPY --from=builder /usr/bin/fwatchdog /

ENV fprocess="dotnet ./root.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:5000"

EXPOSE 8080
CMD ["/fwatchdog"]