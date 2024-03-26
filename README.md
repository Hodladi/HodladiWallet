This is a .NET 8 Blazor Web application that works as an UI for ([Phoenix Server](https://phoenix.acinq.co/server)

It's avalible for ARM64, ARMv7 and AMD64 architecture, see packages to determine which to pull.

To use this application you need to add a appsettings.json file that should contain the below.
Place the appsettings.json file at a suitable place and relate to the file path in the next step.
```
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "PhoenixApi": {
        "BaseUrl": "http://ip-to-Phoneix-server:port",
        "Password": "password-to-phoneix-server"
    }
}
```
To run the application, use below command but replace /path/on/host/appsettings.json with the path to your appsettings.json
```
**Start AMD64 version.**
docker run -v /path/on/host/appsettings.json:/app/appsettings.json -p 5799:5799 docker pull ghcr.io/hodladi/hodladiwallet/hodladiwallet:main-51ef0b6-amd64

**Start ARM64 version.**
docker run -v /path/on/host/appsettings.json:/app/appsettings.json -p 5799:5799 docker pull ghcr.io/hodladi/hodladiwallet/hodladiwallet:main-51ef0b6-arm64

**Start ARMv7 version.**
docker run -v /path/on/host/appsettings.json:/app/appsettings.json -p 5799:5799 docker pull ghcr.io/hodladi/hodladiwallet/hodladiwallet:main-51ef0b6-armv7
```
This will start the container and the application will listen to port 5799 in a browser.
